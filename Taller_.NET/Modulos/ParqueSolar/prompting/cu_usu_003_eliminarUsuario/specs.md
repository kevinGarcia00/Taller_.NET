# Especificaciones Técnicas: Eliminar de Usuario

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.User).
- **Patrones:** Application Services (Arquitectura en capas clásica o CQRS mediante Servicios).

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.User.Domain)
- **Entidades:** 
    - `Usuario`
    - `Rol`

### 2. Capa Application (Servicios)
- **Parametros de Entrada:** `long id`, `long idUsuarioAutenticado` (IdAdministrador)
- **Servicio:** `UsuarioService` (implementa la interfaz `IUsuarioService`)
  - Dependencias: `IUsersDbContext` (para acceso a BD).
  - Método: `EliminarUsuarioAsync(long id, long idUsuarioAutenticado)`
  - Lógica: 
        1. Validar que `id != idUsuarioAutenticado`. Si un usuario intenta eliminarse a sí mismo, lanzar excepción.
        2. Consultar el usuario en la BD por `Id` incluyendo sus roles. Si no existe, lanzar excepción.
        3. Remover la entidad del DbSet: `Usuarios.Remove(usuario)`.
        4. Confirmar la operación con `SaveChangesAsync()`.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:** 
    - `UsuarioConfiguration : IEntityTypeConfiguration<Usuario>`
        - Reutilizar la configuración existente. Asegurar que la relación N:N con `Rol` permita la eliminación en cascada de los registros asociados en la tabla intermedia `UsuarioRoles`.  
    - `RolConfiguration : IEntityTypeConfiguration<Rol>`
        - Reutilizar la configuración existente.
- **DbContext:** Asegurarse de exponer en `IUsersSbConxtext` los conjuntos:
    - `DbSet<Usuario> Usuario { get; set;} `
    - `DbSet<Rol> Roles { get; set;} `

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `ListaUsuarios.razor`
  - **Ruta:** `@page "/usuarios"`
  - **Seguridad:** Utilizar el componente `<AuthorizeView Roles="Administrador">` para proteger el contenido o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Grilla `RadzenDataGrid<UsuarioDto>` con el listado de usuarios.
    - En la columna de acciones, incluir un `RadzenButton` con icono de papelera.
    - Inyectar `DialogService` para desplegar el modal de confirmación.
  - **Flujo:** 
    1. El Administrador hace clic en el botón de eliminar de una fila en la grilla.
    2. Se ejecuta un diálogo de confirmación mediante `DialogService`.
    3. Si se confirma la acción, se obtiene el ID del usuario en sesión desde `AuthenticationStateProvider` y se invoca `IUsuarioService.EliminarUsuarioAsync(id, idUsuarioAutenticado)`.
    4. Tras el éxito, se muestra una notificación con `NotificationService` de Radzen y se recarga el listado de la grilla.
    5. En caso de error de negocio , se captura la excepción y se despliega una notificación de advertencia con `NotificationService`.