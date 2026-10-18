# Especificaciones Técnicas: Ver Inversor

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.Inversor).
- **Patrones:** Application Services. La lectura reutiliza `Inversor` e `IInversorRepository`. La comprobación del nodo y la obtención del `InversorId` usan `IMediadorNodoInversor`, sin referencia directa al módulo Nodo.

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.Inversor.Domain)
- **Entidades:**
    - Reutilizar `Inversor`.
- **Comportamiento:**
    - La consulta expone `Nombre` y `FechaCreacion`. Este caso de uso no agrega métodos que cambien el estado del inversor.

### 2. Capa Application (Servicios)
- **DTO de salida:** `InversorDetalleDto`
  - Propiedades: `Id` (int), `Nombre` (string), `FechaCreacion` (DateTime), `NodoId` (int), `ParqueSolarId` (int).
- **Servicio:** `InversorService` (implementa la interfaz `IInversorService`)
  - Dependencias: `IInversorRepository` e `IMediadorNodoInversor`.
  - Método: `ObtenerInversorDelNodoAsync(int nodoId, int parqueSolarId)`
  - Lógica:
    1. Confirmar mediante el mediador que el nodo existe y pertenece al parque seleccionado. Si no, lanzar excepción.
    2. Obtener el `InversorId` de ese nodo. Si no tiene inversor, retornar un resultado vacío, sin lanzar excepción.
    3. Si tiene `InversorId`, cargar el inversor y proyectarlo a `InversorDetalleDto` sin modificarlo.
    4. Si el id figura en el nodo pero el inversor no existe, lanzar excepción.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:**
    - Reutilizar `InversorConfiguration` y `DbSet<Inversor> Inversores`.
- **Repositorio:**
    - La consulta del detalle obtiene el inversor por `Id` y proyecta `Id`, `Nombre` y `FechaCreacion`.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `DetalleInversor.razor`
  - **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/inversor"`
  - **Seguridad:** Utilizar `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Contexto de parque y nodo en solo lectura.
    - Si hay inversor: tarjeta o formulario de solo lectura con Nombre y Fecha de creación, más botones para editar y eliminar.
    - Si no hay inversor: mensaje indicando que el nodo no tiene inversor y un `RadzenButton` ("Registrar inversor") hacia `/parques/{ParqueId}/nodos/{NodoId}/inversor/alta`.
  - **Flujo:**
    1. `DetalleInversor.razor` llama a `ObtenerInversorDelNodoAsync` en `OnInitializedAsync`.
    2. Con datos, los muestra en solo lectura.
    3. Sin datos, muestra el estado vacío y el acceso al alta.
    4. Si el nodo o el parque no son válidos, `NotificationService` muestra el error.
