# Tareas de Implementación: Registrar Panel

- [ ] **1. Capa Domain**
  - [ ] 1.1 Crear la entidad `Panel` y el enum `EstadoPanel` en la capa de Dominio de `Modules.ParqueSolar`.
  - [ ] 1.2 Definir las propiedades de `Panel` (`Id`, `Nombre`, `FechaCreacion`, `Estado`, `Potencia`, `VidaUtil`, `Nodo`).
  - [ ] 1.3 Implementar el constructor con validación de `Nombre`, `Estado`, `Potencia` y `VidaUtil`, el método `AsignarNodo(Nodo nodo)` y la asignación automática de `FechaCreacion`.
  - [ ] 1.4 Crear las pruebas unitarias (xUnit) del comportamiento de dominio: alta válida, nombre obligatorio, potencia mayor a cero y asignación del nodo.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Crear `PanelConfiguration` implementando `IEntityTypeConfiguration<Panel>`.
  - [ ] 2.2 Configurar la clave primaria `Id`, las longitudes máximas (`Nombre` hasta 100 y `VidaUtil` hasta 50), el estado como string, la relación con `Nodo` y el índice único de `Nombre` + `NodoId`.
  - [ ] 2.3 Agregar `DbSet<Panel> Paneles` en `IParqueDbContext` / `ParqueDbContext` e implementar `PanelRepository` (`IPanelRepository`).
  - [ ] 2.4 Generar la migración de Entity Framework (ej. `dotnet ef migrations add AddPanel`).
  - [ ] 2.5 Aplicar la migración a la base de datos (ej. `dotnet ef database update`).

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO de entrada `CrearPanelDto` (`Nombre`, `Estado`, `Potencia`, `VidaUtil`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Implementar el validador `CrearPanelDtoValidator` usando FluentValidation (`Nombre` requerido y máximo 100, `Estado` requerido, `Potencia` mayor a cero, `VidaUtil` requerida y máximo 50, `NodoId` y `ParqueSolarId` requeridos).
  - [ ] 3.3 Definir la interfaz `IPanelService` en `Modules.ParqueSolar`.
  - [ ] 3.4 Implementar `PanelService`, con `IPanelRepository` e `INodoRepository` inyectados por constructor.
  - [ ] 3.5 Implementar `CrearPanelAsync(CrearPanelDto dto)`: cargar el nodo del parque, validar la unicidad del nombre en el nodo, instanciar `Panel`, asignar el `Nodo`, persistir y retornar el `Id`.
  - [ ] 3.6 Crear las pruebas unitarias del servicio para nodo inexistente, nodo de otro parque, nombre duplicado en el nodo y alta exitosa.
  - [ ] 3.7 Preparar el código para que quede conectado con la implementación de Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `AltaPanel.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/alta"`.
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.3 Diseñar el formulario con `<RadzenTemplateForm>` vinculado a `CrearPanelDto`, usando `RadzenTextBox` para Nombre y Vida útil, `RadzenNumeric` para Potencia y `RadzenDropDown` para Estado. El parque y el nodo se muestran en solo lectura.
  - [ ] 4.4 Implementar `OnValidSubmit` para invocar `IPanelService.CrearPanelAsync(dto)`.
  - [ ] 4.5 Notificar el éxito con `NotificationService` de Radzen y navegar al listado de paneles del nodo con `NavigationManager`.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
