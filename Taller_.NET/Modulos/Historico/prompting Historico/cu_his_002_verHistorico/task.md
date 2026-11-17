# Tareas de Implementación: Ver Histórico del Panel

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Historico` y `ContextoPanel` de `Modules.Historico.Domain`.
  - [ ] 1.2 Verificar que la entidad exponga `Id`, `Fecha` y `EnergiaTotal`, sin agregar métodos que modifiquen el registro.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `HistoricoConfiguration`, `DbSet<Historico> Registros` de `IHistoricoDbContext` y `HistoricoRepository`.
  - [ ] 2.2 Implementar la consulta por `PanelId`, ordenada por `Fecha` descendente, proyectando solo `Id`, `Fecha` y `EnergiaTotal`.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO de salida `HistoricoListadoDto` (`Id`, `Fecha`, `EnergiaTotal`).
  - [ ] 3.2 Declarar en `IHistoricoService` el método `ObtenerRegistrosPorPanelAsync(int panelId, int nodoId, int parqueSolarId)` y en `IMediadorHistoricoParqueSolar` el método `ExistePanelAsync(int panelId, int nodoId, int parqueSolarId)`.
  - [ ] 3.3 Implementar el método en `HistoricoService`, con el repositorio y el mediador inyectados por constructor.
  - [ ] 3.4 Implementar la lectura confirmando antes el panel con el mediador y retornando la lista sin modificar las entidades. Si no hay registros, retornar una lista vacía. Lanzar `PanelNoEncontradoException` si el panel no pertenece al nodo y parque.
  - [ ] 3.5 Crear las pruebas unitarias del servicio: listado con datos, listado vacío y panel inexistente en el nodo.
  - [ ] 3.6 Preparar el código para que quede conectado con Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Agregar en la ficha del panel (`DetallePanel.razor`) el botón **"Ver histórico"** (visible para el rol "Administrador") que abre el listado del panel.
  - [ ] 4.2 Crear el componente `HistorialPanel.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{PanelId:int}/historico"`.
  - [ ] 4.3 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.4 Configurar el `RadzenDataGrid<HistoricoListadoDto>` con las columnas `Id`, `Fecha` y `EnergiaTotal`, y el mensaje de listado vacío cuando el panel no tiene registros.
  - [ ] 4.5 Invocar `ObtenerRegistrosPorPanelAsync` en `OnInitializedAsync`.
  - [ ] 4.6 Si la consulta falla, notificar con `NotificationService` de Radzen y ofrecer el regreso a la ficha del panel.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
