# Requerimientos: Eliminar Usuario

## Descripción General
Como Administrador del sistema SolarUY, quiero poder eliminar un usuario registrado en la plataforma para eliminar su acceso al sistema cuando ya no forme parte de la organización o no requiera sus permisos.

## Criterios de Aceptación
1. Confirmación de Acción: Al seleccionar la opción de eliminar un usuario desde la interfaz, el sistema debe mostrar un diálogo modal de confirmación (ej. «¿Está seguro de que desea eliminar al usuario [Nombre Apellido]?»).
2. Seguridad: Solo los usuarios con el rol de "Administrador" están autorizados a realizar la eliminación de usuarios.
3. Notificación y Actualización: Tras una eliminación exitosa, el sistema debe mostrar una notificación mediante el `NotificationService` y refrescar el listado general de usuarios de forma inmediata.
4. Manejo de Errores: Si la eliminación falla por reglas de negocio o problemas del servidor, se debe notificar al Administrador sin romper la interfaz.

## Reglas de Negocio
- Un Administrador no puede eliminarse a sí mismo del sistema mientras mantenga su sesión activa.
- No se permite eliminar al único usuario existente con el rol "Administrador".


