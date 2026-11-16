# Requerimientos: Consultar Estado de Panel del Inversor

## Descripción General
Como sensor del sistema SolarUY desplegado en un equipo ARM, quiero consultar a la API de SolarUY el estado y los datos técnicos de un panel específico del inversor, para entregar esa información al backend o al usuario sin que este se comunique directamente con el inversor.

## Criterios de Aceptación
1. Contexto previo: El sensor expone la consulta mediante la URL `/inversor/{idInversor}/nodo/{idNodo}/panel/{idPanel}`, accesible desde la interfaz web o por CURL.
2. Visualización de datos: Al invocar la URL, el sensor consulta la API de SolarUY del inversor y responde con los atributos del panel: `id`, `nombre`, `fechaCreacion`, `estadoPanel`, `encendido`, `potencia` y `vidaUtil`.
3. Pertenencia: El panel solo se devuelve si pertenece al nodo y al inversor indicados. Si el inversor, el nodo o el panel no existen o no coinciden, se responde con un mensaje de error claro.
4. Listado: El sensor puede exponer el listado de inversores, sus nodos y los paneles de cada nodo para que el usuario seleccione el panel a consultar.
5. Navegación: Desde la vista se puede volver al listado de inversores, que permite buscar por nombre del inversor.
6. Disponibilidad: Si la API de SolarUY no responde, el sensor informa el error sin bloquearse y sin inventar datos.

## Reglas de Negocio
- La consulta es de solo lectura, no modifica los datos del panel en SolarUY.
- El sensor actúa como intermediario: no se comunica directamente con el hardware del inversor ni persiste el estado del panel.
- Un panel solo es visible dentro del inversor y del nodo a los que pertenece.
- Las credenciales y la URL base de la API de SolarUY se configuran en el sensor, no se exponen al cliente.
