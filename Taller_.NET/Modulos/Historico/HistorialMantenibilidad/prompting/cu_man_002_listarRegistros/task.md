# Tareas de Implementación: Listar Registros de Mantenibilidad

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `RegistroMantenibilidad`, `EstadoPanelRegistrado`, `Mantenedor` y `ContextoPanel`.
  - [ ] 1.2 Verificar que la entidad exponga el identificador del mantenedor, la fecha, las observaciones, el estado inicial y el estado resultante, sin agregar métodos que modifiquen el registro.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `RegistroMantenibilidadConfiguration`, `DbSet<RegistroMantenibilidad>` y `RegistroMantenibilidadRepository`.
  - [ ] 2.2 Implementar la consulta por `PanelId`, ordenada por `Fecha` descendente, proyectando solo los campos del listado.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear `RegistroMantenibilidadListadoDto` con `Id`, `IdMantenedor`, `Fecha`, `Observaciones`, `EstadoInicial` y `EstadoResultante`.
  - [ ] 3.2 Agregar a `IRegistroMantenibilidadService` e implementar en `RegistroMantenibilidadService` el método `ObtenerRegistrosPorPanelAsync(int panelId, int nodoId, int parqueSolarId)`.
  - [ ] 3.3 Implementar la lectura confirmando antes el panel con `IMediadorHistorialPanel` y retornando la lista sin modificar las entidades.
  - [ ] 3.4 Crear pruebas unitarias para listado con datos, listado vacío y panel inexistente en el nodo.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear `HistorialMantenibilidad.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{PanelId:int}/mantenibilidad"`.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Configurar el `RadzenDataGrid<RegistroMantenibilidadListadoDto>` con las columnas Id del mantenedor, Fecha, Observaciones, Estado inicial y Estado resultante.
  - [ ] 4.4 Invocar `ObtenerRegistrosPorPanelAsync` en `OnInitializedAsync` y mostrar el listado vacío cuando no hay registros.
  - [ ] 4.5 Notificar con `NotificationService` si la consulta falla y ofrecer el regreso a la ficha del panel.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni dependencia directa entre módulos.
  - [ ] 5.2 Ejecutar la inspección de código (o el análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
