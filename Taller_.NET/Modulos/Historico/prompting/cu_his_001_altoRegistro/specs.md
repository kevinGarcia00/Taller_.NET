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
    - Método: `GenerarRegistrosDiariosAsync(CancellationToken cancellationToken = default)`, retorna la cantidad de registros creados.
    - Lógica:
        1. Obtener los paneles y su energía total mediante `IMediadorHistoricoParqueSolar`.
        2. Para cada panel, verificar con el repositorio que no exista un registro con la misma fecha.
        3. Instanciar `Historico` con la fecha del sistema, la energía y el contexto del panel.
        4. Persistir los registros mediante el repositorio y ejecutar `GuardarCambiosAsync()`.
        5. Retornar la cantidad de registros generados.
    - No hay DTO de entrada ni validador de FluentValidation: el proceso no recibe datos de un usuario.

### 3. Capa Infrastructure (Persistencia y proceso programado)
- **Repositorio:** `HistoricoRepository` implementa `IHistoricoRepository` (`AgregarAsync`, `ExisteRegistroDelPanelEnFechaAsync`, `GuardarCambiosAsync`).
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
    - Usa `PeriodicTimer` con un intervalo de 24 horas.
    - En cada ciclo crea un scope propio e invoca `IHistoricoService.GenerarRegistrosDiariosAsync()`.
    - Registra los errores con `ILogger` sin detener el servicio ni el arranque de la aplicación.
- **Composición del módulo:** `RegistroModuloHistorico.AgregarModuloHistorico(builder.Configuration)` registra `HistoricoDbContext`, `IHistoricoDbContext`, `IHistoricoRepository`, `IHistoricoService`, `IMediadorHistoricoParqueSolar` y el `BackgroundService`. `Program.cs` solo invoca `AgregarModuloHistorico`.
- **Nota de arquitectura:** El adaptador `MediadorHistoricoParqueSolar` se registra en el punto de composición (fuera de `Taller_.NET.Modulos.*`) y es el único lugar que conoce ParqueSolar. De este modo ni Historico depende de ParqueSolar ni ParqueSolar depende de Historico, cumpliendo la regla de módulos validada por ArchUnitNET.

### 4. Capa Presentation (Blazor Server con Radzen)
- **No aplica:** el alta la genera el sistema de forma automática, por lo que no existe pantalla, formulario ni navegación para este caso de uso.
- La consulta o visualización del histórico corresponde a otro caso de uso.
