# Requerimientos: Alta Registro Histórico

## Descripción General
El sistema SolarUY, de forma automática según el intervalo configurado (por defecto cada 24 horas), debe generar un registro del histórico del panel con los datos `Id`, `Fecha` y `EnergiaTotal`, para conservar la evolución de la energía total de cada panel sin intervención de un usuario.

## Criterios de Aceptación
1. Periodicidad: El sistema ejecuta el alta de forma automática según el intervalo configurado (`Historico:IntervaloHoras`); con el intervalo por defecto (24 horas) genera un registro por día, sin acción de un usuario.
2. Datos del registro: Cada registro contiene:
   - `Id` (generado por el sistema).
   - `Fecha` (asignada por el sistema en el momento de generar el registro).
   - `EnergiaTotal` (energía total del panel, obtenida del módulo ParqueSolar).
3. Pertenencia: Cada registro corresponde a un panel existente. El sistema conserva la referencia del panel (parque solar, nodo y panel) sin depender de las entidades de ParqueSolar.
4. Cálculo del dato: La `EnergiaTotal` no se ingresa manualmente; el sistema la toma del panel en el momento de generar el registro.
5. Persistencia: Cada ejecución persiste los registros generados. Si no hay paneles, no genera registros y no falla.
6. Sin duplicados: No puede existir más de un registro para el mismo panel dentro del mismo período (el intervalo configurado).
7. Ejecución desatendida: El proceso corre en segundo plano y no bloquea el arranque ni la operación de la aplicación.
8. Interfaz: Los registros generados se consultan desde el apartado del panel, mediante el botón "Ver histórico", que presenta una tabla con los registros (`Id`, `Fecha`, `EnergiaTotal`).

## Interfaz (apartado de panel)
- La visualización de los registros generados vive dentro del apartado del panel.
- En la ficha del panel se muestra el botón "Ver histórico".
- Al presionarlo se presenta una tabla con los registros (`Id`, `Fecha`, `EnergiaTotal`), del más reciente al más antiguo.
- La `Fecha` se muestra en la hora local del sistema: el registro se persiste en UTC y se convierte a la hora local solo para mostrarlo.
- El alta en sí no tiene pantalla: la genera el sistema de forma automática.

## Reglas de Negocio
- Un registro de histórico pertenece siempre a un panel (relación Panel 1:N Historico).
- La `EnergiaTotal` no puede ser negativa.
- La `Fecha` la asigna el sistema y no puede ser futura.
- No puede existir más de un registro del mismo panel dentro del mismo período (el intervalo configurado).
- El alta es automática: no requiere autenticación, rol ni permiso de Auth0 porque la ejecuta el propio sistema, no un usuario.
