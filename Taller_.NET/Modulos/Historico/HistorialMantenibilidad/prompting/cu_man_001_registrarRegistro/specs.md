# Especificaciones Técnicas: Alta Registro de Mantenibilidad

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.HistorialMantenibilidad).
- **Patrones:** Application Services. El módulo no referencia a Panel ni a Usuario. Cada relación externa usa un mediador limitado a dos módulos: `IMediadorHistorialPanel` entre HistorialMantenibilidad y Panel, e `IMediadorHistorialUsuario` entre HistorialMantenibilidad y Usuario.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.HistorialMantenibilidad.Domain)
- **Entidades:**
    - **RegistroMantenibilidad**
        - **Propiedades:**
            - `Id` (int, PK)
            - `Mantenedor` (objeto de valor propio del módulo, no la entidad `Usuario`)
            - `Fecha` (DateTime)
            - `Observaciones` (string, max 500 caracteres)
            - `EstadoInicial` (EstadoPanelRegistrado)
            - `EstadoResultante` (EstadoPanelRegistrado)
            - `ContextoPanel` (objeto de valor propio del módulo, no la entidad `Panel`)
    - **EstadoPanelRegistrado**
        - Valores: `Sucio`, `Roto`, `Encendido`, `Deteriorado`, `Apagado`.
    - **Mantenedor**
        - **Propiedades:** `IdMantenedor` (int).
    - **ContextoPanel**
        - **Propiedades:** `PanelId` (int), `NodoId` (int), `ParqueSolarId` (int).
- **Comportamiento:**
    - Constructor que exige mantenedor, fecha, observaciones, estado inicial, estado resultante y contexto del panel.
    - Valida que las observaciones no sean vacías, que la fecha no sea posterior al día de la carga y que ambos estados pertenezcan al enum.
    - El constructor no modifica el panel. Solo deja armado el registro.

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `CrearRegistroMantenibilidadDto`
  - Propiedades: `IdMantenedor`, `Fecha`, `EstadoResultante`, `Observaciones`, `PanelId`, `NodoId`, `ParqueSolarId`.
- **Validador:** `CrearRegistroMantenibilidadDtoValidator` (usando FluentValidation)
  - Reglas: `IdMantenedor`, `PanelId`, `NodoId` y `ParqueSolarId` requeridos; `Fecha` requerida y no posterior a hoy; `EstadoResultante` requerido y dentro del enum; `Observaciones` requeridas (max 500).
- **Servicio:** `RegistroMantenibilidadService` (implementa la interfaz `IRegistroMantenibilidadService`)
  - Dependencias: `IRegistroMantenibilidadRepository`, `IMediadorHistorialPanel` e `IMediadorHistorialUsuario`, inyectadas por constructor.
  - Método: `RegistrarRegistroAsync(CrearRegistroMantenibilidadDto dto)`
  - Lógica:
    1. Consultar al mediador Historial–Panel para confirmar que el panel existe en el nodo y en el parque seleccionados. Si no, lanzar excepción.
    2. Obtener por ese mismo mediador el estado actual del panel y usarlo como estado inicial.
    3. Consultar al mediador Historial–Usuario para confirmar que el usuario existe y tiene el rol "Mantenimiento". Si no, lanzar excepción.
    4. Instanciar `RegistroMantenibilidad` con el mantenedor, la fecha, las observaciones, el estado inicial, el estado resultante y el contexto del panel.
    5. Persistir el registro mediante el repositorio y retornar el Id generado.
- **Mediadores:** Ninguna capa de HistorialMantenibilidad referencia tipos de Panel ni de Usuario. El estado del panel se recibe como valor del enum propio `EstadoPanelRegistrado`.

### 3. Capa Infrastructure (Persistencia)
- **Repositorio:** `RegistroMantenibilidadRepository` implementa `IRegistroMantenibilidadRepository`.
- **Configuración de Entity Framework:**
- `RegistroMantenibilidadConfiguration : IEntityTypeConfiguration<RegistroMantenibilidad>`
  - Definir la tabla y la longitud máxima de `Observaciones`.
  - Configurar `Id` como clave primaria de tipo int.
  - Persistir `EstadoInicial` y `EstadoResultante` como string.
  - Mapear `Mantenedor` como columna `IdMantenedor`.
  - Mapear `ContextoPanel` como columnas `PanelId`, `NodoId` y `ParqueSolarId`, sin navegación hacia entidades de otros módulos.
  - Crear un índice por `PanelId` y `Fecha` para consultar el historial del panel.
- **DbContext:** Exponer en `IHistorialMantenibilidadDbContext` el conjunto:
    - `DbSet<RegistroMantenibilidad> Registros { get; set; }`
- **Migraciones:** Generar y aplicar una migración para reflejar la tabla de registros.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `AltaRegistroMantenibilidad.razor`
  - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{PanelId:int}/mantenibilidad/alta"`
  - **Seguridad:** Utilizar `<AuthorizeView Roles="Mantenimiento">` o el atributo `[Authorize(Roles = "Mantenimiento")]`.
  - **UI (Radzen):**
    - Utilizar `RadzenTemplateForm` vinculado al modelo `CrearRegistroMantenibilidadDto`.
    - Campos:
        - `RadzenDropDown` para el usuario de mantenimiento, con los usuarios de rol "Mantenimiento".
        - `RadzenDatePicker` para la fecha.
        - `RadzenDropDown` para el estado resultante, con los valores de `EstadoPanelRegistrado`.
        - `RadzenTextArea` para las observaciones.
    - El parque, el nodo y el panel se muestran como contexto de solo lectura, tomados de la ruta.
  - **Flujo:**
    1. En `OnValidSubmit`, el componente inyecta `IRegistroMantenibilidadService` y ejecuta `RegistrarRegistroAsync(dto)` con los identificadores de la ruta.
    2. Tras el éxito, muestra una notificación con `NotificationService` de Radzen y redirige al historial del panel con `NavigationManager`.
