# Requerimientos: Listar Usuarios

## Descripción General
Como usuario con el permiso `users:read` en el sistema SolarUY, quiero consultar la lista general de usuarios registrados para gestionar sus datos, verificar sus roles asignados y controlar su acceso a la plataforma.

## Criterios de Aceptación
1. Visualización de Datos: El sistema debe presentar un listado tabular que incluya los campos clave de los usuarios: Nombre, Apellido, Correo, Teléfono, Rol(es) asignado(s).
2. Seguridad: El acceso al listado general debe estar restringido exclusivamente a usuarios autenticados mediante Auth0 que tengan el permiso `users:read`. Los permisos se leen del claim `permissions` del Access Token.
3. Navegación a Acciones: Cada registro de la tabla debe incluir accesos directos para realizar las acciones asociadas sobre el usuario seleccionado (ej. Editar, Eliminar).
4. Origen de los datos: el listado de datos personales se obtiene de la base local; los roles de cada usuario se consultan a Auth0 (`GET /api/v2/users/{id}/roles`).

## Reglas de Negocio
- Solo los usuarios con el permiso `users:read` están autorizados a visualizar el directorio completo de usuarios del sistema.
- Los roles mostrados son los asignados en Auth0; la base local no almacena roles.
- Si Auth0 no responde al consultar los roles de un usuario, el listado se muestra sin roles para ese usuario y se registra una advertencia.
