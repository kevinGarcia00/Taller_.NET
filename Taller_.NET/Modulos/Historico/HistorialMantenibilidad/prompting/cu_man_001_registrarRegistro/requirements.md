# Requerimientos: Alta Registro de Mantenibilidad

## Descripción General
Como usuario de Mantenimiento del sistema SolarUY, quiero registrar el mantenimiento realizado sobre un panel ya seleccionado para dejar constancia del trabajo y del estado en que quedó el panel.

## Criterios de Aceptación
1. Contexto previo: El alta solo está disponible cuando ya se seleccionó un parque solar, un nodo de ese parque y el panel sobre el que se hizo el mantenimiento.
2. Formulario de registro: El sistema debe permitir el ingreso de los siguientes datos:
   - Usuario de mantenimiento (Obligatorio).
   - Fecha (Obligatoria).
   - Estado del panel resultante (Obligatorio: Sucio, Roto, Encendido, Deteriorado o Apagado).
   - Observaciones (Obligatorias).
3. Estado inicial: El sistema toma el estado actual del panel al confirmar el alta. Ese dato no se escribe en el formulario.
4. Confirmación: Al confirmar, el sistema valida las reglas de negocio, persiste el registro y muestra un mensaje de éxito.
5. Seguridad: Solo los usuarios con el rol de "Mantenimiento" están autorizados a registrar un mantenimiento.

## Reglas de Negocio
- Todo registro pertenece al panel seleccionado, dentro del nodo y del parque solar ya elegidos.
- El usuario de mantenimiento debe existir y tener el rol "Mantenimiento".
- El estado inicial es el estado del panel en el momento del alta.
- El estado resultante debe ser uno de los valores definidos para un panel.
- La fecha del registro no puede ser posterior al día de la carga.
