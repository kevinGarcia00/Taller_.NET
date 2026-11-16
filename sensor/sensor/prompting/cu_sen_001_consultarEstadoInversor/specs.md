# Especificaciones Técnicas: Consultar Estado de Panel del Inversor

## Arquitectura y Patrones
- **Estilo:** Servicio autónomo con Arquitectura Limpia (Modules.Sensor), ejecutado sobre equipo ARM y .NET 10.
- **Patrones:** Servicios de aplicación y Puertos y Adaptadores. El dominio modela los datos de consulta, la aplicación orquesta y la infraestructura implementa el cliente HTTP de SolarUY.
- **Integración:** El sensor expone `/inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}` y obtiene la información desde la API de SolarUY del inversor. No accede directamente al hardware.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Sensor.Domain)
- **Entidades y modelos de lectura:**
    - Reutilizar los conceptos `Inversor`, `Nodo`, `Panel` y `EstadoPanel` definidos por SolarUY, adaptados como modelos de consulta.
    - El panel expone `Id`, `Nombre`, `FechaCreacion`, `EstadoPanel`, `Encendido`, `Potencia` y `VidaUtil`.
- **Comportamiento:**
    - Solo lectura. No se agregan métodos que cambien el estado del panel.

### 2. Capa Application (Servicios)
- **DTO de salida:** `PanelDetalleDto`
  - Propiedades: `Id` (int), `Nombre` (string), `FechaCreacion` (DateTime), `EstadoPanel` (string), `Encendido` (bool), `Potencia` (decimal), `VidaUtil` (string).
- **DTOs de listado:** `InversorListadoDto` (`Id`, `Nombre`) y `NodoListadoDto` (`Id`, `Nombre`, `Paneles`).
- **Puerto:** `ISolarUyApiPort`
  - Métodos: `ObtenerInversoresAsync()`, `ObtenerNodosAsync(int idInversor)`, `ObtenerPanelesAsync(int idInversor, int idNodo)` y `ObtenerPanelAsync(int idInversor, int idNodo, int idPanel)`.
- **Servicio:** `ConsultaPanelService` (implementa `IConsultaPanelService`)
  - Dependencias: `ISolarUyApiPort`.
  - Lógica:
    1. Recibe `idInversor`, `idNodo` e `idPanel` desde el endpoint.
    2. Valida que los identificadores sean positivos.
    3. Consulta la API de SolarUY y verifica que el panel pertenezca al nodo y al inversor indicados.
    4. Si la API informa que el recurso no existe, devuelve el error correspondiente sin inventar datos.
    5. Proyecta la respuesta a `PanelDetalleDto` y la retorna sin modificarla.

### 3. Capa de Infraestructura (Cliente de SolarUY)
- **Adaptador:** `SolarUyApiClient` implementa `ISolarUyApiPort` sobre `HttpClient`.
  - URL base, credenciales y tiempo de espera configurados en `appsettings`.
  - Maneja respuestas 404 y errores de red como resultados controlados.
  - Compatible con ARM: sin dependencias de escritorio y con uso acotado de memoria.

### 4. Capa de Integración (API mínima)
- **Endpoint:** `GET /inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}`.
- **Endpoints de navegación:** `GET /inversor`, `GET /inversor/{idInversor}/nodo` y `GET /inversor/{idInversor}/nodo/{idNodo}/panel`.
- **Respuestas:** `200` con el detalle del panel, `404` si el inversor, nodo o panel no existe o no coincide, `502` si la API de SolarUY no responde.
