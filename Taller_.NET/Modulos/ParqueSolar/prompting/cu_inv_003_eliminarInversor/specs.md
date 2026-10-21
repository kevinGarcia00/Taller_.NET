# Especificaciones Técnicas: Eliminar Inversor

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.ParqueSolar).
- **Patrones:** Application Services. Se reutilizan `Inversor`, `Nodo` y los repositorios del mismo módulo.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.ParqueSolar.Domain)
- **Entidades:**
    - Reutilizar `Inversor` y la navegación desde `Nodo`.
- **Comportamiento:**
    - Método de dominio `QuitarInversor()` en `Nodo`, que deja vacía la relación 1:1.

### 2. Capa Application (Servicios)
- **Parámetros de entrada:** `int nodoId`, `int parqueSolarId`.
- **Servicio:** `InversorService` (implementa la interfaz `IInversorService`)
  - Dependencias: `IInversorRepository` e `INodoRepository`.
  - Método: `EliminarInversorAsync(int nodoId, int parqueSolarId)`
  - Lógica:
    1. Cargar el nodo y comprobar que pertenece al parque seleccionado. Si no, lanzar excepción.
    2. Obtener el inversor de ese nodo. Si no tiene, lanzar excepción.
    3. Invocar `QuitarInversor()` sobre el nodo.
    4. Remover el inversor mediante el repositorio y confirmar la persistencia.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Reutilizar `InversorConfiguration` y `DbSet<Inversor> Inversores` en `IParqueDbContext`.
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
