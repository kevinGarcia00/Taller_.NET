# Tareas de Implementación: Listar Usuarios

- [x] **1. Capa Domain**
  - [x] 1.1 Reutilizar la entidad `Usuario` existente (sin entidad `Rol` local).
  - [x] 1.2 Validar que la entidad del dominio cuente con las propiedades necesarias para consultar la información general de los usuarios.

- [x] **2. Capa Infrastructure (Persistencia)**
  - [x] 2.1 Reutilizar la configuración de entidad existente (`UsuarioConfiguration`).
  - [x] 2.2 Verificar que la consulta proyectada consulte únicamente los campos necesarios de la base de datos sin sobrecargar memoria.

- [x] **3. Capa Application**
  - [x] 3.1 Crear el DTO de salida `UsuarioListadoDto` con las propiedades: `Id`, `Nombre`, `Apellido`, `Correo`, `Telefono`, `Auth0UserId` y `Roles` (`List<string>`).
  - [x] 3.2 Agregar a `IUsuarioService` e implementar en `UsuarioService` el método `ObtenerUsuariosAsync()`.
  - [x] 3.3 Implementar la lógica en `ObtenerUsuariosAsync`:
    - Verificar el permiso `users:read`.
    - Delegar la consulta de datos personales a `IUsuarioRepository.ObtenerListadoAsync()`.
    - Consultar los roles de cada usuario en Auth0 y mapearlos a nombres.
  - [x] 3.4 Crear Test Unitarios para verificar:
      - Retorno exitoso de la lista de usuarios cuando existen registros en la base de datos.
      - Retorno de una lista vacía cuando no existen usuarios registrados.
      - Mapeo y proyección correcta de los atributos del usuario y sus roles obtenidos de Auth0.

- [x] **4. Capa Presentation (Blazor Server con Radzen)**
  - [x] 4.1 Ubicar el componente `Usuarios.razor` con la directiva `@page "/usuarios"`.
  - [x] 4.2 Restringir el acceso con `[Authorize(Policy = PermisosUsuarios.PoliticaLeer)]` y `<AuthorizeView Policy=...>`.
  - [x] 4.3 Agregar la barra superior con el botón `RadzenButton` para navegar hacia el formulario de creación (`/usuarios/alta`).
  - [x] 4.4 Configurar el componente `RadzenDataGrid<UsuarioListadoDto>` enlazado a la colección devuelta por el servicio.
  - [x] 4.5 Definir las columnas `RadzenDataGridColumn` para `Nombre`, `Apellido`, `Correo`, `Telefono`, `Roles` y la columna de acciones (editar y eliminar).
  - [x] 4.6 Invocar `ObtenerUsuariosAsync()` dentro del ciclo de vida `OnInitializedAsync`, capturando posibles excepciones con `NotificationService`.

- [x] **5. Verificación de Calidad (Guardarraíles)**
  - [x] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [x] 5.2 Ejecutar inspección de código para asegurar que no hay code smells.
