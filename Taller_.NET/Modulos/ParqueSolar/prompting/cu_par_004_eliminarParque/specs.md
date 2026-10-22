# Especificaciones Técnicas: Eliminar Parque Solar

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.ParqueSolar).
- **Patrones:** Application Services. Se reutilizan `Parque`, `Nodo` y los repositorios del mismo módulo.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.ParqueSolar.Domain)
- **Entidades:**
    - Reutilizar `Parque` y `Nodo`.
- **Comportamiento:**
    - La eliminación no agrega comportamiento de negocio sobre la entidad.

### 2. Capa Application (Servicios)
- **Parámetros de entrada:** `int idParqueSolar`.
- **Servicio:** `ParqueService` (implementa la interfaz `IParqueService`)
  - Dependencias: `IParqueRepository`.
  - Método: `EliminarParqueAsync(int idParqueSolar)`
  - Lógica:
    1. Cargar el parque solar y comprobar que no tiene nodos asociados. Si no, lanzar excepción.
    2. Obtener el parque solar por `Id`. Si no existe, lanzar excepción.
    3. Remover el parque solar mediante el repositorio y confirmar la persistencia.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Reutilizar `ParqueConfiguration` y `DbSet<Parque> Parques` en `IParqueDbContext`.
- **Repositorio:** `ParqueRepository` expone la baja del parque solar ya cargado y la confirmación de cambios.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `Parque.razor`
  - **Ruta:** `@page "/parques/eliminar/{ParqueId:int}"`
  - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Grilla `RadzenDataGrid` con los parques solares.
    - En la columna de acciones, un `RadzenButton` con icono de papelera.
    - `DialogService` para el modal de confirmación.
  - **Flujo:**
    1. El Administrador pulsa eliminar en una fila.
    2. `DialogService` pide confirmación mostrando el nombre del parque.
    3. Si confirma, se invoca `IParqueService.EliminarParqueAsync(idParqueSolar)` con los identificadores de la ruta.
    4. Tras el éxito, `NotificationService` informa el resultado y se recarga la grilla.
    5. Si la operación falla, se captura la excepción y se muestra una notificación de advertencia.
