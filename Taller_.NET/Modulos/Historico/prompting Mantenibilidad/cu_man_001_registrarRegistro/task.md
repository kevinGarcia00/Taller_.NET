# Tareas de Implementación: Alta Registro de Mantenibilidad

- [ ] **1. Capa Domain**
  - [ ] 1.1 Crear la entidad `HistoricoMantenibilidad`, el enum `EstadoPanelRegistrado` y los objetos de valor `Mantenedor` y `ContextoPanel` en la capa de Dominio de `Modules.Historico`.
  - [ ] 1.2 Definir las propiedades (`Id`, `Mantenedor`, `Fecha`, `Observaciones`, `EstadoInicial`, `EstadoResultante`, `ContextoPanel`).
  - [ ] 1.3 Implementar el constructor con validación de observaciones, fecha no posterior a hoy y estados pertenecientes al enum. El constructor no modifica el panel.
  - [ ] 1.4 Crear las pruebas unitarias (xUnit) del comportamiento de dominio: registro válido, observaciones vacías y fecha posterior a hoy.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Crear `HistoricoMantenibilidadConfiguration` implementando `IEntityTypeConfiguration<HistoricoMantenibilidad>`.
  - [ ] 2.2 Configurar la clave primaria `Id`, la longitud máxima de `Observaciones` (hasta 500), los estados como string, el mapeo de `Mantenedor` y `ContextoPanel`, y el índice de `PanelId` + `Fecha`.
  - [ ] 2.3 Agregar `DbSet<HistoricoMantenibilidad> Registros` en `IHistoricoDbContext` e implementar `HistoricoMantenibilidadRepository` (`IHistoricoMantenibilidadRepository`).
  - [ ] 2.4 Generar la migración de Entity Framework (ej. `dotnet ef migrations add AddHistoricoMantenibilidad`).
  - [ ] 2.5 Aplicar la migración a la base de datos (ej. `dotnet ef database update`).

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO de entrada `CrearHistoricoMantenibilidadDto` (`IdMantenedor`, `Fecha`, `EstadoResultante`, `Observaciones`, `PanelId`, `NodoId`, `ParqueSolarId`).
  - [ ] 3.2 Implementar el validador `CrearHistoricoMantenibilidadDtoValidator` usando FluentValidation (identificadores requeridos, fecha no posterior a hoy, estado resultante válido y observaciones requeridas con máximo 500).
  - [ ] 3.3 Definir la interfaz `IHistoricoMantenibilidadService` y los mediadores `IMediadorHistoricoParqueSolar` e `IMediadorHistoricoUsuario` (cada uno, único canal con su módulo).
  - [ ] 3.4 Implementar `HistoricoMantenibilidadService`, con el repositorio y los dos mediadores inyectados por constructor.
  - [ ] 3.5 Implementar `RegistrarRegistroAsync(CrearHistoricoMantenibilidadDto dto)`: confirmar el panel en ParqueSolar, tomar su estado actual como estado inicial, confirmar que el usuario tiene rol "Mantenimiento", persistir el registro y retornar el `Id`.
  - [ ] 3.6 Crear las pruebas unitarias del servicio para panel inexistente, usuario sin rol "Mantenimiento" y alta exitosa.
  - [ ] 3.7 Preparar el código para que quede conectado con la implementación de Auth0 y verifique el rol "Mantenimiento".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `AltaHistoricoMantenibilidad.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{PanelId:int}/mantenibilidad/alta"`.
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Mantenimiento">` o el atributo `[Authorize(Roles = "Mantenimiento")]`.
  - [ ] 4.3 Diseñar el formulario con `<RadzenTemplateForm>` vinculado a `CrearHistoricoMantenibilidadDto`, usando `RadzenDropDown` para el usuario de mantenimiento y el estado resultante, `RadzenDatePicker` para la fecha y `RadzenTextArea` para las observaciones. Parque, nodo y panel quedan en solo lectura.
  - [ ] 4.4 Implementar `OnValidSubmit` para invocar `IHistoricoMantenibilidadService.RegistrarRegistroAsync(dto)`.
  - [ ] 4.5 Notificar el éxito con `NotificationService` de Radzen y navegar al historial del panel con `NavigationManager`.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
