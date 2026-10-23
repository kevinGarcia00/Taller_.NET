# Requerimientos: Alta Usuario

## Descripción General
Como usuario con rol en el sistema SolarUY, quiero poder registrar un nuevo usuario en la plataforma para otorgarle acceso al sistema según las responsabilidades y permisos de su rol.

## Criterios de Aceptación
1. Formulario de registro: El sistema debe permitir el ingreso de los siguientes datos obligatorios:
   - Nombre (Obligatorio).
   - Apellido (Obligatorio).
   - Correo (Obligatorio, único en el sistema).
   - Telefono (Opcional).
   - Contrasena (Obligatorio, minimo 8 caracteres, con letras y numeros).
   - Rol asignado (Obligatorio, p. ej., Administrador, Mantenimiento).
2. Validación de formato: El correo electrónico debe cumplir con una estructura válida (usuario@dominio.com).
3. Seguridad: Solo los usuarios autenticados mediante Auth0 (pending implementation) y con el rol de "Administrador" están autorizados a realizar el alta de nuevos usuarios
4. Seguridad: Verifica los datos ingresados para evitar inyecciones SQL.
5. Seguridad: La contrasena se define en Auth0 (proveedor de identidad) mediante su Management API; la aplicacion no la almacena.

## Reglas de Negocio
- No pueden existir dos usuarios en la plataforma con el mismo correo electrónico.
- Todo usuario debe tener al menos un rol asignado para determinar sus permisos dentro del sistema.
- El usuario creado debe poder iniciar sesion con el correo y la contrasena registrados en Auth0.
- Los roles seleccionados deben existir en Auth0 para que el usuario pueda operar segun el claim `https://solaryu.com/roles`.

