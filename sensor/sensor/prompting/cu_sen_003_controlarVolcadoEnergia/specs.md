# Especificaciones Técnicas: Controlar Volcado de Energía del Panel

## Arquitectura y Patrones
- **Estilo:** Servicio autónomo con Arquitectura Limpia (Modules.Sensor), ejecutado sobre equipo ARM y .NET 10.
- **Patrones:** Servicios de aplicación y Puertos y Adaptadores. El dominio modela el control del panel, la aplicación orquesta y la infraestructura implementa el cliente HTTP de SolarUY.
- **Integración:** El sensor expone `PUT /inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/volcado` y delega el cambio en la API de SolarUY del inversor. No accede directamente al hardware.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Sensor.Domain)
- **Entidades y objetos de valor:**
    - Reutilizar `Panel` y agregar la propiedad booleana `VolcarEnergia`.
    - El valor forma parte del estado de lectura del panel.
- **Comportamiento:**
    - Validar que el panel tenga identificador y nodo válidos.
    - Un panel con el mismo valor de `VolcarEnergia` no puede volver a cambiarse al mismo valor.
    - El dominio no conoce HTTP ni la API de SolarUY.

### 2. Capa Application (Servicios)
- **Datos de entrada:** `CambiarVolcadoEnergiaDto`
  - Propiedades: `IdInversor` (int), `IdNodo` (int), `IdPanel` (int), `VolcarEnergia` (bool).
- **DTO de salida:** `PanelDetalleDto`
  - Propiedades: `Id` (int), `Nombre` (string), `FechaCreacion` (DateTime), `EstadoPanel` (string), `Encendido` (bool), `VolcarEnergia` (bool), `Potencia` (decimal), `VidaUtil` (string).
- **Puerto:** `ISolarUyApiPort`
  - Métodos: `ObtenerPanelAsync(int idInversor, int idNodo, int idPanel)` y `CambiarVolcadoEnergiaAsync(int idInversor, int idNodo, int idPanel, bool volcarEnergia)`.
- **Servicio:** `CambiarVolcadoEnergiaService` (implementa `ICambiarVolcadoEnergiaService`)
  - Dependencias: `ISolarUyApiPort`.
  - Lógica:
    1. Recibe `idInversor`, `idNodo`, `idPanel` y el valor de `volcarEnergia`.
    2. Valida que los identificadores sean positivos.
    3. Consulta el panel en la API de SolarUY y verifica que pertenezca al nodo y al inversor.
    4. Si el panel ya tiene el valor solicitado, devuelve un error de conflicto sin enviar el cambio.
    5. Envía el cambio a la API de SolarUY y proyecta la respuesta a `PanelDetalleDto`.
    6. Si la API informa un error, lo devuelve sin inventar un resultado exitoso.

### 3. Capa de Infraestructura (Cliente de SolarUY)
- **Adaptador:** `SolarUyApiClient` implementa la consulta y el cambio de volcado sobre `HttpClient`.
  - URL base, credenciales y tiempo de espera configurados en `appsettings`.
  - Envía el valor booleano `volcarEnergia` y lee la respuesta actualizada.
  - Maneja respuestas 404, 409 y errores de red como resultados controlados.
  - Compatible con ARM: sin dependencias de escritorio y con uso acotado de memoria.

### 4. Capa de Integración (API mínima)
- **Endpoint:** `PUT /inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/volcado`.
- **Cuerpo:** `{ "volcarEnergia": true }` o `{ "volcarEnergia": false }`.
- **Respuestas:** `200` con el panel actualizado, `400` si el cuerpo o los identificadores son inválidos, `404` si el panel no existe o no pertenece al nodo/inversor, `409` si el panel ya tiene el valor solicitado, `502` si la API de SolarUY no responde.

### 5. Cambios en Taller_.NET
- **Domain:** Agregar `VolcarEnergia` a `Panel`, inicializarlo en el alta en `false` y permitir cambiarlo mediante `CambiarVolcadoEnergia(bool volcarEnergia)`.
- **Persistencia:** Agregar la columna `volcar_energia` a la tabla `Paneles` y crear la migración de Entity Framework correspondiente.
- **DTOs y validadores:** Incluir `VolcarEnergia` en `CrearPanelDto`, `ModificarPanelDto`, `PanelDetalleDto`, `PanelConsultaDto` y `PanelListadoDto`.
- **Interfaz:** Mostrar el atributo en el detalle y el listado de paneles, y permitir editarlo en el formulario de modificación del panel.
- **API:** Exponer `PUT /api/inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/volcado` con un servicio y controlador que validen la pertenencia y actualicen el panel.
