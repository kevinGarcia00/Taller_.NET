# Tareas de Implementación: Recuperar Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar el modelo de lectura `Inversor` con `Id`, `Nombre` y `FechaCreacion`.
  - [ ] 1.2 Verificar que el modelo no tenga métodos de modificación ni dependencias de HTTP.

- [ ] **2. Capa Application**
  - [ ] 2.1 Crear `InversorDetalleDto` con `Id`, `Nombre`, `FechaCreacion`, `FechaConsulta` y `UptimeSensor`.
  - [ ] 2.2 Definir el puerto `ISolarUyApiPort` con `ObtenerInversorAsync(int idInversor)`.
  - [ ] 2.3 Definir el puerto `IProveedorTiempoSensor` para obtener la fecha actual y el uptime del sensor.
  - [ ] 2.4 Implementar `RecuperarInversorService` con validación del identificador y consulta a SolarUY.
  - [ ] 2.5 Manejar el caso de inversor inexistente y de API no disponible.

- [ ] **3. Capa de Infraestructura**
  - [ ] 3.1 Implementar la consulta del inversor en `SolarUyApiClient`.
  - [ ] 3.2 Traducir errores HTTP y de red a resultados controlados para la aplicación.
  - [ ] 3.3 Implementar `ProveedorTiempoSensor` con el instante de inicio y un reloj monotónico.
  - [ ] 3.4 Confirmar que los adaptadores sean compatibles con ARM y no accedan directamente al hardware del inversor.

- [ ] **4. Capa de Integración (API mínima)**
  - [ ] 4.1 Exponer `GET /api/inversor/{idInversor}`.
  - [ ] 4.2 Devolver el inversor junto con `FechaConsulta` y `UptimeSensor`.
  - [ ] 4.3 Devolver los códigos `200`, `400`, `404` y `502` según el resultado.

- [ ] **5. Cambios en Taller_.NET**
  - [ ] 5.1 Crear `InversorDetalleDto` con `Id`, `Nombre` y `FechaCreacion`.
  - [ ] 5.2 Agregar `ObtenerInversorAsync` a `IConsultaInversorService` y `ConsultaInversorService`.
  - [ ] 5.3 Crear el controlador y exponer `GET /api/inversor/{idInversor}`.
  - [ ] 5.4 Devolver `404` cuando el inversor no exista.

- [ ] **6. Verificación de Calidad (Guardarraíles)**
  - [ ] 6.1 Ejecutar la suite de pruebas para validar que el dominio no dependa de HTTP, de ASP.NET Core ni del hardware.
  - [ ] 6.2 Ejecutar las pruebas de arquitectura (`ArchitectureTest`/ArchUnitNET) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 6.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
