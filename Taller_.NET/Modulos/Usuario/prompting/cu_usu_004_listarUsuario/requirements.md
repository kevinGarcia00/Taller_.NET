# Requerimientos: Listar Usuarios

## Descripción General
Como Administrador del sistema SolarUY, quiero consultar la lista general de usuarios registrados para gestionar sus datos, verificar sus roles asignados y controlar su acceso a la plataforma.

## Criterios de Aceptación
1. Visualización de Datos: El sistema debe presentar un listado tabular que incluya los campos clave de los usuarios: Nombre, Apellido, Correo, Teléfono, Rol(es) asignado(s).
4. Seguridad: El acceso al listado general debe estar restringido exclusivamente a usuarios con el rol "Administrador".
5. Navegación a Acciones: Cada registro de la tabla debe incluir accesos directos para realizar las acciones asociadas sobre el usuario seleccionado (ej. Editar, Eliminar).

## Reglas de Negocio
- Solo los usuarios con el rol "Administrador" están autorizados a visualizar el directorio completo de usuarios del sistema.

