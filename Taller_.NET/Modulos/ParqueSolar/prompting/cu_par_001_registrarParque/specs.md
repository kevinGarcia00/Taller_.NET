# Especificaciones Técnicas: Registrar Panel

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Panel).
- **Patrones:** Application Services. El módulo Panel no referencia a Nodo ni a ParqueSolar. La comprobación del nodo seleccionado se hace por un mediador exclusivo entre Panel y Nodo.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Panel.Domain)
- **Entidades:**
    - **Panel**
        - **Propiedades:**
            - `Id` (int, PK)
            - `Nombre` (string, max 100 caracteres)
            - `Ubicacion` (String, max 100 caracteres)
            - `FechaCreacion` (date)
            - `CantMaxNodos` (bigint)
- **Comportamiento:**
    - Constructor que exige nombre, ubicacion, fechaCreacion y cantMaxNodos, asigna `FechaCreacion` en el constructor.

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `CrearParque`
    - Propiedades: `Nombre`, `Ubicacion`, `CantMaxNodos`.
- **Validador:** `CrearParqueValidation` (usando FluentValidation)
    - Reglas: `Nombre` requerido (max 100), `Ubicacion` requerido  (max 100), `CantMaxNodos` requerido.
- **Servicio:** `ParqueService` (implementa la interfaz `IParqueService`)
    - Dependencias: `IParqueRepository` y `IMediadorParqueNodo`, inyectadas por constructor.
    - Método: `CrearParqueAsync(CrearParqueDto dto)`
    - Lógica:
        1. Validar que no exista otro panel con el mismo nombre en ese nodo.
        2. Instanciar `ParqueSolar`, asignar `FechaCreacion` y persistir.
        3. Retornar el Id generado.
- **Mediador:** `IMediadorParqueNodo` es el único canal entre Parque y Nodo. Ninguna capa de Parque referencia tipos del módulo Nodo.

### 3. Capa Infrastructure (Persistencia)
- **Repositorio:** `ParqueRepository` implementa `IParqueRepostory`.
- **Configuración de Entity Framework:**
- `ParqueConfiguration : IEntityTypeConfiguration<Panel>`
    - Definir la tabla y la longitud máxima de columnas.
    - Configurar `Id` como clave primaria de tipo int.
- **DbContext:** Exponer en `IPanelDbContext` el conjunto:
    - `DbSet<Panel> Paneles { get; set; }`
- **Migraciones:** Generar y aplicar una migración para reflejar la tabla de parques.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `AltaPanel.razor`
    - **Ruta:** `@page "/parques/alta"`
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
