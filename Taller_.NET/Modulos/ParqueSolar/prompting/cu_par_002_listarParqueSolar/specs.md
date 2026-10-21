# Especificaciones Técnicas: Ver Parques Solares

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Panel).
- **Patrones:** Application Services. La lectura reutiliza `Parque` y `IParqueRepository`. para la comprobación del nodo usa `IMediadorParqueNodo`, sin referencia directa al módulo Nodo.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Panel.Domain)
- **Entidades:**
    - Reutilizar `Parque`.
- **Comportamiento:**
    - La consulta expone los datos ya definidos en la entidad. Este caso de uso no agrega métodos que cambien el estado del parque solar.

### 2. Capa Application (Servicios)
- **DTO de salida:** `ParqueDetalleDto`
    - Propiedades: `Id` (int), `Nombre` (string), `Ubicacion` (string), `CantMaxNodos` (decimal), `CantidadNodosActuales` (decimal), `FechaCreacion` (DateTime).
- **DTO de listado:** `ParqueListadoDto`
    - Propiedades: `Id` (int), `Nombre` (string), `Ubicacion` (string), `CantidadNodosActuales` (decimal).
- **Servicio:** `ParqueService` (implementa la interfaz `IParqueService`)
    - Dependencias: `IParqueRepository` y `IMediadorParqueNodo`.
    - Métodos:
        - `ObtenerParqueAsync(int parqueSolarId)`
        - `ObtenerParquesAsync()`
    - Lógica:
        1. En el listado, obtener los parques y mostrarlos como `ParqueListadoDto`.
        2. En el detalle, obtener el parque por `Id`, si no existe lanzar excepción.
        3. Mostrar la entidad a `ParqueDetalleDto` y retornarla sin modificarla.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Reutilizar `ParqueConfiguration` y `DbSet<Parque> Parques`.
- **Repositorio:**
    - La consulta del listado ordena por `Id`.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente de listado:** `Paneles.razor`
    - **Ruta:** `@page "/parques"`
    - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
    - **UI (Radzen):**
        - `RadzenDataGrid<ParqueListadoDto>` con columnas Nombre, Ubicacion y CantidadNodosActuales.
        - Columna de acciones con botones para ver, editar y eliminar.
- **Componente de detalle:** `DetalleParque.razor`
    - **Ruta:** `@page "/parques/{ParqueId}"`
    - **UI (Radzen):**
        - Tarjeta o formulario de solo lectura con Nombre, Ubicacion, CantMaxNodos, CantidadNodosActuales, ListaNodos y Fecha de creación.
        - `RadzenButton` para volver al listado del nodo.
    - **Flujo:**
        1. `Parques.razor` llama a `ObtenerParquesAsync` en `OnInitializedAsync`.
        2. Al elegir un parque solar, `NavigationManager` abre `DetalleParque.razor`.
        3. `DetalleParque.razor` llama a `ObtenerParqueAsync` y muestra los datos.
        4. Si la consulta falla, `NotificationService` muestra el error.
