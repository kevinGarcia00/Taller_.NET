# Tareas de Implementación: Eliminar Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Inversor` y la navegación desde `Nodo` en `Modules.ParqueSolar`.
  - [ ] 1.2 Implementar `QuitarInversor()` en `Nodo` para dejar vacía la relación 1:1.
  - [ ] 1.3 Crear la prueba unitaria (xUnit) de que un nodo con inversor queda sin inversor después de quitarlo.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `InversorConfiguration`, `DbSet<Inversor> Inversores` de `IParqueDbContext` e `InversorRepository`.
  - [ ] 2.2 Exponer en el repositorio la baja del inversor cargado y la confirmación de cambios.

- [ ] **3. Capa Application**
  - [ ] 3.1 Declarar en `IInversorService` el método `EliminarInversorAsync(int nodoId, int parqueSolarId)`.
  - [ ] 3.2 Implementar el método en `InversorService`, con `IInversorRepository` e `INodoRepository` inyectados por constructor.
  - [ ] 3.3 Implementar la lógica: cargar el nodo del parque, obtener su inversor, lanzar excepción si no tiene, quitar la relación, remover el inversor y persistir.
  - [ ] 3.4 Crear las pruebas unitarias del servicio para nodo inválido, nodo sin inversor y eliminación exitosa.
  - [ ] 3.5 Preparar el código para que quede conectado con la implementación de Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Ubicar la acción de eliminar en `DetalleInversor.razor` (`@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/inversor"`).
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.3 Agregar un `RadzenButton` de eliminación e integrar `DialogService` para confirmar, mostrando el nombre del inversor.
  - [ ] 4.4 Si se confirma, invocar `IInversorService.EliminarInversorAsync(nodoId, parqueSolarId)`.
  - [ ] 4.5 Notificar el resultado con `NotificationService` de Radzen y mostrar el estado sin inversor. Ante un error de negocio, mostrar una advertencia sin romper la página.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
