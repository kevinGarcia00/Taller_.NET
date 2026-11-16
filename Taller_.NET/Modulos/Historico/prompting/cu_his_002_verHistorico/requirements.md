# Requerimientos: Ver Histórico del Panel

## Descripción General
Como Administrador del sistema SolarUY, quiero ver el histórico de energía de un panel del nodo seleccionado para consultar cómo evolucionó su energía total a lo largo del tiempo.

## Criterios de Aceptación
1. Contexto previo: El Administrador ingresa a la sección de nodos de un parque solar ya seleccionado y, dentro del nodo, elige el panel cuyo histórico desea ver.
2. Acción: En la ficha del panel (apartado de panel) el Administrador presiona el botón "Ver histórico".
3. Visualización de datos: El sistema presenta un listado de los registros del histórico de ese panel. Cada registro muestra:
   - `Id`.
   - `Fecha` (en la hora local del sistema).
   - `EnergiaTotal`.
4. Orden: Los registros se muestran del más reciente al más antiguo.
5. Pertenencia: Solo se listan registros del panel seleccionado. Si el panel no existe en ese nodo o en ese parque solar, se informa con un mensaje de error.
6. Sin registros: Si el panel todavía no tiene registros, el listado se muestra vacío con un mensaje.
7. Seguridad: Solo los usuarios autenticados en Auth0 con el rol "Administrador" pueden ver el histórico.
8. Consulta: El listado no modifica los registros ni el panel.

## Reglas de Negocio
- El histórico es visible únicamente para el rol "Administrador".
- Un registro solo se muestra dentro del panel, el nodo y el parque solar a los que pertenece.
- El listado es de solo lectura: no se pueden editar ni eliminar registros.

## Seguridad e Integración con Auth0
- El actor es un usuario autenticado en Auth0; el sistema autoriza la pantalla por rol "Administrador".
- Al iniciar sesión, los roles del usuario se obtienen de Auth0 (Management API) y se agregan a la cookie de autenticación; los permisos viajan en el Access Token.
- La operación es de solo lectura, por lo que no requiere permisos de creación, modificación ni eliminación.
