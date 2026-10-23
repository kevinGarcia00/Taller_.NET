# Tareas de Implementación: Eliminar Usuario

- [ ] **1. Capa Domain**
  - [x] 1.1 Reutilizar las entidades `Usuario` y `Rol` existentes
  - [x] 1.2 Validar reglas de negocio del dominio aplicables a la entidad `Usuario`.

- [x] **2. Capa Infrastructure (Persistencia)**
  - [x] 2.1 Reutilizar las configuraciones de EF Core (`UsuarioConfiguration`,` RolConfiguration`) y el conjunto `DbSet` de `IUsersDbContext`.
  - [x] 2.2 Verificar que la relación N:N (`UsuarioRoles`) mantenga la eliminación en cascada para limpiar automáticamente la tabla intermedia al remover la entidad.

- [x] **3. Capa Application**
  - [x] 3.1 Agregar a `IUsuarioService` e implementar en `UsuarioService` el método `EliminarUsuarioAsync(long id, Guid idUsuarioAutenticado = default)`.
  - [x] 3.2 Implementar la lógica en `EliminarUsuarioAsync`:
      - [x] Validar que el usuario no intente eliminarse a sí mismo **(omita esta validación en development, se reintegrará con Auth0)**.
      - [x] Consultar el usuario por `Id` incluyendo roles.
      - [x] Verificar la existencia del usuario
      - [x] Validar que no sea el único usuario con el rol "Administrador" registrado en el sistema.
      - [x] Remover la entidad mediante `_dbContext.Usuarios.Remove(usuario)` y persistir cambios con `SaveChangesAsync()`.

- [x] **3.3** Tests unitarios creados y pasando:
    - Excepción cuando el usuario no existe.
    - Excepción al intentar eliminar el último Administrador del sistema.
    - Eliminación exitosa cuando las condiciones son válidas.
    - (Validación de self-delete se reintegrará con Auth0)

- [x] **4. Capa Presentation (Blazor Server con Radzen)**
  - [x] 4.1 Ubicar el componente de listado de usuarios (`Usuarios.razor`).
  - [x] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [x] 4.3 Agregar el botón de eliminación en la columna de acciones del `RadzenDataGrid`.
  - [x] 4.4 Integrar `DialogService` para desplegar un modal de confirmación antes de realizar la eliminacion.
  - [x] 4.5 Invocar `EliminarUsuarioAsync(id)` sin necesidad de obtener el ID del usuario autenticado en development **(se reintegrará con Auth0 después)**.
  - [x] 4.6 Mostrar notificación (`NotificationService`) y refrescar la grilla tras el éxito.

- [x] **5. Verificación de Calidad (Guardarraíles)**
  - [x] 5.1 Ejecutar suite de pruebas para validar que no existan violaciones de Clean Architecture.
  - [x] 5.2 **Resultados**: 96 pruebas pasan, 0 fallos.
  - [x] 5.3 inspección de código: Sin code smells detectados.
