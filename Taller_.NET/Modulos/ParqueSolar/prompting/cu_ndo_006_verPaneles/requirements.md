# Requerimientos: Ver Paneles

## Descripción General
Como Administrador del sistema SolarUY, quiero consultar el listado de paneles asociados a un nodo específico dentro de un parque solar seleccionado, para visualizar la infraestructura de generación fotovoltaica instalada en dicho nodo.

## Criterios de Aceptación
1. Visualización de Paneles: El sistema debe permitir al Administrador seleccionar un nodo de un parque solar previamente elegido y presionar la opción para visualizar sus paneles.
2. Contenido del Listado: El sistema debe mostrar el listado completo de los paneles pertenecientes de forma exclusiva al nodo seleccionado.
3. Seguridad: Solo los usuarios autenticados mediante Auth0 y con el rol de "Administrador" están autorizados a acceder a la sección de nodos y consultar la lista de paneles.

## Reglas de Negocio
- La consulta de paneles debe estar estrictamente contextualizada al nodo y al parque solar seleccionados previamente.
- Si el nodo no tiene paneles asociados, el sistema debe mostrar una vista informativa indicando la ausencia de registros de paneles para dicho nodo.