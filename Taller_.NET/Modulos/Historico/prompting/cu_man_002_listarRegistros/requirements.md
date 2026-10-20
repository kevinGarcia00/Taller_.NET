# Requerimientos: Listar Registros de Mantenibilidad

## Descripción General
Como Administrador del sistema SolarUY, quiero consultar el historial de mantenibilidad de un panel ya seleccionado para revisar los trabajos realizados y el cambio de estado de ese panel.

## Criterios de Aceptación
1. Contexto previo: El Administrador ingresa a la sección de nodos de un parque solar ya seleccionado, elige el panel y presiona ver historial.
2. Visualización de datos: El sistema presenta un listado de los registros de mantenibilidad de ese panel. Cada registro muestra el identificador del mantenedor, la fecha, las observaciones, el estado inicial del panel y el estado resultante.
3. Pertenencia: Solo se listan registros del panel seleccionado. Si el panel no existe en ese nodo, se informa con un mensaje de error.
4. Seguridad: Solo los usuarios con el rol de "Administrador" pueden ver el historial.
5. Consulta: El listado no modifica los registros ni el panel.

## Reglas de Negocio
- El historial es visible únicamente para el rol "Administrador".
- Un registro solo se muestra dentro del panel, del nodo y del parque solar a los que pertenece.
- Si el panel no tiene registros, el listado se muestra vacío.
