# Tareas de Implementación: Ver Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Verificar la propiedad `InversorId` (`long?`) en la entidad `Nodo`.
  - [ ] 1.2 Agregar el método de consulta `TieneInversorAsignado` en la entidad `Nodo` para determinar la presencia del inversor.

- [ ] **2. Capa Infrastructure (Persistencia y Comunicación Inter-Módulo)**
  - [ ] 2.1 Implementar el método de lectura `ObtenerPorIdAsync(long id)` en `NodoRepository`.
  - [ ] 2.2 Crear o extender la implementación de `MediadorNodoInversor` para consultar los datos del inversor (`Nombre`, `FechaCreacion`) desde la capa de aplicación/repositorio del módulo `Modules.Inversor`.

- [ ] **3. Capa Application ()**
  - [ ] 3.1 Crear el DTO `InversorDto`.
  - [ ] 3.2 Definir en la interfaz `IMediadorNodoInversor` el método `ObtenerPorIdAsync(long inversorId)`.
  - [ ] 3.3 Declarar en `INodoService` la firma `Task<InversorDto?> ObtenerInversorPorNodoAsync(long nodoId, int parqueSolarId)`.
  - [ ] 3.4 Implementar la lógica en `NodoService`.
  - [ ] 3.5 Crear Tests Unitarios para evaluar la consulta de un nodo con inversor, la respuesta correcta para un nodo sin inversor, y las excepciones por nodo inexistente o parque solar no coincidente.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente de diálogo `VerInversorDialog.razor` para visualizar en campos de solo lectura la información del inversor (`Nombre`, `FechaCreacion`).
  - [ ] 4.2 Proteger el acceso y visualización de la acción únicamente para usuarios con el rol `Administrador`.
  - [ ] 4.3 Incorporar el botón de acción por fila en la grilla del listado de nodos (`ListadoNodos.razor`).
  - [ ] 4.4 Implementar el evento en el componente: invocar `INodoService.ObtenerInversorPorNodoAsync`, desplegar el modal con `DialogService.OpenAsync<VerInversorDialog>` si existe inversor, o mostrar una notificación informativa con `NotificationService` si el nodo no posee inversor asignado.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [ ] 5.2 Ejecutar inspección de código para asegurar que no hay code smells.