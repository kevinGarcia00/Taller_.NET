# Tareas de Implementación: Modificar Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar la entidad `Inversor`.
  - [ ] 1.2 Implementar `ActualizarNombre(string nombre)` sin modificar `Id` ni `FechaCreacion`.
  - [ ] 1.3 Crear las pruebas unitarias del método de actualización (nombre válido y nombre vacío o en blanco).

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `InversorConfiguration`, `DbSet<Inversor>` e `InversorRepository`.
  - [ ] 2.2 Verificar que la consulta de edición obtenga el inversor por el `Id` asociado al nodo, y no por un identificador ajeno a ese nodo.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear `ModificarInversorDto` (`Id`, `Nombre`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Crear el validador `ModificarInversorDtoValidator` con FluentValidation.
  - [ ] 3.3 Declarar en `IInversorService` e implementar en `InversorService`:
    - `Task<ModificarInversorDto> ObtenerParaEdicionAsync(int nodoId, int parqueSolarId)`
    - `Task ModificarInversorAsync(ModificarInversorDto dto)`
  - [ ] 3.4 Implementar `ModificarInversorAsync`: confirmar el nodo con el mediador, cargar solo el inversor de ese nodo, invocar `ActualizarNombre` y persistir.
  - [ ] 3.5 Crear pruebas unitarias para nodo inválido, nodo sin inversor y modificación exitosa.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear `EditarInversor.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/inversor/editar"`.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Cargar el inversor en `OnInitializedAsync` mediante `ObtenerParaEdicionAsync(NodoId, ParqueId)`.
  - [ ] 4.4 Diseñar el formulario con `<RadzenTemplateForm>`, el nombre editable y el contexto de parque y nodo en solo lectura.
  - [ ] 4.5 Implementar `OnValidSubmit` para invocar `ModificarInversorAsync`, notificar el éxito y volver a la vista del inversor del nodo.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni dependencia directa entre módulos.
  - [ ] 5.2 Ejecutar la inspección de código (o el análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
