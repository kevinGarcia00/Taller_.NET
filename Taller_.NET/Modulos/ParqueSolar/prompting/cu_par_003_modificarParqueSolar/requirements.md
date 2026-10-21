# Requerimientos: Modificar Parque Solar

## Descripción General
Como Administrador del sistema SolarUY, quiero modificar un parque solar existente en el sistema.

## Criterios de Aceptación
1. Contexto previo: La modificacion solo está disponible cuando el Administrador.
2. Formulario de registro: El sistema debe permitir la modificacion de los siguientes datos:
    - Nombre (Obligatorio).
    - Ubicacion (Obligatorio).
    - CantMaxNodos (Obligatoria).
3. Confirmación: Al confirmar, el sistema valida las reglas de negocio, persiste el parque y muestra un mensaje de éxito.
4. Seguridad: Solo los usuarios con el rol de "Administrador" están autorizados a modificar parques solares.

## Reglas de Negocio
- No pueden existir dos parques con el mismo nombre.
