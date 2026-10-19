# Tareas de Implementación: Ver Paneles

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Parque`
  - [ ] 1.2 Verificar que la entidad exponga los atributos declarados previamente en specs.md .

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `ParqueConfiguration`, `DbSet<Panel>` y `ParqueRepository`.
  - [ ] 2.2 Implementar la consulta por nodo, ordenada por nombre, y la consulta de detalle por `Id` y `NodoId`, proyectando solo los campos necesarios.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear `ParqueListadoDto` (`Id`, `Nombre`, `FechaCreacion`, `Ubicacion`, `CantMaxNodos`) y `PanelDetalleDto` (`Id`, `Nombre`, `FechaCreacion`, `Ubicacion`, `ListaNodos`).
  - [ ] 3.2 Agregar a `IParqueService` e implementar en `ParqueService`:
    - `ObtenerParquePorNodoAsync(int nodoId)`
    - `ObtenerParqueAsync(int id)`
  - [ ] 3.3 Implementar ambas lecturas confirmando antes el nodo con `IMediadorParqueNodo` y sin modificar la entidad.
  - [ ] 3.4 Crear pruebas unitarias para listado con datos, listado vacío, detalle exitoso, parque inexistente.
  - [ ] 3.5 Que el codigo permita la implementacion e implemente la verificacion de roles con Auth0.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear `Parque.razor` con la directiva `@page "/parques/alta"`.
  - [ ] 4.2 Crear `DetalleParque.razor` con la directiva `@page "/parques/detalle/{idParque}"`.
  - [ ] 4.3 Restringir ambas páginas con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.4 Configurar el `RadzenDataGrid<ParqueListadoDto>` y el botón "Registrar Parque".
  - [ ] 4.5 Mostrar en el detalle, en solo lectura, Nombre, FechaCreacion, Ubicacion, CantMaxNodos y ListaDeLosNodos.
  - [ ] 4.6 Cargar los datos en `OnInitializedAsync` y notificar con `NotificationService` si la consulta falla.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar las pruebas para validar que se cumplen las pautas establecidas (dotnet test).
