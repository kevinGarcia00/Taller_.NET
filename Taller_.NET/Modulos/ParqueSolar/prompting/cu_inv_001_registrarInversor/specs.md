# Especificaciones Técnicas: Registrar Inversor

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.ParqueSolar).
- **Patrones:** Application Services. Inversor, Nodo y Parque pertenecen al mismo módulo. La relación 1:1 se guarda en `Nodo` como navegación hacia `Inversor`. No hay mediador interno. La persistencia usa `IParqueDbContext`.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.ParqueSolar.Domain)
- **Entidades:**
    - **Inversor**
        - **Propiedades:**
            - `Id` (int, PK)
            - `Nombre` (string, max 100 caracteres)
            - `FechaCreacion` (DateTime)
- **Comportamiento:**
    - Constructor que exige `Nombre`, asigna `FechaCreacion` y valida que el nombre no sea nulo ni espacios en blanco.
    - En `Nodo`, método `AsignarInversor(Inversor inversor)` que rechaza la asignación si el nodo ya tiene un inversor.

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `CrearInversorDto`
  - Propiedades: `Nombre`, `NodoId`, `ParqueSolarId`.
- **Validador:** `CrearInversorDtoValidator` (usando FluentValidation)
  - Reglas: `Nombre` requerido (max 100), `NodoId` requerido, `ParqueSolarId` requerido.
- **Servicio:** `InversorService` (implementa la interfaz `IInversorService`)
  - Dependencias: `IInversorRepository` e `INodoRepository`, inyectadas por constructor.
  - Método: `CrearInversorAsync(CrearInversorDto dto)`
  - Lógica:
    1. Cargar el nodo y comprobar que pertenece al parque seleccionado. Si no, lanzar excepción.
    2. Si ese nodo ya tiene un inversor, lanzar excepción.
    3. Instanciar `Inversor`, persistirlo y asignarlo al nodo.
    4. Retornar el Id generado.

### 3. Capa Infrastructure (Persistencia)
- **Repositorio:** `InversorRepository` implementa `IInversorRepository`.
- **Configuración de Entity Framework:**
- `InversorConfiguration : IEntityTypeConfiguration<Inversor>`
  - Definir la tabla y la longitud máxima de `Nombre`.
  - Configurar `Id` como clave primaria de tipo int.
- En `NodoConfiguration`, configurar la relación 1:1 opcional con `Inversor` y un índice único sobre `InversorId`.
- **DbContext:** Exponer en `IParqueDbContext` el conjunto:
    - `DbSet<Inversor> Inversores { get; set; }`
- **Migraciones:** Generar y aplicar una migración del módulo ParqueSolar para reflejar la tabla de inversores.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `AltaInversor.razor`
  - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/inversor/alta"`
  - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Utilizar `RadzenTemplateForm` vinculado al modelo `CrearInversorDto`.
    - Campos:
        - `RadzenTextBox` para Nombre.
    - El parque y el nodo se muestran como contexto de solo lectura, tomados de la ruta.
    - Botón de envío: `RadzenButton` de tipo submit.
  - **Flujo:**
    1. Si el nodo ya tiene inversor, no se muestra el alta: se informa y se redirige a la vista del inversor del nodo.
    2. En `OnValidSubmit`, el componente inyecta `IInversorService` y ejecuta `CrearInversorAsync(dto)` con el `NodoId` y el `ParqueSolarId` de la ruta.
    3. Tras el éxito, muestra una notificación con `NotificationService` de Radzen y redirige a la vista del inversor del nodo con `NavigationManager`.
