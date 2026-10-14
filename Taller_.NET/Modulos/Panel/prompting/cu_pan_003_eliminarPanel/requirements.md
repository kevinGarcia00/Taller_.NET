# Requerimientos: Eliminar Panel

## Descripción General
Como Administrador del sistema SolarUY, quiero eliminar un panel del nodo seleccionado para retirarlo del inventario cuando ya no forme parte de la instalación.

## Criterios de Aceptación
1. Contexto previo: La eliminación solo está disponible con un parque solar y un nodo ya seleccionados, y sobre un panel de ese nodo.
2. Confirmación de acción: Al elegir eliminar, el sistema muestra un diálogo de confirmación (ej. «¿Está seguro de que desea eliminar el panel [Nombre]?»).
3. Seguridad: Solo los usuarios con el rol de "Administrador" están autorizados a eliminar paneles.
4. Notificación y actualización: Tras una eliminación exitosa, el sistema muestra una notificación y actualiza de inmediato el listado de paneles del nodo.
5. Manejo de errores: Si el panel no existe, no pertenece al nodo o la operación falla, se informa al Administrador sin romper la interfaz.

## Reglas de Negocio
- Solo se puede eliminar un panel que pertenezca al nodo seleccionado.
- La eliminación exige confirmación explícita del Administrador.
