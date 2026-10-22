# Tareas de Implementación: Eliminar Panel

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Parque`.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `ParqueConfiguration`, `DbSet<Parque> Parques` de `IParqueDbContext` y `ParqueRepository`.
  - [ ] 2.2 Exponer en el repositorio la baja del parque cargado y la confirmación de cambios.

- [ ] **3. Capa Application**
  - [ ] 3.1 Declarar en `IParqueService` el método `EliminarParqueAsync(idParqueSolar)`.
  - [ ] 3.2 Implementar el método en `ParqueService`, con `IParqueRepository` inyectados por constructor.
  - [ ] 3.3 Implementar la lógica: cargar el nodo del parque, obtener el parque por `Id`, lanzar excepción si no existe y removerlo persistiendo los cambios.
  - [ ] 3.4 Crear las pruebas unitarias del servicio para nodo inválido, parque solar inexistente y eliminación exitosa.
  - [ ] 3.5 Preparar el código para que quede conectado con la implementación de Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Ubicar la acción de eliminar en `Parque.razor` (`@page "/parques/eliminar/{ParqueId:int}"`).
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.3 Agregar un `RadzenButton` de eliminación en la columna de acciones del `RadzenDataGrid` e integrar `DialogService` para confirmar, mostrando el nombre del parque solar.
  - [ ] 4.4 Si se confirma, invocar `IParqueService.EliminarParquelAsync(idParqueSolar)`.
  - [ ] 4.5 Notificar el resultado con `NotificationService` de Radzen y refrescar la grilla. Ante un error de negocio, mostrar una advertencia sin romper la página.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
