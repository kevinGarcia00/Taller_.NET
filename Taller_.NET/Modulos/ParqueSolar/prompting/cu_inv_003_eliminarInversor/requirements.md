# Requerimientos: Eliminar Inversor

## Descripción General
Como Administrador del sistema SolarUY, una vez seleccionado el parque solar y el nodo, quiero eliminar el inversor de ese nodo y confirmar la eliminación.

## Criterios de Aceptación
1. Contexto previo: La eliminación solo está disponible con un parque solar y un nodo ya seleccionados, y sobre el inversor de ese nodo.
2. Confirmación de acción: Al elegir eliminar, el sistema muestra un diálogo de confirmación (ej. ¿Está seguro de que desea eliminar el inversor [Nombre]?).
3. Seguridad: Solo los usuarios con el rol de "Administrador" están autorizados a eliminar inversores.
4. Notificación y actualización: Tras una eliminación exitosa, el sistema muestra una notificación y deja el nodo sin inversor, habilitando de nuevo el alta.
5. Manejo de errores: Si el nodo no tiene inversor, el inversor no existe o la operación falla, se informa al Administrador sin romper la interfaz.

## Reglas de Negocio
- Solo se puede eliminar el inversor asociado al nodo seleccionado.
- La eliminación exige confirmación explícita del Administrador.
- Al eliminar el inversor, el nodo queda sin inversor asociado. La relación 1:1 se libera.
