# Tareas de Implementación: Eliminar Nodo

- [ ] **1. Capa Domain (`Modules.Nodo.Domain`)**
  - [ ] 1.1 Definir métodos de dominio y reglas en la entidad `Nodo` para validar que el estado del nodo permita su eliminación.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Extender `INodoRepository` y `NodoRepository` incorporando el método `EliminarAsync(Nodo nodo)` para remover la entidad de la base de datos mediante EF Core (`DbSet<Nodo>.Remove`).
  - [ ] 2.2 Verificar que la eliminación física del registro libere automáticamente el índice único asignado a `InversorId` en la base de datos.

- [ ] **3. Capa Application**
  - [ ] 3.1 Declarar la operacion `Task EliminarNodoAsync(long nodoId, int parqueSolarId)` en la interfaz `INodoService`.
  - [ ] 3.2 Definir el método `ExistePanelesAsociadosAsync(long nodoId)` en `IMediadorNodoPanel` para consultar dependencias con el módulo de Paneles Solares.
  - [ ] 3.3 Definir el método `LiberarInversorAsync(long inversorId)` en `IMediadorNodoInversor` para desvincular el inversor asociado si corresponde.
  - [ ] 3.4 Implementar la lógica de `EliminarNodoAsync` en `NodoService`.
  - [ ] 3.5 Crear Tests Unitarios para evaluar la eliminación exitosa, error por nodo inexistente, error por parque no coincidente y denegación por presencia de paneles vinculados.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Modificar la vista de listado (`ListadoNodos.razor` / `NodosParque.razor`) agregando el botón de acción de borrado (`RadzenButton` con `Icon="delete"`).
  - [ ] 4.2 Proteger el acceso y visualización de la acción únicamente para usuarios con el rol `Administrador`.
  - [ ] 4.3 Inyectar e integrar `DialogService` de Radzen para mostrar el modal emergente de confirmación al presionar el botón de eliminación.
  - [ ] 4.4 Implementar el flujo de confirmación: invocar `INodoService.EliminarNodoAsync`, actualizar la grilla/lista de nodos y notificar el resultado mediante `NotificationService` de Radzen.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [ ] 5.2 Ejecutar inspección de código (o análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.