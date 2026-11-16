# Requerimientos: Controlar Volcado de Energía del Panel

## Descripción General
Como sensor del sistema SolarUY desplegado en un equipo ARM, quiero recibir una solicitud para habilitar o deshabilitar el volcado de energía de un panel específico del inversor y enviarla a la API de SolarUY, para que el usuario pueda controlar esa función sin comunicarse directamente con el inversor.

## Criterios de Aceptación
1. Contexto previo: El sensor expone la operación mediante `PUT /inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/volcado`, accesible desde la interfaz web o por CURL.
2. Solicitud: El cuerpo de la solicitud indica el valor booleano `volcarEnergia`, por ejemplo `{ "volcarEnergia": true }`.
3. Pertenencia: Solo se permite cambiar el panel si pertenece al nodo y al inversor indicados. Si no existen o no coinciden, se responde con un mensaje de error claro.
4. Ejecución: El sensor consulta el estado actual en SolarUY, envía el cambio a la API y devuelve el panel actualizado con su atributo `volcarEnergia`.
5. Sin cambios innecesarios: Si el panel ya tiene el valor solicitado, se informa el conflicto sin repetir la operación.
6. Edición: El panel guarda el atributo `volcarEnergia` como booleano y la pantalla de modificación del panel en SolarUY permite editarlo.
7. Disponibilidad: Si la API de SolarUY no responde, el sensor informa el error sin bloquearse y sin inventar el resultado.

## Reglas de Negocio
- Toda solicitud debe indicar el valor deseado de `volcarEnergia`.
- El sensor actúa como intermediario: no se comunica directamente con el hardware del inversor ni persiste el valor del panel.
- Un panel solo puede cambiarse dentro del inversor y del nodo a los que pertenece.
- El alta del panel inicia `volcarEnergia` en `false` y la edición permite cambiarlo.
- La respuesta refleja el valor devuelto por SolarUY; el sensor no confirma un cambio que la API no confirmó.
- Las credenciales y la URL base de la API de SolarUY se configuran en el sensor, no se exponen al cliente.
