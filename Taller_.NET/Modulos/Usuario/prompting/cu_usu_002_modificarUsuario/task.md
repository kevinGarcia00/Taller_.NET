# Tareas de Implementación: Modificar Usuario

- [x] **1. Capa Domain**
  - [x] 1.1 Reutilizar las entidades `Usuario` y `Rol` existentes.
  - [x] 1.2 Verificar/implementar los métodos de dominio en la entidad `Usuario`:
    - `ActualizarDatosPersonales(string nombre, string apellido, string correo, string telefono)` con validaciones de campos requeridos.
    - `ActualizarRoles(IEnumerable<Rol> nuevosRoles)` para refrescar la colección de roles y garantizar consistencia en la relación N:N.

- [x] **2. Capa Infrastructure (Persistencia)**
  - [x] 2.1 Reutilizar las configuraciones de EF Core (`UsuarioConfiguration`,` RolConfiguration`) y el conjunto `DbSet` de `IUsuarioDbContext`.

- [x] **3. Capa Application**
  - [x] 3.1 Crear la estructura del DTO `ModificarUsuarioDto (Id, Nombre, Apellido, Correo, Telefono, RolesIds)`.
  - [x] 3.2 Crear el validador `ModificarUsuarioDtoValidator` con FluentValidation.
  - [x] 3.3 Declarar en `IUsuarioService` e implementar en `UsuarioService` los métodos:
    - `Task<ModificarUsuarioDto> ObtenerParaEdicionAsync(Guid id)`
    - `Task ModificarUsuarioAsync(ModificarUsuarioDto dto)`
  - [x] 3.4 Implementar la lógica en `ModificarUsuarioAsync`:
    - Consultar el usuario por `Id` incluyendo sus roles asociados (lanzar excepción si no existe).
    - Validar que el correo ingresado no pertenezca a otro usuario registrado.
    - Consultar la base de datos para verificar que todos los `RolesIds` enviados existan.
    - Invocar `ActualizarDatosPersonales` y `ActualizarRoles` sobre la entidad.
    - Persistir cambios mediante `SaveChangesAsync()`.
  - [x] 3.5 Crear Test Unitarios para verificar:
      - Error si el usuario a modificar no existe.
      - Error si el correo ingresado pertenece a otro usuario.
      - Error si alguno de los `RolesIds` no existe.
      - Modificación exitosa al pasar validaciones.

- [x] **4. Capa Presentation (Blazor Server con Radzen)**
  - [x] 4.1 Crear el componente `EditarUsuario.razor` con la directiva `@page "/usuarios/editar/{Id:guid}"`.
  - [x] 4.2 Restringir el acceso con `[Authorize(Roles = "Administrador")]` o `<AuthorizeView>`.
  - [x] 4.3 Cargar los datos del usuario en `OnParametersSetAsync` invocando `ObtenerParaEdicionAsync(Id)`.
  - [x] 4.4 Diseñar el formulario con `<RadzenTemplateForm>` usando `RadzenTextBox` para los datos personales y `RadzenDropDown` para los roles.
  - [x] 4.5 Implementar `OnValidSubmit` para invocar `ModificarUsuarioAsync`, mostrar notificación de éxito con `NotificationService` y redirigir al listado (/usuarios).

- [x] **5. Verificación de Calidad (Guardarraíles)**
  - [x] 5.1 Ejecutar suite de pruebas  para validar que no existan violaciones de Clean Architecture.
  - [x] 5.2 Ejecutar inspección de código para asegurar que no hay code smells.

## Notas de implementación

- **Identificador**: la ruta usa `{Id:guid}` porque `Usuario.Id` es un `Guid` generado por la aplicación (el enunciado-plantilla propone `long`).
- **Acceso a la edición**: se agregó la ruta `/usuarios/editar` (sin identificador) que permite **ingresar el id** del usuario a modificar, ya que el listado (CU-004) todavía no existe. Si el identificador no corresponde a un usuario registrado, se informa con un mensaje de error descriptivo.
- **Identificador de solo lectura**: el campo del id se muestra deshabilitado y no forma parte de los campos editables.
- **Precarga**: `ObtenerParaEdicionAsync` devuelve el `ModificarUsuarioDto` con los datos actuales y los ids de los roles asignados, que quedan preseleccionados en el desplegable.
- **Persistencia de la relación N:N**: `UsuarioRepository.ActualizarAsync` usa `Update` + `SaveChangesAsync`; la entidad se carga con `Include(u => u.Roles)` para que EF detecte las altas y bajas en la tabla `UsuarioRoles`.
- **Atomicidad del dominio**: `ActualizarDatosPersonales` valida todos los datos antes de asignarlos, de modo que un dato inválido no deja al usuario modificado a medias (cubierto por prueba unitaria).
- **Validación en la UI**: la lógica de validación se extrajo a `ValidacionFormulario` (compartida por el alta y la modificación) para no duplicar el manejo del `EditContext` y del `ValidationMessageStore`.
- **Pruebas**: `Tests/Modulos/Usuario/Domain/UsuarioTests.cs` cubre `ActualizarDatosPersonales` (validaciones, normalización del correo, atomicidad) y `ActualizarRoles` (reemplazo, múltiples roles, roles repetidos, lista vacía). Suite completa: **93 pruebas en verde**, incluidas las 2 de arquitectura (ArchUnitNET).
- **Verificación funcional**: comprobada en navegador (precarga del formulario, modificación de datos y de roles, correo duplicado y usuario inexistente).
