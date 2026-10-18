# Requerimientos: Listar Nodo

## Descripción General
Como Administrador del sistema SolarUY, ingreso a la sección de nodos (de un parque anteriormente seleccionado) y se listan los nodos registrados al parque seleccionado.

## Criterios de Aceptación
1. Contexto previo: La visualización de nodos solo está disponible cuando el Administrador ingresa a la sección de nodos de un parque previamente seleccionado.
2. Visualización de datos: El sistema debe presentar una tabla o grilla con los nodos registrados correspondientes al parque seleccionado, incluyendo los siguientes campos:
   - Nombre
   - Ubicación en el parque (`UbicacionParque`)
   - Energía generada (`EnergiaGenerada`)
   - Fecha de creación (`FechaCreacion`)
   - Inversor asignado.
3. Navegación y acciones: Desde la interfaz del listado se debe permitir el acceso a las demás acciones del caso de uso de nodos (Registrar, Modificar y Eliminar).
4. Sin datos: Si el parque solar no posee nodos registrados, la interfaz debe mostrar un mensaje descriptivo advirtiendo que no existen registros asociados.
5. Seguridad: Solo los usuarios autenticados con el rol de "Administrador" están autorizados a visualizar el listado de nodos.

## Reglas de Negocio
- La consulta debe delimitarse exclusivamente a los nodos que pertenecen al parque solar actualmente seleccionado.
- En caso de existir un inversor asociado al nodo, el listado debe obtener y presentar los datos básicos del mismo garantizando la independencia modular de la arquitectura.