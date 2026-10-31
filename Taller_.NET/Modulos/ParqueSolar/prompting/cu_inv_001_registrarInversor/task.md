# Tareas de Implementación: Registrar Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Crear la entidad `Inversor` en la capa de Dominio de `Modules.ParqueSolar`.
  - [ ] 1.2 Definir las propiedades de `Inversor` (`Id`, `Nombre`, `FechaCreacion`) y, en `Nodo`, la navegación opcional hacia `Inversor`.
  - [ ] 1.3 Implementar el constructor con validación de obligatoriedad de `Nombre`, asignación automática de `FechaCreacion` y el método `AsignarInversor(Inversor inversor)` en `Nodo`.
  - [ ] 1.4 Crear las pruebas unitarias (xUnit) del comportamiento de dominio: alta válida, nombre obligatorio y rechazo si el nodo ya tiene inversor.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Crear `InversorConfiguration` implementando `IEntityTypeConfiguration<Inversor>`.
  - [ ] 2.2 Configurar la clave primaria `Id`, la longitud máxima de `Nombre` (hasta 100 caracteres) y, en `Nodo`, la relación 1:1 con índice único de `InversorId`.
  - [ ] 2.3 Agregar `DbSet<Inversor> Inversores` en `IParqueDbContext` / `ParqueDbContext` e implementar `InversorRepository` (`IInversorRepository`).
  - [ ] 2.4 Generar la migración de Entity Framework (ej. `dotnet ef migrations add AddInversor`).
  - [ ] 2.5 Aplicar la migración a la base de datos (ej. `dotnet ef database update`).

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO de entrada `CrearInversorDto` (`Nombre`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Implementar el validador `CrearInversorDtoValidator` usando FluentValidation (`Nombre` requerido y máximo 100, `NodoId` y `ParqueSolarId` requeridos).
  - [ ] 3.3 Definir la interfaz `IInversorService` en `Modules.ParqueSolar`.
  - [ ] 3.4 Implementar `InversorService`, con `IInversorRepository` e `INodoRepository` inyectados por constructor.
  - [ ] 3.5 Implementar `CrearInversorAsync(CrearInversorDto dto)`: cargar el nodo del parque, rechazar el alta si ya tiene inversor, instanciar `Inversor`, asignarlo al nodo, persistir y retornar el `Id`.
  - [ ] 3.6 Crear las pruebas unitarias del servicio para nodo inexistente, nodo que ya tiene inversor y alta exitosa.
  - [ ] 3.7 Preparar el código para que quede conectado con la implementación de Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `AltaInversor.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/inversor/alta"`.
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.3 Diseñar el formulario con `<RadzenTemplateForm>` vinculado al modelo `CrearInversorDto`, usando `RadzenTextBox` para Nombre. El parque y el nodo se muestran en solo lectura.
  - [ ] 4.4 Implementar `OnValidSubmit` para invocar `IInversorService.CrearInversorAsync(dto)`.
  - [ ] 4.5 Notificar el éxito con `NotificationService` de Radzen y navegar a la vista del inversor del nodo con `NavigationManager`. Si el nodo ya tiene inversor, informar y no abrir un segundo alta.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
