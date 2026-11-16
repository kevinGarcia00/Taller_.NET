# Requerimientos: Recuperar Inversor

## Descripción General
Como sensor del sistema SolarUY desplegado en un equipo ARM, quiero consultar a la API de SolarUY los datos de un inversor específico por su identificador, para devolver esa información al backend o al usuario sin que este se comunique directamente con el inversor.

## Criterios de Aceptación
1. Contexto previo: El sensor expone la consulta mediante `GET /api/inversor/{idInversor}`, accesible desde la interfaz web o por CURL.
2. Datos del inversor: Al invocar la URL, el sensor consulta la API de SolarUY y responde con `id`, `nombre` y `fechaCreacion` del inversor.
3. Uptime: La respuesta incluye el tiempo que lleva activo el sensor, calculado desde su inicio, junto con la fecha de la consulta.
4. Existencia: Si el inversor no existe, se responde con un mensaje de error claro.
5. Sin listados: La operación devuelve solo los datos del inversor solicitado; no incluye nodos, paneles ni históricos.
6. Disponibilidad: Si la API de SolarUY no responde, el sensor informa el error sin bloquearse y sin inventar datos.

## Reglas de Negocio
- La consulta es de solo lectura, no modifica los datos del inversor en SolarUY.
- El sensor actúa como intermediario: no se comunica directamente con el hardware del inversor ni persiste sus datos.
- El identificador del inversor debe ser mayor a cero.
- El uptime se calcula desde que inicia el proceso del sensor, no desde el arranque del inversor.
- La fecha de consulta se informa en UTC y el uptime en formato de tiempo transcurrido.
- Las credenciales y la URL base de la API de SolarUY se configuran en el sensor, no se exponen al cliente.
