# Requerimientos: Modificacion de Usuario

## Descripción General
Como Administrador del sistema SolarUY, quiero modificar los datos generales y los permisos de un usuario registrado para mantener actualizada la información de acceso a la plataforma.

## Criterios de Aceptación
1. Selección de entidad: El sistema debe permitir seleccionar o ingresar el id del usuario a modificar. Si no existe, debe mostrar un mensaje de error descriptivo.
2. Precarga de datos: Al seleccionar un usuario válido, el sistema debe presentar un formulario precargado con sus datos actuales (Nombre, Apellido, Correo, Rol y Telefono).
3. Campos editables: El Administrador podrá modificar el Nombre, Apellido, Correo, Rol asignado y el Telefono del usuario.
4. Confirmación y guardado: Al confirmar los cambios, el sistema valida las reglas de negocio, actualiza la base de datos y despliega un mensaje de éxito.
5. Seguridad: Solo los usuarios autenticados con el rol de "Administrador" están autorizados a realizar esta acción.

## Reglas de Negocio
- Si se habilita la edición del correo electrónico, el nuevo valor no debe coincidir con el correo de ningún otro usuario registrado.
- El identificador único del usuario (id) es de solo lectura y no puede modificarse.
