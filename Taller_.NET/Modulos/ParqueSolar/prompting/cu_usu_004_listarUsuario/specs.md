# Especificaciones Técnicas: Listar Usuarios

## Arquitectura y Patrones

- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.User).
- **Patrones:** Application Services (Arquitectura en capas clásica o CQRS mediante Servicios).

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.User.Domain)

- **Entidades:** 
    - Reutilizar las entidades `Usuario` y `Rol` existentes.
- **Comportamiento:** 
  - Propiedades y métodos de dominio para consultar los datos básicos del usuario y sus relaciones.

### 2. Capa Application (Servicios)

- **DTO de Salida:** `UsuarioListadoDto`
    - Propiedades: `Id` (long), `Nombre` (string), `Apellido` (string), `Correo` (string), `Telefono` (string), `Roles` (List<string>).
- **Servicio:** `UsuarioService` (implementa la interfaz `IUsuarioService`)
  - Dependencias: `IUsersDbContext` (para acceso a BD).
  - Método: `ObtenerUsuariosAsync()`
  - Lógica: 
    1. Aplicar ordenamiento por defecto.
    2. Proyectar la entidad directamente a `UsuarioListadoDto` mediante `.Select()`.
    3. Ejecutar la consulta con `ToListAsync()` y retornar la lista completa de usuarios.

### 3. Capa Infrastructure (Persistencia)

- **Configuración de Entity Framework:** 
    -Reutilizar las configuraciones de entidades existentes (`UsuarioConfiguration` y `RolConfiguration`).
- `RolConfiguration : IEntityTypeConfiguration<Rol>`
  - Definir la tabla y longitud maxima de columnas.
  - Configurar Id como clave primaria de tipo long (bigint)
- **DbContext:** Asegurarse de exponer en `IUsersSbConxtext` los conjuntos:
    - `DbSet<Usuario> Usuario { get; set;} `
    - `DbSet<Rol> Roles { get; set;} `

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `Usuarios.razor`
  - **Ruta:** `@page "/usuarios"`
  - **Seguridad:** Utilizar el componente `<AuthorizeView Roles="Administrador">` para proteger el contenido o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Barra de herramientas superior con un `RadzenButton` ("Nuevo Usuario") que redirige a `/usuarios/alta` usando `NavigationManager`.
    - Componente `RadzenDataGrid<UsuarioListadoDto>` enlazado a la lista completa de usuarios.
    -Columnas
        - `RadzenDataGridColumn` para `Nombre`, `Apellido`, `Correo` y `Telefono`.
        - `RadzenDataGridColumn` personalizada para `Roles`, desplegando la lista de nombres mediante componentes visuales.
        - Columna de Acciones con componentes `RadzenButton` con íconos para Editar y Eliminar.
  - **Flujo:** 
    1. En el método del ciclo de vida `OnInitializedAsync`, el componente inyecta `IUsuarioService` e invoca `ObtenerUsuariosAsync()`.
    2. Los datos devueltos se asignan a la propiedad local enlazada al `RadzenDataGrid`.
    3. En caso de error durante la consulta, se captura la excepción y se muestra una alerta visual al Administrador mediante `NotificationService`.