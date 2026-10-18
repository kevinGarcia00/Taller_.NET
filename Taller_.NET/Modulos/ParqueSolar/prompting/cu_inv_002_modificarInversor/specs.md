# Especificaciones Técnicas: Modificar Inversor

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Inversor).
- **Patrones:** Application Services. Se reutilizan la entidad, el repositorio y el mediador `IMediadorNodoInversor` definidos en el alta.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Inversor.Domain)
- **Comportamiento para modificación:**
    - Método de dominio `ActualizarNombre(string nombre)`, con la misma validación del constructor: el nombre no puede ser nulo ni espacios en blanco.
    - `Id` y `FechaCreacion` no se modifican en este método.

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `ModificarInversorDto`
  - Propiedades: `Id` (int), `Nombre`, `NodoId`, `ParqueSolarId`.
- **Validador:** `ModificarInversorDtoValidator` (usando FluentValidation)
  - Reglas: `Id`, `NodoId` y `ParqueSolarId` requeridos; `Nombre` requerido (max 100).
- **Servicio:** `InversorService` (implementa la interfaz `IInversorService`)
  - Métodos:
    - `Task<ModificarInversorDto> ObtenerParaEdicionAsync(int nodoId, int parqueSolarId)`
    - `Task ModificarInversorAsync(ModificarInversorDto dto)`
  - Lógica de `ModificarInversorAsync(dto)`:
    1. Confirmar mediante `IMediadorNodoInversor` que el nodo existe y pertenece al parque seleccionado.
    2. Obtener por el mediador el `InversorId` de ese nodo. Si el nodo no tiene inversor, o el id no coincide con el del DTO, lanzar excepción.
    3. Cargar el inversor por ese `Id`. Si no existe, lanzar excepción.
    4. Invocar `ActualizarNombre` sobre la entidad.
    5. Persistir con el repositorio.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Se reutilizan `InversorConfiguration`, `IInversorDbContext` e `InversorRepository` creados en el alta.
- **Consulta de modificación:**
    - La lectura para edición obtiene el inversor por el `Id` que el mediador indica para ese nodo. No se carga un inversor ajeno al nodo seleccionado.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `EditarInversor.razor`
  - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/inversor/editar"`
  - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Formulario `RadzenTemplateForm` vinculado a `ModificarInversorDto`.
    - Campo editable: `RadzenTextBox` para Nombre.
    - Parque, nodo, identificador y fecha de creación se muestran como solo lectura.
    - Botón de envío: `RadzenButton` de tipo submit.
    - Botón de cancelación: `RadzenButton` para volver a la vista del inversor del nodo.
  - **Flujo:**
    1. Carga inicial (`OnInitializedAsync` / `OnParametersSetAsync`): tomar `NodoId` y `ParqueId` de la ruta e invocar `ObtenerParaEdicionAsync(NodoId, ParqueId)`.
    2. En `OnValidSubmit`, invocar `ModificarInversorAsync(dto)`.
    3. Tras el éxito, notificar con `NotificationService` y redirigir a `/parques/{ParqueId}/nodos/{NodoId}/inversor`.
