# Tareas de Implementación: Alta Usuario

- [x] **1. Capa Domain**
  - [x] 1.1 Crear la entidad `Usuario` en la capa de Dominio (sin entidad `Rol` local).
  - [x] 1.2 Agregar las propiedades de Usuario (`Id`, `Nombre`, `Apellido`, `Telefono`, `Correo`, `Auth0UserId`).
  - [x] 1.3 Agregar constructor con validaciones básicas en `Usuario` y el método de dominio `VincularConAuth0`.

- [x] **2. Capa Infrastructure (Persistencia)**
  - [x] 2.1 Crear la clase `UsuarioConfiguration` implementando `IEntityTypeConfiguration`.
  - [x] 2.2 Configurar la clave primaria `Guid`, el índice único para la propiedad `Correo` y las longitudes máximas.
  - [x] 2.2.1 Agregar la columna `auth0_user_id` (índice único) como vínculo con el proveedor de identidad.
  - [x] 2.3 Agregar la propiedad `DbSet<Usuario>` en el `IUsuarioDbContext`/`UsuarioDbContext`.
  - [x] 2.4 Generar la migración de base de datos que elimina las tablas locales `UsuarioRoles` y `Roles`.
  - [x] 2.5 Actualizar la base de datos (ej. `dotnet ef database update`).
  - [x] 2.6 Implementar el puerto `IAuth0AdministracionProvider` y su implementación `Auth0AdministracionProvider` (Management API de Auth0).

- [x] **3. Capa Application**
  - [x] 3.1 Crear el DTO `CrearUsuarioDto` (con `Contrasena`, que se envía a Auth0 y no se almacena; `RolesIds` como `List<string>` de Auth0).
  - [x] 3.2 Crear la clase `CrearUsuarioDtoValidator` usando FluentValidation.
  - [x] 3.3 Crear la interfaz `IUsuarioService` y su implementación `UsuarioService`.
  - [x] 3.4 Implementar la lógica del método de creación (verificar unicidad de correo, validar roles en Auth0, crear la cuenta, asignar roles y guardar el registro local).
  - [x] 3.4.1 Implementar `Auth0AdministracionProviderAnonimo` para el modo `PermitirUsuariosAnonimos`, sin llamadas a Auth0.
  - [x] 3.4.2 Implementar la compensación: si falla la asignación de roles o el guardado local, eliminar de Auth0 la cuenta recién creada.
  - [x] 3.4.3 Definir `PermisosUsuarios` y validar el permiso `users:create` desde el claim `permissions`.
  - [x] 3.5 Crear Test Unitario para verificar que el servicio lanza un error si el correo del usuario ya existe.

- [x] **4. Capa Presentation (Blazor Server con Radzen)**
  - [x] 4.1 Crear el componente `AltaUsuario.razor` con la directiva `@page "/usuarios/alta"`.
  - [x] 4.2 Restringir el acceso con `[Authorize(Policy = PermisosUsuarios.PoliticaCrear)]` y `<AuthorizeView Policy=...>`.
  - [x] 4.3 Diseñar el formulario con `<RadzenTemplateForm>` y los controles de entrada (`RadzenTextBox`, para texto/correo, `RadzenPassword` para la contraseña y `RadzenDropDown` para la selección múltiple de roles).
  - [x] 4.4 Implementar el método `OnValidSubmit` para invocar el método correspondiente en `IUsuarioService`.
  - [x] 4.5 Mostrar mensaje de éxito usando el `NotificationService` de Radzen y redirigir al listado de usuarios (`/usuarios`).

- [x] **5. Inicialización del módulo y Auth0**
  - [x] 5.1 Registrar `InicializadorDatosUsuario` en el contenedor de dependencias del módulo.
  - [x] 5.2 Invocar `InicializarModuloUsuarioAsync()` al arrancar, después de `builder.Build()`.
  - [x] 5.3 Garantizar el registro inicial del Administrador vinculado a Auth0.
  - [x] 5.4 Completar el vínculo con Auth0 si el usuario ya existe, sin duplicarlo. Los roles se administran en Auth0.

- [x] **6. Verificación de Calidad (Guardarraíles)**
  - [x] 6.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [x] 6.2 Ejecutar inspección de código para asegurar que no hay code smells.

- [x] **7. Composición del módulo**
  - [x] 7.1 Centralizar el registro de dependencias en `RegistroModuloUsuario.AgregarModuloUsuario`.
  - [x] 7.2 Registrar las políticas de permisos (`Usuarios:Leer/Crear/Actualizar/Eliminar`) en ese mismo punto.
  - [x] 7.3 Elegir `Auth0AdministracionProviderAnonimo` cuando `Auth0:PermitirUsuariosAnonimos=true` y el proveedor real en caso contrario.
  - [x] 7.4 Exponer `InicializarModuloUsuarioAsync` para garantizar el administrador inicial.

