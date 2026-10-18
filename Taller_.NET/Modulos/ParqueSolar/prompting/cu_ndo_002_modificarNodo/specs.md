# Especificaciones Técnicas: Modificar Nodo

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo.
- **Patrones:** Application Services (Arquitectura en capas clásica o CQRS mediante Servicios).

## Detalles de Implementación por Capa

### 1. Capa Domain
- **Entidades:**
  - **Nodo:**
    - **Propiedades:**
      - `Id` (long, PK)
      - `Nombre` (string, max 100 caracteres)
      - `UbicacionParque` (string, max 200 caracteres)
      - `FechaCreacion` (DateTime, no modificable)
      - `EnergiaGenerada` (decimal, no modificable)
      - `InversorId` (long, FK con índice único)
      - `ContextoParque` (objeto de valor `ContextoParque`)
    - **Comportamiento:**
      - Método `Actualizar(string nombre, string ubicacionParque, long inversorId)` que valida datos obligatorios y actualiza el estado manteniendo intactos `FechaCreacion` y `EnergiaGenerada`.

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `EditarNodoDto`
  - Propiedades: `Id`, `Nombre`, `UbicacionParque`, `InversorId`, `ParqueSolarId`.
- **DTO de Salida Auxiliar:** `InversorOpcionDto` (`Id`, `Nombre`).
- **Validador:** `EditarNodoDtoValidator` (FluentValidation)
  - Reglas: `Id` requerido, `Nombre` requerido (max 100), `UbicacionParque` requerida (max 200), `InversorId` requerido, `ParqueSolarId` requerido.
- **Servicio:** `NodoService` (`INodoService`)
  - Dependencias: `INodoRepository`, `IMediadorNodoParque`, `IMediadorNodoInversor`.
  - Método: `ModificarNodoAsync(EditarNodoDto dto)`
  - **Lógica:**
    1. Obtener la entidad `Nodo` por `Id`. Si no existe, lanzar excepción.
    2. Si cambia el `InversorId`, consultar al mediador `IMediadorNodoInversor` para confirmar que el nuevo inversor existe y no está asignado a ningún otro nodo.
    3. Validar unicidad de nombre en el parque excluyendo el `Id` del nodo actual.
    4. Invocar `nodo.Actualizar(dto.Nombre, dto.UbicacionParque, dto.InversorId)`.
    5. Persistir cambios vía `INodoRepository`.

### 3. Capa Infrastructure (Persistencia)
- **Repositorio:** `NodoRepository`
  - Consulta `ExisteInversorEnOtroNodoAsync(long inversorId, long nodoIdActual)` para asegurar la unicidad del inversor.
- **Configuración Entity Framework:**
  - `NodoConfiguration : IEntityTypeConfiguration<Nodo>`
  - Configurar `InversorId` con un índice único para garantizar a nivel de base de datos la relación 1 a 1 estricta.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `EditarNodo.razor`
- **Ruta:** `@page "/parques/{ParqueId:int}/nodos/{NodoId:long}/editar"`
- **UI (Radzen):**
  - Formulario `RadzenTemplateForm` vinculado a `EditarNodoDto`.
  - Controles `RadzenTextBox` para Nombre y Ubicación.
  - `RadzenDropDown` para Inversor: Muestra la lista de inversores sin nodo asignado más el inversor actualmente asignado a este nodo.