# Especificaciones Técnicas: Eliminar Inversor

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Inversor).
- **Patrones:** Application Services. Se reutilizan la entidad, el repositorio y el mediador `IMediadorNodoInversor`. Inversor no referencia al módulo Nodo.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Inversor.Domain)
- **Entidades:**
    - Reutilizar `Inversor`.
- **Comportamiento:**
    - La eliminación no agrega comportamiento de negocio sobre la entidad. La regla de pertenencia se resuelve al obtener, mediante el mediador, el inversor del nodo seleccionado.

### 2. Capa Application (Servicios)
- **Parámetros de entrada:** `int nodoId`, `int parqueSolarId`.
- **Servicio:** `InversorService` (implementa la interfaz `IInversorService`)
  - Dependencias: `IInversorRepository` e `IMediadorNodoInversor`.
  - Método: `EliminarInversorAsync(int nodoId, int parqueSolarId)`
  - Lógica:
    1. Confirmar mediante el mediador que el nodo existe y pertenece al parque seleccionado. Si no, lanzar excepción.
    2. Obtener el `InversorId` de ese nodo. Si el nodo no tiene inversor, lanzar excepción.
    3. Cargar el inversor. Si no existe, lanzar excepción.
    4. Remover el inversor mediante el repositorio y confirmar la persistencia.
    5. Pedir al mediador que deje el `InversorId` del nodo en vacío, para liberar la relación 1:1.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Reutilizar `InversorConfiguration`. El inversor no mantiene colecciones propias que exijan un borrado en cascada adicional.
- **DbContext:** Reutilizar `DbSet<Inversor> Inversores` en `IInversorDbContext`.
- **Repositorio:** `InversorRepository` expone la baja del inversor ya cargado y la confirmación de cambios.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `DetalleInversor.razor`
  - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/inversor"`
  - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Vista del inversor del nodo, con un `RadzenButton` de eliminar.
    - `DialogService` para el modal de confirmación.
  - **Flujo:**
    1. El Administrador pulsa eliminar en la vista del inversor del nodo.
    2. `DialogService` pide confirmación mostrando el nombre del inversor.
    3. Si confirma, se invoca `IInversorService.EliminarInversorAsync(nodoId, parqueSolarId)` con los identificadores de la ruta.
    4. Tras el éxito, `NotificationService` informa el resultado y la vista pasa al estado sin inversor, con la opción de registrar uno nuevo.
    5. Si la operación falla, se captura la excepción y se muestra una notificación de advertencia.
