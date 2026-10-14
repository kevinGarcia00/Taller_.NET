# Especificaciones Técnicas: Modificar de Usuario

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.User).
- **Patrones:** Application Services (Arquitectura en capas clásica o CQRS mediante Servicios).

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.User.Domain)

- **Comportamiento para Modificación:** 

    - Metodo de dominio para actulizar datos personales: `ActualizarDatosPersonales(string nombre, string apellido, string correo, string telefono)`, ejecutando las validaciones de negocio correspondientes (no nulos, no vacíos).

    - Método de dominio para actualizar roles: `ActualizarRoles(IEnumerable<Rol> nuevosRoles)`, el cual limpia la colección actual y asigna las nuevas entidades Rol para garantizar la consistencia en la relación N:N.

### 2. Capa Application (Servicios)

- **DTO de Entrada:** `ModificarUsuarioDto`

  - Propiedades: `Id (long)`, `Nombre`, `Apellido`, `Correo`, `Telefono`, `RolesIds (List<long>)`.

- **Validador:** `ModificarUsuarioDtoValidator` (usando FluentValidation)
  - Reglas: 
    - `Id` requerido (max 100).
    - `Apellido` requerida (max 100).
    - `Correo` requerido (max 100) y formato valido.
    - `RolesIds` requerido (al menos 1 ID de rol).

- **Servicio:** `UsuarioService` (implementa la interfaz `IUsuarioService`)

  - Métodos:
    - `Task<ModificarUsuarioDto> ObtenerParaEdicionAsync(long id)` (Obtiene los datos del usuario antes de realizar la modificacion para motrar los mismos.)
    - `Task ModificarUsuarioAsync (ModificarUsuarioDto dto)` (Realiza la modificacion deseada al usuario)

  - Lógica de `ModificarUsuarioAsync(dto)`:
    1. Consulta el usuario en la BD por `Id` incluyendo sus roles asociados. Si no existe, lanzar excepción.
    2. Verificar que no exista otro usuario registrado con el mismo correo. Si existe, lanzar excepción.
    3. Consultar la base de datos para verificar que todos los RolesIds enviados existan.
    4. Invocar los métodos del dominio sobre la entidad para actualizar sus datos personales y refrescar la colección de roles.
    5. Ejecutar SaveChangesAsync() (Entity Framework gestionará las diferencias en la tabla intermedia mediante Change Tracking).

### 3. Capa Infrastructure (Persistencia)

- **Configuración de Entity Framework:** 
    - Las configuraciones de mapeo (`UsuarioConfiguration` y `RolConfiguration`) e índices creados para el Alta se reutilizan completamente.

- **Consultas de modificacion**
   - Asegurar que la implementación de lectura en UsuarioService cargue explícitamente las relaciones requeridas para que EF Core identifique correctamente las altas y bajas en la tabla intermedia (`UsuarioRoles`) al momento de hacer `SaveChangesAsync()`.

### 4. Capa Presentation (Blazor Server con Radzen)

- **Componente:** `EditarUsuario.razor`
  - **Ruta:** `@page "/usuarios/editar/{Id:Long}"`
  - **Seguridad:** Utilizar el componente `<AuthorizeView Roles="Administrador">` para proteger el contenido o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Formulario `RadzenTemplateForm` vinculado al modelo `ModificarUsuarioDto`.
    - Campos: 
        - `RadzenTextBox` para Nombre y Apellido, Telefono y Correo.
        - `RadzenDropDown` configurado con seleccion multiple para la propiedad `RolesIds`, mostrando los roles disponibles
    - Botón de envío: `RadzenButton` de tipo submit.
    - Botón de cancelacion: `RadzenButton` para regresar al listado.

  - **Flujo:** 
  1. Carga Inicial (`OnInitializedAsync / OnParametersSetAsync`):
        - Obtener el parametro `Id` desde la URL.
        - Inyectar `IUsuarioService` para invocar `ObtenerParaEdicionAsync(Id)` y llenar la isntancia local de `ModificarUsuarioDto`.
  2. Procesamiento del Formulario:
        - Al ejecutarse `OnValidSubmit`, invocar `IUsuarioService.ModificarUsuarioAsync(dto)`.
        - Tras completar la operación con éxito, mostrar un mensaje emergente mediante `NotificationService` de Radzen.
        - Redirigir al usuario hacia la lista general mediante `NavigationManager.NavigateTo("/usuarios")`.