# Tareas de Implementación: Registrar Parque Solar

- [ ] **1. Capa Domain**
  - [ ] 1.1 Crear la entidad `ParqueSolar` en la capa de Dominio de `Modules.ParqueSolar`.
  - [ ] 1.2 Definir las propiedades de `ParqueSolar` (`Id`, `Nombre`, `Ubicacion`, `CantMaxNodos`).
  - [ ] 1.3 Implementar el constructor con validación de obligatoriedad de `Nombre`, `Ubicacion` y `CantMaxNodos`.
  - [ ] 1.4 Crear las pruebas unitarias (xUnit) del comportamiento de dominio: alta válida, nombre/ubicacion obligatorios, `CantMaxNodos` requerido.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Crear `ParqueConfiguration` implementando `IEntityTypeConfiguration<Parque>`.
  - [ ] 2.2 Configurar la clave primaria `Id` y las longitudes máximas de columnas (`Nombre` y `Ubicacion` hasta 100 caracteres).
  - [ ] 2.3 Agregar `DbSet<Parque> Parque` en `IParqueDbContext` / `ParqueDbContext` e implementar `ParqueRepository` (`IParqueRepository`).
  - [ ] 2.4 Generar la migración de Entity Framework (ej. `dotnet ef migrations add AddParque`).
  - [ ] 2.5 Aplicar la migración a la base de datos (ej. `dotnet ef database update`).

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO de entrada `ModificarParqueDto` (`Nombre`, `Ubicacion`, `CantMaxNodos`).
  - [ ] 3.2 Implementar el validador `ModificarParqueDtoValidator` usando FluentValidation (Nombre requerido y máximo 100, Ubicacion requerida y máximo 100, CantMaxNodos requerido).
  - [ ] 3.3 Definir la interfaz `IParqueService` y la interfaz del mediador `IMediadorParqueNodo` (único canal de comunicación con el módulo Nodo).
  - [ ] 3.4 Implementar `ParqueService`, con `IParqueRepository` e `IMediadorParqueNodo` inyectados por constructor.
  - [ ] 3.5 Implementar `ModificarParqueAsync(CrearParqueDto dto)`: validar la unicidad del nombre del parque (regla de negocio) a través del mediador, instanciar `Parque` y persistir.
  - [ ] 3.6 Crear las pruebas unitarias del servicio para los escenarios de error (nombre duplicado) y de éxito (alta exitosa retornando el Id).
  - [ ] 3.7 Prepara el codigo para que quede conectado con la implementacion de auth0 y verifique el rol de usuario. 

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `ModificarParque.razor` con la directiva `@page "/parques/modificar/{idParque}"`.
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.3 Diseñar el formulario con `<RadzenTemplateForm>` vinculado al modelo `CrearParqueDto`, usando `RadzenTextBox` para Nombre y Ubicación y `RadzenNumeric` para CantMaxNodos.
  - [ ] 4.4 Implementar `OnValidSubmit` para invocar `IParqueService.ModificarParqueAsync(dto)`.
  - [ ] 4.5 Notificar el éxito con `NotificationService` de Radzen y navegar al listado de parques con `NavigationManager`.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos (debe existir solo por mediadores).
  - [ ] 5.2 Ejecutar el análisis estático de código (o SonarQube si está configurado) para garantizar cero code smells y cumplimiento de estándares.
