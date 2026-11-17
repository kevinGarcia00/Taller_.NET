# Especificaciones Técnicas: Ver Histórico del Panel

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Historico).
- **Patrones:** Application Services. La lectura reutiliza `Historico`, `ContextoPanel` e `IHistoricoRepository`.
- **Comunicación entre módulos:** Historico no referencia tipos de ParqueSolar. La comprobación del panel se realiza mediante el mediador `IMediadorHistoricoParqueSolar` (canal único entre Historico y ParqueSolar).
- **Auth0:** el actor es el Administrador. La pantalla se autoriza por rol "Administrador", resuelto desde Auth0.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Historico.Domain)
- **Entidades:**
    - Reutilizar `Historico` y `ContextoPanel`.
- **Comportamiento:**
    - La consulta expone los datos ya definidos en el alta. Este caso de uso no agrega métodos que modifiquen el registro ni el panel.

### 2. Capa Application (Servicios)
- **DTO de salida:** `HistoricoListadoDto`
    - Propiedades: `Id` (int), `Fecha` (DateTime), `EnergiaTotal` (decimal).
- **Puerto (mediador):** `IMediadorHistoricoParqueSolar`
    - `Task<bool> ExistePanelAsync(int panelId, int nodoId, int parqueSolarId, CancellationToken cancellationToken = default)`.
- **Servicio:** `HistoricoService` (implementa la interfaz `IHistoricoService`)
    - Dependencias: `IHistoricoRepository` e `IMediadorHistoricoParqueSolar`, inyectadas por constructor.
    - Método: `ObtenerRegistrosPorPanelAsync(int panelId, int nodoId, int parqueSolarId, CancellationToken cancellationToken = default)`
    - Lógica:
        1. Confirmar mediante `IMediadorHistoricoParqueSolar` que el panel existe en el nodo y en el parque seleccionados. Si no, lanzar `PanelNoEncontradoException`.
        2. Obtener los registros de ese panel, ordenados por `Fecha` descendente.
        3. Proyectar cada entidad a `HistoricoListadoDto` y retornar la lista sin modificarla.
        4. Si no hay registros, retornar una lista vacía.
- **Seguridad:** la autorización por rol "Administrador" se aplica en la capa de presentación (Auth0).

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Reutilizar `HistoricoConfiguration` y `DbSet<Historico> Registros` en `IHistoricoDbContext`.
- **Repositorio:** `IHistoricoRepository`
    - La consulta filtra por `PanelId`, ordena por `Fecha` descendente y proyecta solo `Id`, `Fecha` y `EnergiaTotal`.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Punto de entrada (apartado de panel):** en la ficha del panel (`DetallePanel.razor`) se agrega el botón **"Ver histórico"**, visible para el rol "Administrador". Al presionarlo se abre la tabla de registros del panel.
- **Componente:** `HistorialPanel.razor`
    - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{PanelId:int}/historico"`
    - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`. Si el usuario no tiene el rol, se muestra el bloque `NotAuthorized`.
    - **UI (Radzen):**
        - Contexto de parque, nodo y panel en solo lectura, tomados de la ruta.
        - `RadzenDataGrid<HistoricoListadoDto>` con las columnas `Id`, `Fecha` y `EnergiaTotal`. La `Fecha` se muestra en la hora local del sistema (se persiste en UTC y se convierte al mostrar).
        - Mensaje de listado vacío cuando el panel no tiene registros.
        - `RadzenButton` para volver a la ficha del panel.
    - **Flujo:**
        1. El acceso llega desde la acción "Ver histórico" del panel seleccionado.
        2. En `OnInitializedAsync`, el componente inyecta `IHistoricoService` e invoca `ObtenerRegistrosPorPanelAsync(PanelId, NodoId, ParqueId)`.
        3. Los datos devueltos se asignan a la grilla.
        4. Si la consulta falla, `NotificationService` muestra el error.
