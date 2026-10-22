# Tareas de Implementación: Modificar Usuario

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar las entidades `Usuario` y `Rol` existentes.
  - [ ] 1.2 Verificar/implementar los métodos de dominio en la entidad `Usuario`:
    - `ActualizarDatosPersonales(string nombre, string apellido, string correo, string telefono)` con validaciones de campos requeridos.
    - `ActualizarRoles(IEnumerable<Rol> nuevosRoles)` para refrescar la colección de roles y garantizar consistencia en la relación N:N.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar las configuraciones de EF Core (`UsuarioConfiguration`,` RolConfiguration`) y el conjunto `DbSet` de `IUsersDbContext`.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear la estructura del DTO `ModificarUsuarioDto (Id, Nombre, Apellido, Correo, Telefono, RolesIds)`.
  - [ ] 3.2 Crear el validador `ModificarUsuarioDtoValidator` con FluentValidation.
  - [ ] 3.3 Declarar en `IUsuarioService` e implementar en `UsuarioService` los métodos:
    - `Task<ModificarUsuarioDto> ObtenerParaEdicionAsync(long id)`
    - `Task ModificarUsuarioAsync(ModificarUsuarioDto dto)`
  - [ ] 3.4 Implementar la lógica en `ModificarUsuarioAsync`:
    - Consultar el usuario por `Id` incluyendo sus roles asociados (lanzar excepción si no existe).
    - Validar que el correo ingresado no pertenezca a otro usuario registrado.
    - Consultar la base de datos para verificar que todos los `RolesIds` enviados existan.
    - Invocar `ActualizarDatosPersonales` y `ActualizarRoles` sobre la entidad.
    - Persistir cambios mediante `SaveChangesAsync()`.
  - [ ] 3.5 Crear Test Unitarios para verificar:
      - Error si el usuario a modificar no existe.
      - Error si el correo ingresado pertenece a otro usuario.
      - Error si alguno de los `RolesIds` no existe.
      - Modificación exitosa al pasar validaciones.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `EditarUsuario.razor` con la directiva `@page "/usuarios/editar/{Id:long}"`.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Cargar los datos del usuario en `OnInitializedAsync` invocando `ObtenerParaEdicionAsync(Id)`.
  - [ ] 4.4 Diseñar el formulario con `<RadzenTemplateForm>` usando `RadzenTextBox` para los datos personales y `RadzenDropDown` para los roles.
  - [ ] 4.5 Implementar `OnValidSubmit` para invocar `ModificarUsuarioAsync`, mostrar notificación de éxito con `NotificationService` y redirigir al listado (/usuarios).

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [ ] 5.2 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y deuda técnica.
  - [ ] 5.3 Ejecutar inspección de código para asegurar que no hay code smells.
