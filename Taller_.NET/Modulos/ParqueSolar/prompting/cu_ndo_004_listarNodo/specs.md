# Especificaciones Técnicas: Listar Nodos

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Nodo).
- **Patrones:** Application Services (Consultas / Read Models / CQRS mediante Servicios de Aplicación).

---

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Nodo.Domain)
- **Modelos / Consultas de Dominio:**
  - Las operaciones de lectura no modifican el estado del dominio; utilizan entidades `Nodo` de solo lectura o proyecciones ligeras orientadas a consulta.
  - La consulta obtiene la totalidad de los registros restringidos únicamente por el `ParqueSolarId` del contexto seleccionado.

---

### 2. Capa Application (Servicios)
- **DTO de Salida:** `NodoListadoDto`
  - **Propiedades:** `Id` (`long`), `Nombre` (`string`), `UbicacionParque` (`string`), `FechaCreacion` (`DateTime`), `EnergiaGenerada` (`decimal`), `InversorId` (`long?`), `NombreInversor` (`string?`), `ParqueSolarId` (`long`).

- **Servicio:** `NodoService` (implementa la interfaz `INodoService`)
  - **Dependencias:** `INodoRepository`, `IMediadorNodoParque` e `IMediadorNodoInversor`.
  - **Método:** `ObtenerNodosPorParqueAsync(long parqueSolarId)`
  - **Lógica:**
    1. Consultar a `IMediadorNodoParque` para verificar la existencia del parque solar con el `parqueSolarId` provisto. Si no existe, lanzar excepción de negocio.
    2. Consultar el repositorio `INodoRepository` para obtener la lista completa de nodos pertenecientes al parque solar.
    3. Para los nodos que tengan `InversorId`, consultar a `IMediadorNodoInversor` para obtener los nombres descriptivos de los inversores vinculados y completar la propiedad `NombreInversor`.
    4. Retornar la lista `List<NodoListadoDto>`.


### 3. Capa Infrastructure (Persistencia)
- **Repositorio:** `NodoRepository` (implementa `INodoRepository`).
  - **Método:** `ObtenerPorParqueAsync(long parqueSolarId)`
  - **Lógica de consulta EF Core:**
    - Consultar el `DbSet<Nodo>`.
    - Aplicar filtro único por contexto de parque.
    - Mapear hacia `NodoListadoDto`.
    - Retornar el listado completo con `ToListAsync()`.

- **DbContext:** Exposición de consultas mediante `INodoDbContext`.


### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `ListadoNodos.razor`
- **Ruta:** `@page "/parques/{ParqueId:long}/nodos"`
- **Seguridad:** `<AuthorizeView Roles="Administrador, Operador">` o `[Authorize(Roles = "Administrador, Operador")]`.

- **UI (Radzen):**
  - **Encabezado:** Título del módulo, identificación del Parque Solar y botón `RadzenButton` ("Nuevo Nodo") que redirige a `/parques/{ParqueId}/nodos/alta`.
  - **Grilla de datos:** `RadzenDataGrid<NodoListadoDto>`
    - Propiedades de la grilla: `Data="@nodos"`, `IsLoading="@cargando"`.
    - **Columnas (`RadzenDataGridColumn`):**
      - `Nombre`.
      - `UbicacionParque`.
      - `EnergiaGenerada`.
      - `NombreInversor`: Muestra el nombre del inversor o un `RadzenBadge` con texto "Sin Inversor" si es nulo.
      - `FechaCreacion`.
      - **Columna de Acciones:**
        - Botón `RadzenButton` icono `edit` para modificar el nodo.
        - Botón `RadzenButton` icono `delete` (solo para rol `Administrador`) con diálogo de confirmación mediante `DialogService`.

- **Flujo:**
  1. En `OnParametersSetAsync`, captura el parámetro `ParqueId` proveniente de la URL.
  2. Invocación directa a `ObtenerNodosPorParqueAsync(parqueId)` inyectando `INodoService`.
  3. Carga la lista local `nodos` y desactiva el estado de carga (`IsLoading = false`).
  4. Muestra notificaciones de error con `NotificationService` en caso de fallos en la consulta o al intentar realizar acciones sobre la grilla.