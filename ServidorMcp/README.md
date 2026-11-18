# Servidor MCP — SolarUY

Servidor Model Context Protocol (MCP) que publica consultas de solo lectura de los módulos **ParqueSolar** y **Usuario** de Taller_.NET. Los clientes MCP pueden invocar estas herramientas desde preguntas en lenguaje natural.

## Alcance y fuentes de datos

- **Parques, nodos, paneles e inversores:** se consultan por HTTP a la API de Taller_.NET.
- **Usuarios:** `listar_usuarios` lee directamente la tabla `Usuarios` del archivo SQLite configurado. No llama a la API de usuarios ni necesita un token de Auth0.
- **Roles:** no hay herramientas MCP para consultar roles.
- No se exponen operaciones de escritura, histórico de energía ni mantenibilidad.

Las consultas de usuarios solo leen los campos `id`, `nombre`, `apellido`, `correo`, `telefono` y `auth0_user_id`. El repositorio abre SQLite en modo de solo lectura.

## Herramientas disponibles

| Herramienta MCP | Función |
| --- | --- |
| `listar_parques_solares` | Lista parques con ubicación, cantidad actual de nodos y capacidad máxima. |
| `obtener_detalle_parque_solar` | Obtiene el detalle de un parque por nombre, incluidos sus nodos. |
| `listar_nodos_parque_solar` | Lista los nodos de un parque y su energía generada. |
| `listar_nodos_ordenados_por_energia` | Lista los nodos de un parque ordenados por energía descendente. |
| `listar_nodos_con_cantidad_paneles` | Lista los nodos de un parque con su cantidad de paneles. |
| `listar_paneles_del_nodo` | Lista los paneles de un nodo; el nombre del parque es opcional. |
| `contar_paneles_del_nodo` | Cuenta los paneles de un nodo; el nombre del parque es opcional. |
| `listar_paneles_apagados_del_parque` | Lista los paneles apagados de un parque. |
| `obtener_panel_por_nombre` | Busca un panel por nombre, opcionalmente acotado por parque o nodo. |
| `obtener_inversor_del_nodo` | Obtiene el inversor de un nodo; el nombre del parque es opcional. |
| `listar_inversores` | Lista los inversores registrados. |
| `listar_usuarios` | Lista usuarios de la base SQLite local. |


## Requisitos

- **.NET 10 SDK**.
- Archivo `Taller_.NET/parquesolar.db` accesible en modo lectura para el proceso MCP.
- API web de Taller_.NET en ejecución para las herramientas de parques (por defecto `http://127.0.0.1:5284`). La consulta de usuarios no depende de que la API esté levantada.
- Un cliente compatible con MCP para conectarse al servidor. Antigravity CLI (`agy`) instalado y autenticado solo es necesario si se utiliza ese cliente o la integración de chat de Taller_.NET.

## Configuración

La configuración está en `src/ServidorMcp/appsettings.json`:

```json
{
  "ApiSolarUy": {
    "BaseUrl": "http://127.0.0.1:5284"
  },
  "DatosSolarUy": {
    "RutaUsuarios": "../../../../../../Taller_.NET/parquesolar.db"
  },
  "Mcp": {
    "Urls": "http://127.0.0.1:5285"
  }
}
```

La ruta relativa de SQLite se resuelve desde el directorio de ejecución del MCP (`AppContext.BaseDirectory`) y supone la estructura de carpetas de este repositorio. Para otra instalación, configura una ruta absoluta. Las variables de entorno tienen prioridad sobre `appsettings.json`:

- `ApiSolarUy__BaseUrl`: URL base de la API usada por consultas de parques.
- `DatosSolarUy__RutaUsuarios`: ruta al archivo SQLite compartido.
- `Mcp__Urls`: URL en la que escucha el servidor MCP.


## Ejecución local

Desde la raíz del repositorio, inicia la API y el MCP en terminales separadas:

```bash
# Terminal 1: API de Taller_.NET
dotnet run --project Taller_.NET/Taller_.NET.csproj --launch-profile http

# Terminal 2: servidor MCP
dotnet run --project ServidorMcp/src/ServidorMcp/ServidorMcp.csproj --no-launch-profile
```

El endpoint MCP queda en `http://127.0.0.1:5285/mcp`. Para registrar esa URL en Antigravity CLI:

```bash
agy mcp add SolarUy http://127.0.0.1:5285/mcp
```

`agy` utiliza su configuración global (`~/.gemini/config/mcp_config.json`). El MCP también puede registrarse en otro cliente compatible con Streamable HTTP.

## Arquitectura

El servidor usa un único proyecto organizado por capas:

- `Domain`: modelo `Parque` y su regla de comparación de capacidad; no accede a HTTP, SQLite ni MCP.
- `Application`: DTOs, puertos e implementación de los casos de consulta. `ParqueConsultaService` coordina consultas y lógica de búsqueda; `UsuarioConsultaService` delega la lectura de usuarios al puerto del repositorio.
- `Infrastructure`: `ParqueSolarHttpRepository` obtiene datos de parques por HTTP; `UsuarioSqliteRepository` consulta SQLite en solo lectura.
- `Presentacion/Herramientas`: herramientas MCP que reciben parámetros y delegan en los servicios de Application.
- `Program.cs`: composición de dependencias, configuración del host Streamable HTTP y publicación del endpoint `/mcp`.
- `tests/Domain.Tests`: pruebas unitarias del dominio.

Las interfaces de repositorio se definen en Application y sus implementaciones concretas, en Infrastructure. Las dependencias se inyectan por constructor.

## Seguridad y acceso a datos

El MCP se configura por defecto para escuchar únicamente en `127.0.0.1`. No tiene autenticación propia. En particular, `listar_usuarios` accede directamente a SQLite y, por lo tanto, no pasa por la autorización `users:read` de la API. No expongas el endpoint MCP a una red no confiable sin agregar una protección apropiada. El proceso también necesita permisos de lectura sobre el archivo SQLite.
