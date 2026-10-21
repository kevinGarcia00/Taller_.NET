# Tareas de Implementación: Listar Nodos

- [ ] **1. Capa Domain**
  - [ ] 1.1 Validar que los criterios y proyecciones de lectura mantengan la restricción estricta por `ParqueSolarId`.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Agregar el método `ObtenerPorParqueAsync(long parqueSolarId)` en `INodoRepository` y `NodoRepository`.
  - [ ] 2.2 Configurar la consulta de EF Core proyectando hacia `NodoListadoDto`.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO `NodoListadoDto`.
  - [ ] 3.2 Agregar el método `ObtenerNodosPorParqueAsync(long parqueSolarId)` en la interfaz `INodoService` y su implementación `NodoService`.
  - [ ] 3.3 Implementar la lógica del servicio.
  - [ ] 3.4 Crear Test Unitario para verificar que el servicio lanza una excepción de negocio si el parque solar no existe.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `ListadoNodos.razor` con la directiva `@page "/parques/{ParqueId:long}/nodos"`.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador, Operador")]` o `<AuthorizeView Roles="Administrador, Operador">`.
  - [ ] 4.3 Diseñar el encabezado con el título, la identificación del Parque Solar y el botón `RadzenButton` ("Nuevo Nodo") apuntando a `/parques/{ParqueId}/nodos/alta`.
  - [ ] 4.4 Configurar la grilla `<RadzenDataGrid>` con las columnas (`Nombre`, `UbicacionParque`, `EnergiaGenerada`, `NombreInversor`, `FechaCreacion`) y botones de acción (editar / eliminar).
  - [ ] 4.5 Implementar `OnParametersSetAsync` para capturar el `ParqueId` de la URL e invocar `INodoService.ObtenerNodosPorParqueAsync(parqueId)`.
  - [ ] 4.6 Controlar el manejo de excepciones mediante `NotificationService` y el diálogo de confirmación de borrado mediante `DialogService`.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar suite de pruebas para validar que no existan violaciones de Clean Architecture.
  - [ ] 5.2 Ejecutar inspección de código para asegurar que no hay code smells.