# Tareas de Implementación: Modificar Panel

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar la entidad `Panel`, el enum `EstadoPanel` y la navegación `Nodo` de `Modules.ParqueSolar`.
  - [ ] 1.2 Implementar `ActualizarDatos(string nombre, EstadoPanel estado, decimal potencia, string vidaUtil)` sin modificar `Id`, `FechaCreacion` ni `Nodo`.
  - [ ] 1.3 Crear las pruebas unitarias (xUnit) del método: datos válidos, nombre vacío y potencia no positiva.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `PanelConfiguration`, `DbSet<Panel> Paneles` de `IParqueDbContext` y `PanelRepository`.
  - [ ] 2.2 Verificar que la consulta de edición filtre por `Id` y `NodoId`.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO de entrada `ModificarPanelDto` (`Id`, `Nombre`, `Estado`, `Potencia`, `VidaUtil`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Implementar el validador `ModificarPanelDtoValidator` usando FluentValidation.
  - [ ] 3.3 Declarar en `IPanelService` los métodos `ObtenerParaEdicionAsync(int id, int nodoId)` y `ModificarPanelAsync(ModificarPanelDto dto)`.
  - [ ] 3.4 Implementar ambos métodos en `PanelService`, con `IPanelRepository` e `INodoRepository` inyectados por constructor.
  - [ ] 3.5 Implementar `ModificarPanelAsync`: cargar el nodo del parque, cargar el panel de ese nodo, validar que el nombre no esté repetido, invocar `ActualizarDatos` y persistir.
  - [ ] 3.6 Crear las pruebas unitarias del servicio para panel inexistente en el nodo, nombre duplicado, nodo inválido y modificación exitosa.
  - [ ] 3.7 Preparar el código para que quede conectado con la implementación de Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `EditarPanel.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/editar/{Id:int}"`.
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.3 Diseñar el formulario con `<RadzenTemplateForm>` vinculado a `ModificarPanelDto`, usando `RadzenTextBox` para Nombre y Vida útil, `RadzenNumeric` para Potencia y `RadzenDropDown` para Estado. Parque, nodo, identificador y fecha de creación quedan en solo lectura.
  - [ ] 4.4 Cargar el panel en `OnInitializedAsync` con `ObtenerParaEdicionAsync` e implementar `OnValidSubmit` para invocar `ModificarPanelAsync`.
  - [ ] 4.5 Notificar el éxito con `NotificationService` de Radzen y navegar al listado de paneles del nodo con `NavigationManager`.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
