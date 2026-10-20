# Requerimientos: Modificar Nodo

## Descripción General
Como Administrador del sistema SolarUY, quiero modificar los datos de un nodo existente dentro del parque solar seleccionado y gestionar su inversor asociado para mantener actualizada la infraestructura del parque.

## Criterios de Aceptación
1. Contexto previo: La edición solo está disponible cuando el Administrador ya seleccionó un parque solar, accedió a la sección de nodos y seleccionó un nodo específico para modificar. El formulario se presenta precargado con la información actual del nodo y su inversor.
2. Formulario de edición: El sistema debe permitir modificar los siguientes datos:
   - **Nombre** (Obligatorio).
   - **UbicacionParque** (Obligatorio).
   - **Inversor** (Obligatorio: Se selecciona de la lista de inversores disponibles o se conserva el actual).
3. Datos no modificables o asignados por el sistema: 
   - **Fecha de creación (`fechaCreacion`):** Se conserva el valor registrado originalmente en el alta y no es editable.
   - **Energía generada (`energiaGenerada`):** No se modifica de forma manual (se mantiene el valor acumulado o calculado automáticamente).
4. Confirmación: Al confirmar los cambios, el sistema valida las reglas de negocio, actualiza la información del nodo y muestra un mensaje de éxito.
5. Seguridad: Solo los usuarios autenticados mediante Auth0 y con el rol de "Administrador" están autorizados a modificar nodos.

## Reglas de Negocio
- El nodo se mantiene asociado a su parque solar original. No es posible transferir un nodo a otro parque solar mediante esta operación.
- Relación 1:1 Estricta con Inversor: Cada nodo debe tener un único inversor asignado, y un mismo inversor no puede pertenecer a más de un nodo simultáneamente.
- No pueden existir dos nodos con el mismo nombre dentro del mismo parque solar (excluyendo el nodo actual que se está editando).
- El nombre, la ubicación en el parque y el inversor son obligatorios. El nombre y la ubicación no pueden consistir únicamente en espacios en blanco.