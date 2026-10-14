# Requerimientos: Modificar Panel

## Descripción General
Como Administrador del sistema SolarUY, quiero modificar los datos de un panel del nodo seleccionado para mantener actualizado su estado y sus características técnicas.

## Criterios de Aceptación
1. Contexto previo: La modificación solo está disponible con un parque solar y un nodo ya seleccionados.
2. Selección de entidad: El Administrador elige el panel a modificar dentro de ese nodo. Si no existe o no pertenece al nodo, el sistema muestra un mensaje de error descriptivo.
3. Precarga de datos: Al seleccionar un panel válido, el sistema presenta un formulario precargado con Nombre, Estado, Potencia y Vida útil.
4. Campos editables: El Administrador puede modificar Nombre, Estado, Potencia y Vida útil. El identificador, la fecha de creación y el nodo no se editan.
5. Confirmación y guardado: Al confirmar, el sistema valida las reglas de negocio, actualiza el panel y muestra un mensaje de éxito.
6. Seguridad: Solo los usuarios autenticados con el rol de "Administrador" están autorizados a realizar esta acción.

## Reglas de Negocio
- El nuevo nombre no debe coincidir con el de otro panel del mismo nodo.
- El panel no puede cambiar de nodo ni de parque solar desde esta operación.
- El identificador y la fecha de creación son de solo lectura.
- La potencia debe ser mayor a cero y el estado debe ser uno de los valores definidos.
