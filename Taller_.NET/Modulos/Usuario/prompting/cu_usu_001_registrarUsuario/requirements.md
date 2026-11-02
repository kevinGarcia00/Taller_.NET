# Requerimientos: Alta Usuario

## Descripción General
Como usuario con el permiso `users:create` en el sistema SolarUY, quiero poder registrar un nuevo usuario en la plataforma para otorgarle acceso al sistema según las responsabilidades y permisos de sus roles en Auth0.

## Criterios de Aceptación
1. Formulario de registro: El sistema debe permitir el ingreso de los siguientes datos obligatorios:
   - Nombre (Obligatorio).
   - Apellido (Obligatorio).
   - Correo (Obligatorio, único en el sistema).
   - Telefono (Opcional).
   - Contrasena (Obligatorio, minimo 8 caracteres, con letras y numeros).
   - Roles asignados (Obligatorio, al menos uno; se eligen entre los roles definidos en Auth0).
2. Validación de formato: El correo electrónico debe cumplir con una estructura válida (usuario@dominio.com).
3. Seguridad: Solo los usuarios autenticados mediante Auth0 que tengan el permiso `users:create` están autorizados a realizar el alta. Los permisos se leen del claim `permissions` del Access Token.
4. Seguridad: Verifica los datos ingresados para evitar inyecciones SQL.
5. Seguridad: La contrasena se define en Auth0 (proveedor de identidad) mediante su Management API; la aplicacion no la almacena.
6. Integración con Auth0: el alta realizada desde el sistema debe crear la cuenta de identidad en Auth0, asignarle los roles elegidos y luego persistir el usuario local vinculado por `Auth0UserId`.
7. Consistencia: si falla la asignación de roles o el guardado local después de crear la cuenta en Auth0, el sistema debe intentar eliminar esa cuenta para no dejar un usuario huérfano en el proveedor de identidad.

## Reglas de Negocio
- No pueden existir dos usuarios en la plataforma con el mismo correo electrónico.
- Todo usuario debe tener al menos un rol asignado en Auth0 para determinar sus permisos dentro del sistema.
- El usuario creado debe poder iniciar sesion con el correo y la contrasena registrados en Auth0.
- Los roles seleccionados se asignan exclusivamente en Auth0 (`POST /api/v2/users/{id}/roles`); la base local no guarda roles.
- Auth0 es la fuente única de identidad, roles y permisos.

## Integración con Auth0
- El alta de usuarios realizada desde la aplicación genera una cuenta en Auth0 y un registro local.
- La contraseña se envía a Auth0 y nunca se guarda en la base local.
- Los roles disponibles se obtienen de Auth0 (`GET /api/v2/roles`) y se asignan al usuario recién creado.
- Si el correo ya existe en Auth0, la Management API responde con conflicto y el alta local no se completa.
- Al iniciar sesión, Auth0 devuelve el `sub` y los permisos; la aplicación vincula el principal con el registro local por `auth0_user_id` y aplica las políticas sobre el claim `permissions`.
- Un usuario autenticado en Auth0 que no exista en la base local queda sin registro de negocio y no puede operar en las pantallas restringidas.
- `Auth0:PermitirUsuariosAnonimos=true` solo afecta la autenticación de la aplicación: reemplaza el login real por un handler anónimo con los cuatro permisos de usuarios y el rol `Administrador`.
