# Especificaciones Técnicas: Listar Usuarios

## Arquitectura y Patrones

- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.User).
- **Patrones:** Application Services (Arquitectura en capas clásica o CQRS mediante Servicios).

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.User.Domain)

- **Entidades:** 
    - Reutilizar la entidad `Usuario` existente.
- **Comportamiento:** 
  - Propiedades y métodos de dominio para consultar los datos básicos del usuario. Los roles no forman parte del dominio local.

### 2. Capa Application (Servicios)

- **DTO de Salida:** `UsuarioListadoDto`
    - Propiedades: `Id` (Guid), `Nombre` (string), `Apellido` (string), `Correo` (string), `Telefono` (string), `Auth0UserId` (string), `Roles` (List<string>).
- **Servicio:** `UsuarioService` (implementa la interfaz `IUsuarioService`)
  - Dependencias: `IUsuarioRepository`, `IUsuarioActualProvider`, `IAuth0AdministracionProvider`, `IValidator<CrearUsuarioDto>`, `IValidator<ModificarUsuarioDto>` y `ILogger<UsuarioService>`.
  - Método: `Task<List<UsuarioListadoDto>> ObtenerUsuariosAsync(CancellationToken cancellationToken = default)`
  - Lógica: 
    1. Verificar que el usuario autenticado tenga el permiso `users:read`.
    2. Delegar la consulta en `IUsuarioRepository.ObtenerListadoAsync()`.
    3. Para cada usuario con `Auth0UserId`, consultar sus roles en Auth0 y mapearlos a nombres.
    4. Si falla la consulta de roles de un usuario, registrar una advertencia y devolver una lista vacía para ese usuario.
    5. Retornar la lista completa de usuarios.

- **Método auxiliar:** `Task<List<RolDto>> ObtenerRolesAsync(CancellationToken cancellationToken = default)`
  - Verifica el permiso `users:read` y devuelve los roles definidos en Auth0 (`GET /api/v2/roles`).

### 3. Capa Infrastructure (Persistencia)

- **Configuración de Entity Framework:** 
    - Reutilizar la configuración de entidad existente (`UsuarioConfiguration`).
    - No existe configuración ni tabla local de roles.
- **DbContext:** Asegurarse de exponer en `IUsuarioDbContext` el conjunto:
    - `DbSet<Usuario> Usuarios { get; set; }`
- **Repositorio:** `UsuarioRepository` implementa `IUsuarioRepository.ObtenerListadoAsync()` y encapsula la consulta; la capa Application no accede directamente al `DbContext`.
- **Integración:** `Auth0AdministracionProvider` consulta roles por usuario (`GET /api/v2/users/{id}/roles`).

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `Usuarios.razor`
  - **Ruta:** `@page "/usuarios"`
  - **Seguridad:** `@attribute [Authorize(Policy = PermisosUsuarios.PoliticaLeer)]` y `<AuthorizeView Policy="@PermisosUsuarios.PoliticaLeer">`.
  - **UI (Radzen):**
    - Barra de herramientas superior con un `RadzenButton` ("Nuevo Usuario") que redirige a `/usuarios/alta` usando `NavigationManager`.
    - Componente `RadzenDataGrid<UsuarioListadoDto>` enlazado a la lista completa de usuarios.
    -Columnas
        - `RadzenDataGridColumn` para `Nombre`, `Apellido`, `Correo` y `Telefono`.
        - `RadzenDataGridColumn` personalizada para `Roles`, desplegando la lista de nombres de Auth0 mediante `RadzenBadge`.
        - Columna de Acciones con componentes `RadzenButton` con íconos para Editar y Eliminar.
  - **Flujo:** 
    1. En el método del ciclo de vida `OnInitializedAsync`, el componente inyecta `IUsuarioService` e invoca `ObtenerUsuariosAsync()`.
    2. Los datos devueltos se asignan a la propiedad local enlazada al `RadzenDataGrid`.
    3. En caso de error durante la consulta, se captura la excepción y se muestra una alerta visual mediante `NotificationService`.