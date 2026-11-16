# Tareas de Implementación: Cambiar Estado de Panel del Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Panel` y `EstadoPanel`.
  - [ ] 1.2 Crear `AccionEstadoPanel` con `Encender` y `Apagar`.
  - [ ] 1.3 Validar que la acción sea válida y que el panel no esté ya en el estado solicitado.
  - [ ] 1.4 Crear pruebas unitarias para acción válida, acción inválida y panel ya en el estado solicitado.

- [ ] **2. Capa Application**
  - [ ] 2.1 Crear `CambiarEstadoPanelDto` y `PanelDetalleDto`.
  - [ ] 2.2 Definir el puerto `ISolarUyApiPort` con la consulta del panel y el cambio de estado.
  - [ ] 2.3 Implementar `CambiarEstadoPanelService` con validación de identificadores, pertenencia y estado actual.
  - [ ] 2.4 Manejar los casos de panel inexistente, panel ajeno al nodo o inversor y API no disponible.
  - [ ] 2.5 Crear pruebas unitarias para cambio exitoso, panel ya en el estado, panel inexistente y API no disponible.

- [ ] **3. Capa de Infraestructura**
  - [ ] 3.1 Implementar el cliente HTTP de SolarUY con URL base, credenciales y tiempo de espera configurables.
  - [ ] 3.2 Enviar la acción de encendido o apagado y leer la respuesta actualizada.
  - [ ] 3.3 Traducir errores HTTP y de red a resultados controlados para la aplicación.
  - [ ] 3.4 Confirmar que el cliente sea compatible con ARM y no acceda directamente al hardware del inversor.

- [ ] **4. Capa de Integración (API mínima)**
  - [ ] 4.1 Exponer `PUT /inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/estado`.
  - [ ] 4.2 Validar que el cuerpo indique `Encender` o `Apagar`.
  - [ ] 4.3 Devolver los códigos `200`, `400`, `404`, `409` y `502` según el resultado.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que el dominio no dependa de HTTP, de ASP.NET Core ni del hardware.
  - [ ] 5.2 Ejecutar las pruebas de arquitectura (`ArchitectureTest`/ArchUnitNET) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
