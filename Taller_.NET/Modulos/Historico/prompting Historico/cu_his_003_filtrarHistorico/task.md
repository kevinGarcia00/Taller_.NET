# Tareas de Implementación: Filtrar Histórico del Panel

- [x] **1. Capa Domain**
  - [x] 1.1 *(Eliminado por cambio de alcance: ya no existe `TipoPeriodoHistorico`.)*
  - [x] 1.2 *(Eliminado por cambio de alcance: ya no existe el objeto de valor `PeriodoHistorico`.)*
  - [x] 1.3 *(Eliminado por cambio de alcance.)*
  - [x] 1.4 *(Eliminado por cambio de alcance: se borraron las pruebas de `PeriodoHistorico`.)*

- [x] **2. Capa Infrastructure (Persistencia)**
  - [x] 2.1 Reutilizar `HistoricoConfiguration`, `DbSet<Historico> Registros` de `IHistoricoDbContext` y `HistoricoRepository`.
  - [x] 2.2 Implementar `ObtenerPorPanelYFechaAsync` filtrando por `PanelId` y por el día completo de la fecha indicada, ordenado por `Fecha` descendente y proyectando solo `Id`, `Fecha` y `EnergiaTotal`.

- [x] **3. Capa Application**
  - [x] 3.1 Crear el DTO de entrada `FiltroHistoricoDto` (`Referencia`) reutilizando `HistoricoListadoDto` como salida.
  - [x] 3.2 Implementar el validador `FiltroHistoricoDtoValidator` usando FluentValidation (referencia requerida).
  - [x] 3.3 Declarar en `IHistoricoService` el método `ObtenerRegistrosFiltradosPorPanelAsync(int panelId, int nodoId, int parqueSolarId, FiltroHistoricoDto filtro)` y en `IMediadorHistoricoParqueSolar` el método `ExistePanelAsync(int panelId, int nodoId, int parqueSolarId)`.
  - [x] 3.4 Implementar el método en `HistoricoService`, con el repositorio, el mediador y el validador inyectados por constructor.
  - [x] 3.5 Implementar la lectura: validar el filtro, confirmar el panel con el mediador, consultar los registros de la fecha indicada y retornar la lista sin modificar las entidades. Si no hay registros, retornar una lista vacía. Lanzar `PanelNoEncontradoException` si el panel no pertenece al nodo y parque.
  - [ ] 3.6 Crear las pruebas unitarias del servicio: filtro con resultados, filtro sin resultados y panel inexistente en el nodo. *(Omitido: las reglas del proyecto prohíben pruebas unitarias en la capa Application; solo el Domain se prueba.)*
  - [x] 3.7 Preparar el código para que quede conectado con Auth0 y verifique el rol "Administrador".

- [x] **4. Capa Presentation (Blazor Server con Radzen)**
  - [x] 4.1 Agregar en la ficha del panel (`DetallePanel.razor`) el botón **"Ver histórico"** (visible para el rol "Administrador") y, desde la tabla del histórico, el acceso al filtro por fecha.
  - [x] 4.2 Integrar el filtro en `HistorialPanel.razor` (misma sección del listado), sin página aparte.
  - [x] 4.3 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [x] 4.4 Diseñar el filtro solo con un `RadzenDatePicker` para `Referencia` (búsqueda de una fecha específica) y botones "Buscar"/"Limpiar".
  - [x] 4.5 Configurar el `RadzenDataGrid<HistoricoListadoDto>` con las columnas `Id`, `Fecha` y `EnergiaTotal`, y el mensaje de listado vacío cuando no hay registros para la fecha seleccionada.
  - [x] 4.6 Invocar `ObtenerRegistrosFiltradosPorPanelAsync` en `OnValidSubmit` y notificar los errores con `NotificationService` de Radzen.

- [x] **5. Verificación de Calidad (Guardarraíles)**
  - [x] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [x] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [x] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo. *(Ejecutado: 0 hallazgos en el módulo Historico. Quedan 11 hallazgos preexistentes en archivos de tests (`UsuarioTests`, `ParqueTests`) pendientes de autorización explícita para modificarlos.)*
