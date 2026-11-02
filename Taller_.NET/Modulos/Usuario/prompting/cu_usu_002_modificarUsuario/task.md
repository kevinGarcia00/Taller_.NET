# Tareas de Implementación: Modificar Usuario

- [x] **1. Capa Domain**
  - [x] 1.1 Reutilizar la entidad `Usuario` existente (sin entidad `Rol` local).
  - [x] 1.2 Verificar/implementar el método de dominio en la entidad `Usuario`:
    - `ActualizarDatosPersonales(string nombre, string apellido, string correo, string telefono)` con validaciones de campos requeridos.

- [x] **2. Capa Infrastructure (Persistencia)**
  - [x] 2.1 Reutilizar la configuración de EF Core (`UsuarioConfiguration`) y el conjunto `DbSet` de `IUsuarioDbContext`.

- [x] **3. Capa Application**
  - [x] 3.1 Crear la estructura del DTO `ModificarUsuarioDto (Id, Nombre, Apellido, Correo, Telefono, RolesIds)` con `RolesIds` como `List<string>` de Auth0.
  - [x] 3.2 Crear el validador `ModificarUsuarioDtoValidator` con FluentValidation.
  - [x] 3.3 Declarar en `IUsuarioService` e implementar en `UsuarioService` los métodos:
    - `Task<ModificarUsuarioDto> ObtenerParaEdicionAsync(Guid id)`
    - `Task ModificarUsuarioAsync(ModificarUsuarioDto dto)`
  - [x] 3.4 Implementar la lógica en `ModificarUsuarioAsync`:
    - Verificar el permiso `users:update`.
    - Consultar el usuario por `Id` (lanzar excepción si no existe).
    - Validar que el correo ingresado no pertenezca a otro usuario registrado.
    - Consultar Auth0 para verificar que todos los `RolesIds` enviados existan.
    - Invocar `ActualizarDatosPersonales` sobre la entidad.
    - Persistir cambios mediante `ActualizarAsync()` + `GuardarCambiosAsync()`.
  - [x] 3.4.1 Sincronizar nombre, apellido, correo y roles con Auth0 cuando el usuario tiene `Auth0UserId`.
  - [x] 3.4.2 Restaurar los datos locales y compensar Auth0 (datos y roles anteriores) si falla la actualización remota o el guardado local.
  - [x] 3.5 Crear Test Unitarios para verificar:
      - Error si el usuario a modificar no existe.
      - Error si el correo ingresado pertenece a otro usuario.
      - Error si alguno de los `RolesIds` no existe.
      - Modificación exitosa al pasar validaciones.

- [x] **4. Capa Presentation (Blazor Server con Radzen)**
  - [x] 4.1 Crear el componente `EditarUsuario.razor` con la directiva `@page "/usuarios/editar/{Id:guid}"`.
  - [x] 4.2 Restringir el acceso con `[Authorize(Policy = PermisosUsuarios.PoliticaActualizar)]` y `<AuthorizeView Policy=...>`.
  - [x] 4.3 Cargar los datos del usuario en `OnParametersSetAsync` invocando `ObtenerParaEdicionAsync(Id)`.
  - [x] 4.4 Diseñar el formulario con `<RadzenTemplateForm>` usando `RadzenTextBox` para los datos personales y `RadzenDropDown` para los roles de Auth0.
  - [x] 4.5 Implementar `OnValidSubmit` para invocar `ModificarUsuarioAsync`, mostrar notificación de éxito con `NotificationService` y redirigir al listado (/usuarios).

- [x] **5. Verificación de Calidad (Guardarraíles)**
  - [x] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [x] 5.2 Ejecutar inspección de código para asegurar que no hay code smells.
