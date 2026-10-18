# Especificaciones Técnicas: Eliminar Nodo

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Nodo).
- **Patrones:** Application Services (Arquitectura en capas clásica o CQRS mediante Servicios/Comandos).

## Detalles de Implementación por Capa

### 1. Capa Domain (`Modules.Nodo.Domain`)
- **Entidades:**
  - **Nodo:**
    - **Propiedades** 
        - `Id` (`long`)
        - `ContextoParque` (`ParqueSolarId`)
        -`InversorId` (`long?`)

### 2. Capa Application (Servicios)
- **Servicio:** `NodoService` (implementa la interfaz `INodoService`)
  - **Firma del método:** `Task EliminarNodoAsync(long nodoId, int parqueSolarId)`
  - **Dependencias:** `INodoRepository`, `IMediadorNodoParque`, `IMediadorNodoInversor`, `IMediadorNodoPanel`.
  - **Lógica del proceso:**
    1. Obtener el `Nodo` por `nodoId` desde `INodoRepository`. Si no existe, lanzar excepción de entidad no encontrada.
    2. Validar que el `ParqueSolarId` del contexto del nodo coincida con el `parqueSolarId` proporcionado en el parámetro.
    3. Consultar al mediador `IMediadorNodoPanel` para verificar si existen paneles solares vinculados a este nodo. Si existen paneles asociados, lanzar excepción de regla de negocio impidiendo la eliminación.
    4. Si el nodo tiene un `InversorId` asignado, notificar al mediador `IMediadorNodoInversor` para liberar la asignación del inversor correspondiente.
    5. Invocar el método de eliminación en `INodoRepository` (`EliminarAsync(nodo)`).

- **Mediadores involucrados:**
  - `IMediadorNodoParque`: Verifica la validez del parque solar.
  - `IMediadorNodoPanel`: Consulta la existencia de paneles solares dependientes del nodo.
  - `IMediadorNodoInversor`: Gestiona la desvinculación/liberación del inversor asociado.

### 3. Capa Infrastructure (`Modules.Nodo.Infrastructure`)
- **Repositorio:** `NodoRepository` (implementa `INodoRepository`)
  - Método `EliminarAsync(Nodo nodo)` que remueve la entidad de la unidad de trabajo (`DbSet<Nodo>.Remove(nodo)`).
  - Confirmación de persistencia mediante `DbContext.SaveChangesAsync()`.
- **Configuración de Entity Framework:**
  - La eliminación del registro liberará automáticamente el índice único en `InversorId`, permitiendo que dicho inversor pueda volver a utilizarse.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** Integrado en el listado de nodos (`ListadoNodos.razor` / `NodosParque.razor`) o invocado vía diálogo.
- **Ruta:** `@page "/parques/{ParqueId:int}/nodos"`
- **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
- **UI (Radzen Dialog):**
  - Botón de acción por cada fila de la tabla/grid (`RadzenButton` con icono de papelera).
  - Invocación de `DialogService.Confirm` de Radzen con mensaje de confirmación.
- **Flujo de Ejecución:**
  1. El Administrador presiona el botón "Eliminar" en una fila del listado.
  2. Se despliega el modal de confirmación mediante `RadzenDialogService`.
  3. Si el usuario selecciona **Aceptar**:
     - Se invoca `INodoService.EliminarNodoAsync(nodo.Id, ParqueId)`.
     - Al completarse con éxito, se muestra un mensaje mediante `NotificationService` de Radzen (`NotificationSeverity.Success`).
     - Se recarga la lista de nodos en el componente.
  4. Si el usuario selecciona **Cancelar**:
     - Se cierra el diálogo y no se ejecuta ninguna operación.