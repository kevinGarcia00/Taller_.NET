# Especificaciones Técnicas: Recuperar Inversor

## Arquitectura y Patrones
- **Estilo:** Servicio autónomo con Arquitectura Limpia (Modules.Sensor), ejecutado sobre equipo ARM y .NET 10.
- **Patrones:** Servicios de aplicación y Puertos y Adaptadores. El dominio modela los datos de consulta, la aplicación orquesta y la infraestructura implementa el cliente HTTP de SolarUY.
- **Integración:** El sensor expone `GET /api/inversor/{idInversor}`, obtiene el inversor desde la API de SolarUY y agrega el uptime del sensor a la respuesta. No accede directamente al hardware.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Sensor.Domain)
- **Entidades y modelos de lectura:**
    - Reutilizar `Inversor` con `Id`, `Nombre` y `FechaCreacion`.
    - El modelo es de solo lectura y no conoce HTTP ni la API de SolarUY.
- **Comportamiento:**
    - No se agregan métodos que cambien los datos del inversor.

### 2. Capa Application (Servicios)
- **DTO de salida:** `InversorDetalleDto`
  - Propiedades: `Id` (int), `Nombre` (string), `FechaCreacion` (DateTime), `FechaConsulta` (DateTime) y `UptimeSensor` (TimeSpan).
- **Puerto:** `ISolarUyApiPort`
  - Método: `ObtenerInversorAsync(int idInversor)`.
- **Puerto:** `IProveedorTiempoSensor`
  - Métodos: `ObtenerFechaActualUtc()` y `ObtenerUptime()`.
- **Servicio:** `RecuperarInversorService` (implementa `IRecuperarInversorService`)
  - Dependencias: `ISolarUyApiPort` e `IProveedorTiempoSensor`.
  - Lógica:
    1. Recibe `idInversor` desde el endpoint.
    2. Valida que el identificador sea positivo.
    3. Consulta la API de SolarUY y verifica que el inversor exista.
    4. Obtiene la fecha de consulta y el uptime del sensor.
    5. Proyecta la respuesta a `InversorDetalleDto` sin incluir nodos ni paneles.
    6. Si la API informa un error, lo devuelve sin inventar datos.

### 3. Capa de Infraestructura (Cliente de SolarUY)
- **Adaptador:** `SolarUyApiClient` implementa `ISolarUyApiPort` sobre `HttpClient`.
  - Consulta `GET api/inversor/{idInversor}` usando la URL base, credenciales y tiempo de espera configurados.
  - Maneja respuestas 404 y errores de red como resultados controlados.
- **Adaptador:** `ProveedorTiempoSensor` implementa `IProveedorTiempoSensor`.
  - Guarda el instante de inicio del sensor al arrancar y calcula el uptime con un reloj monotónico.
  - Devuelve la fecha actual en UTC y el tiempo activo con precisión de segundos.
  - Compatible con ARM, sin dependencias de escritorio.

### 4. Capa de Integración (API mínima)
- **Endpoint:** `GET /api/inversor/{idInversor}`.
- **Respuestas:** `200` con `InversorDetalleDto`, `400` si el identificador es inválido, `404` si el inversor no existe, `502` si la API de SolarUY no responde.

### 5. Cambios en Taller_.NET
- **Aplicación:** Agregar `ObtenerInversorAsync(int idInversor)` a `IConsultaInversorService`.
- **Servicio:** Implementar la consulta en `ConsultaInversorService` reutilizando `IConsultaInversorRepository.ObtenerInversorPorIdAsync`.
- **DTO:** Crear `InversorDetalleDto` con `Id`, `Nombre` y `FechaCreacion`.
- **API:** Exponer `GET /api/inversor/{idInversor}` con un controlador que devuelva `404` si el inversor no existe.
