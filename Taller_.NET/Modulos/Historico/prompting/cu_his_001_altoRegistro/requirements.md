# Requerimientos: Alta Registro Histórico

## Descripción General
El sistema SolarUY, de forma automática cada 24 horas, debe generar un registro del histórico del panel con los datos `Id`, `Fecha` y `EnergiaTotal`, para conservar la evolución de la energía total de cada panel sin intervención de un usuario.

## Criterios de Aceptación
1. Periodicidad: El sistema ejecuta el alta de forma automática cada 24 horas, sin acción de un usuario.
2. Datos del registro: Cada registro contiene:
   - `Id` (generado por el sistema).
   - `Fecha` (asignada por el sistema en el momento de generar el registro).
   - `EnergiaTotal` (energía total del panel, obtenida del módulo ParqueSolar).
3. Pertenencia: Cada registro corresponde a un panel existente. El sistema conserva la referencia del panel (parque solar, nodo y panel) sin depender de las entidades de ParqueSolar.
4. Cálculo del dato: La `EnergiaTotal` no se ingresa manualmente; el sistema la toma del panel en el momento de generar el registro.
5. Persistencia: Cada ejecución persiste los registros generados. Si no hay paneles, no genera registros y no falla.
6. Sin duplicados: En una misma fecha no puede existir más de un registro para el mismo panel.
7. Ejecución desatendida: El proceso corre en segundo plano y no bloquea el arranque ni la operación de la aplicación.

## Reglas de Negocio
- Un registro de histórico pertenece siempre a un panel (relación Panel 1:N Historico).
- La `EnergiaTotal` no puede ser negativa.
- La `Fecha` la asigna el sistema y no puede ser futura.
- No puede existir más de un registro del mismo panel en la misma fecha.
- El alta es automática: no requiere autenticación, rol ni permiso de Auth0 porque la ejecuta el propio sistema, no un usuario.
