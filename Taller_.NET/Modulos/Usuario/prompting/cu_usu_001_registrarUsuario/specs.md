# Especificaciones Técnicas: Alta de Usuario

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.User).
- **Patrones:** Application Services (Arquitectura en capas clásica o CQRS mediante Servicios).

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.User.Domain)
- **Entidades:** 
    - **Usuario**
        - **Propiedades:** 
            - `Id` (Guid, PK)
            - `Nombre` (string, max 100 caracteres)
            - `Apellido` (string, max 100 caracteres)
            - `Correo` (string, max 100 caracteres)
            - `Telefono` (string, max 25 caracteres)
            - `Auth0UserId` (string, max 100 caracteres)
        - **Nota:** el dominio no modela roles; Auth0 es la fuente única de identidad, roles y permisos.

- **Comportamiento:** 
    - Constructor con validaciones básicas (por ejemplo, `Nombre` y `Correo` no pueden ser null o whitespace).
    - Método de dominio `VincularConAuth0(string auth0UserId)` que valida el identificador y lo asigna al usuario.

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `CrearUsuarioDto`
  - Propiedades: `Nombre`, `Apellido`, `Correo`, `Telefono`, `Contrasena`, `RolesIds (List<string>)`.
- **Validador:** `CrearUsuarioDtoValidator` (usando FluentValidation)
  - Reglas: `Nombre` requerido (max 100), `Apellido` requerida (max 100), `Correo` requerido (max 100) y formato valido, `Contrasena` requerida (min 8, max 128, letras y numeros), `RolesIds` requerido (al menos 1 rol).
- **Seguridad:** `PermisosUsuarios` define los permisos (`users:read`, `users:create`, `users:update`, `users:delete`) y las políticas (`Usuarios:Leer`, `Usuarios:Crear`, `Usuarios:Actualizar`, `Usuarios:Eliminar`). El claim leído es `permissions`.
- **Servicio:** `UsuarioService` (implementa la interfaz `IUsuarioService`)
  - Dependencias: `IUsuarioRepository`, `IUsuarioActualProvider`, `IAuth0AdministracionProvider` (Management API de Auth0), `IValidator<CrearUsuarioDto>`, `IValidator<ModificarUsuarioDto>` y `ILogger<UsuarioService>`.
  - Método: `CrearUsuarioAsync(CrearUsuarioDto dto, CancellationToken cancellationToken = default)`
  - Lógica: 
    1. Validar el DTO con FluentValidation y verificar que el usuario autenticado tenga el permiso `users:create`.
    2. Validar que no exista un Usuario registrado con el mismo Correo.
    3. Obtener los roles disponibles desde Auth0 (`GET /api/v2/roles`) y verificar que todos los `RolesIds` enviados existan. Si alguno no existe, lanzar `RolNoEncontradoException`.
    4. Crear la cuenta en Auth0 con la contraseña informada (Management API, `POST /api/v2/users`) y obtener el `user_id` devuelto.
    5. Asignar los roles al usuario en Auth0 (`POST /api/v2/users/{id}/roles`).
    6. Instanciar la entidad Usuario, vincularla con el `auth0_user_id` y agregarla al repositorio.
    7. Ejecutar `GuardarCambiosAsync()`.
    8. Si falla la asignación de roles o la persistencia local, intentar eliminar la cuenta recién creada en Auth0 con `CancellationToken.None` para que la compensación se ejecute aunque la petición original se haya cancelado.
    9. Retornar el Id generado.

  La contraseña se envía a Auth0 y **nunca se guarda en la base local**. Los roles se administran exclusivamente en Auth0.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:** 
- `UsuarioConfiguration : IEntityTypeConfiguration<Usuario>`
  - Definir la tabla y longitud máxima de columnas.
  - Configurar el Id de `Usuario` como clave primaria de tipo `Guid`.
  - Crear un Índice Único para la columna Correo para garantizar la regla de negocio a nivel de BD.
  - Configurar `Auth0UserId` (columna `auth0_user_id`, índice único) como vínculo con el proveedor de identidad.
  - No existe configuración ni tabla local de roles.
- **DbContext:** Asegurarse de exponer en `IUsuarioDbContext` el conjunto:
    - `DbSet<Usuario> Usuarios { get; set;} `
- **Migraciones:** Se deberá generar y aplicar una migración para reflejar estos cambios en la base de datos. La migración `EliminarRolesLocales` retira las tablas `UsuarioRoles` y `Roles`.
- **Puerto de integración:** `IAuth0AdministracionProvider` expone la creación/actualización/eliminación de usuarios y la consulta, asignación y conteo de roles en Auth0.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Página:** `Components/Pages/Usuarios/AltaUsuario.razor`
  - **Ruta:** `@page "/usuarios/alta"`
  - **Seguridad:** `@attribute [Authorize(Policy = PermisosUsuarios.PoliticaCrear)]` y `<AuthorizeView Policy="@PermisosUsuarios.PoliticaCrear">`. Si el usuario autenticado no tiene el permiso, se muestra el bloque `NotAuthorized` con el detalle de `users:create`.
  - **Responsabilidad:** solo compone la página y resuelve la navegación posterior al alta.
- **Formulario:** `Components/Shared/Forms/Usuarios/AltaUsuarioForm.razor`
  - **UI (Radzen):**
    - `RadzenTemplateForm` vinculado al modelo `CrearUsuarioDto`.
    - Campos: 
        - `RadzenTextBox` para Nombre y Apellido, Telefono y Correo.
        - `RadzenPassword` para la contraseña.
        - `RadzenDropDown` con `Multiple="true"` para `RolesIds`, mostrando los roles disponibles obtenidos de Auth0 (`ObtenerRolesAsync`).
    - Botón de envío: `RadzenButton` de tipo submit.
  - **Flujo:** 

  1. En `OnInitializedAsync` carga los roles disponibles desde Auth0 con `IUsuarioService.ObtenerRolesAsync()`.
  2. En `OnValidSubmit` valida con FluentValidation y ejecuta `IUsuarioService.CrearUsuarioAsync(dto)`.
  3. Traduce las excepciones de negocio (`CorreoUsuarioDuplicadoException`, `RolNoEncontradoException`, `UsuarioNoAutorizadoException`, `Auth0IntegracionException`, `UsuarioInvalidoException`) a notificaciones con `NotificationService`.
  4. Tras el éxito, muestra la notificación y dispara `OnRegistrado`; la página redirige a `/usuarios` con `NavigationManager`.

### 5. Inicialización del administrador

- Al arrancar, `Program.cs` invoca `app.Services.InicializarModuloUsuarioAsync()` después de `builder.Build()`.
- `InicializadorDatosUsuario` verifica la base local y garantiza el registro del administrador vinculado a Auth0.
- El inicializador **no crea la cuenta en Auth0** ni le asigna roles: solo crea o completa el registro local de un usuario que debe existir previamente en Auth0.
- Si el usuario ya existe, no lo duplica; si existe por correo sin vínculo, completa el `Auth0UserId`. Los roles del administrador se administran en Auth0.

### 6. Composición del módulo (Infrastructure)

- `Modulos/Usuario/Infrastructure/RegistroModuloUsuario.cs` es el punto de composición interno del módulo. `Program.cs` solo invoca `builder.Services.AgregarModuloUsuario(builder.Configuration)` y `await app.Services.InicializarModuloUsuarioAsync()`.
- `AgregarModuloUsuario` registra:
  - `UsuarioDbContext` (SQLite), `IUsuarioDbContext` e `IUsuarioRepository`.
  - `IUsuarioService`, `IUsuarioAutenticacionService`, `IUsuarioActualProvider` e `InicializadorDatosUsuario`.
  - Los validadores `CrearUsuarioDtoValidator` y `ModificarUsuarioDtoValidator`.
  - `IAuth0AdministracionProvider` real o `Auth0AdministracionProviderAnonimo` según `Auth0:PermitirUsuariosAnonimos`.
  - Las políticas `Usuarios:Leer`, `Usuarios:Crear`, `Usuarios:Actualizar` y `Usuarios:Eliminar`, cada una con `RequireAuthenticatedUser()` y `RequireClaim("permissions", ...)`.
- `InicializarModuloUsuarioAsync` crea un scope propio y ejecuta `AsegurarAdministradorInicialAsync`.
- Esto evita que el punto de composición conozca clases concretas de `Infrastructure` y mantiene encapsulado el cableado del módulo.
