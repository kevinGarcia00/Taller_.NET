# Tareas de Implementación: Eliminar Panel

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Panel`, `EstadoPanel` y la navegación `Nodo` de `Modules.ParqueSolar`.
  - [ ] 1.2 Mantener la pertenencia al nodo como condición de carga: no se elimina un panel que no corresponda al nodo seleccionado.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `PanelConfiguration`, `DbSet<Panel> Paneles` de `IParqueDbContext` y `PanelRepository`.
  - [ ] 2.2 Exponer en el repositorio la baja del panel cargado y la confirmación de cambios.

- [ ] **3. Capa Application**
  - [ ] 3.1 Declarar en `IPanelService` el método `EliminarPanelAsync(int id, int nodoId, int parqueSolarId)`.
  - [ ] 3.2 Implementar el método en `PanelService`, con `IPanelRepository` e `INodoRepository` inyectados por constructor.
  - [ ] 3.3 Implementar la lógica: cargar el nodo del parque, obtener el panel por `Id` y `NodoId`, lanzar excepción si no existe y removerlo persistiendo los cambios.
  - [ ] 3.4 Crear las pruebas unitarias del servicio para nodo inválido, panel inexistente en el nodo y eliminación exitosa.
  - [ ] 3.5 Preparar el código para que quede conectado con la implementación de Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Ubicar la acción de eliminar en `Paneles.razor` (`@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles"`).
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.3 Agregar un `RadzenButton` de eliminación en la columna de acciones del `RadzenDataGrid` e integrar `DialogService` para confirmar, mostrando el nombre del panel.
  - [ ] 4.4 Si se confirma, invocar `IPanelService.EliminarPanelAsync(id, nodoId, parqueSolarId)`.
  - [ ] 4.5 Notificar el resultado con `NotificationService` de Radzen y refrescar la grilla. Ante un error de negocio, mostrar una advertencia sin romper la página.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
