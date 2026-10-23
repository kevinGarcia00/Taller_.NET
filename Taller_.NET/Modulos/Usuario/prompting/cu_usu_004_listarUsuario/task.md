# Tareas de Implementación: Listar Usuarios

- [x] **1. Capa Domain**
  - [x] 1.1 Reutilizar las entidades `Usuario` y `Rol` existentes.
  - [x] 1.2 Validar que las entidades del dominio cuenten con las propiedades y relaciones necesarias para consultar la información general de los usuarios.

- [x] **2. Capa Infrastructure (Persistencia)**
  - [x] 2.1 Reutilizar las configuraciones de entidades existentes (`UsuarioConfiguration` y `RolConfiguration`).
  - [x] 2.2 Verificar que la consulta proyectada consulte únicamente los campos necesarios de la base de datos sin sobrecargar memoria.

- [x] **3. Capa Application**
  - [x] 3.1 Crear el DTO de salida `UsuarioListadoDto` con las propiedades: `Id`, `Nombre`, `Apellido`, `Correo`, `Telefono` y `Roles` (`List<string>`).
  - [x] 3.2 Agregar a `IUsuarioService` e implementar en `UsuarioService` el método `ObtenerUsuariosAsync()`.
  - [x] 3.3 Implementar la lógica en `ObtenerUsuariosAsync`:
    - Iniciar la consulta sobre `_dbContext.Usuarios`.
    - Aplicar ordenamiento por defecto.
    - Proyectar la entidad directamente a `UsuarioListadoDto` mediante `.Select()`.
    - Ejecutar la consulta con `ToListAsync()` y retornar la lista completa de usuarios.
  - [x] 3.4 Crear Test Unitarios para verificar:
      - Retorno exitoso de la lista de usuarios cuando existen registros en la base de datos.
      - Retorno de una lista vacía cuando no existen usuarios registrados.
      - Mapeo y proyección correcta de los atributos del usuario y sus nombres de roles asociados.

- [x] **4. Capa Presentation (Blazor Server con Radzen)**
  - [x] 4.1 Ubicar el componente `Usuarios.razor` con la directiva `@page "/usuarios"`.
  - [x] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [x] 4.3 Agregar la barra superior con el botón `RadzenButton` para navegar hacia el formulario de creación (`/usuarios/alta`).
  - [x] 4.4 Configurar el componente `RadzenDataGrid<UsuarioListadoDto>` enlazado a la colección devuelta por el servicio.
  - [x] 4.5 Definir las columnas `RadzenDataGridColumn` para `Nombre`, `Apellido`, `Correo`, `Telefono`, `Roles` y la columna de acciones (botón de edición).
  - [x] 4.6 Invocar `ObtenerUsuariosAsync()` dentro del ciclo de vida `OnInitializedAsync`, capturando posibles excepciones con `NotificationService`.

- [x] **5. Verificación de Calidad (Guardarraíles)**
  - [x] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [x] 5.2 Ejecutar inspección de código para asegurar que no hay code smells.

## Notas de implementación

- **Componente único**: el enunciado-plantilla menciona `Usuarios.razor` (CU-004) y `ListaUsuarios.razor` (CU-003). Se implementó una sola página `Usuarios.razor` en la ruta `/usuarios`, que alojará también el botón de eliminar cuando se implemente el caso de uso CU-003.
- **Botón de eliminar**: la columna de acciones incluye por ahora el botón **Editar** (con la navegación a `/usuarios/editar/{Id}`). El botón de eliminar y su modal de confirmación se agregan con el caso de uso CU-003, que ya define esa interacción.
- **Proyección**: la consulta se resuelve en la base con `.Select()` sobre `Usuarios`, trayendo solo las columnas necesarias y los nombres de los roles (`u.Roles.Select(r => r.Nombre)`); el orden por defecto es por apellido y luego por nombre.
- **Acceso**: `ObtenerUsuariosAsync` verifica en el servidor que el usuario autenticado tenga el rol `Administrador`, además del `[Authorize(Roles = "Administrador")]` de la página.
- **Pruebas**: este caso de uso no agrega comportamiento de dominio, por lo que no se generan pruebas nuevas (AGENTS.md reserves las pruebas unitarias para la capa Domain y prohíbe generar tests en Application, Infrastructure y Presentation). Suite completa: **93 pruebas en verde**, incluidas las 2 de arquitectura (ArchUnitNET).
- **Verificación funcional**: comprobada en navegador (filas con sus roles, botón "Nuevo Usuario" y navegación desde el botón de editar hacia el formulario precargado).
