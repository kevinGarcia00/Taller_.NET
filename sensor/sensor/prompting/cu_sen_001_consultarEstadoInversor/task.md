# Tareas de Implementación: Ver Paneles

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Panel`, `EstadoPanel` y la navegación `Nodo` de `Modules.ParqueSolar`.
  - [ ] 1.2 Verificar que la entidad exponga Nombre, Estado, Potencia, Vida útil, Fecha de creación y nodo para la consulta, sin agregar métodos que modifiquen el panel.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `PanelConfiguration`, `DbSet<Panel> Paneles` de `IParqueDbContext` y `PanelRepository`.
  - [ ] 2.2 Implementar la consulta por nodo, ordenada por nombre, y la consulta de detalle por `Id` y `NodoId`, proyectando solo los campos necesarios.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear `PanelListadoDto` (`Id`, `Nombre`, `Estado`, `Potencia`) y `PanelDetalleDto` (`Id`, `Nombre`, `Estado`, `Potencia`, `VidaUtil`, `FechaCreacion`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Declarar en `IPanelService` los métodos `ObtenerPanelesPorNodoAsync(int nodoId, int parqueSolarId)` y `ObtenerPanelAsync(int id, int nodoId, int parqueSolarId)`.
  - [ ] 3.3 Implementar ambos métodos en `PanelService`, con `IPanelRepository` e `INodoRepository` inyectados por constructor.
  - [ ] 3.4 Implementar las lecturas cargando antes el nodo del parque, sin modificar la entidad. El detalle lanza excepción si el panel no existe en ese nodo.
  - [ ] 3.5 Crear las pruebas unitarias del servicio para listado con datos, listado vacío, detalle exitoso, panel inexistente en el nodo y nodo inválido.
  - [ ] 3.6 Preparar el código para que quede conectado con la implementación de Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear `Paneles.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles"`.
  - [ ] 4.2 Crear `DetallePanel.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{Id:int}"`.
  - [ ] 4.3 Proteger ambas páginas con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.4 Configurar el `RadzenDataGrid<PanelListadoDto>` (Nombre, Estado y Potencia) y el `RadzenButton` "Registrar panel". En el detalle, mostrar en solo lectura Nombre, Estado, Potencia, Vida útil y Fecha de creación.
  - [ ] 4.5 Cargar el listado y el detalle en `OnInitializedAsync`. Si la consulta falla, notificar con `NotificationService` de Radzen.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
