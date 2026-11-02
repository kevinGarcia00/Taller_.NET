# Especificaciones Técnicas: Eliminar de Usuario

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.User.Domain)
- **Entidades:** 
    - `Usuario`
    - No existe entidad `Rol` local: Auth0 es la fuente única de identidad, roles y permisos.

### 2. Capa Application (Servicios)
- **Parámetro de Entrada:** `Guid id` del usuario a eliminar.
- **Servicio:** `UsuarioService` (implementa la interfaz `IUsuarioService`)
  - Dependencias: `IUsuarioRepository`, `IUsuarioActualProvider`, `IAuth0AdministracionProvider` y `ILogger<UsuarioService>`.
  - Método: `Task EliminarUsuarioAsync(Guid id, CancellationToken cancellationToken = default)`
  - Lógica: 
        1. Verificar que el usuario autenticado tenga el permiso `users:delete`; si no, lanzar `UsuarioNoAutorizadoException`.
        2. Consultar el usuario en la BD por `Id`. Si no existe, lanzar `UsuarioNoEncontradoException`.
        3. Verificar que el usuario no se esté eliminando a sí mismo: se compara el `Auth0UserId` del usuario autenticado y, como respaldo, su correo. Si coincide, lanzar `UsuarioNoPuedeEliminarseException`.
        4. Si el usuario tiene `Auth0UserId`, consultar sus roles en Auth0. Si tiene el rol `Administrador`, contar cuántos usuarios lo tienen en Auth0 y, si es el último, lanzar `NoSePuedeEliminarUltimoAdministradorException`.
        5. Si el usuario tiene `Auth0UserId`, eliminarlo primero en Auth0 (`DELETE /api/v2/users/{id}`). Si Auth0 responde `404`, se considera exitoso.
        6. Eliminar el registro local mediante `EliminarAsync()` y `GuardarCambiosAsync()`.
        7. Si falla la eliminación local después de haber eliminado la cuenta en Auth0, registrar el error con `ILogger` y relanzar la excepción para permitir reintentar la limpieza.
        8. Los usuarios sin `Auth0UserId` se eliminan solo localmente.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:** 
    - `UsuarioConfiguration : IEntityTypeConfiguration<Usuario>`
        - Reutilizar la configuración existente.
- **DbContext:** Asegurarse de exponer en `IUsuarioDbContext` el conjunto:
    - `DbSet<Usuario> Usuarios { get; set; }`
- **Repositorio:** `UsuarioRepository` implementa `IUsuarioRepository` y encapsula las consultas y la eliminación. La capa Application no accede directamente al `DbContext`.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `Usuarios.razor`
  - **Ruta:** `@page "/usuarios"`
  - **Seguridad:** `@attribute [Authorize(Policy = PermisosUsuarios.PoliticaLeer)]` y `<AuthorizeView Policy="@PermisosUsuarios.PoliticaLeer">`. El acceso al listado requiere `users:read`; la eliminación se valida en el servidor con `users:delete` dentro de `EliminarUsuarioAsync`.
  - **UI (Radzen):**
    - Grilla `RadzenDataGrid<UsuarioListadoDto>` con el listado de usuarios.
    - En la columna de acciones, incluir un `RadzenButton` con icono de papelera.
    - Inyectar `DialogService` para desplegar el modal de confirmación.
  - **Flujo:** 
    1. El usuario autorizado hace clic en el botón de eliminar de una fila en la grilla.
    2. Se ejecuta un diálogo de confirmación mediante `DialogService`.
    3. Si se confirma la acción, se invoca `IUsuarioService.EliminarUsuarioAsync(id)`. El servicio resuelve el usuario autenticado mediante `IUsuarioActualProvider` para validar que no se elimine a sí mismo.
    4. Tras el éxito, se muestra una notificación con `NotificationService` de Radzen y se recarga el listado de la grilla.
    5. En caso de error de negocio o de Auth0, se captura la excepción y se despliega una notificación con `NotificationService`.

### 5. Integración con Auth0

- La eliminación de la identidad se hace primero en Auth0 (`DELETE /api/v2/users/{id}`) y luego en la base local.
- Auth0 considera exitosa la operación si el usuario ya no existe (`404`).
- Los roles y permisos del usuario desaparecen con su cuenta en Auth0; la base local no guarda roles.
- Los usuarios sin `Auth0UserId` solo se eliminan localmente.

