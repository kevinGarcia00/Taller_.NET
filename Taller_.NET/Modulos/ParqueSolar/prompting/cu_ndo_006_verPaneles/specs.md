# Especificaciones Técnicas: Ver Paneles

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (`Modules.Nodo`).
- **Patrones:** Application Services / Mediadores entre módulos (o CQRS mediante consultas).

## Detalles de Implementación por Capa

### 1. Capa Domain (`Modules.Nodo.Domain`)
- **Entidades:**
  - **Nodo:**
    - Propiedades involucradas: `Id` (`long`), `Nombre`, `UbicacionParque`, `FechaCreacion`, `EnergiaGenerada`, `ContextoParque` o `ParqueSolarId`.
- **Relación conceptual:**
  - El nodo tiene un contexto de pertenencia al parque solar y puede estar asociado a múltiples paneles, pero esa entidad `Panel` no se modela ni se importa dentro del dominio de `Nodo`.
  - La relación con paneles se trata como un dato de contexto / consulta intermodular y no como una dependencia directa de dominio.
- **Comportamiento:**
  - La entidad `Nodo` no modifica su estado en esta operación.
  - La consulta expone solo datos de lectura para la visualización de paneles asociados.

### 2. Capa Application (Servicios)
- **DTO de Salida:** `PanelListadoDto`
  - Propiedades: `Id` (`long`), `Nombre` (`string`), `EstadoPanel` (`string` o `EstadoPanel`), `Potencia` (`decimal`), `VidaUtil` (`string`), `FechaCreacion` (`DateTime`), `NodoId` (`long`), `ParqueSolarId` (`long`).
- **Servicio:** `NodoService` (implementa la interfaz `INodoService`)
  - Dependencias: `INodoRepository`, `IMediadorNodoParque`, `IMediadorNodoPanel`.
  - Método: `ObtenerPanelesPorNodoAsync(long nodoId, long parqueSolarId)`
  - **Lógica:**
    1. Validar que el nodo exista.
    2. Verificar que el nodo pertenezca al parque solar indicado.
    3. Consultar los paneles asociados exclusivamente al nodo seleccionado.
    4. Mapear la entidad o proyección a `PanelListadoDto`.
    5. Retornar la lista completa o vacía si no hay registros.

- **Mediadores involucrados:**
  - `IMediadorNodoParque`: canal exclusivo para validar el contexto del parque solar.
  - `IMediadorNodoPanel`: canal exclusivo para consultar paneles del módulo Panel sin romper el aislamiento entre módulos.

### 3. Capa Infrastructure (Persistencia y Comunicación Inter-Módulo)
- **Integración inter-módulo:** `IMediadorNodoPanel` y su implementación `MediadorNodoPanel`.
  - El módulo `Nodo` no conoce la implementación concreta del módulo `Panel`; solo usa el contrato del mediador.
  - La implementación del mediador es la responsable de consultar los paneles asociados al nodo y devolverlos ya proyectados hacia `PanelListadoDto`.
  - La consulta de solo lectura debe filtrar por `NodoId` y `ParqueSolarId`.
- **Persistencia del módulo Panel:**
  - El acceso real a los datos se resuelve dentro de la infraestructura del módulo `Panel` mediante su repositorio y entidades.
  - La relación entre `Nodo` y `Panel` se mantiene mediante la clave foránea `NodoId` o contexto equivalente del dominio, garantizando que la consulta siempre se ejecute bajo el nodo y el parque seleccionados.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `VerPaneles.razor`
- **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles"`
- **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
- **UI (Radzen):**
  - `RadzenDataGrid<PanelListadoDto>`
  - Columnas: `Nombre`, `EstadoPanel`, `Potencia`, `VidaUtil`, `FechaCreacion`.
  - `EmptyTemplate` para mostrar el mensaje: “Este nodo no tiene paneles asociados.”
  - Botón para volver al listado.
- **Flujo:**
  1. El componente recibe `ParqueId` y `NodoId` desde la URL.
  2. En `OnParametersSetAsync` o `OnInitializedAsync`, invoca `ObtenerPanelesPorNodoAsync(nodoId, parqueId)`.
  3. Si la consulta devuelve datos, se cargan en la grilla.
  4. Si devuelve lista vacía, la UI muestra el `EmptyTemplate`.
  5. Si ocurre un error, se muestra una notificación con `NotificationService`.
