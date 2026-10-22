# Requerimientos: Eliminar Parque Solar

## Descripción General
Como Administrador del sistema SolarUY, quiero eliminar un parque solar para retirarlo.

## Criterios de Aceptación
1. Contexto previo: La eliminación solo está disponible si el parque solar no tiene nodos relacionados.
2. Confirmación de acción: Al elegir eliminar, el sistema muestra un diálogo de confirmación (ej. «¿Está seguro de que desea eliminar el parque solar [Nombre]?»).
3. Seguridad: Solo los usuarios con el rol de "Administrador" están autorizados a eliminar parques solares.
4. Notificación y actualización: Tras una eliminación exitosa, el sistema muestra una notificación y actualiza de inmediato el listado de parques solares.
5. Manejo de errores: Si el parque solar no existe o tiene nodos asociados, se informa al Administrador sin romper la interfaz mediante un popup.

## Reglas de Negocio
- Solo se puede eliminar un parque solar que no tenga nodos asociados.
- La eliminación exige confirmación explícita del Administrador.
