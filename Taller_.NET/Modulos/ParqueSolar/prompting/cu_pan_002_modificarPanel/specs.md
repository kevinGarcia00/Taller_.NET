# Especificaciones Técnicas: Modificar Panel

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.ParqueSolar).
- **Patrones:** Application Services. Se reutilizan `Panel`, `Nodo` y los repositorios del mismo módulo. No hay mediador entre Panel y Nodo.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.ParqueSolar.Domain)
- **Comportamiento para modificación:**
    - Método de dominio `ActualizarDatos(string nombre, EstadoPanel estado, decimal potencia, string vidaUtil)`, con las mismas validaciones del constructor: nombre no vacío, potencia mayor a cero y vida útil no vacía.
    - `Id`, `FechaCreacion` y `Nodo` no se modifican en este método.

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `ModificarPanelDto`
  - Propiedades: `Id` (int), `Nombre`, `Estado`, `Potencia`, `VidaUtil`, `NodoId`, `ParqueSolarId`.
- **Validador:** `ModificarPanelDtoValidator` (usando FluentValidation)
  - Reglas: `Id`, `NodoId` y `ParqueSolarId` requeridos; `Nombre` requerido (max 100); `Estado` requerido; `Potencia` mayor a cero; `VidaUtil` requerida (max 50).
- **Servicio:** `PanelService` (implementa la interfaz `IPanelService`)
  - Dependencias: `IPanelRepository` e `INodoRepository`.
  - Métodos:
    - `Task<ModificarPanelDto> ObtenerParaEdicionAsync(int id, int nodoId)`
    - `Task ModificarPanelAsync(ModificarPanelDto dto)`
  - Lógica de `ModificarPanelAsync(dto)`:
    1. Cargar el nodo y comprobar que pertenece al parque seleccionado.
    2. Obtener el panel por `Id` dentro de ese nodo. Si no existe, lanzar excepción.
    3. Verificar que el nombre no pertenezca a otro panel del mismo nodo.
    4. Invocar `ActualizarDatos` sobre la entidad.
    5. Persistir con el repositorio.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Se reutilizan `PanelConfiguration`, `IParqueDbContext` y `PanelRepository`.
- **Consulta de modificación:**
    - La lectura para edición filtra por `Id` y `NodoId`, de modo que un panel de otro nodo no pueda cargarse en este contexto.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `EditarPanel.razor`
  - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/editar/{Id:int}"`
  - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Formulario `RadzenTemplateForm` vinculado a `ModificarPanelDto`.
    - Campos editables: `RadzenTextBox` para Nombre y Vida útil, `RadzenNumeric` para Potencia y `RadzenDropDown` para Estado.
    - Parque, nodo, identificador y fecha de creación se muestran como solo lectura.
    - Botón de envío: `RadzenButton` de tipo submit.
    - Botón de cancelación: `RadzenButton` para volver al listado del nodo.
  - **Flujo:**
    1. Carga inicial (`OnInitializedAsync` / `OnParametersSetAsync`): tomar `Id`, `NodoId` y `ParqueId` de la ruta e invocar `ObtenerParaEdicionAsync(Id, NodoId)`.
    2. En `OnValidSubmit`, invocar `ModificarPanelAsync(dto)`.
    3. Tras el éxito, notificar con `NotificationService` y redirigir a `/parques/{ParqueId}/nodos/{NodoId}/paneles`.
