# Tareas de Implementación: Eliminar Panel

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Panel`, `EstadoPanel` y `ContextoNodo`.
  - [ ] 1.2 Mantener la pertenencia al nodo como condición de carga: no se elimina un panel que no corresponda al `NodoId` seleccionado.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `PanelConfiguration`, `DbSet<Panel>` y `PanelRepository`.
  - [ ] 2.2 Exponer en el repositorio la operación de baja del panel cargado y la confirmación de cambios.

- [ ] **3. Capa Application**
  - [ ] 3.1 Agregar a `IPanelService` e implementar en `PanelService` el método `EliminarPanelAsync(int id, int nodoId, int parqueSolarId)`.
  - [ ] 3.2 Implementar la lógica de `EliminarPanelAsync`:
    - Confirmar el nodo y el parque mediante `IMediadorPanelNodo`.
    - Obtener el panel por `Id` y `NodoId`.
    - Lanzar excepción si el panel no existe en ese nodo.
    - Remover el panel y persistir los cambios.
  - [ ] 3.3 Crear pruebas unitarias para nodo inválido, panel inexistente en el nodo y eliminación exitosa.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Ubicar la acción de eliminar en el listado `Paneles.razor` del nodo seleccionado.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Agregar el botón de eliminación en la columna de acciones del `RadzenDataGrid`.
  - [ ] 4.4 Integrar `DialogService` para confirmar antes de eliminar.
  - [ ] 4.5 Invocar `EliminarPanelAsync`, notificar con `NotificationService` y refrescar la grilla. Ante un error de negocio, mostrar una advertencia sin romper la página.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni dependencia directa entre módulos.
  - [ ] 5.2 Ejecutar la inspección de código (o el análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
