# Especificaciones Técnicas: Listar Registros de Mantenibilidad

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.HistorialMantenibilidad).
- **Patrones:** Application Services. La lectura reutiliza `RegistroMantenibilidad` y `IRegistroMantenibilidadRepository`. La comprobación del panel usa `IMediadorHistorialPanel`, sin referencia directa al módulo Panel.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.HistorialMantenibilidad.Domain)
- **Entidades:**
    - Reutilizar `RegistroMantenibilidad`, `EstadoPanelRegistrado`, `Mantenedor` y `ContextoPanel`.
- **Comportamiento:**
    - La consulta expone los datos ya definidos en el alta. Este caso de uso no agrega métodos que cambien el registro ni el panel.

### 2. Capa Application (Servicios)
- **DTO de salida:** `RegistroMantenibilidadListadoDto`
  - Propiedades: `Id` (int), `IdMantenedor` (int), `Fecha` (DateTime), `Observaciones` (string), `EstadoInicial` (string), `EstadoResultante` (string).
- **Servicio:** `RegistroMantenibilidadService` (implementa la interfaz `IRegistroMantenibilidadService`)
  - Dependencias: `IRegistroMantenibilidadRepository` e `IMediadorHistorialPanel`.
  - Método: `ObtenerRegistrosPorPanelAsync(int panelId, int nodoId, int parqueSolarId)`
  - Lógica:
    1. Confirmar mediante el mediador Historial–Panel que el panel existe en el nodo y en el parque seleccionados. Si no, lanzar excepción.
    2. Obtener los registros de ese panel, ordenados por fecha descendente.
    3. Proyectar cada entidad a `RegistroMantenibilidadListadoDto` y retornar la lista sin modificarla.
    4. Si no hay registros, retornar una lista vacía.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Reutilizar `RegistroMantenibilidadConfiguration` y `DbSet<RegistroMantenibilidad> Registros`.
- **Repositorio:**
    - La consulta filtra por `PanelId` y ordena por `Fecha` descendente.
    - Proyecta solo `Id`, `IdMantenedor`, `Fecha`, `Observaciones`, `EstadoInicial` y `EstadoResultante`.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `HistorialMantenibilidad.razor`
  - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{PanelId:int}/mantenibilidad"`
  - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Contexto de parque, nodo y panel en solo lectura, tomados de la ruta.
    - `RadzenDataGrid<RegistroMantenibilidadListadoDto>` con columnas Id del mantenedor, Fecha, Observaciones, Estado inicial y Estado resultante.
    - Mensaje de listado vacío cuando el panel no tiene registros.
    - `RadzenButton` para volver a la ficha del panel.
  - **Flujo:**
    1. El acceso llega desde la acción "Ver historial" del panel seleccionado.
    2. En `OnInitializedAsync`, el componente inyecta `IRegistroMantenibilidadService` e invoca `ObtenerRegistrosPorPanelAsync(PanelId, NodoId, ParqueId)`.
    3. Los datos devueltos se asignan a la grilla.
    4. Si la consulta falla, `NotificationService` muestra el error.
