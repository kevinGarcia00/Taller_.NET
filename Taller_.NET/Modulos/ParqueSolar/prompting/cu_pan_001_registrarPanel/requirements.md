# Requerimientos: Registrar Panel

## Descripción General
Como Administrador del sistema SolarUY, quiero registrar un panel en el nodo ya seleccionado de un parque solar para incorporarlo al inventario de ese nodo.

## Criterios de Aceptación
1. Contexto previo: El alta solo está disponible cuando el Administrador ya seleccionó un parque solar y, dentro de él, un nodo. El formulario queda asociado a ese nodo.
2. Formulario de registro: El sistema debe permitir el ingreso de los siguientes datos:
   - Nombre (Obligatorio).
   - Estado (Obligatorio: Sucio, Roto, Encendido, Deteriorado o Apagado).
   - Potencia (Obligatoria).
   - Vida útil (Obligatoria).
3. Datos asignados por el sistema: La fecha de creación se registra al confirmar el alta y no se ingresa en el formulario.
4. Confirmación: Al confirmar, el sistema valida las reglas de negocio, persiste el panel y muestra un mensaje de éxito.
5. Seguridad: Solo los usuarios con el rol de "Administrador" están autorizados a registrar paneles.

## Reglas de Negocio
- Todo panel pertenece al nodo seleccionado. No se puede dar de alta un panel sin nodo ni fuera del parque solar ya elegido.
- No pueden existir dos paneles con el mismo nombre dentro del mismo nodo.
- La potencia debe ser mayor a cero.
- El estado debe ser uno de los valores definidos para un panel.
