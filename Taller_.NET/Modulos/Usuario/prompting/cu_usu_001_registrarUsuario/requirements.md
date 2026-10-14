# Requerimientos: Alta Usuario

## Descripción General
Como Administrador del sistema SolarUY, quiero poder registrar un nuevo usuario en la plataforma para otorgarle acceso al sistema según las responsabilidades y permisos de su rol.

## Criterios de Aceptación
1. Formulario de registro: El sistema debe permitir el ingreso de los siguientes datos obligatorios:
   - Nombre (Obligatorio).
   - Apellido (Obligatorio).
   - Correo (Obligatorio, único en el sistema).
   - Telefono (Opcional).
   - Rol asignado (Obligatorio, p. ej., Administrador, Mantenimiento).
2. Validación de formato: El correo electrónico debe cumplir con una estructura válida (usuario@dominio.com).
3. Seguridad: Solo los usuarios con el rol de "Administrador" están autorizados a realizar el alta de nuevos usuarios.

## Reglas de Negocio
- No pueden existir dos usuarios en la plataforma con el mismo correo electrónico.
- Todo usuario debe tener al menos un rol asignado para determinar sus permisos dentro del sistema.

