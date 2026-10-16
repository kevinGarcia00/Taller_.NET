# Tareas de Implementación: Registrar Inversor

- [ ] **1. Capa Domain**
  - [ ] 1.1 Crear la entidad `Inversor` en la capa de Dominio.
  - [ ] 1.2 Agregar las propiedades de `Inversor` (`Id`, `Nombre`, `FechaCreacion`).
  - [ ] 1.3 Agregar el constructor que exige el nombre, asigna `FechaCreacion` y rechaza un nombre vacío.
  - [ ] 1.4 Crear las pruebas unitarias del constructor (nombre válido y nombre vacío o en blanco).

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Crear la clase `InversorConfiguration` implementando `IEntityTypeConfiguration<Inversor>`.
  - [ ] 2.2 Configurar la clave primaria int y la longitud máxima de `Nombre`, sin claves foráneas hacia otros módulos.
  - [ ] 2.3 Agregar `DbSet<Inversor>` en `IInversorDbContext` / `InversorDbContext` e implementar `InversorRepository`.
  - [ ] 2.4 Generar la migración de base de datos (ej. `dotnet ef migrations add AddInversor`).
  - [ ] 2.5 Actualizar la base de datos (ej. `dotnet ef database update`).

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO `CrearInversorDto`.
  - [ ] 3.2 Crear la clase `CrearInversorDtoValidator` usando FluentValidation.
  - [ ] 3.3 Crear la interfaz `IInversorService` y su implementación `InversorService`, con `IInversorRepository` e `IMediadorNodoInversor` inyectados por constructor.
  - [ ] 3.4 Implementar `CrearInversorAsync`: confirmar el nodo mediante el mediador, rechazar el alta si el nodo ya tiene inversor, persistir y asociar el Id al nodo.
  - [ ] 3.5 Crear pruebas unitarias del servicio para nodo inexistente, nodo que ya tiene inversor y alta exitosa.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `AltaInversor.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/inversor/alta"`.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Diseñar el formulario con `<RadzenTemplateForm>` y `RadzenTextBox` para el nombre, con el parque y el nodo en solo lectura.
  - [ ] 4.4 Implementar `OnValidSubmit` para invocar `CrearInversorAsync` en `IInversorService`.
  - [ ] 4.5 Mostrar el mensaje de éxito con `NotificationService` y redirigir a la vista del inversor del nodo. Si el nodo ya tiene inversor, informar y no abrir un segundo alta.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni dependencia directa entre módulos.
  - [ ] 5.2 Ejecutar la inspección de código (o el análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
