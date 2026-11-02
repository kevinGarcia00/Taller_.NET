# Requerimientos: Eliminar Usuario

## Descripción General
Como usuario con el permiso `users:delete` en el sistema SolarUY, quiero poder eliminar un usuario registrado en la plataforma para eliminar su acceso al sistema cuando ya no forme parte de la organización o no requiera sus permisos.

## Criterios de Aceptación
1. Confirmación de Acción: Al seleccionar la opción de eliminar un usuario desde la interfaz, el sistema debe mostrar un diálogo modal de confirmación (ej. «¿Está seguro de que desea eliminar al usuario [Nombre Apellido]?»).
2. Seguridad: Solo los usuarios autenticados mediante Auth0 que tengan el permiso `users:delete` están autorizados a realizar la eliminación. Los permisos se leen del claim `permissions` del Access Token.
3. Notificación y Actualización: Tras una eliminación exitosa, el sistema debe mostrar una notificación mediante el `NotificationService` y refrescar el listado general de usuarios de forma inmediata.
4. Manejo de Errores: Si la eliminación falla por reglas de negocio o problemas del servidor, se debe notificar al usuario sin romper la interfaz.
5. Sincronización con Auth0: si el usuario tiene `Auth0UserId`, primero se elimina su cuenta en Auth0 y luego el registro local. Si la cuenta ya no existe en Auth0, la operación remota se considera exitosa.

## Reglas de Negocio
- Un usuario no puede eliminarse a sí mismo del sistema mientras mantenga su sesión activa. La identificación se hace por el `Auth0UserId` del usuario autenticado y, como respaldo, por su correo.
- No se permite eliminar al único usuario existente con el rol "Administrador". La cantidad se consulta a Auth0.
- Los roles se administran exclusivamente en Auth0; la base local no los almacena.
- Si Auth0 se elimina correctamente pero falla la eliminación local, se registra el error y el registro local se conserva para poder reintentar la operación.

## Integración con Auth0
- La eliminación de la cuenta de identidad se realiza con `DELETE /api/v2/users/{id}`.
- Los roles del usuario se eliminan junto con su cuenta en Auth0; no existe tabla local de roles.
- Los usuarios sin `Auth0UserId` solo se eliminan localmente.
