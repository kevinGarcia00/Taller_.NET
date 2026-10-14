# Tareas de Implementación: Modificar Panel

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar la entidad `Panel`, el enum `EstadoPanel` y `ContextoNodo`.
  - [ ] 1.2 Implementar `ActualizarDatos(string nombre, EstadoPanel estado, decimal potencia, string vidaUtil)` sin modificar `Id`, `FechaCreacion` ni `ContextoNodo`.
  - [ ] 1.3 Crear las pruebas unitarias del método de actualización (datos válidos, nombre vacío y potencia no positiva).

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `PanelConfiguration`, `DbSet<Panel>` y `PanelRepository`.
  - [ ] 2.2 Verificar que la consulta de edición filtre por `Id` y `NodoId`.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear `ModificarPanelDto` (`Id`, `Nombre`, `Estado`, `Potencia`, `VidaUtil`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Crear el validador `ModificarPanelDtoValidator` con FluentValidation.
  - [ ] 3.3 Declarar en `IPanelService` e implementar en `PanelService`:
    - `Task<ModificarPanelDto> ObtenerParaEdicionAsync(int id, int nodoId)`
    - `Task ModificarPanelAsync(ModificarPanelDto dto)`
  - [ ] 3.4 Implementar `ModificarPanelAsync`: confirmar el nodo con el mediador, cargar el panel del nodo, validar el nombre, invocar `ActualizarDatos` y persistir.
  - [ ] 3.5 Crear pruebas unitarias para panel inexistente en el nodo, nombre duplicado, nodo inválido y modificación exitosa.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear `EditarPanel.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/editar/{Id:int}"`.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Cargar el panel en `OnInitializedAsync` mediante `ObtenerParaEdicionAsync(Id, NodoId)`.
  - [ ] 4.4 Diseñar el formulario con `<RadzenTemplateForm>`, campos editables y el contexto de parque y nodo en solo lectura.
  - [ ] 4.5 Implementar `OnValidSubmit` para invocar `ModificarPanelAsync`, notificar el éxito y volver al listado del nodo.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni dependencia directa entre módulos.
  - [ ] 5.2 Ejecutar la inspección de código (o el análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
