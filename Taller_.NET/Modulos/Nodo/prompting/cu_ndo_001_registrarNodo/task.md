# Tareas de Implementación: Registrar Nodo

- [ ] **1. Capa Domain**
  - [ ] 1.1 Crear la entidad `Nodo` y la propiedad opcional `InversorId` (`long?`) para la relación con `Inversor`.
  - [ ] 1.2 Definir las propiedades de `Nodo` (`Id`, `Nombre`, `UbicacionParque`, `FechaCreacion`, `EnergiaGenerada`, `InversorId`, `ContextoParque`) y `ContextoParque` (`ParqueSolarId`).
  - [ ] 1.3 Implementar el constructor con validación de obligatoriedad para `Nombre` y `UbicacionParque`, asignación automática de `FechaCreacion` e inicialización de `EnergiaGenerada` en 0.
  - [ ] 1.4 Agregar el método de dominio `AsignarContexto(ContextoParque contexto)`.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Crear `NodoConfiguration` implementando `IEntityTypeConfiguration<Nodo>`.
  - [ ] 2.2 Configurar la PK `bigint`, longitudes máximas, índice único filtrado para `InversorId` e índice único compuesto (`Nombre`, `ParqueSolarId`).
  - [ ] 2.3 Crear `NodoRepository` e implementar el método `ExisteInversorEnUsoAsync(long inversorId)`.
  - [ ] 2.4 Agregar `DbSet<Nodo> Nodos` en `INodoDbContext` / `NodoDbContext`.
  - [ ] 2.5 Generar la migración de Entity Framework.
  - [ ] 2.6 Aplicar la migración a la base de datos.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear `CrearNodoDto`.
  - [ ] 3.2 Implementar `CrearNodoDtoValidator` usando FluentValidation.
  - [ ] 3.3 Definir interfaces de los mediadores (`IMediadorNodoParque`, `IMediadorNodoInversor`) y de `INodoService`.
  - [ ] 3.4 Implementar `NodoService` y la lógica de `CrearNodoAsync` (validar parque vía mediador, validar inversor si no es nulo, validar unicidad de nombre en el parque, guardar y retornar Id).
  - [ ] 3.5 Crear Tests Unitarios para los escenarios de éxito (con y sin inversor) y de error (parque inexistente, nombre duplicado).

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `AltaNodo.razor` en la ruta `@page "/parques/{ParqueId:int}/nodos/alta"`.
  - [ ] 4.2 Proteger el componente para el rol `Administrador`.
  - [ ] 4.3 Cargar la lista de inversores disponibles en `OnInitializedAsync`.
  - [ ] 4.4 Diseñar la vista usando `<RadzenTemplateForm>` con controles para Nombre, Ubicación en Parque y selector desplegable de Inversor opcional.
  - [ ] 4.5 Implementar `OnValidSubmit` para invocar `INodoService.CrearNodoAsync`.
  - [ ] 4.6 Notificar el éxito con `NotificationService` de Radzen y navegar a la lista de nodos del parque (`/parques/{ParqueId}/nodos`).

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar pruebas de arquitectura para confirmar que `Modules.Nodo` mantiene el aislamiento con otros módulos mediante mediadores.
  - [ ] 5.2 Ejecutar análisis estático de código para garantizar cero code smells y cumplimiento de estándares.