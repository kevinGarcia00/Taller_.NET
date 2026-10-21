# Requerimientos: Eliminar Nodo

## Descripción General
Como Administrador del sistema SolarUY, ingreso a la sección de nodos de un parque anteriormente seleccionado, selecciono el nodo deseado a eliminar, presiono para eliminar nodo y realizo la confirmación para la eliminación del mismo.

## Criterios de Aceptación
1. Contexto previo: La eliminación de nodos solo está disponible cuando el Administrador ya seleccionó un parque solar e ingresó a su correspondiente sección de nodos.
2. Diálogo de confirmación: Al presionar la opción de eliminar sobre un nodo seleccionado, el sistema debe desplegar una ventana o modal de confirmación advirtiendo sobre la acción.
3. Confirmación y resultado: Al realizar la confirmación explícita por parte del Administrador, el sistema procesa la eliminación del nodo, actualiza la lista de nodos del parque solar y muestra un mensaje de éxito.
4. Cancelación: Si el Administrador cancela el diálogo de confirmación, la operación se interrumpe sin aplicar ningún cambio sobre la base de datos ni sobre la entidad.
5. Seguridad: Solo los usuarios autenticados mediante Auth0 y con el rol de "Administrador" están autorizados a eliminar nodos.

## Reglas de Negocio
- Solo es posible eliminar nodos pertenecientes al parque solar actualmente seleccionado.
- Si el nodo a eliminar tiene un inversor asignado, la eliminación del nodo debe liberar la relación 1:1, dejando el inversor disponible para ser asociado a un nuevo nodo.
- No se puede eliminar un nodo sin antes resolver o desvincular los paneles solares asociados al mismo.