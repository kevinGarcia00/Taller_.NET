# Tareas de Implementación: Ver Paneles

- [ ] **1. Capa Domain**
  - [ ] 1.1 Verificar que la entidad `Nodo` mantenga su identidad, contexto de parque y datos de consulta necesarios para la operación.
  - [ ] 1.2 Mantener la relación conceptual de que un nodo puede estar asociado a varios paneles, sin introducir la entidad `Panel` en el dominio de `Nodo`.
  - [ ] 1.3 Confirmar que la operación es de lectura y no modifica el estado del nodo ni de los paneles.

- [ ] **2. Capa Infrastructure (Persistencia y Comunicación Inter-Módulo)**
  - [ ] 2.1 Crear o extender `MediadorNodoPanel` para consultar los paneles asociados a un nodo desde el módulo `Panel`.
  - [ ] 2.2 Implementar la consulta de solo lectura filtrada por `NodoId` y `ParqueSolarId`, ordenando los resultados para una presentación estable.
  - [ ] 2.3 Asegurar que la capa del módulo `Panel` exponga los datos ya proyectados a `PanelListadoDto` o a un DTO equivalente de integración.
  - [ ] 2.4 Garantizar que `Nodo` no dependa directamente de entidades ni repositorios del módulo `Panel`.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO `PanelListadoDto` con las propiedades necesarias para la grilla (`Id`, `Nombre`, `EstadoPanel`, `Potencia`, `VidaUtil`, `FechaCreacion`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Definir en `IMediadorNodoPanel` el método `ObtenerPanelesPorNodoAsync(long nodoId, long parqueSolarId)`.
  - [ ] 3.3 Declarar en `INodoService` la firma `Task<List<PanelListadoDto>> ObtenerPanelesPorNodoAsync(long nodoId, long parqueSolarId)`.
  - [ ] 3.4 Implementar la lógica en `NodoService` validando la existencia del nodo y que pertenezca al parque solar indicado.
  - [ ] 3.5 Manejar el caso de nodo sin paneles asociados devolviendo una lista vacía.
  - [ ] 3.6 Crear Tests Unitarios para los escenarios de nodo con paneles, nodo sin paneles y nodo con contexto inconsistente.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `VerPaneles.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles"`.
  - [ ] 4.2 Proteger el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView Roles="Administrador">`.
  - [ ] 4.3 En `OnParametersSetAsync` o `OnInitializedAsync`, obtener `ParqueId` y `NodoId` desde la URL e invocar `INodoService.ObtenerPanelesPorNodoAsync`.
  - [ ] 4.4 Diseñar la grilla `RadzenDataGrid<PanelListadoDto>` con columnas para `Nombre`, `EstadoPanel`, `Potencia`, `VidaUtil` y `FechaCreacion`.
  - [ ] 4.5 Implementar el `EmptyTemplate` con el texto: “Este nodo no tiene paneles asociados.”
  - [ ] 4.6 Mostrar notificaciones con `NotificationService` si falla la carga.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [ ] 5.2 Ejecutar el análisis estático con SonarQube para comprobar code smells, riesgos de seguridad y cumplimiento de calidad.
  - [ ] 5.3 Ejecutar inspección de código para asegurar que no hay code smells.
  - [ ] 5.4 Validar que un nodo ajeno al parque no pueda devolver paneles del contexto incorrecto.
  - [ ] 5.5 Confirmar que un usuario sin rol `Administrador` no pueda acceder a la funcionalidad.

