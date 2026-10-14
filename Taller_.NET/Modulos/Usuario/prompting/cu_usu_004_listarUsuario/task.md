# Tareas de Implementación: Listar Usuarios

- [ ] **1. Capa Domain**
  - [ ] 1.1 Reutilizar las entidades `Usuario` y `Rol` existentes
  - [ ] 1.2 Validar que las entidades del dominio cuenten con las propiedades y relaciones necesarias para consultar la información general de los usuarios.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar las configuraciones de EF Core (`UsuarioConfiguration`,` RolConfiguration`) y el conjunto `DbSet` de `IUsersDbContext`.
  - [ ] 2.2 Verificar que la consulta proyectada consulte únicamente los campos necesarios de la base de datos sin sobrecargar memoria
  
- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO de salida `UsuarioListadoDto` con las propiedades: `Id`, `Nombre`, `Apellido`, `Correo`, `Telefono` y `Roles` (`List<string>`).
  - [ ] 3.2 Agregar a `IUsuarioService` e implementar en `UsuarioService` el método `ObtenerUsuariosAsync()`.
  - [ ] 3.3 Implementar la lógica en `ObtenerUsuariosAsync`:
    - Iniciar la consulta sobre `_dbContext.Usuarios`.
    - Aplicar ordenamiento por defecto.
    - Proyectar la entidad directamente a `UsuarioListadoDto` mediante `.Select()`.
    - Ejecutar la consulta con `ToListAsync()` y retornar la lista completa de usuarios.
  - [ ] 3.4 Crear Test Unitarios para verificar:
    - Retorno exitoso de la lista de usuarios cuando existen registros en la base de datos.
    - Retorno de una lista vacía cuando no existen usuarios registrados.
    - Mapeo y proyección correcta de los atributos del usuario y sus nombres de roles asociados.

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Ubicar el componente `Usuarios.razor` con la directiva `@page "/usuarios"`.
  - [ ] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [ ] 4.3 Agregar la barra superior con el botón `RadzenButton` para navegar hacia el formulario de creación (`/usuarios/alta`).
  - [ ] 4.4 Configurar el componente `RadzenDataGrid<UsuarioListadoDto>` enlazado a la colección devuelta por el servicio.
  - [ ] 4.5 Definir las columnas `RadzenDataGridColumn` para `Nombre`, `Apellido`, `Correo`, `Telefono`, `Roles` y la columna de acciones (botones de edición y eliminación).
  - [ ] 4.6 Invocar `ObtenerUsuariosAsync()` dentro del ciclo de vida `OnInitializedAsync`, capturando posibles excepciones con `NotificationService`.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [ ] 5.2 Ejecutar inspección de código (o análisis de SonarQube si está configurado en el pipeline local) para asegurar que no hay code smells.
