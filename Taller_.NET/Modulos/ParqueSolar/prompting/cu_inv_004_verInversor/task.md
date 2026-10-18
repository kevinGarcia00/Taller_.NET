# Tareas de Implementación: Ver Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Inversor`.
  - [ ] 1.2 Verificar que la entidad exponga Nombre y Fecha de creación para la consulta, sin agregar métodos que modifiquen el inversor.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `InversorConfiguration`, `DbSet<Inversor>` e `InversorRepository`.
  - [ ] 2.2 Implementar la consulta por `Id`, proyectando solo `Id`, `Nombre` y `FechaCreacion`.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear `InversorDetalleDto` (`Id`, `Nombre`, `FechaCreacion`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Agregar a `IInversorService` e implementar en `InversorService` el método `ObtenerInversorDelNodoAsync(int nodoId, int parqueSolarId)`.
  - [ ] 3.3 Implementar la lectura confirmando antes el nodo con `IMediadorNodoInversor`. Si el nodo no tiene inversor, retornar vacío sin modificar nada.
  - [ ] 3.4 Crear pruebas unitarias para detalle exitoso, nodo sin inversor y nodo inválido.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear `DetalleInversor.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/inversor"`.
  - [ ] 4.2 Restringir la página con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Mostrar, en solo lectura, Nombre y Fecha de creación cuando el nodo tiene inversor.
  - [ ] 4.4 Mostrar el estado vacío y el botón "Registrar inversor" cuando el nodo no tiene inversor.
  - [ ] 4.5 Cargar los datos en `OnInitializedAsync` y notificar con `NotificationService` si la consulta falla.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni dependencia directa entre módulos.
  - [ ] 5.2 Ejecutar la inspección de código (o el análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
