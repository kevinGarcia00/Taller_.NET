# Especificaciones Técnicas: Ver Inversor

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (`Modules.Nodo`).
- **Patrones:** Application Services / Mediadores entre Módulos (o CQRS mediante Consultas).

## Detalles de Implementación por Capa

### 1. Capa Domain (`Modules.Nodo.Domain`)
- **Entidades:**
  - **Nodo:**
    - Propiedades involucradas: `Id` (`long`), `InversorId` (`long?`, opcional), `ContextoParque` (`ParqueSolarId`).
    - Propiedad calculada / método de lectura de dominio: `TieneInversorAsignado` (evalúa `InversorId.HasValue`).

### 2. Capa Application (Servicios)
- **DTO de Salida:** `InversorDto` (o `DetalleInversorDto`)
  - Propiedades: `Id` (`long`), `Nombre` (`string`), `FechaCreacion` (`DateTime`).
- **Servicio:** `NodoService` (implementa la interfaz `INodoService`)
  - Dependencias: `INodoRepository`, `IMediadorNodoParque`, `IMediadorNodoInversor`.
  - Método: `ObtenerInversorPorNodoAsync(long nodoId, int parqueSolarId)`
  - **Lógica:**
    1. Obtener el `Nodo` mediante `INodoRepository`. Si no existe, lanzar excepción de entidad no encontrada.
    2. Validar que el `ParqueSolarId` del nodo coincida con el `parqueSolarId` proporcionado en el parámetro.
    3. Verificar si el nodo posee un `InversorId` asignado.
    4. Si `InversorId` es `null`, retornar `null`.
    5. Si posee `InversorId`, consultar al mediador `IMediadorNodoInversor.ObtenerPorIdAsync(nodo.InversorId.Value)` para obtener los detalles del inversor sin romper los límites modulares.
    6. Mapear y retornar el `InversorDto`.

- **Mediadores involucrados:**
  - `IMediadorNodoParque`: Canal exclusivo para validar la pertenencia del nodo al parque solar seleccionado.
  - `IMediadorNodoInversor`: Canal exclusivo para consultar la información del inversor.

### 3. Capa Infrastructure (Persistencia y Comunicación Inter-Módulo)
- **Repositorio:** `NodoRepository` (implementa `INodoRepository`)
  - Método de consulta de solo lectura `ObtenerPorIdAsync(long nodoId)`.
- **Mediador Inter-Módulo:** `MediadorNodoInversor` (implementa `IMediadorNodoInversor`)
  - Realiza la consulta de lectura contra la capa de aplicación/repositorio del módulo `Inversor` y mapea los datos al `InversorDto`.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** Modal emergente `VerInversorDialog.razor` o integrado dentro del listado de nodos.
- **Ruta:** Invocado desde la vista `@page "/parques/{ParqueId:int}/nodos"`.
- **Seguridad:** Proteger con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
- **UI (Radzen):**
  - Botón de acción por fila en la grilla de nodos (`RadzenButton` con icono `Icon="visibility"`).
  - Diálogo modal desplegado mediante `DialogService.OpenAsync<VerInversorDialog>`.
  - Contenido del diálogo: `RadzenCard`, `RadzenText` y campos de solo lectura mostrando los atributos del inversor (`Nombre` y `FechaCreacion`).
  - Botón "Cerrar": `RadzenButton` para finalizar la lectura.
- **Flujo de Ejecución:**
  1. El Administrador hace clic en la opción "Ver Inversor" de un nodo en la grilla.
  2. El componente invoca `INodoService.ObtenerInversorPorNodoAsync(nodo.Id, ParqueId)`.
  3. Si el método retorna un `InversorDto`:
     - Se abre el modal `DialogService` mostrando la información detallada del inversor.
  4. Si el método retorna `null` (nodo sin inversor):
     - Se despliega una notificación de advertencia mediante `NotificationService`.