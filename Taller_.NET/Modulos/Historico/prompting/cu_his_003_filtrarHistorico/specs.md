# Especificaciones Técnicas: Filtrar Histórico del Panel

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Historico).
- **Patrones:** Application Services. La lectura reutiliza `Historico`, `ContextoPanel` e `IHistoricoRepository`, y agrega el objeto de valor `PeriodoHistorico` para el filtro.
- **Comunicación entre módulos:** Historico no referencia tipos de ParqueSolar. La comprobación del panel se realiza mediante el mediador `IMediadorHistoricoParqueSolar` (canal único entre Historico y ParqueSolar).
- **Auth0:** el actor es el Administrador. La pantalla se autoriza por rol "Administrador", resuelto desde Auth0.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Historico.Domain)
- **Entidades y objetos de valor:**
    - Reutilizar `Historico` y `ContextoPanel`.
    - **`TipoPeriodoHistorico`** (enum): `Dia = 1`, `Mes = 2`, `Anio = 3`.
    - **`PeriodoHistorico`** (objeto de valor):
        - **Propiedades:** `Tipo` (TipoPeriodoHistorico), `Inicio` (DateTime, inclusive), `Fin` (DateTime, exclusivo).
        - **Comportamiento:** Constructor `PeriodoHistorico(TipoPeriodoHistorico tipo, DateTime referencia)` que calcula el rango:
            - `Dia`: `Inicio` = fecha de referencia (00:00), `Fin` = `Inicio` + 1 día.
            - `Mes`: `Inicio` = primer día del mes, `Fin` = primer día del mes siguiente.
            - `Anio`: `Inicio` = 1 de enero del año, `Fin` = 1 de enero del año siguiente.
        - Método `Contiene(DateTime fecha)` que indica si una fecha cae dentro del rango (`fecha >= Inicio && fecha < Fin`).
        - Valida que `Tipo` sea un valor definido del enum; si no, lanza `HistoricoInvalidoException`.

> **Nota:** `PeriodoHistorico` convierte el filtro (Día/Mes/Año + fecha de referencia) en un rango `[Inicio, Fin)` que el repositorio consulta con `Fecha >= Inicio && Fecha < Fin`; `Contiene(fecha)` valida si una fecha cae dentro.

### 2. Capa Application (Servicios)
- **DTO de entrada:** `FiltroHistoricoDto`
    - Propiedades: `TipoPeriodo` (TipoPeriodoHistorico), `Referencia` (DateTime).
- **Validador:** `FiltroHistoricoDtoValidator` (usando FluentValidation)
    - Reglas: `TipoPeriodo` requerido y dentro del enum; `Referencia` requerida.
- **DTO de salida:** Reutilizar `HistoricoListadoDto` (`Id`, `Fecha`, `EnergiaTotal`).
- **Puerto (mediador):** `IMediadorHistoricoParqueSolar`
    - `Task<bool> ExistePanelAsync(int panelId, int nodoId, int parqueSolarId, CancellationToken cancellationToken = default)`.
- **Servicio:** `HistoricoService` (implementa la interfaz `IHistoricoService`)
    - Dependencias: `IHistoricoRepository`, `IMediadorHistoricoParqueSolar`, `IValidator<FiltroHistoricoDto>` e `ILogger<HistoricoService>`, inyectadas por constructor.
    - Método: `ObtenerRegistrosFiltradosPorPanelAsync(int panelId, int nodoId, int parqueSolarId, FiltroHistoricoDto filtro, CancellationToken cancellationToken = default)`
    - Lógica:
        1. Validar el DTO con FluentValidation.
        2. Confirmar mediante `IMediadorHistoricoParqueSolar` que el panel existe en el nodo y en el parque seleccionados. Si no, lanzar `PanelNoEncontradoException`.
        3. Construir el `PeriodoHistorico` con el tipo y la referencia del filtro.
        4. Obtener los registros de ese panel dentro del período, ordenados por `Fecha` descendente.
        5. Proyectar cada entidad a `HistoricoListadoDto` y retornar la lista sin modificarla.
        6. Si no hay registros en el período, retornar una lista vacía.
- **Seguridad:** la autorización por rol "Administrador" se aplica en la capa de presentación (Auth0).

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Reutilizar `HistoricoConfiguration` y `DbSet<Historico> Registros` en `IHistoricoDbContext`.
- **Repositorio:** `IHistoricoRepository`
    - Método `ObtenerPorPanelYPeriodoAsync(int panelId, PeriodoHistorico periodo, CancellationToken cancellationToken = default)`.
    - La consulta filtra por `PanelId` y por `Fecha >= periodo.Inicio && Fecha < periodo.Fin`, ordena por `Fecha` descendente y proyecta solo `Id`, `Fecha` y `EnergiaTotal`.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `FiltrarHistorico.razor`
    - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{PanelId:int}/historico/filtrar"`
    - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`. Si el usuario no tiene el rol, se muestra el bloque `NotAuthorized`.
    - **UI (Radzen):**
        - Contexto de parque, nodo y panel en solo lectura, tomados de la ruta.
        - `RadzenSelectBar` o `RadzenDropDown` para `TipoPeriodo` (Día / Mes / Año).
        - `RadzenDatePicker` para `Referencia`.
        - `RadzenButton` para aplicar el filtro.
        - `RadzenDataGrid<HistoricoListadoDto>` con las columnas `Id`, `Fecha` y `EnergiaTotal`.
        - Mensaje de listado vacío cuando no hay registros en el período.
        - `RadzenButton` para volver a la ficha del panel.
    - **Flujo:**
        1. El acceso llega desde la acción "Ver histórico" del panel seleccionado.
        2. En `OnValidSubmit`, el componente inyecta `IHistoricoService` e invoca `ObtenerRegistrosFiltradosPorPanelAsync(PanelId, NodoId, ParqueId, filtro)`.
        3. Los datos devueltos se asignan a la grilla.
        4. Si la consulta falla, `NotificationService` muestra el error.
