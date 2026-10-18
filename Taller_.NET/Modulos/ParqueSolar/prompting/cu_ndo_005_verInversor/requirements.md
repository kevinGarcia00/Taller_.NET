# Requerimientos: Ver Inversor

## Descripción General
Como Administrador del sistema SolarUY, ingreso a la sección de nodos de un parque anteriormente seleccionado, selecciono el nodo del cual deseo ver el inversor, presiono la opción para ver el inversor y se muestran los datos del inversor asignado al nodo (en caso de que este exista).

## Criterios de Aceptación
1. Contexto previo: La visualización del inversor solo está disponible cuando el Administrador ya seleccionó un parque solar e ingresó a su correspondiente sección de nodos.
2. Visualización de datos: Al presionar la opción para ver el inversor sobre un nodo seleccionado, el sistema despliega una vista o modal informativo presentando los datos del inversor asociado.
3. Nodo sin inversor: Si el nodo seleccionado no cuenta con un inversor asignado, el sistema debe mostrar un mensaje o indicador claro de que dicho nodo no posee un inversor asociado.
4. Seguridad: Solo los usuarios con el rol de "Administrador" están autorizados a consultar los datos del inversor del nodo.

## Reglas de Negocio
- Solo es posible consultar los inversores de los nodos pertenecientes al parque solar actualmente seleccionado.
- El sistema debe contemplar la ausencia de inversor sin emitir errores de sistema, permitiendo cerrar la consulta limpiamente.
- La información del inversor presentada mediante este caso de uso es exclusivamente informativa y no permite edición o desvinculación desde esta vista.