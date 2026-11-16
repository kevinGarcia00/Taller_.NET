# Tareas de Implementación: Consultar Estado de Panel del Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar los modelos de lectura `Inversor`, `Nodo`, `Panel` y `EstadoPanel`.
  - [ ] 1.2 Verificar que `Panel` exponga Id, Nombre, FechaCreacion, EstadoPanel, Encendido, Potencia y VidaUtil, sin métodos de modificación.

- [ ] **2. Capa Application**
  - [ ] 2.1 Crear `PanelDetalleDto`, `InversorListadoDto` y `NodoListadoDto`.
  - [ ] 2.2 Definir el puerto `ISolarUyApiPort` con las consultas de inversores, nodos, paneles y detalle de panel.
  - [ ] 2.3 Implementar `ConsultaPanelService` con validación de identificadores, consulta a SolarUY y proyección del detalle.
  - [ ] 2.4 Manejar los casos de inversor, nodo o panel inexistente y de panel ajeno al nodo o al inversor.
  - [ ] 2.5 Crear pruebas unitarias para detalle exitoso, panel inexistente, panel ajeno al nodo y API no disponible.

- [ ] **3. Capa de Infraestructura**
  - [ ] 3.1 Implementar `SolarUyApiClient` sobre `HttpClient` con URL base, credenciales y tiempo de espera configurables.
  - [ ] 3.2 Traducir errores HTTP y de red a resultados controlados para la aplicación.
  - [ ] 3.3 Confirmar que el cliente sea compatible con ARM y no acceda directamente al hardware del inversor.

- [ ] **4. Capa de Integración (API mínima)**
  - [ ] 4.1 Exponer `GET /inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}`.
  - [ ] 4.2 Exponer los listados `GET /inversor`, `GET /inversor/{idInversor}/nodo` y `GET /inversor/{idInversor}/nodo/{idNodo}/panel`.
  - [ ] 4.3 Devolver los códigos `200`, `404` y `502` según el resultado de la consulta.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que el dominio no dependa de HTTP, de ASP.NET Core ni del hardware.
  - [ ] 5.2 Ejecutar las pruebas de arquitectura (`ArchitectureTest`/ArchUnitNET) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
