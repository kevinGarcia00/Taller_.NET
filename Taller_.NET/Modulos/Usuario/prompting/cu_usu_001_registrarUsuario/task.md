# Tareas de Implementación: Alta Usuario

- [ ] **1. Capa Domain**
  - [ ] 1.1 Crear la entidades `Usuario` y `Rol` en la capa de Dominio.
  - [ ] 1.2 Agregar las propiedades de Usuario (`Id`, `Nombre`, `Apellido`, `Telefono`, `Correo`, `Roles`) y `Rol` (`Id`, `Nombre`).
  - [ ] 1.3 Agregar constructor con validaciones básicas en `Usuario` y método de dominio para asociar roles.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Crear la clase `UsuarioConfiguration` y `RolConfiguration` implementando `IEntityTypeConfiguration`.
  - [ ] 2.2 Configurar la clave primaria bigint (long), el índice único para la propiedad `Correo`, las longitudes máximas y la relación N:N mediante la tabla intermedia (ej. `UsuarioRoles`).
  - [ ] 2.3 Agregar la propiedad `DbSet<Usuario>` y `DbSet<Rol>` en el `IUsersDbContext`/`UsersDbContext`.
  - [ ] 2.4 Generar la migración de base de datos (ej. usando el CLI de Entity Framework: `dotnet ef migrations add AddUsuarioYRol`).
  - [ ] 2.5 Actualizar la base de datos (ej. `dotnet ef database update`).

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO `UsuarioDto`.
  - [ ] 3.2 Crear la clase `CrearUsuarioDtoValidator` usando FluentValidation.
  - [ ] 3.3 Crear la interfaz `IUsuarioService` y su implementación `UsuarioService`.
  - [ ] 3.4 Implementar la lógica del método de creación (inyectar DbContext, verificar unicidad de correo, guardar cambios, retornar Id).
  - [ ] 3.5 Crear Test Unitario para verificar que el servicio lanza un error si el correo del usuario ya existe.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `AltaUsuario.razor` con la directiva `@page "/usuarios/alta"`.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Diseñar el formulario con `<RadzenTemplateForm>` y los controles de entrada (`RadzenTextBox`, para texto/correo y `RadzenDropDown` para la selección de roles).
  - [ ] 4.4 Implementar el método `OnValidSubmit` para invocar el método correspondiente en `IUsuarioService`.
  - [ ] 4.5 Mostrar mensaje de éxito usando el `NotificationService` de Radzen y redirigir al listado de usuarios (`/usuarios`).

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [ ] 5.2 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y deuda técnica.
  - [ ] 5.3 Ejecutar inspección de código para asegurar que no hay code smells.
