# Especificaciones Técnicas: Filtrar Histórico del Panel

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Historico).
- **Patrones:** Application Services. La lectura reutiliza `Historico`, `ContextoPanel` e `IHistoricoRepository` para filtrar los registros de un panel por una fecha específica.
- **Comunicación entre módulos:** Historico no referencia tipos de ParqueSolar. La comprobación del panel se realiza mediante el mediador `IMediadorHistoricoParqueSolar` (canal único entre Historico y ParqueSolar).
- **Auth0:** el actor es el Administrador. La pantalla se autoriza por rol "Administrador", resuelto desde Auth0.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Historico.Domain)
- **Entidades y objetos de valor:**
    - Reutilizar `Historico` y `ContextoPanel` sin agregar nuevos tipos: el filtro por fecha no requiere comportamiento de dominio adicional.
    - La "fecha específica" se interpreta como el día completo (00:00 a 24:00) de la fecha elegida por el usuario.

### 2. Capa Application (Servicios)
- **DTO de entrada:** `FiltroHistoricoDto`
    - Propiedades: `Referencia` (DateTime) — la fecha exacta a buscar.
- **Validador:** `FiltroHistoricoDtoValidator` (usando FluentValidation)
    - Reglas: `Referencia` requerida.
- **DTO de salida:** Reutilizar `HistoricoListadoDto` (`Id`, `Fecha`, `EnergiaTotal`).
- **Puerto (mediador):** `IMediadorHistoricoParqueSolar`
    - `Task<bool> ExistePanelAsync(int panelId, int nodoId, int parqueSolarId, CancellationToken cancellationToken = default)`.
- **Servicio:** `HistoricoService` (implementa la interfaz `IHistoricoService`)
    - Dependencias: `IHistoricoRepository`, `IMediadorHistoricoParqueSolar`, `IValidator<FiltroHistoricoDto>` e `ILogger<HistoricoService>`, inyectadas por constructor.
    - Método: `ObtenerRegistrosFiltradosPorPanelAsync(int panelId, int nodoId, int parqueSolarId, FiltroHistoricoDto filtro, CancellationToken cancellationToken = default)`
    - Lógica:
        1. Validar el DTO con FluentValidation.
        2. Confirmar mediante `IMediadorHistoricoParqueSolar` que el panel existe en el nodo y en el parque seleccionados. Si no, lanzar `PanelNoEncontradoException`.
        3. Obtener los registros de ese panel cuya `Fecha` cae en la fecha de referencia, ordenados por `Fecha` descendente.
        4. Proyectar cada entidad a `HistoricoListadoDto` y retornar la lista sin modificarla.
        5. Si no hay registros en la fecha, retornar una lista vacía.
- **Seguridad:** la autorización por rol "Administrador" se aplica en la capa de presentación (Auth0).

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Reutilizar `HistoricoConfiguration` y `DbSet<Historico> Registros` en `IHistoricoDbContext`.
- **Repositorio:** `IHistoricoRepository`
    - Método `ObtenerPorPanelYFechaAsync(int panelId, DateTime fecha, CancellationToken cancellationToken = default)`.
    - La consulta filtra por `PanelId` y por el día completo de `fecha` (en hora local convertida a UTC), ordena por `Fecha` descendente y proyecta solo `Id`, `Fecha` y `EnergiaTotal`.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Punto de entrada (apartado de panel):** en la ficha del panel (`DetallePanel.razor`) el botón **"Ver histórico"** (visible para el rol "Administrador") lleva a la sección del histórico del panel. El filtro por fecha se integra en esa misma sección, encima del listado.
- **Componente:** `HistorialPanel.razor` (misma sección del listado)
    - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{PanelId:int}/historico"`
    - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`. Si el usuario no tiene el rol, se muestra el bloque `NotAuthorized`.
    - **UI (Radzen):**
        - Contexto de parque, nodo y panel en solo lectura, tomados de la ruta.
        - `RadzenDatePicker` para `Referencia` (fecha exacta a buscar); la búsqueda se resuelve para ese día.
        - `RadzenButton` "Buscar" para aplicar el filtro y `RadzenButton` "Limpiar" para volver al listado completo.
        - `RadzenDataGrid<HistoricoListadoDto>` con las columnas `Id`, `Fecha` y `EnergiaTotal`.
        - Mensaje de listado vacío cuando no hay registros para la fecha seleccionada.
        - `RadzenButton` para volver a la ficha del panel.
    - **Flujo:**
        1. El acceso llega desde la acción "Ver histórico" del panel seleccionado; al entrar se listan todos los registros del panel.
        2. En `OnValidSubmit`, el componente inyecta `IHistoricoService` e invoca `ObtenerRegistrosFiltradosPorPanelAsync(PanelId, NodoId, ParqueId, filtro)`.
        3. Los datos devueltos se asignan a la grilla.
        4. Si la consulta falla, `NotificationService` muestra el error.
