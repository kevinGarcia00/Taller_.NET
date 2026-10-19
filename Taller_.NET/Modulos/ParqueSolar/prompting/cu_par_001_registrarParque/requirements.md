# Requerimientos: Registrar Parque Solar

## Descripción General
Como Administrador del sistema SolarUY, quiero registrar un parque solar para incorporarlo al sistema.

## Criterios de Aceptación
1. Contexto previo: El alta solo está disponible cuando el Administrador.
2. Formulario de registro: El sistema debe permitir el ingreso de los siguientes datos:
    - Nombre (Obligatorio).
    - Ubicacion (Obligatorio).
    - FechaCreacion (Obligatoria).
    - CantMaxNodos (Obligatoria).
3. Datos asignados por el sistema: La fecha de creación se registra al confirmar el alta y no se ingresa en el formulario.
4. Confirmación: Al confirmar, el sistema valida las reglas de negocio, persiste el panel y muestra un mensaje de éxito.
5. Seguridad: Solo los usuarios con el rol de "Administrador" están autorizados a registrar parques solares.

## Reglas de Negocio
- No pueden existir dos parques con el mismo nombre dentro del mismo nodo.
