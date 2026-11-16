# Requerimientos: Cambiar Estado de Panel del Inversor

## Descripción General
Como sensor del sistema SolarUY desplegado en un equipo ARM, quiero recibir una solicitud para encender o apagar un panel específico del inversor y enviarla a la API de SolarUY, para que el usuario pueda cambiar su estado sin comunicarse directamente con el inversor.

## Criterios de Aceptación
1. Contexto previo: El sensor expone la operación mediante `PUT /inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}/estado`, accesible desde la interfaz web o por CURL.
2. Solicitud: El cuerpo de la solicitud indica la acción `Encender` o `Apagar`.
3. Pertenencia: Solo se permite cambiar el panel si pertenece al nodo y al inversor indicados. Si no existen o no coinciden, se responde con un mensaje de error claro.
4. Ejecución: El sensor consulta el estado actual en SolarUY, envía el cambio a la API y devuelve el estado actualizado del panel.
5. Sin cambios innecesarios: Si el panel ya está en el estado solicitado, se informa el conflicto sin repetir la operación.
6. Disponibilidad: Si la API de SolarUY no responde, el sensor informa el error sin bloquearse y sin inventar el resultado.
7. Seguridad: Solo los usuarios autenticados con rol "Administrador" pueden solicitar el cambio. El sensor se autentica contra el backend.

## Reglas de Negocio
- Toda solicitud debe tener una acción válida y un panel identificado.
- El sensor actúa como intermediario: no se comunica directamente con el hardware del inversor ni persiste el estado del panel.
- Un panel solo puede cambiarse dentro del inversor y del nodo a los que pertenece.
- La respuesta refleja el estado devuelto por SolarUY; el sensor no confirma un cambio que la API no confirmó.
- Las credenciales y la URL base de la API de SolarUY se configuran en el sensor, no se exponen al cliente.
