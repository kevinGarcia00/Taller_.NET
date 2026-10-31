# Tareas de Implementación: Ver Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar `Inversor` y la navegación desde `Nodo` en `Modules.ParqueSolar`.
  - [ ] 1.2 Verificar que exponga `Nombre` y `FechaCreacion` para la consulta, sin agregar métodos que modifiquen el inversor.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `InversorConfiguration`, `DbSet<Inversor> Inversores` de `IParqueDbContext` e `InversorRepository`.
  - [ ] 2.2 Implementar la consulta del inversor asignado al nodo, proyectando solo `Id`, `Nombre` y `FechaCreacion`.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO de salida `InversorDetalleDto` (`Id`, `Nombre`, `FechaCreacion`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Declarar en `IInversorService` el método `ObtenerInversorDelNodoAsync(int nodoId, int parqueSolarId)`.
  - [ ] 3.3 Implementar el método en `InversorService`, con `IInversorRepository` e `INodoRepository` inyectados por constructor.
  - [ ] 3.4 Implementar la lectura: cargar el nodo del parque. Si no tiene inversor, retornar vacío. Si tiene, proyectarlo a `InversorDetalleDto` sin modificarlo.
  - [ ] 3.5 Crear las pruebas unitarias del servicio para detalle exitoso, nodo sin inversor y nodo inválido.
  - [ ] 3.6 Preparar el código para que quede conectado con la implementación de Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `DetalleInversor.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/inversor"`.
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.3 Mostrar en solo lectura Nombre y Fecha de creación cuando el nodo tiene inversor, con botones para editar y eliminar.
  - [ ] 4.4 Si el nodo no tiene inversor, mostrar el estado vacío y un `RadzenButton` ("Registrar inversor") hacia el alta.
  - [ ] 4.5 Cargar los datos en `OnInitializedAsync` con `ObtenerInversorDelNodoAsync`. Si la consulta falla, notificar con `NotificationService` de Radzen.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
