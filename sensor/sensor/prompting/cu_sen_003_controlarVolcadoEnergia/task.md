# Tareas de Implementación: Controlar Volcado de Energía del Panel

- [ ] **1. Capa Domain (Sensor)**
  - [ ] 1.1 Agregar la propiedad booleana `VolcarEnergia` a `Panel`.
  - [ ] 1.2 Crear el método `CambiarVolcadoEnergia` con validación de valor repetido.
  - [ ] 1.3 Crear pruebas unitarias para cambio exitoso, valor repetido y panel inválido.

- [ ] **2. Capa Application (Sensor)**
  - [ ] 2.1 Crear `CambiarVolcadoEnergiaDto` y agregar `VolcarEnergia` a `PanelDetalleDto`.
  - [ ] 2.2 Definir el puerto `ISolarUyApiPort` con la consulta del panel y el cambio de volcado.
  - [ ] 2.3 Implementar `CambiarVolcadoEnergiaService` con validación de identificadores, pertenencia y valor actual.
  - [ ] 2.4 Manejar los casos de panel inexistente, panel ajeno al nodo o inversor y API no disponible.

- [ ] **3. Capa de Infraestructura (Sensor)**
  - [ ] 3.1 Implementar el cambio de volcado en `SolarUyApiClient`.
  - [ ] 3.2 Enviar el valor booleano y leer la respuesta actualizada.
  - [ ] 3.3 Traducir errores HTTP y de red a resultados controlados para la aplicación.
  - [ ] 3.4 Confirmar que el cliente sea compatible con ARM y no acceda directamente al hardware del inversor.

- [ ] **4. Capa de Integración (Sensor)**
  - [ ] 4.1 Exponer `PUT /inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/volcado`.
  - [ ] 4.2 Validar que el cuerpo incluya el booleano `volcarEnergia`.
  - [ ] 4.3 Devolver los códigos `200`, `400`, `404`, `409` y `502` según el resultado.

- [ ] **5. Cambios en Taller_.NET**
  - [ ] 5.1 Agregar `VolcarEnergia` al dominio `Panel` y al modelo de persistencia.
  - [ ] 5.2 Actualizar `CrearPanelDto`, `ModificarPanelDto`, `PanelDetalleDto`, `PanelConsultaDto` y `PanelListadoDto`.
  - [ ] 5.3 Actualizar los validadores y el mapeo de `PanelService`.
  - [ ] 5.4 Crear la migración para la columna `volcar_energia`.
  - [ ] 5.5 Mostrar y permitir editar `VolcarEnergia` en las pantallas de alta, detalle, listado y modificación de paneles.
  - [ ] 5.6 Crear el servicio y el endpoint `PUT /api/inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/volcado`.

- [ ] **6. Verificación de Calidad (Guardarraíles)**
  - [ ] 6.1 Ejecutar la suite de pruebas para validar que el dominio no dependa de HTTP, de ASP.NET Core ni del hardware.
  - [ ] 6.2 Ejecutar las pruebas de arquitectura (`ArchitectureTest`/ArchUnitNET) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 6.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
