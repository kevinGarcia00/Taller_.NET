# Especificaciones Técnicas: Registrar Panel

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.ParqueSolar).
- **Patrones:** Application Services. Panel, Nodo y Parque pertenecen al mismo módulo. El panel referencia la entidad `Nodo` de forma directa. No hay mediador interno. La persistencia usa el `IParqueDbContext` del módulo.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.ParqueSolar.Domain)
- **Entidades:**
    - **Panel**
        - **Propiedades:**
            - `Id` (int, PK)
            - `Nombre` (string, max 100 caracteres)
            - `FechaCreacion` (DateTime)
            - `Estado` (EstadoPanel)
            - `Potencia` (decimal)
            - `VidaUtil` (string, max 50 caracteres)
            - `Nodo` (entidad del mismo módulo)
    - **EstadoPanel**
        - Valores: `Sucio`, `Roto`, `Encendido`, `Deteriorado`, `Apagado`.
- **Comportamiento:**
    - Constructor que exige nombre, estado, potencia y vida útil, asigna `FechaCreacion` y valida que la potencia sea mayor a cero y que el nombre no sea vacío.
    - Método de dominio `AsignarNodo(Nodo nodo)` para vincular el panel al nodo ya seleccionado.

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `CrearPanelDto`
  - Propiedades: `Nombre`, `Estado`, `Potencia`, `VidaUtil`, `NodoId`, `ParqueSolarId`.
- **Validador:** `CrearPanelDtoValidator` (usando FluentValidation)
  - Reglas: `Nombre` requerido (max 100), `Estado` requerido y dentro del enum, `Potencia` requerida y mayor a cero, `VidaUtil` requerida (max 50), `NodoId` y `ParqueSolarId` requeridos.
- **Servicio:** `PanelService` (implementa la interfaz `IPanelService`)
  - Dependencias: `IPanelRepository` e `INodoRepository`, inyectadas por constructor.
  - Método: `CrearPanelAsync(CrearPanelDto dto)`
  - Lógica:
    1. Cargar el nodo y comprobar que pertenece al parque seleccionado. Si no, lanzar excepción.
    2. Validar que no exista otro panel con el mismo nombre en ese nodo.
    3. Instanciar `Panel`, asignar el `Nodo` y persistir mediante el repositorio.
    4. Retornar el Id generado.

### 3. Capa Infrastructure (Persistencia)
- **Repositorio:** `PanelRepository` implementa `IPanelRepository`.
- **Configuración de Entity Framework:**
- `PanelConfiguration : IEntityTypeConfiguration<Panel>`
  - Definir la tabla y la longitud máxima de columnas.
  - Configurar `Id` como clave primaria de tipo int.
  - Persistir `EstadoPanel` como string.
  - Configurar la relación con `Nodo` y el índice único compuesto por `Nombre` y `NodoId`.
- **DbContext:** Exponer en `IParqueDbContext` el conjunto:
    - `DbSet<Panel> Paneles { get; set; }`
- **Migraciones:** Generar y aplicar una migración del módulo ParqueSolar para reflejar la tabla de paneles.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `AltaPanel.razor`
  - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/alta"`
  - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Utilizar `RadzenTemplateForm` vinculado al modelo `CrearPanelDto`.
    - Campos:
        - `RadzenTextBox` para Nombre y Vida útil.
        - `RadzenNumeric` para Potencia.
        - `RadzenDropDown` para Estado, con los valores de `EstadoPanel`.
    - El parque y el nodo se muestran como contexto de solo lectura, tomados de la ruta.
    - Botón de envío: `RadzenButton` de tipo submit.
  - **Flujo:**
    1. En `OnValidSubmit`, el componente inyecta `IPanelService` y ejecuta `CrearPanelAsync(dto)` con el `NodoId` y el `ParqueSolarId` de la ruta.
    2. Tras el éxito, muestra una notificación con `NotificationService` de Radzen y redirige al listado de paneles del nodo con `NavigationManager`.
