# Tareas de Implementación: Eliminar Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Inversor`.
  - [ ] 1.2 Mantener la pertenencia al nodo como condición de carga: no se elimina un inversor que no sea el asociado al nodo seleccionado.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `InversorConfiguration`, `DbSet<Inversor>` e `InversorRepository`.
  - [ ] 2.2 Exponer en el repositorio la operación de baja del inversor cargado y la confirmación de cambios.

- [ ] **3. Capa Application**
  - [ ] 3.1 Agregar a `IInversorService` e implementar en `InversorService` el método `EliminarInversorAsync(int nodoId, int parqueSolarId)`.
  - [ ] 3.2 Implementar la lógica de `EliminarInversorAsync`:
    - Confirmar el nodo y el parque mediante `IMediadorNodoInversor`.
    - Obtener el inversor asociado a ese nodo.
    - Lanzar excepción si el nodo no tiene inversor.
    - Remover el inversor, persistir los cambios y desvincular el `InversorId` del nodo.
  - [ ] 3.3 Crear pruebas unitarias para nodo inválido, nodo sin inversor y eliminación exitosa.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Ubicar la acción de eliminar en `DetalleInversor.razor`, la vista del inversor del nodo seleccionado.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Agregar el botón de eliminación en esa vista.
  - [ ] 4.4 Integrar `DialogService` para confirmar antes de eliminar.
  - [ ] 4.5 Invocar `EliminarInversorAsync`, notificar con `NotificationService` y mostrar el estado sin inversor. Ante un error de negocio, mostrar una advertencia sin romper la página.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni dependencia directa entre módulos.
  - [ ] 5.2 Ejecutar la inspección de código (o el análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
