# Tareas de Implementación: Modificar Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar la entidad `Inversor` y la navegación desde `Nodo` en `Modules.ParqueSolar`.
  - [ ] 1.2 Implementar `ActualizarNombre(string nombre)` sin modificar `Id`, `FechaCreacion` ni el nodo.
  - [ ] 1.3 Crear las pruebas unitarias (xUnit) del método: nombre válido y nombre vacío o en blanco.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `InversorConfiguration`, `DbSet<Inversor> Inversores` de `IParqueDbContext` e `InversorRepository`.
  - [ ] 2.2 Verificar que la consulta de edición obtenga el inversor asignado al nodo, y no uno ajeno a ese nodo.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO de entrada `ModificarInversorDto` (`Id`, `Nombre`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Implementar el validador `ModificarInversorDtoValidator` usando FluentValidation.
  - [ ] 3.3 Declarar en `IInversorService` los métodos `ObtenerParaEdicionAsync(int nodoId, int parqueSolarId)` y `ModificarInversorAsync(ModificarInversorDto dto)`.
  - [ ] 3.4 Implementar ambos métodos en `InversorService`, con `IInversorRepository` e `INodoRepository` inyectados por constructor.
  - [ ] 3.5 Implementar `ModificarInversorAsync`: cargar el nodo del parque, cargar solo su inversor, invocar `ActualizarNombre` y persistir.
  - [ ] 3.6 Crear las pruebas unitarias del servicio para nodo inválido, nodo sin inversor y modificación exitosa.
  - [ ] 3.7 Preparar el código para que quede conectado con la implementación de Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `EditarInversor.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/inversor/editar"`.
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.3 Diseñar el formulario con `<RadzenTemplateForm>` vinculado a `ModificarInversorDto`, usando `RadzenTextBox` para Nombre. Parque, nodo, identificador y fecha de creación quedan en solo lectura.
  - [ ] 4.4 Cargar el inversor en `OnInitializedAsync` con `ObtenerParaEdicionAsync` e implementar `OnValidSubmit` para invocar `ModificarInversorAsync`.
  - [ ] 4.5 Notificar el éxito con `NotificationService` de Radzen y navegar a la vista del inversor del nodo con `NavigationManager`.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
