# Tareas de Implementación: Alta Usuario

- [x] **1. Capa Domain**
  - [x] 1.1 Crear la entidades `Usuario` y `Rol` en la capa de Dominio.
  - [x] 1.2 Agregar las propiedades de Usuario (`Id`, `Nombre`, `Apellido`, `Telefono`, `Correo`, `Roles`) y `Rol` (`Id`, `Nombre`).
  - [x] 1.3 Agregar constructor con validaciones básicas en `Usuario` y método de dominio para asociar roles.

- [x] **2. Capa Infrastructure (Persistencia)**
  - [x] 2.1 Crear la clase `UsuarioConfiguration` y `RolConfiguration` implementando `IEntityTypeConfiguration`.
  - [x] 2.2 Configurar la clave primaria bigint (long), el índice único para la propiedad `Correo`, las longitudes máximas y la relación N:N mediante la tabla intermedia (ej. `UsuarioRoles`).
  - [x] 2.2.1 Agregar la columna `auth0_user_id` (índice único) como vínculo con el proveedor de identidad.
  - [x] 2.3 Agregar la propiedad `DbSet<Usuario>` y `DbSet<Rol>` en el `IUsuarioDbContext`/`UsuarioDbContext`.
  - [x] 2.4 Generar la migración de base de datos (ej. usando el CLI de Entity Framework: `dotnet ef migrations add AddUsuarioYRol`).
  - [x] 2.5 Actualizar la base de datos (ej. `dotnet ef database update`).

- [x] **3. Capa Application**
  - [x] 3.1 Crear el DTO `UsuarioDto` (con `Contrasena`, que se envía a Auth0 y no se almacena).
  - [x] 3.2 Crear la clase `CrearUsuarioDtoValidator` usando FluentValidation.
  - [x] 3.3 Crear la interfaz `IUsuarioService` y su implementación `UsuarioService`.
  - [x] 3.4 Implementar la lógica del método de creación (verificar unicidad de correo, crear el usuario en Auth0 con su contraseña y roles, guardar el registro local y retornar Id).
  - [x] 3.4.1 Implementar el puerto `IAuth0AdministracionProvider` y su implementación `Auth0AdministracionProvider` (Management API de Auth0).
  - [x] 3.5 Crear Test Unitario para verificar que el servicio lanza un error si el correo del usuario ya existe.

- [x] **4. Capa Presentation (Blazor Server con Radzen)**
  - [x] 4.1 Crear el componente `AltaUsuario.razor` con la directiva `@page "/usuarios/alta"`.
  - [x] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [x] 4.3 Diseñar el formulario con `<RadzenTemplateForm>` y los controles de entrada (`RadzenTextBox`, para texto/correo, `RadzenPassword` para la contraseña y `RadzenDropDown` para la selección de roles).
  - [x] 4.4 Implementar el método `OnValidSubmit` para invocar el método correspondiente en `IUsuarioService`.
  - [x] 4.5 Mostrar mensaje de éxito usando el `NotificationService` de Radzen y redirigir al listado de usuarios (`/usuarios`).

- [x] **5. Verificación de Calidad (Guardarraíles)**
  - [x] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [x] 5.2 Ejecutar inspección de código para asegurar que no hay code smells.

## Notas de implementación

- **`Id` de `Usuario`**: es un `Guid` generado por la aplicación (el enunciado-plantilla propone `long`/`Guid` indistintamente). El vínculo con la identidad de Auth0 se hace con la columna `auth0_user_id` (el `user_id` de Auth0).
- **Contraseña**: se define en Auth0 mediante su Management API y **no se guarda en la base de la aplicación**. El alta del usuario en el dominio crea la cuenta en Auth0 (con la contraseña y los roles indicados) y luego persiste el registro local vinculado.
- **Patrón de repositorio**: el servicio depende del puerto `IUsuarioRepository` (definido en `Application/Puetertos`) y no de `IUsersDbContext`, porque AGENTS.md prohibe que la capa Application dependa de Infrastructure. La implementación `UsuarioRepository` vive en `Infrastructure/Repositorios`.
- **Entity Framework**: la migración `AddUsuarioYRol` quedó en `Modulos/Usuario/Infrastructure/Persistencia/Migraciones`. La tabla intermedia `UsuarioRoles` se crea con borrado en cascada y los roles `Administrador` y `Mantenimiento` se siembran por `HasData`.
- **Errores de validación en la UI**: el formulario reutiliza el mismo `CrearUsuarioDtoValidator` de FluentValidation y vuelca los errores en el `EditContext` del `RadzenTemplateForm`, de modo que cada campo muestra su mensaje y queda marcado como inválido. El `ValidationMessageStore` se conserva como instancia única por `EditContext`: en .NET cada store se asocia a los campos y crear uno nuevo por llamada dejaba los mensajes anteriores colgados, lo que bloqueaba el reenvío del formulario.
- **Host de Radzen**: el `<RadzenComponents />` de la página `/usuarios/alta` se ubica dentro de la página interactiva para que los avisos y modales compartan el scope del circuito con el formulario.
- **Pruebas**: `Tests/Modulos/Usuario/Domain/UsuarioTests.cs` y `RolTests.cs` cubren las validaciones del constructor, el formateo y normalización del correo, el largo máximo de los campos y el método de dominio `AsignarRol`. Suite completa: **74 pruebas en verde**, incluidas las 2 de arquitectura (ArchUnitNET).
- **Configuración de la Management API**: requiere una aplicación *Machine to Machine* de Auth0 con el scope `create:users` (crear el usuario). Los roles se envían en `app_metadata` al crear el usuario, que es donde Auth0 los guarda, por lo que no hace falta el endpoint adicional de asignación de roles. **NOTA: La implementación de Auth0 aún no está completa, esto se reintegrará después**. Para el caso de uso CU-003 (eliminar) hará falta además `delete:users`. Se define con `dotnet user-secrets set "Auth0:ManagementClientId" "..."` y `dotnet user-secrets set "Auth0:ManagementClientSecret" "..."`.
- **Roles en Auth0**: los roles deben existir en el tenant (Administrador, Mantenimiento) para que la Management API pueda asignarlos al usuario.
