# Requerimientos: Ver Paneles

## Descripción General
Como Administrador del sistema SolarUY, quiero ver los datos de un panel del nodo seleccionado para consultar su estado y sus características técnicas.

## Criterios de Aceptación
1. Contexto previo: El Administrador ingresa a la sección de nodos de un parque solar ya seleccionado y, dentro del nodo, elige el panel que desea consultar.
2. Visualización de datos: Al seleccionar el panel, el sistema muestra Nombre, Estado, Potencia, Vida útil y Fecha de creación.
3. Pertenencia: Solo se muestran paneles del nodo seleccionado. Si el panel no existe en ese nodo, se informa con un mensaje de error.
4. Seguridad: Solo los usuarios con el rol de "Administrador" pueden ver los paneles.
5. Navegación: Desde la vista se puede volver al listado de paneles del nodo. El listado ofrece el acceso a registrar, editar y eliminar.

## Reglas de Negocio
- La consulta no modifica los datos del panel.
- Un panel solo es visible dentro del nodo al que pertenece y del parque solar previamente seleccionado.
