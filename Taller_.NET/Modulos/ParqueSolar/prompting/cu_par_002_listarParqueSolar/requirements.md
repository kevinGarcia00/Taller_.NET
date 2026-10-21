# Requerimientos: Ver Parques Solares

## Descripción General
Como Administrador del sistema SolarUY, quiero ver los datos de un parque solar para poder ver sus características técnicas.

## Criterios de Aceptación
1. Contexto previo: El Administrador ingresa a la sección de parques solares, se listan todos los parques solares y si el usuario clickea en "Ver informacion" se abre un popup con la informacion del parque solar seleccionado.
2. Visualización de datos: Al seleccionar el parque solar, el sistema muestra Nombre, Ubicacion, CantidadDeNodos, CantMaxNodos, Fecha de creación y la lista de los nodos que tiene.
3. Pertenencia: Solo se muestran los nodos del parque solar seleccionado. Si el parque solar no existe, se informa con un mensaje de error.
4. Seguridad: Solo los usuarios con el rol de "Administrador" pueden ver los parques solares.
5. Navegación: Desde el popup se puede volver al listado de parques solares. El popup ofrece el acceso a editar y eliminar parque solar.

## Reglas de Negocio
- La consulta no modifica los datos del parque solar.
