# Tareas de Implementación: Alta Registro de Mantenibilidad

- [ ] **1. Capa Domain**
  - [ ] 1.1 Crear la entidad `RegistroMantenibilidad`, el enum `EstadoPanelRegistrado` y los objetos de valor `Mantenedor` y `ContextoPanel`.
  - [ ] 1.2 Agregar las propiedades `Id`, `Mantenedor`, `Fecha`, `Observaciones`, `EstadoInicial`, `EstadoResultante` y `ContextoPanel`.
  - [ ] 1.3 Agregar el constructor con las validaciones de observaciones, fecha y estados.
  - [ ] 1.4 Crear las pruebas unitarias del comportamiento de dominio (observaciones vacías, fecha posterior a hoy, estados válidos y registro armado sin modificar el panel).

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Crear la clase `RegistroMantenibilidadConfiguration` implementando `IEntityTypeConfiguration<RegistroMantenibilidad>`.
  - [ ] 2.2 Configurar la clave primaria int, la longitud de `Observaciones`, los estados como string, el mapeo de `Mantenedor` y `ContextoPanel`, y el índice de `PanelId` + `Fecha`.
  - [ ] 2.3 Agregar `DbSet<RegistroMantenibilidad>` en `IHistorialMantenibilidadDbContext` e implementar `RegistroMantenibilidadRepository`.
  - [ ] 2.4 Generar la migración de base de datos (ej. `dotnet ef migrations add AddRegistroMantenibilidad`).
  - [ ] 2.5 Actualizar la base de datos (ej. `dotnet ef database update`).

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO `CrearRegistroMantenibilidadDto`.
  - [ ] 3.2 Crear la clase `CrearRegistroMantenibilidadDtoValidator` usando FluentValidation.
  - [ ] 3.3 Crear la interfaz `IRegistroMantenibilidadService` y su implementación `RegistroMantenibilidadService`, con el repositorio y los dos mediadores inyectados por constructor.
  - [ ] 3.4 Implementar `RegistrarRegistroAsync`: confirmar el panel, tomar su estado actual como estado inicial, confirmar el usuario de mantenimiento, persistir el registro y retornar el Id.
  - [ ] 3.5 Crear pruebas unitarias del servicio para panel inexistente, usuario sin rol "Mantenimiento" y alta exitosa.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `AltaRegistroMantenibilidad.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{PanelId:int}/mantenibilidad/alta"`.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Mantenimiento")]` o `<AuthorizeView>`.
  - [ ] 4.3 Diseñar el formulario con `<RadzenTemplateForm>`, `RadzenDropDown`, `RadzenDatePicker` y `RadzenTextArea`.
  - [ ] 4.4 Implementar `OnValidSubmit` para invocar `RegistrarRegistroAsync` en `IRegistroMantenibilidadService`.
  - [ ] 4.5 Mostrar el mensaje de éxito con `NotificationService` y redirigir al historial del panel.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni dependencia directa entre módulos.
  - [ ] 5.2 Ejecutar la inspección de código (o el análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
