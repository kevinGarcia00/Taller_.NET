# Especificaciones Técnicas: Eliminar Panel

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Panel).
- **Patrones:** Application Services. Se reutilizan la entidad, el repositorio y el mediador Panel–Nodo. Panel no referencia al módulo Nodo.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Panel.Domain)
- **Entidades:**
    - Reutilizar `Panel`, `EstadoPanel` y `ContextoNodo`.
- **Comportamiento:**
    - La eliminación no agrega comportamiento de negocio sobre la entidad. La regla de pertenencia al nodo se resuelve al cargar el panel por `Id` y `NodoId`.

### 2. Capa Application (Servicios)
- **Parámetros de entrada:** `int id`, `int nodoId`, `int parqueSolarId`.
- **Servicio:** `PanelService` (implementa la interfaz `IPanelService`)
  - Dependencias: `IPanelRepository` y `IMediadorPanelNodo`.
  - Método: `EliminarPanelAsync(int id, int nodoId, int parqueSolarId)`
  - Lógica:
    1. Confirmar mediante el mediador que el nodo existe y pertenece al parque seleccionado. Si no, lanzar excepción.
    2. Obtener el panel por `Id` dentro de ese nodo. Si no existe, lanzar excepción.
    3. Remover el panel mediante el repositorio y confirmar la persistencia.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Reutilizar `PanelConfiguration`. El panel no mantiene colecciones propias que exijan un borrado en cascada adicional.
- **DbContext:** Reutilizar `DbSet<Panel> Paneles` en `IPanelDbContext`.
- **Repositorio:** `PanelRepository` expone la baja del panel ya cargado y la confirmación de cambios.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `Paneles.razor`
  - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles"`
  - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Grilla `RadzenDataGrid` con los paneles del nodo.
    - En la columna de acciones, un `RadzenButton` con icono de papelera.
    - `DialogService` para el modal de confirmación.
  - **Flujo:**
    1. El Administrador pulsa eliminar en una fila.
    2. `DialogService` pide confirmación mostrando el nombre del panel.
    3. Si confirma, se invoca `IPanelService.EliminarPanelAsync(id, nodoId, parqueSolarId)` con los identificadores de la ruta.
    4. Tras el éxito, `NotificationService` informa el resultado y se recarga la grilla.
    5. Si la operación falla, se captura la excepción y se muestra una notificación de advertencia.
