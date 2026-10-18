# Especificaciones Técnicas: Ver Paneles

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Panel).
- **Patrones:** Application Services. La lectura reutiliza `Panel` y `IPanelRepository`. La comprobación del nodo usa `IMediadorPanelNodo`, sin referencia directa al módulo Nodo.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Panel.Domain)
- **Entidades:**
    - Reutilizar `Panel`, `EstadoPanel` y `ContextoNodo`.
- **Comportamiento:**
    - La consulta expone los datos ya definidos en la entidad. Este caso de uso no agrega métodos que cambien el estado del panel.

### 2. Capa Application (Servicios)
- **DTO de salida:** `PanelDetalleDto`
  - Propiedades: `Id` (int), `Nombre` (string), `Estado` (string), `Potencia` (decimal), `VidaUtil` (string), `FechaCreacion` (DateTime), `NodoId` (int), `ParqueSolarId` (int).
- **DTO de listado:** `PanelListadoDto`
  - Propiedades: `Id` (int), `Nombre` (string), `Estado` (string), `Potencia` (decimal).
- **Servicio:** `PanelService` (implementa la interfaz `IPanelService`)
  - Dependencias: `IPanelRepository` y `IMediadorPanelNodo`.
  - Métodos:
    - `ObtenerPanelesPorNodoAsync(int nodoId, int parqueSolarId)`
    - `ObtenerPanelAsync(int id, int nodoId, int parqueSolarId)`
  - Lógica:
    1. Confirmar mediante el mediador que el nodo existe y pertenece al parque seleccionado.
    2. En el listado, obtener los paneles de ese nodo y proyectarlos a `PanelListadoDto`.
    3. En el detalle, obtener el panel por `Id` y `NodoId`. Si no existe, lanzar excepción.
    4. Proyectar la entidad a `PanelDetalleDto` y retornarla sin modificarla.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Reutilizar `PanelConfiguration` y `DbSet<Panel> Paneles`.
- **Repositorio:**
    - La consulta del listado filtra por `NodoId` y ordena por `Nombre`.
    - La consulta del detalle filtra por `Id` y `NodoId`, y proyecta solo los campos necesarios.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente de listado:** `Paneles.razor`
  - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles"`
  - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Barra superior con un `RadzenButton` ("Registrar panel") hacia `/parques/{ParqueId}/nodos/{NodoId}/paneles/alta`.
    - `RadzenDataGrid<PanelListadoDto>` con columnas Nombre, Estado y Potencia.
    - Columna de acciones con botones para ver, editar y eliminar.
- **Componente de detalle:** `DetallePanel.razor`
  - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{Id:int}"`
  - **UI (Radzen):**
    - Tarjeta o formulario de solo lectura con Nombre, Estado, Potencia, Vida útil y Fecha de creación.
    - Contexto de parque y nodo en solo lectura.
    - `RadzenButton` para volver al listado del nodo.
  - **Flujo:**
    1. `Paneles.razor` llama a `ObtenerPanelesPorNodoAsync` en `OnInitializedAsync`.
    2. Al elegir un panel, `NavigationManager` abre `DetallePanel.razor`.
    3. `DetallePanel.razor` llama a `ObtenerPanelAsync` y muestra los datos.
    4. Si la consulta falla, `NotificationService` muestra el error.
