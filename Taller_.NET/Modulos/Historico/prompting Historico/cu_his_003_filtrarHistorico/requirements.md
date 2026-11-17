# Requerimientos: Filtrar Histórico del Panel

## Descripción General
Como Administrador del sistema SolarUY, quiero filtrar el histórico de energía de un panel por una fecha específica para consultar únicamente los registros de ese día.

## Criterios de Aceptación
1. Contexto previo: El Administrador ingresa a la sección de nodos de un parque solar ya seleccionado y, dentro del nodo, elige el panel cuyo histórico desea consultar; en la ficha del panel (apartado de panel) presiona el botón "Ver histórico".
2. Filtro: El sistema debe permitir elegir una fecha específica a filtrar.
3. Listado: Al aplicar el filtro, el sistema lista los registros del panel que pertenecen a esa fecha. Cada registro muestra:
   - `Id`.
   - `Fecha`.
   - `EnergiaTotal`.
4. Orden: Los registros se muestran del más reciente al más antiguo.
5. Pertenencia: Solo se listan registros del panel seleccionado. Si el panel no existe en ese nodo o en ese parque solar, se informa con un mensaje de error.
6. Sin resultados: Si ningún registro cae en la fecha seleccionada, el listado se muestra vacío con un mensaje.
7. Seguridad: Solo los usuarios autenticados en Auth0 con el rol "Administrador" pueden filtrar el histórico.
8. Consulta: El filtro no modifica los registros ni el panel.

## Reglas de Negocio
- El filtro comprende la fecha completa, desde las 00:00 hasta las 23:59:59 de ese día.
- Un registro pertenece al filtro si su `Fecha` cae dentro de ese día.
- El listado es de solo lectura: no se pueden editar ni eliminar registros.

## Seguridad e Integración con Auth0
- El actor es un usuario autenticado en Auth0; el sistema autoriza la pantalla por rol "Administrador".
- Al iniciar sesión, los roles del usuario se obtienen de Auth0 (Management API) y se agregan a la cookie de autenticación; los permisos viajan en el Access Token.
- La operación es de solo lectura, por lo que no requiere permisos de creación, modificación ni eliminación.
