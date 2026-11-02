# Especificaciones Técnicas: Modificar de Usuario

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.User).
- **Patrones:** Application Services (Arquitectura en capas clásica o CQRS mediante Servicios).

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.User.Domain)

- **Comportamiento para Modificación:** 

    - Metodo de dominio para actulizar datos personales: `ActualizarDatosPersonales(string nombre, string apellido, string correo, string telefono)`, ejecutando las validaciones de negocio correspondientes (no nulos, no vacíos).

    - La entidad `Usuario` **no** administra roles: los roles viven en Auth0 y se resuelven a través de `IAuth0AdministracionProvider`.

### 2. Capa Application (Servicios)

- **DTO de Entrada:** `ModificarUsuarioDto`

  - Propiedades: `Id (Guid)`, `Nombre`, `Apellido`, `Correo`, `Telefono`, `RolesIds (List<string>)`.

- **Validador:** `ModificarUsuarioDtoValidator` (usando FluentValidation)
  - Reglas: 
    - `Id` requerido (max 100).
    - `Apellido` requerida (max 100).
    - `Correo` requerido (max 100) y formato valido.
    - `RolesIds` requerido (al menos 1 rol).

- **Servicio:** `UsuarioService` (implementa la interfaz `IUsuarioService`)

  - Dependencias: `IUsuarioRepository`, `IUsuarioActualProvider`, `IAuth0AdministracionProvider`, `IValidator<ModificarUsuarioDto>`, `IValidator<CrearUsuarioDto>` y `ILogger<UsuarioService>`.

  - Métodos:
    - `Task<ModificarUsuarioDto> ObtenerParaEdicionAsync(Guid id, CancellationToken cancellationToken = default)` (verifica el permiso `users:update`, obtiene los datos del usuario y sus roles de Auth0 antes de realizar la modificacion.)
    - `Task ModificarUsuarioAsync (ModificarUsuarioDto dto, CancellationToken cancellationToken = default)` (Realiza la modificacion deseada al usuario)

  - Lógica de `ModificarUsuarioAsync(dto)`:
    1. Validar el DTO con FluentValidation y verificar que el usuario autenticado tenga el permiso `users:update`.
    2. Consultar el usuario en la BD por `Id`. Si no existe, lanzar `UsuarioNoEncontradoException`.
    3. Verificar que no exista otro usuario registrado con el mismo correo. Si existe, lanzar `CorreoUsuarioDuplicadoException`.
    4. Obtener los roles disponibles desde Auth0 y verificar que todos los `RolesIds` enviados existan; si falta alguno, lanzar `RolNoEncontradoException`.
    5. Guardar una copia de los datos anteriores (nombre, apellido, correo, teléfono) y de los roles actuales en Auth0 para poder compensar si falla la sincronización.
    6. Invocar `ActualizarDatosPersonales` sobre la entidad.
    7. Si el usuario no tiene `Auth0UserId`, persistir solo en la base local y finalizar.
    8. Si tiene `Auth0UserId`, actualizar primero la identidad en Auth0 (`PATCH /api/v2/users/{id}`) y luego reemplazar sus roles (`DELETE` + `POST /api/v2/users/{id}/roles`). Si Auth0 falla, restaurar los datos locales y relanzar la excepción sin persistir.
    9. Persistir los cambios locales con `ActualizarAsync()` + `GuardarCambiosAsync()`. Si falla el guardado local, restaurar los datos de la entidad e intentar compensar Auth0 con los valores y roles anteriores.
    10. Los roles nunca se persisten localmente.

### 3. Capa Infrastructure (Persistencia)

- **Configuración de Entity Framework:** 
    - La configuración de mapeo `UsuarioConfiguration` e índices creados para el Alta se reutilizan completamente.
    - No existe configuración ni tabla local de roles.

- **Consultas de modificacion**
   - `UsuarioRepository` encapsula las consultas y la actualización; la capa Application no accede directamente al `DbContext`.

### 4. Capa Presentation (Blazor Server con Radzen)

- **Página:** `Components/Pages/Usuarios/EditarUsuario.razor`
  - **Rutas:** `@page "/usuarios/editar"` y `@page "/usuarios/editar/{Id:guid}"`.
  - **Seguridad:** `@attribute [Authorize(Policy = PermisosUsuarios.PoliticaActualizar)]` y `<AuthorizeView Policy="@PermisosUsuarios.PoliticaActualizar">`.
  - **Responsabilidad:** resolver el `Id`, cargar el usuario con `ObtenerParaEdicionAsync` y navegar al finalizar.
- **Formulario:** `Components/Shared/Forms/Usuarios/ModificarUsuarioForm.razor`
  - **UI (Radzen):**
    - `RadzenTemplateForm` vinculado al modelo `ModificarUsuarioDto`.
    - Campos: 
        - `RadzenTextBox` para Nombre y Apellido, Telefono y Correo; el identificador se muestra de solo lectura.
        - `RadzenDropDown` con `Multiple="true"` para `RolesIds`, mostrando los roles disponibles obtenidos de Auth0 (`ObtenerRolesAsync`).
    - Botón de envío: `RadzenButton` de tipo submit.
    - Botón de cancelacion: `RadzenButton` para regresar al listado.

  - **Flujo:** 
  1. Carga Inicial (`OnInitializedAsync / OnParametersSetAsync`):
        - Obtener el parametro `Id` desde la URL (si no viene, la página permite ingresarlo manualmente).
        - Inyectar `IUsuarioService` para invocar `ObtenerParaEdicionAsync(Id)` y llenar la isntancia local de `ModificarUsuarioDto`.
        - El formulario carga los roles disponibles desde Auth0.
  2. Procesamiento del Formulario:
        - Al ejecutarse `OnValidSubmit`, invocar `IUsuarioService.ModificarUsuarioAsync(dto)`.
        - Traducir las excepciones de negocio y de Auth0 a notificaciones de error.
        - Tras completar la operación con éxito, mostrar un mensaje emergente mediante `NotificationService` de Radzen.
        - Redirigir al usuario hacia la lista general mediante `NavigationManager.NavigateTo("/usuarios")`.

- **Composición:** el registro de `UsuarioService`, el proveedor de Auth0 y las políticas de permisos se centraliza en `RegistroModuloUsuario` (ver CU-001, sección 6).

### 5. Sincronización con Auth0

- La modificación de nombre, apellido y correo se sincroniza con la Management API (`PATCH /api/v2/users/{id}`).
- La modificación de roles se sincroniza con Auth0 (`DELETE` y `POST /api/v2/users/{id}/roles`).
- Los errores de Auth0 se exponen como `Auth0IntegracionException` y la UI muestra una notificación específica.
- Los usuarios sin `Auth0UserId` (creados antes de la integración) se actualizan solo localmente.
