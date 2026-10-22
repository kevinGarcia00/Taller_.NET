# Tareas de Implementación: Eliminar Usuario

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar las entidades `Usuario` y `Rol` existentes
  - [ ] 1.2 Validar reglas de negocio del dominio aplicables a la entidad `Usuario`.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar las configuraciones de EF Core (`UsuarioConfiguration`,` RolConfiguration`) y el conjunto `DbSet` de `IUsersDbContext`.
  - [ ] 2.2 Verificar que la relación N:N (`UsuarioRoles`) mantenga la eliminación en cascada para limpiar automáticamente la tabla intermedia al remover la entidad.

- [ ] **3. Capa Application**
  - [ ] 3.1 Agregar a `IUsuarioService` e implementar en `UsuarioService` el método `EliminarUsuarioAsync(long id, long idUsuarioAutenticado)`.
  - [ ] 3.2 Implementar la lógica en `EliminarUsuarioAsync`:
      - Validar que el usuario no intente eliminarse a sí mismo.
      - Consultar el usuario por `Id` incluyendo roles.
      - Verificar la existencia del usuario
      - Validar que no sea el único usuario con el rol "Administrador" registrado en el sistema.
      - Remover la entidad mediante `_dbContext.Usuarios.Remove(usuario)` y persistir cambios con `SaveChangesAsync()`.
  - [ ] 3.3 Crear Test Unitarios para verificar:
    - Excepción cuando el usuario no existe.
    - Excepción al intentar eliminar el usuario autenticado actualmente.
    - Excepción al intentar eliminar el último Administrador del sistema.
    - Eliminación exitosa cuando las condiciones son válidas.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Ubicar el componente de listado de usuarios (`Usuarios.razor`).
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Agregar el botón de eliminación en la columna de acciones del `RadzenDataGrid`.
  - [ ] 4.4 Integrar `DialogService` para desplegar un modal de confirmación antes de realizar la eliminacion.
  - [ ] 4.5 Obtener el ID del usuario autenticado mediante `AuthenticationStateProvider`, invocar `EliminarUsuarioAsync`, mostrar notificación (`NotificationService`) y refrescar la grilla.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [ ] 5.2 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y deuda técnica.
  - [ ] 5.3 Ejecutar inspección de código para asegurar que no hay code smells.
