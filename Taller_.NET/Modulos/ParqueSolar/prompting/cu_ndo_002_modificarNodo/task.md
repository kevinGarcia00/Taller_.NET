# Tareas de Implementación: Modificar Nodo

- [ ] **1. Capa Domain**
  - [ ] 1.1 Incluir la propiedad `InversorId` (`long`) en la entidad `Nodo` para la relación 1:1 con `Inversor`.
  - [ ] 1.2 Actualizar el constructor de `Nodo` y el método `Actualizar(string nombre, string ubicacionParque, long inversorId)` agregando validación de `InversorId` obligatorio.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Configurar en `NodoConfiguration` un índice único para `InversorId` garantizando la unicidad 1:1 en la BD.
  - [ ] 2.2 Implementar en `NodoRepository` la consulta `ExisteInversorEnOtroNodoAsync(long inversorId, long nodoIdActual)`.
  - [ ] 2.3 Generar y aplicar la migración de Entity Framework.

- [ ] **3. Capa Application**
  - [ ] 3.1 Incluir `InversorId` en `EditarNodoDto`, actualizando sus correspondientes validadores de FluentValidation.
  - [ ] 3.2 Definir en `IMediadorNodoInversor` métodos para consultar la existencia de inversores y obtener la lista de inversores disponibles (no asignados).
  - [ ] 3.3 Implementar en `NodoService` la verificación de disponibilidad del inversor antes de guardar o modificar el nodo.
  - [ ] 3.4 Agregar pruebas unitarias para confirmar que el sistema impide asignar un inversor que ya pertenece a otro nodo.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Agregar a `EditarNodo.razor` un control `RadzenDropDown` para la selección del inversor.
  - [ ] 4.2 Cargar en el componente la lista de inversores disponibles (filtrando aquellos que ya tienen un nodo asignado, salvo el actual en modo edición).
  - [ ] 4.3 Vincular la selección del inversor al DTO y enviar el formulario confirmando los cambios.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Validar mediante pruebas de arquitectura que la relación con Inversor respete los límites modulares a través del mediador.
  - [ ] 5.2 Ejecutar la suite completa de pruebas unitarias y de integración.