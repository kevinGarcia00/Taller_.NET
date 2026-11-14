# Tareas de Implementación: Filtrar Histórico del Panel

- [ ] **1. Capa Domain**
  - [ ] 1.1 Crear el enum `TipoPeriodoHistorico` (`Dia`, `Mes`, `Anio`) en `Modules.Historico.Domain`.
  - [ ] 1.2 Crear el objeto de valor `PeriodoHistorico` (`Tipo`, `Inicio`, `Fin`) con el constructor que calcula el rango según el tipo y la fecha de referencia, y el método `Contiene(DateTime fecha)`.
  - [ ] 1.3 Validar en el constructor que `Tipo` sea un valor definido del enum y lanzar `HistoricoInvalidoException` en caso contrario.
  - [ ] 1.4 Crear las pruebas unitarias (xUnit) del comportamiento de dominio: rango de día, rango de mes, rango de año, `Contiene` dentro y fuera del rango, y tipo indefinido.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Reutilizar `HistoricoConfiguration`, `DbSet<Historico> Registros` de `IHistoricoDbContext` y `HistoricoRepository`.
  - [ ] 2.2 Implementar `ObtenerPorPanelYPeriodoAsync` filtrando por `PanelId` y por `Fecha >= periodo.Inicio && Fecha < periodo.Fin`, ordenado por `Fecha` descendente y proyectando solo `Id`, `Fecha` y `EnergiaTotal`.

- [ ] **3. Capa Application**
  - [ ] 3.1 Crear el DTO de entrada `FiltroHistoricoDto` (`TipoPeriodo`, `Referencia`) reutilizando `HistoricoListadoDto` como salida.
  - [ ] 3.2 Implementar el validador `FiltroHistoricoDtoValidator` usando FluentValidation (tipo de período válido y referencia requerida).
  - [ ] 3.3 Declarar en `IHistoricoService` el método `ObtenerRegistrosFiltradosPorPanelAsync(int panelId, int nodoId, int parqueSolarId, FiltroHistoricoDto filtro)` y en `IMediadorHistoricoParqueSolar` el método `ExistePanelAsync(int panelId, int nodoId, int parqueSolarId)`.
  - [ ] 3.4 Implementar el método en `HistoricoService`, con el repositorio, el mediador y el validador inyectados por constructor.
  - [ ] 3.5 Implementar la lectura: validar el filtro, confirmar el panel con el mediador, construir el `PeriodoHistorico`, consultar por período y retornar la lista sin modificar las entidades. Si no hay registros, retornar una lista vacía. Lanzar `PanelNoEncontradoException` si el panel no pertenece al nodo y parque.
  - [ ] 3.6 Crear las pruebas unitarias del servicio: filtro con resultados, filtro sin resultados y panel inexistente en el nodo.
  - [ ] 3.7 Preparar el código para que quede conectado con Auth0 y verifique el rol "Administrador".

- [ ] **4. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 4.1 Crear el componente `FiltrarHistorico.razor` con la directiva `@page "/parques/{ParqueId:int}/nodos/{NodoId:int}/paneles/{PanelId:int}/historico/filtrar"`.
  - [ ] 4.2 Proteger el componente con `<AuthorizeView Roles="Administrador">` o el atributo `[Authorize(Roles = "Administrador")]`.
  - [ ] 4.3 Diseñar el filtro con un `RadzenSelectBar`/`RadzenDropDown` para `TipoPeriodo`, un `RadzenDatePicker` para `Referencia` y un `RadzenButton` para aplicar.
  - [ ] 4.4 Configurar el `RadzenDataGrid<HistoricoListadoDto>` con las columnas `Id`, `Fecha` y `EnergiaTotal`, y el mensaje de listado vacío cuando no hay registros en el período.
  - [ ] 4.5 Invocar `ObtenerRegistrosFiltradosPorPanelAsync` en `OnValidSubmit` y notificar los errores con `NotificationService` de Radzen.

- [ ] **5. Verificación de Calidad (Guardarraíles)**
  - [ ] 5.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 5.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 5.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
