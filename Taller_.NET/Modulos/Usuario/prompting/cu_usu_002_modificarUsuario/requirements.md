# Requerimientos: Modificacion de Usuario

## Descripción General
Como usuario con el permiso `users:update` en el sistema SolarUY, quiero modificar los datos generales y los roles de un usuario registrado para mantener actualizada la información de acceso a la plataforma.

## Criterios de Aceptación
1. Selección de entidad: El sistema debe permitir seleccionar o ingresar el id del usuario a modificar. Si no existe, debe mostrar un mensaje de error descriptivo.
2. Precarga de datos: Al seleccionar un usuario válido, el sistema debe presentar un formulario precargado con sus datos actuales (Nombre, Apellido, Correo, Roles y Telefono). Los roles se consultan a Auth0.
3. Campos editables: El usuario autorizado podrá modificar el Nombre, Apellido, Correo, los roles asignados y el Telefono del usuario.
4. Confirmación y guardado: Al confirmar los cambios, el sistema valida las reglas de negocio, actualiza Auth0 y la base de datos, y despliega un mensaje de éxito.
5. Seguridad: Solo los usuarios autenticados mediante Auth0 que tengan el permiso `users:update` están autorizados a realizar esta acción. Los permisos se leen del claim `permissions` del Access Token.
6. Sincronización con Auth0: si el usuario tiene `Auth0UserId`, los cambios de nombre, apellido, correo y roles deben actualizarse también en Auth0 antes de persistir los cambios locales.

## Reglas de Negocio
- Si se habilita la edición del correo electrónico, el nuevo valor no debe coincidir con el correo de ningún otro usuario registrado.
- El identificador único del usuario (id) es de solo lectura y no puede modificarse.
- Los roles se modifican exclusivamente en Auth0; la base local no almacena roles.
- Si la actualización en Auth0 falla, no se deben persistir los cambios locales.
- Si Auth0 se actualiza pero falla el guardado local, se restauran los datos locales y se intenta compensar Auth0 con los valores y roles anteriores.
- Los usuarios creados antes de la integración, sin `Auth0UserId`, se modifican solo en la base local y no se les asignan roles.

## Integración con Auth0
- Auth0 es la fuente única de identidad, roles y permisos.
- La modificación de datos personales sincroniza con la Management API de Auth0 (`PATCH /api/v2/users/{id}`).
- El cambio de roles se sincroniza con Auth0 mediante `DELETE /api/v2/users/{id}/roles` y `POST /api/v2/users/{id}/roles`.
