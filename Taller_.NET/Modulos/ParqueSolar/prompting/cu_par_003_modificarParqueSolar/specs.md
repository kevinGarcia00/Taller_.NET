# Especificaciones Técnicas: Modificar ParqueSolar

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.ParqueSolar).
- **Patrones:** Application Services. La comprobación del nodo seleccionado se hace por un mediador exclusivo entre Parque y Nodo.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.ParqueSolar.Domain)
- **Entidades:**
    - **Parque**
        - **Propiedades:**
            - `Id` (int, PK)
            - `Nombre` (string, max 100 caracteres)
            - `Ubicacion` (String, max 100 caracteres)
            - `FechaCreacion` (date)
            - `CantMaxNodos` (bigint)

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `ModificarParque`
    - Propiedades: `Nombre`, `Ubicacion`, `CantMaxNodos`.
- **Validador:** `ModificarParqueValidation` (usando FluentValidation)
    - Reglas: `Nombre` requerido (max 100), `Ubicacion` requerido  (max 100), `CantMaxNodos` requerido.
- **Servicio:** `ParqueService` (implementa la interfaz `IParqueService`)
    - Dependencias: `IParqueRepository` y `IMediadorParqueNodo`, inyectadas por constructor.
    - Método: `ModificarParqueAsync(CrearParqueDto dto)`
    - Lógica:
        1. Validar que no exista otro ParqueSolar con el mismo nombre.
        2. Instanciar `ParqueSolar`.
- **Mediador:** `IMediadorParqueNodo` es el único canal entre Parque y Nodo. Ninguna capa de Parque referencia tipos del módulo Nodo.

### 3. Capa Infrastructure (Persistencia)
- **Repositorio:** `ParqueRepository` implementa `IParqueRepostory`.
- **Configuración de Entity Framework:**
- `ParqueConfiguration : IEntityTypeConfiguration<Parque>`
    - Definir la tabla y la longitud máxima de columnas.
    - Configurar `Id` como clave primaria de tipo int.
- **DbContext:** Exponer en `IParqueDbContext` el conjunto:
    - `DbSet<ParqueSolar> Parque { get; set; }`
- **Migraciones:** Generar y aplicar una migración para reflejar la tabla de parques.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `ModificarParque.razor`
    - **Ruta:** `@page "/parques/modificar/{idParque}"`
    - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
    - **UI (Radzen):**
        - Utilizar `RadzenTemplateForm` vinculado al modelo `ModificarParqueDto`.
        - Botón de envío: `RadzenButton` de tipo submit.
    - **Flujo:**
        1. En `OnValidSubmit`, el componente inyecta `IParqueService` y ejecuta `ModificarParqueAsync(dto)`.
        2. Tras el éxito, muestra una notificación con `NotificationService` de Radzen y redirige al listado de parques `NavigationManager`.
