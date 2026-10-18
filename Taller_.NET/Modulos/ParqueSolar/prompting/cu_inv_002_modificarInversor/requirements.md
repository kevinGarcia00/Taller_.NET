# Requerimientos: Modificar Inversor

## Descripción General
Como Administrador del sistema SolarUY, una vez seleccionado el parque solar y el nodo, quiero editar los datos del inversor de ese nodo y confirmar la modificación.

## Criterios de Aceptación
1. Contexto previo: La modificación solo está disponible con un parque solar y un nodo ya seleccionados, y solo si ese nodo tiene un inversor.
2. Selección de entidad: El Administrador edita el inversor del nodo seleccionado. Si el nodo no tiene inversor, el sistema muestra un mensaje de error descriptivo y no abre el formulario.
3. Precarga de datos: Al abrir la edición, el sistema presenta un formulario precargado con el Nombre del inversor.
4. Campos editables: El Administrador puede modificar el Nombre. El identificador, la fecha de creación, el nodo y el parque solar no se editan.
5. Confirmación y guardado: Al confirmar, el sistema valida las reglas de negocio, actualiza el inversor y muestra un mensaje de éxito.
6. Seguridad: Solo los usuarios autenticados con el rol de "Administrador" están autorizados a realizar esta acción.

## Reglas de Negocio
- Solo se puede modificar el inversor asociado al nodo seleccionado.
- El inversor no puede cambiar de nodo ni de parque solar desde esta operación.
- El identificador y la fecha de creación son de solo lectura.
- El nombre es obligatorio y no puede ser espacios en blanco.
