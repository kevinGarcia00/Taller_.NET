# Tareas de Implementación: Ver Parques Solares

- [ ] **1. Capa Domain**
    - [ ] 1.1 Reutilizar `Parque`.
    - [ ] 1.2 Verificar que la entidad exponga Nombre, Ubicacion, CantMaxNodos, Fecha de creación y un metodo que permita calcular la cantida de nodos acutales. sin agregar métodos que modifiquen el parque solar.

- [ ] **2. Capa Infrastructure (Persistencia)**
    - [ ] 2.1 Reutilizar `ParqueConfiguration`, `DbSet<Parque>` y `ParqueRepository`.

- [ ] **3. Capa Application**
    - [ ] 3.1 Crear `ParqueListadoDto` (`Id`, `Nombre`, `Ubicacion`, `CantidadNodosActuales`) y `PanelDetalleDto` (`Id`, `Nombre`, `Ubicacion`, `CantMaxNodos`, `CantidadNodosActuales`, `FechaCreacion`).
    - [ ] 3.2 Agregar a `IParqueService` e implementar en `ParqueService` los metodos sincronicos:
        - `ObtenerParqueSync(int parqueSolarId)`
        - `ObtenerParquesSync()`
    - [ ] 3.4 Crear pruebas unitarias para listado con datos, listado vacío, detalle exitoso, parque inexistente.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
    - [ ] 4.1 Crear `Parques.razor` con la directiva `@page "/parques"`.
    - [ ] 4.2 Crear `DetalleParque.razor` con la directiva `@page "/parques/{ParqueId}"`.
    - [ ] 4.3 Restringir ambas páginas con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
    - [ ] 4.4 Mostrar en el detalle, en solo lectura, Nombre, Ubicacion, CantMaxNodos, CantidadNodosActuales, ListaDeNodos y Fecha de creación.
    - [ ] 4.5 Cargar los datos en `OnInitializedAsync` y notificar con `NotificationService` si la consulta falla.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
    - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni dependencia directa entre módulos (dotnet test).