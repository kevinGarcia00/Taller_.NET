# Tareas de Implementación: Eliminar Usuario

- [x] **1. Capa Domain**
  - [x] 1.1 Reutilizar la entidad `Usuario` existente (sin entidad `Rol` local).
  - [x] 1.2 Validar reglas de negocio del dominio aplicables a la entidad `Usuario`.

- [x] **2. Capa Infrastructure (Persistencia)**
  - [x] 2.1 Reutilizar la configuración de EF Core (`UsuarioConfiguration`) y el conjunto `DbSet` de `IUsuarioDbContext`.
  - [x] 2.2 Verificar que la migración `EliminarRolesLocales` haya retirado las tablas `UsuarioRoles` y `Roles`.

- [x] **3. Capa Application**
  - [x] 3.1 Agregar a `IUsuarioService` e implementar en `UsuarioService` el método `EliminarUsuarioAsync(Guid id, CancellationToken cancellationToken = default)`.
  - [x] 3.2 Implementar la lógica en `EliminarUsuarioAsync`:
      - [x] Verificar que el usuario autenticado tenga el permiso `users:delete`.
      - [x] Consultar el usuario por `Id`.
      - [x] Verificar la existencia del usuario.
      - [x] Validar que el usuario no se elimine a sí mismo usando el `Auth0UserId` autenticado y, como respaldo, el correo.
      - [x] Consultar los roles del usuario en Auth0 y validar que no sea el único usuario con el rol "Administrador".
      - [x] Si tiene `Auth0UserId`, eliminar primero la cuenta en Auth0.
      - [x] Remover la entidad local mediante `EliminarAsync()` y persistir cambios con `GuardarCambiosAsync()`.

- [x] **3.3** Tests unitarios existentes y pasando:
    - Excepción al intentar eliminar el último Administrador del sistema.

- [x] **4. Capa Presentation (Blazor Server con Radzen)**
  - [x] 4.1 Ubicar el componente de listado de usuarios (`Usuarios.razor`).
  - [x] 4.2 Restringir el acceso con `[Authorize(Policy = PermisosUsuarios.PoliticaLeer)]` y `<AuthorizeView Policy=...>`.
  - [x] 4.3 Agregar el botón de eliminación en la columna de acciones del `RadzenDataGrid`.
  - [x] 4.4 Integrar `DialogService` para desplegar un modal de confirmación antes de realizar la eliminacion.
  - [x] 4.5 Invocar `EliminarUsuarioAsync(id)`; el servicio valida el permiso `users:delete` y resuelve el usuario autenticado mediante `IUsuarioActualProvider` para validar la autoeliminación.
  - [x] 4.6 Mostrar notificación (`NotificationService`) y refrescar la grilla tras el éxito.

- [x] **5. Verificación de Calidad (Guardarraíles)**
  - [x] 5.1 Ejecutar suite de pruebas para validar que no existan violaciones de Clean Architecture.
  - [x] 5.2 **Resultados**: suite completa en verde.
  - [x] 5.3 inspección de código: Sin code smells detectados.