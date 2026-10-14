# Tareas de Implementación: Ver Paneles

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Panel`, `EstadoPanel` y `ContextoNodo`.
  - [ ] 1.2 Verificar que la entidad exponga Nombre, Estado, Potencia, Vida útil, Fecha de creación y contexto de nodo para la consulta, sin agregar métodos que modifiquen el panel.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `PanelConfiguration`, `DbSet<Panel>` y `PanelRepository`.
  - [ ] 2.2 Implementar la consulta por nodo, ordenada por nombre, y la consulta de detalle por `Id` y `NodoId`, proyectando solo los campos necesarios.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear `PanelListadoDto` (`Id`, `Nombre`, `Estado`, `Potencia`) y `PanelDetalleDto` (`Id`, `Nombre`, `Estado`, `Potencia`, `VidaUtil`, `FechaCreacion`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Agregar a `IPanelService` e implementar en `PanelService`:
    - `ObtenerPanelesPorNodoAsync(int nodoId, int parqueSolarId)`
    - `ObtenerPanelAsync(int id, int nodoId, int parqueSolarId)`
  - [ ] 3.3 Implementar ambas lecturas confirmando antes el nodo con `IMediadorPanelNodo` y sin modificar la entidad.
  - [ ] 3.4 Crear pruebas unitarias para listado con datos, listado vacío, detalle exitoso, panel inexistente en el nodo y nodo inválido.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear `Paneles.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles"`.
  - [ ] 4.2 Crear `DetallePanel.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{Id:int}"`.
  - [ ] 4.3 Restringir ambas páginas con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.4 Configurar el `RadzenDataGrid<PanelListadoDto>` y el botón "Registrar panel".
  - [ ] 4.5 Mostrar en el detalle, en solo lectura, Nombre, Estado, Potencia, Vida útil y Fecha de creación.
  - [ ] 4.6 Cargar los datos en `OnInitializedAsync` y notificar con `NotificationService` si la consulta falla.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni dependencia directa entre módulos.
  - [ ] 5.2 Ejecutar la inspección de código (o el análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
