# Tareas de Implementación: Eliminar Usuario

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar las entidades `Usuario` y `Rol` existentes

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar las configuraciones de EF Core (`UsuarioConfiguration`,` RolConfiguration`) y el conjunto `DbSet` de `IUsersDbContext`.
  - [ ] 2.2 Verificar que la relación N:N (`UsuarioRoles`) mantenga la eliminación en cascada para limpiar automáticamente la tabla intermedia al remover la entidad.

- [ ] **3. Capa Application**
  - [ ] 3.1 Agregar a `IUsuarioService` e implementar en `UsuarioService` el método `EliminarUsuarioAsync(long id, long idUsuarioAutenticado)`.
  - [ ] 3.4 Implementar la lógica en `EliminarUsuarioAsync`:
      - Validar que el usuario no intente eliminarse a sí mismo.
      - Consultar el usuario por `Id` incluyendo roles.

Validar que no sea el único usuario con el rol "Administrador" registrado en el sistema.

Remover la entidad mediante _dbContext.Usuarios.Remove(usuario) y persistir cambios con SaveChangesAsync().
  - [ ] 3.5 Crear Test Unitarios para verificar:
        - Error si el usuario no existe.
        - Error si el correo ingresado ya pertenece a otro usuario.
        - Modificacion exitosa cuando los datos son validos

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `EditarUsuario.razor` con la directiva `@page "/usuarios/editar/{Id:long}"`.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Cargar los datos del usuario en `OnInitializedAsync` invocando `ObtenerParaEdicionAsync(Id)`.
  - [ ] 4.4 Diseñar el formulario con `<RadzenTemplateForm>` usando `RadzenTextBox` para los datos personales y `RadzenDropDown` para los roles.
  - [ ] 4.5 Implementar `OnValidSubmit` para invocar `ModificarUsuarioAsync`, mostrar notificación de éxito con `NotificationService` y redirigir al listado (/usuarios).

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [ ] 5.2 Ejecutar inspección de código (o análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
