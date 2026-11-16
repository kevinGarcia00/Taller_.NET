# Especificaciones Técnicas: Cambiar Estado de Panel del Inversor

## Arquitectura y Patrones
- **Estilo:** Servicio autónomo con Arquitectura Limpia (Modules.Sensor), ejecutado sobre equipo ARM y .NET 10.
- **Patrones:** Servicios de aplicación y Puertos y Adaptadores. El dominio modela el cambio de estado, la aplicación orquesta y la infraestructura implementa el cliente HTTP de SolarUY.
- **Integración:** El sensor expone `PUT /inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/estado` y delega el cambio en la API de SolarUY del inversor. No accede directamente al hardware.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Sensor.Domain)
- **Entidades y objetos de valor:**
    - Reutilizar `Panel` y `EstadoPanel` definidos por SolarUY.
    - `AccionEstadoPanel`: `Encender`, `Apagar`.
- **Comportamiento:**
    - Validar que la acción sea válida y que el panel tenga identificador y estado correctos.
    - Un panel encendido no puede volver a encenderse y uno apagado no puede volver a apagarse.
    - El dominio no conoce HTTP ni la API de SolarUY.

### 2. Capa Application (Servicios)
- **Datos de entrada:** `CambiarEstadoPanelDto`
  - Propiedades: `IdInversor` (int), `IdNodo` (int), `IdPanel` (int), `Accion` (`AccionEstadoPanel`).
- **DTO de salida:** `PanelDetalleDto`
  - Propiedades: `Id` (int), `Nombre` (string), `FechaCreacion` (DateTime), `EstadoPanel` (string), `Encendido` (bool), `Potencia` (decimal), `VidaUtil` (string).
- **Puerto:** `ISolarUyApiPort`
  - Métodos: `ObtenerPanelAsync(int idInversor, int idNodo, int idPanel)` y `CambiarEstadoPanelAsync(int idInversor, int idNodo, int idPanel, AccionEstadoPanel accion)`.
- **Servicio:** `CambiarEstadoPanelService` (implementa `ICambiarEstadoPanelService`)
  - Dependencias: `ISolarUyApiPort`.
  - Lógica:
    1. Recibe `idInversor`, `idNodo`, `idPanel` y la acción.
    2. Valida que los identificadores sean positivos y la acción sea válida.
    3. Consulta el panel en la API de SolarUY y verifica que pertenezca al nodo y al inversor.
    4. Si el panel ya está en el estado solicitado, devuelve un error de conflicto sin enviar el cambio.
    5. Envía la acción a la API de SolarUY y proyecta la respuesta a `PanelDetalleDto`.
    6. Si la API informa un error, lo devuelve sin inventar un resultado exitoso.

### 3. Capa de Infraestructura (Cliente de SolarUY)
- **Adaptador:** `SolarUyApiClient` implementa las operaciones de consulta y cambio sobre `HttpClient`.
  - URL base, credenciales y tiempo de espera configurados en `appsettings`.
  - Envía la acción de encendido o apagado y lee la respuesta actualizada.
  - Maneja respuestas 404, 409 y errores de red como resultados controlados.
  - Compatible con ARM: sin dependencias de escritorio y con uso acotado de memoria.

### 4. Capa de Integración (API mínima)
- **Endpoint:** `PUT /inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/estado`.
- **Cuerpo:** `{ "accion": "Encender" }` o `{ "accion": "Apagar" }`.
- **Respuestas:** `200` con el panel actualizado, `400` si la acción o los identificadores son inválidos, `404` si el panel no existe o no pertenece al nodo/inversor, `409` si el panel ya está en el estado solicitado, `502` si la API de SolarUY no responde.
