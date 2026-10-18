# Requerimientos: Registrar Inversor

## Descripción General
Como Administrador del sistema SolarUY, una vez seleccionado el parque solar y el nodo, quiero registrar el inversor de ese nodo (cuando todavía no tenga uno), completar el formulario y confirmar el alta.

## Criterios de Aceptación
1. Contexto previo: El alta solo está disponible cuando el Administrador ya seleccionó un parque solar y, dentro de él, un nodo. El inversor queda asociado a ese nodo.
2. Inversor inexistente: La opción de registrar solo se ofrece si el nodo seleccionado todavía no tiene un inversor. Si ya tiene uno, el sistema no permite un segundo alta y lo informa.
3. Formulario de registro: El sistema permite el ingreso del siguiente dato:
   - Nombre (Obligatorio).
4. Datos asignados por el sistema: La fecha de creación se registra al confirmar el alta y no se ingresa en el formulario.
5. Confirmación: Al confirmar, el sistema valida las reglas de negocio, persiste el inversor, lo asocia al nodo y muestra un mensaje de éxito.
6. Seguridad: Solo los usuarios con el rol de "Administrador" están autorizados a registrar inversores.

## Reglas de Negocio
- Todo inversor se registra para el nodo ya seleccionado, dentro del parque solar previamente elegido. No se puede dar de alta un inversor sin ese contexto.
- La relación entre nodo e inversor es 1:1. Un nodo no puede tener más de un inversor y un inversor no puede quedar asociado a otro nodo.
- El nombre es obligatorio y no puede ser espacios en blanco.
