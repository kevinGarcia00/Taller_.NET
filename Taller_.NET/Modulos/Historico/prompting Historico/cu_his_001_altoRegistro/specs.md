# Especificaciones Técnicas: Alta Registro Histórico

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Historico).
- **Patrones:** Application Services. El disparador del alta es un proceso programado en segundo plano (`BackgroundService` con `PeriodicTimer`).
- **Comunicación entre módulos:** Historico no referencia tipos de ParqueSolar. La lectura de la energía total del panel se realiza mediante el mediador `IMediadorHistoricoParqueSolar` (canal único entre Historico y ParqueSolar).
- **Auth0:** no aplica. El proceso lo ejecuta el sistema, por lo que no hay usuario, rol ni permiso que validar.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Historico.Domain)
- **Entidades:**
    - **Historico**
        - **Propiedades:**
            - `Id` (int, PK)
            - `Fecha` (DateTime)
            - `EnergiaTotal` (decimal)
            - `ContextoPanel` (objeto de valor propio del módulo, no la entidad `Panel`)
    - **ContextoPanel**
        - **Propiedades:** `PanelId` (int), `NodoId` (int), `ParqueSolarId` (int).
- **Excepción:** `HistoricoInvalidoException` (hereda de `System.Exception`).
- **Comportamiento:**
    - Constructor `Historico(DateTime fecha, decimal energiaTotal, ContextoPanel contextoPanel)`.
    - Valida que `EnergiaTotal` no sea negativa, que `Fecha` no sea futura y que `ContextoPanel` no sea nulo.
    - El constructor no modifica el panel: solo deja armado el registro.

### 2. Capa Application (Servicios)
- **Puerto (mediador):** `IMediadorHistoricoParqueSolar`
    - `Task<IReadOnlyList<EnergiaPanelDto>> ObtenerEnergiaTotalPanelesAsync(CancellationToken cancellationToken = default)`.
    - `EnergiaPanelDto` (definido en Historico): `PanelId`, `NodoId`, `ParqueSolarId`, `EnergiaTotal`.
- **Servicio:** `HistoricoService` (implementa la interfaz `IHistoricoService`)
    - Dependencias: `IHistoricoRepository`, `IMediadorHistoricoParqueSolar` e `ILogger<HistoricoService>`, inyectadas por constructor.
    - Método: `GenerarRegistrosDelPeriodoAsync(TimeSpan intervalo, CancellationToken cancellationToken = default)`, retorna la cantidad de registros creados.
    - Lógica:
        1. Obtener los paneles y su energía total mediante `IMediadorHistoricoParqueSolar`.
        2. Calcular el período actual a partir de la hora del sistema y el intervalo recibido (inicio del período y fin del período).
        3. Para cada panel, verificar con el repositorio que no exista un registro dentro del mismo período.
        4. Instanciar `Historico` con la fecha del sistema, la energía y el contexto del panel.
        5. Persistir los registros mediante el repositorio y ejecutar `GuardarCambiosAsync()`.
        6. Retornar la cantidad de registros generados.
    - No hay DTO de entrada ni validador de FluentValidation: el proceso no recibe datos de un usuario.

### 3. Capa Infrastructure (Persistencia y proceso programado)
- **Repositorio:** `HistoricoRepository` implementa `IHistoricoRepository` (`AgregarAsync`, `ExisteRegistroDelPanelEnPeriodoAsync`, `GuardarCambiosAsync`).
- **Configuración de Entity Framework:**
    - `HistoricoConfiguration : IEntityTypeConfiguration<Historico>`
        - Definir la tabla y la precisión decimal de `EnergiaTotal` (por ejemplo, `decimal(18,2)`).
        - Configurar `Id` como clave primaria de tipo int.
        - Mapear `ContextoPanel` como las columnas `PanelId`, `NodoId` y `ParqueSolarId`, sin navegación hacia entidades de otros módulos.
        - Crear un índice por `PanelId` + `Fecha` para consultar el historial del panel.
- **DbContext:** Exponer en `IHistoricoDbContext` el conjunto:
    - `DbSet<Historico> Registros { get; set; }`
- **Migraciones:** Generar y aplicar una migración del módulo Historico (por ejemplo, `AddHistorico`).
- **Proceso programado:** `GeneradorHistoricoBackgroundService : BackgroundService`
    - Usa `PeriodicTimer` con un intervalo configurable (`Historico:IntervaloHoras`, por defecto 24 horas).
    - En cada ciclo crea un scope propio e invoca `IHistoricoService.GenerarRegistrosDelPeriodoAsync()` pasando el intervalo.
    - Registra los errores con `ILogger` sin detener el servicio ni el arranque de la aplicación.
- **Composición del módulo:** `RegistroModuloHistorico.AgregarModuloHistorico(builder.Configuration)` registra `HistoricoDbContext`, `IHistoricoDbContext`, `IHistoricoRepository`, `IHistoricoService`, `IMediadorHistoricoParqueSolar` y el `BackgroundService`. `Program.cs` solo invoca `AgregarModuloHistorico`.
- **Nota de arquitectura:** El adaptador `MediadorHistoricoParqueSolar` se registra en el punto de composición (fuera de `Taller_.NET.Modulos.*`) y es el único lugar que conoce ParqueSolar. De este modo ni Historico depende de ParqueSolar ni ParqueSolar depende de Historico, cumpliendo la regla de módulos validada por ArchUnitNET.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Ubicación de la interfaz (apartado de panel):** la interfaz del histórico vive dentro del apartado del panel. En la ficha del panel (`DetallePanel.razor`, ruta `/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{Id:int}`) se agrega el botón **"Ver histórico"**.
- **Tabla de registros:** al presionar el botón se muestra una tabla (`RadzenDataGrid<HistoricoListadoDto>`) con los registros del panel, columnas `Id`, `Fecha` y `EnergiaTotal`, ordenados del más reciente al más antiguo. La `Fecha` se muestra en la hora local del sistema (se persiste en UTC y se convierte al mostrar).
- **Seguridad:** el botón y la tabla se muestran solo para el rol "Administrador" (`<AuthorizeView Roles="Administrador">`).
- **Nota:** el alta no tiene formulario propio (es automática); este caso de uso no crea pantallas de alta. El detalle de la lectura y del filtrado del listado está en `cu_his_002_verHistorico` y `cu_his_003_filtrarHistorico`.
