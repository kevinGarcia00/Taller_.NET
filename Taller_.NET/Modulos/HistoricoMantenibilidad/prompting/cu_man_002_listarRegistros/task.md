# Tareas de Implementación: Listar Registros de Mantenibilidad

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `HistoricoMantenibilidad`, `EstadoPanelRegistrado`, `Mantenedor` y `ContextoPanel` de `Modules.Historico`.
  - [ ] 1.2 Verificar que la entidad exponga el identificador del mantenedor, la fecha, las observaciones, el estado inicial y el estado resultante, sin agregar métodos que modifiquen el registro.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `HistoricoMantenibilidadConfiguration`, `DbSet<HistoricoMantenibilidad> Registros` de `IHistoricoDbContext` y `HistoricoMantenibilidadRepository`.
  - [ ] 2.2 Implementar la consulta por `PanelId`, ordenada por `Fecha` descendente, proyectando solo los campos del listado.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO de salida `HistoricoMantenibilidadListadoDto` (`Id`, `IdMantenedor`, `Fecha`, `Observaciones`, `EstadoInicial`, `EstadoResultante`).
  - [ ] 3.2 Declarar en `IHistoricoMantenibilidadService` el método `ObtenerRegistrosPorPanelAsync(int panelId, int nodoId, int parqueSolarId)`. El canal con ParqueSolar sigue siendo `IMediadorHistoricoParqueSolar`.
  - [ ] 3.3 Implementar el método en `HistoricoMantenibilidadService`, con el repositorio y el mediador inyectados por constructor.
  - [ ] 3.4 Implementar la lectura confirmando antes el panel con el mediador y retornando la lista sin modificar las entidades. Si no hay registros, retornar una lista vacía.
  - [ ] 3.5 Crear las pruebas unitarias del servicio para listado con datos, listado vacío y panel inexistente en el nodo.
  - [ ] 3.6 Preparar el código para que quede conectado con la implementación de Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `HistorialMantenibilidad.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{PanelId:int}/mantenibilidad"`.
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.3 Configurar el `RadzenDataGrid<HistoricoMantenibilidadListadoDto>` con las columnas Id del mantenedor, Fecha, Observaciones, Estado inicial y Estado resultante.
  - [ ] 4.4 Invocar `ObtenerRegistrosPorPanelAsync` en `OnInitializedAsync` y mostrar el listado vacío cuando no hay registros.
  - [ ] 4.5 Si la consulta falla, notificar con `NotificationService` de Radzen y ofrecer el regreso a la ficha del panel.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
