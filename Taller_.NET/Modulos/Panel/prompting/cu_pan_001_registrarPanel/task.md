# Tareas de Implementación: Registrar Panel

- [ ] **1. Capa Domain**
  - [ ] 1.1 Crear la entidad `Panel`, el enum `EstadoPanel` y el objeto de valor `ContextoNodo` en la capa de Dominio.
  - [ ] 1.2 Agregar las propiedades de `Panel` (`Id`, `Nombre`, `FechaCreacion`, `Estado`, `Potencia`, `VidaUtil`, `ContextoNodo`).
  - [ ] 1.3 Agregar el constructor con validaciones básicas y el método `AsignarContexto(ContextoNodo contexto)`.
  - [ ] 1.4 Crear las pruebas unitarias del comportamiento de dominio (potencia mayor a cero, nombre obligatorio y asignación de contexto).

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Crear la clase `PanelConfiguration` implementando `IEntityTypeConfiguration<Panel>`.
  - [ ] 2.2 Configurar la clave primaria int, las longitudes máximas, el estado como string, el mapeo de `ContextoNodo` y el índice único de `Nombre` + `NodoId`.
  - [ ] 2.3 Agregar `DbSet<Panel>` en `IPanelDbContext` / `PanelDbContext` e implementar `PanelRepository`.
  - [ ] 2.4 Generar la migración de base de datos (ej. `dotnet ef migrations add AddPanel`).
  - [ ] 2.5 Actualizar la base de datos (ej. `dotnet ef database update`).

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO `CrearPanelDto`.
  - [ ] 3.2 Crear la clase `CrearPanelDtoValidator` usando FluentValidation.
  - [ ] 3.3 Crear la interfaz `IPanelService` y su implementación `PanelService`, con `IPanelRepository` e `IMediadorPanelNodo` inyectados por constructor.
  - [ ] 3.4 Implementar `CrearPanelAsync`: confirmar el nodo mediante el mediador, verificar unicidad del nombre en el nodo, asignar el contexto y retornar el Id.
  - [ ] 3.5 Crear pruebas unitarias del servicio para nodo inexistente, nombre duplicado en el nodo y alta exitosa.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `AltaPanel.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/alta"`.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Diseñar el formulario con `<RadzenTemplateForm>`, `RadzenTextBox`, `RadzenNumeric` y `RadzenDropDown`.
  - [ ] 4.4 Implementar `OnValidSubmit` para invocar `CrearPanelAsync` en `IPanelService`.
  - [ ] 4.5 Mostrar el mensaje de éxito con `NotificationService` y redirigir al listado de paneles del nodo.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni dependencia directa entre módulos.
  - [ ] 5.2 Ejecutar la inspección de código (o el análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
