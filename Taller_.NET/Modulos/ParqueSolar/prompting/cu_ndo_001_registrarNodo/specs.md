# Especificaciones Técnicas: Registrar Nodo

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo.
- **Patrones:** Application Services (CQRS o Servicios de Aplicación por módulo).

## Detalles de Implementación por Capa

### 1. Capa Domain (`Modules.Nodo.Domain`)
- **Entidades:**
  - **Nodo:**
    - **Propiedades:**
      - `Id` (long, PK)
      - `Nombre` (string, max 100 caracteres)
      - `UbicacionParque` (string, max 200 caracteres)
      - `FechaCreacion` (DateTime)
      - `EnergiaGenerada` (decimal)
      - `InversorId` (long?, FK opcional/nullable con índice único)
      - `ContextoParque` (objeto de valor `ContextoParque`)
    - **ContextoParque:**
      - **Propiedades:** `ParqueSolarId` (int / long).

    - **Comportamiento:**
      - Constructor que recibe `Nombre`, `UbicacionParque` e `InversorId` (opcional), asigna `FechaCreacion` (DateTime.UtcNow) e inicializa `EnergiaGenerada` en 0. Valida que el nombre y la ubicación no sean nulos o vacíos.
      - Método de dominio `AsignarContexto(ContextoParque contexto)` para vincular el nodo al parque solar seleccionado.

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `CrearNodoDto`
  - Propiedades: `Nombre`, `UbicacionParque`, `InversorId` (long?), `ParqueSolarId`.
- **Validador:** `CrearNodoDtoValidator` (usando FluentValidation)
  - Reglas: `Nombre` requerido (max 100), `UbicacionParque` requerida (max 200), `ParqueSolarId` requerido. `InversorId` es opcional.
- **Servicio:** `NodoService` (implementa la interfaz `INodoService`)
  - Dependencias: `INodoRepository`, `IMediadorNodoParque` e `IMediadorNodoInversor`.
  - Método: `CrearNodoAsync(CrearNodoDto dto)`
  - **Lógica:**
    1. Consultar al mediador `IMediadorNodoParque` para confirmar que el parque solar seleccionado existe.
    2. Si `InversorId` tiene valor, consultar a `IMediadorNodoInversor` para validar que el inversor existe y está disponible.
    3. Validar que no exista otro nodo con el mismo nombre en el parque solar seleccionado.
    4. Instanciar la entidad `Nodo`, asignar `ContextoParque` y persistir mediante `INodoRepository`.
    5. Retornar el `Id` generado.

- **Mediadores:**
  - `IMediadorNodoParque`: Canal exclusivo de comunicación hacia el módulo de Parque Solar.
  - `IMediadorNodoInversor`: Canal exclusivo para consultar disponibilidad de Inversores.

### 3. Capa Infrastructure (Persistencia)
- **Repositorio:** `NodoRepository` (implementa `INodoRepository`).
  - Consulta `ExisteInversorEnUsoAsync(long inversorId)` para verificar la unicidad en caso de enviarse un `InversorId`.
- **Configuración Entity Framework:**
  - `NodoConfiguration : IEntityTypeConfiguration<Nodo>`
  - Tabla: `Nodos`.
  - Clave primaria: `Id` (`bigint`).
  - Mapeo de `EnergiaGenerada` con precisión decimal.
  - Mapeo de `InversorId` como opcional.
  - Mapeo de `ContextoParque` como columna `ParqueSolarId`.
  - Índice único compuesto por `Nombre` y `ParqueSolarId`.
- **DbContext:** Exponer `DbSet<Nodo> Nodos` en `INodoDbContext`.
- **Migraciones:** Generar y aplicar la migración correspondientes.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `AltaNodo.razor`
- **Ruta:** `@page "/parques/{ParqueId:int}/nodos/alta"`
- **Seguridad:** `<AuthorizeView Roles="Administrador">` o `[Authorize(Roles = "Administrador")]`.
- **UI (Radzen):**
  - `<RadzenTemplateForm>` vinculado a `CrearNodoDto`.
  - Controles `RadzenTextBox` para `Nombre` y `UbicacionParque`.
  - `RadzenDropDown` opcional para seleccionar el `Inversor` de la lista de disponibles (`AllowClear="true"`).
  - El parque solar seleccionado se muestra en modo solo lectura.
  
- **Flujo:**
  1. En `OnInitializedAsync`, el componente obtiene la lista de inversores disponibles sin asociar.
  2. En `OnValidSubmit`, ejecuta `CrearNodoAsync(dto)` inyectando `INodoService`.
  3. Muestra una notificación con `NotificationService` y redirige al listado de nodos del parque con `NavigationManager`.