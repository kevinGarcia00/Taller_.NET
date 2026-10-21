# Requerimientos: Registrar Nodo

## Descripción General
Como Administrador del sistema SolarUY, una vez seleccionado el parque solar e ingresado a la sección de nodos, quiero presionar para registrar un nuevo nodo, completar el formulario con los datos del nodo y confirmar el alta del mismo.

## Criterios de Aceptación
1. Contexto previo: El alta solo está disponible cuando el Administrador ya seleccionó un parque solar e ingresó a la sección de nodos. El formulario de alta queda asociado automáticamente al parque solar seleccionado.
2. Formulario de registro: El sistema permite el ingreso de los siguientes datos del nodo:
   - Nombre (Obligatorio).
   - UbicacionParque (Obligatorio).
   - Inversor (Opcional: Permite seleccionar un inversor disponible para asociarlo al nodo o dejarlo sin asignar).
3. Datos asignados por el sistema:
   - Fecha de creación (`fechaCreacion`):** Se asigna automáticamente al confirmar el alta y no se ingresa en el formulario.
   - Energía generada (`energiaGenerada`):** No se ingresa de forma manual. El sistema la inicializa en 0.
4. Confirmación: Al confirmar la acción, el sistema valida las reglas de negocio, persiste el nuevo nodo y muestra un mensaje de éxito.
5. Seguridad: Solo los usuarios autenticados mediante Auth0 y con el rol de "Administrador" están autorizados a ejecutar este caso de uso.

## Reglas de Negocio
- Todo nodo pertenece al parque solar previamente seleccionado. No se puede dar de alta un nodo sin parque solar asociado.
- No pueden existir dos nodos con el mismo nombre dentro del mismo parque solar.
- El nombre y la ubicación en el parque son obligatorios y no pueden ser espacios en blanco.
- **Asociación de Inversor (Opcional):**
  - No es obligatorio asignar un inversor al momento de registrar el nodo.
  - En caso de seleccionar un inversor, este debe estar disponible y no puede estar asociado a ningún otro nodo (Relación 1:1 estricta).