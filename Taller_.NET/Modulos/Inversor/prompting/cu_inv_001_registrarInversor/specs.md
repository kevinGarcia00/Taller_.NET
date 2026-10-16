# Especificaciones Técnicas: Registrar Inversor

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Inversor).
- **Patrones:** Application Services. El módulo Inversor no referencia a Nodo ni a ParqueSolar. La comprobación del nodo y la asociación 1:1 se hacen por el mediador ya definido entre Nodo e Inversor (`IMediadorNodoInversor`).

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Inversor.Domain)
- **Entidades:**
    - **Inversor**
        - **Propiedades:**
            - `Id` (int, PK)
            - `Nombre` (string, max 100 caracteres)
            - `FechaCreacion` (DateTime)
- **Comportamiento:**
    - Constructor que exige `Nombre`, asigna `FechaCreacion` y valida que el nombre no sea nulo ni espacios en blanco.
    - La pertenencia al nodo no se modela como navegación hacia el módulo Nodo. El vínculo 1:1 lo mantiene el nodo mediante `InversorId` y se actualiza solo a través del mediador.

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `CrearInversorDto`
  - Propiedades: `Nombre`, `NodoId`, `ParqueSolarId`.
- **Validador:** `CrearInversorDtoValidator` (usando FluentValidation)
  - Reglas: `Nombre` requerido (max 100), `NodoId` requerido, `ParqueSolarId` requerido.
- **Servicio:** `InversorService` (implementa la interfaz `IInversorService`)
  - Dependencias: `IInversorRepository` e `IMediadorNodoInversor`, inyectadas por constructor.
  - Método: `CrearInversorAsync(CrearInversorDto dto)`
  - Lógica:
    1. Consultar al mediador para confirmar que el nodo existe y pertenece al parque seleccionado. Si no, lanzar excepción.
    2. Consultar al mediador si ese nodo ya tiene un inversor. Si ya tiene uno, lanzar excepción.
    3. Instanciar `Inversor` y persistirlo mediante el repositorio.
    4. Pedir al mediador que asocie el `Id` generado al nodo (`InversorId`), manteniendo la relación 1:1.
    5. Retornar el Id generado.
- **Mediador:** `IMediadorNodoInversor` es el único canal entre Inversor y Nodo. Ninguna capa de Inversor referencia tipos del módulo Nodo ni del módulo ParqueSolar.

### 3. Capa Infrastructure (Persistencia)
- **Repositorio:** `InversorRepository` implementa `IInversorRepository`.
- **Configuración de Entity Framework:**
- `InversorConfiguration : IEntityTypeConfiguration<Inversor>`
  - Definir la tabla y la longitud máxima de `Nombre`.
  - Configurar `Id` como clave primaria de tipo int.
  - No mapear claves foráneas ni navegaciones hacia Nodo o ParqueSolar.
- **DbContext:** Exponer en `IInversorDbContext` el conjunto:
    - `DbSet<Inversor> Inversores { get; set; }`
- **Migraciones:** Generar y aplicar una migración para reflejar la tabla de inversores.

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
