# Requerimientos: Ver Inversor

## Descripción General
Como Administrador del sistema SolarUY, una vez seleccionado el parque solar y el nodo, quiero ver el inversor de ese nodo para consultar sus datos.

## Criterios de Aceptación
1. Contexto previo: El Administrador ya seleccionó un parque solar y un nodo, y desde ese nodo presiona ver inversor.
2. Visualización de datos: Si el nodo tiene inversor, el sistema muestra Nombre y Fecha de creación.
3. Nodo sin inversor: Si el nodo no tiene un inversor, el sistema lo indica de forma clara y ofrece la opción de registrarlo. No muestra un error de sistema.
4. Seguridad: Solo los usuarios con el rol de "Administrador" pueden ver el inversor.
5. Navegación: Desde la vista, si hay inversor, se puede pasar a editarlo o eliminarlo. Si no hay, se puede pasar al alta.

## Reglas de Negocio
- La consulta no modifica los datos del inversor.
- Solo se muestra el inversor del nodo seleccionado, dentro del parque solar previamente elegido.
- Un nodo muestra como máximo un inversor.
