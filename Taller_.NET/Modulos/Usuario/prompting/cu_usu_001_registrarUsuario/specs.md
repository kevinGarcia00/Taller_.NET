# Especificaciones Técnicas: Alta de Usuario

## Arquitectura y Patrones
- **Estilo:** Monolito Modular con Clean Architecture por Módulo (Modules.User).
- **Patrones:** Application Services (Arquitectura en capas clásica o CQRS mediante Servicios).

## Detalles de Implementación por Capa

### 1. Capa Domain (Modules.User.Domain)
- **Entidades:** 
    - **Usuario**
        - **Propiedades:** 
            - `Id` (Guid, PK)
            - `Nombre` (string, max 100 caracteres)
            - `Apellido` (string, max 100 caracteres)
            - `Correo` (string, max 100 caracteres)
            - `Telefono` (string, max 25 caracteres)
            - `Rol` (IReadOnlyCollection)
    - **Rol** 
        - **Porpiedades:**
            - `Id` (Guid, PK)
            - `Nombre` (string, max 25 caracteres)

- **Comportamiento:** 
    - Constructor con validaciones básicas (por ejemplo, `Nombre` y `Correo` no pueden ser null o whitespace).
    - Método de dominio para asignar roles al usuario (ej. AsignarRol(Rol rol)), garantizando la relación N:N.

### 2. Capa Application (Servicios)
- **DTO de Entrada:** `CrearUsuarioDto`
  - Propiedades: `Nombre`, `Apellido`, `Correo`, `Telefono`, `RolesIds (List<long>)`.
- **Validador:** `CrearUsuarioDtoValidator` (usando FluentValidation)
  - Reglas: `Nombre` requerido (max 100), `Apellido` requerida (max 100), `Correo` requerido (max 100) y formato valido, `RolesIds` requerido (al menos 1 ID de rol).
- **Servicio:** `UsuarioService` (implementa la interfaz `IUsuarioService`)
  - Dependencias: `IUsersDbContext` (para acceso a BD).
  - Método: `CrearUsuarioAsync(CrearUsuarioDto dto)`
  - Lógica: 
    1. Validar que no exista un Usuario registrado con el mismo Correo.
    2. Consultar la base de datos para verificar que todos los RolesIds enviados existan. Si alguno no existe, lanzar excepción.
    3. Instanciar la entidad Usuario y asociar las entidades Rol correspondientes.
    4. Agregar la entidad al DbSet y hacer SaveChangesAsync().
    5. Retornar el Id generado.

### 3. Capa Infrastructure (Persistencia)
- **Configuración de Entity Framework:** 

- `UsuarioConfiguration : IEntityTypeConfiguration<Usuario>`
  - Definir la tabla y longitud máxima de columnas.
  - Configurar el Id como clave primaria de tipo long (bigint).
  - Crear un Índice Único para la columna Correo para garantizar la regla de negocio a nivel de BD.
  - Configurar la relación N:N con Rol mediante una tabla de unión (ej. UsuarioRoles), definiendo las claves foráneas UsuarioId y RolId.
- `RolConfiguration : IEntityTypeConfiguration<Rol>`
  - Definir la tabla y longitud maxima de columnas.
  - Configurar Id como clave primaria de tipo long (bigint)
- **DbContext:** Asegurarse de exponer en `IUsersSbConxtext` los conjuntos:
    - `DbSet<Usuario> Usuario { get; set;} `
    - `DbSet<Rol> Roles { get; set;} `
- **Migraciones:** Se deberá generar y aplicar una migración para reflejar estos cambios en la base de datos.

### 4. Capa Presentation (Blazor Server con Radzen)
- **Componente:** `AltaUsuario.razor`
  - **Ruta:** `@page "/usuarios/alta"`
  - **Seguridad:** Utilizar el componente `<AuthorizeView Roles="Administrador">` para proteger el contenido o el atributo `[Authorize(Roles = "Administrador")]`.
  - **UI (Radzen):**
    - Utilizar `RadzenTemplateForm` vinculado al modelo `CrearUsuarioDto`.
    - Campos: 
        - `RadzenTextBox` para Nombre y Apellido, Telefono y Correo.
        - `RadzenDropDown` configurado con seleccion multiple para la propiedad `RolesIds`, mostrando los roles disponibles
    - Botón de envío: `RadzenButton` de tipo submit.
  - **Flujo:** 

  1. Se ejecuta el `OnValidSubmit`, el componente inyecta IUsuarioService y ejecuta el método `CrearUsuarioAsync(dto)`.
  2. Tras el éxito, muestra una notificación con `NotificationService` de Radzen y redirige a la lista de usuarios con `NavigationManager`.