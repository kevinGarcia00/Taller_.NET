# Tareas de Implementación: Alta Registro Histórico

- [ ] **1. Capa Domain**
  - [ ] 1.1 Crear la entidad `Historico` y el objeto de valor `ContextoPanel` en `Modules.Historico.Domain`.
  - [ ] 1.2 Definir las propiedades de `Historico` (`Id`, `Fecha`, `EnergiaTotal`, `ContextoPanel`) y de `ContextoPanel` (`PanelId`, `NodoId`, `ParqueSolarId`).
  - [ ] 1.3 Implementar el constructor con validación de energía no negativa, fecha no futura y contexto del panel obligatorio. Crear `HistoricoInvalidoException`. El constructor no modifica el panel.

- [ ] **2. Capa Infrastructure (Persistencia)**
  - [ ] 2.1 Crear `HistoricoConfiguration` implementando `IEntityTypeConfiguration<Historico>`.
  - [ ] 2.2 Configurar la clave primaria `Id`, la precisión decimal de `EnergiaTotal`, el mapeo de `ContextoPanel` y el índice de `PanelId` + `Fecha`.
  - [ ] 2.3 Agregar `DbSet<Historico> Registros` en `IHistoricoDbContext` e implementar `HistoricoRepository` (`IHistoricoRepository`).
  - [ ] 2.4 Generar la migración de Entity Framework.
  - [ ] 2.5 Aplicar la migración a la base de datos.

- [ ] **3. Capa Application**
  - [ ] 3.1 Definir el puerto `IMediadorHistoricoParqueSolar` y el DTO `EnergiaPanelDto` (`PanelId`, `NodoId`, `ParqueSolarId`, `EnergiaTotal`) en Historico.
  - [ ] 3.2 Definir la interfaz `IHistoricoService` con el método `GenerarRegistrosDiariosAsync`.
  - [ ] 3.3 Implementar `HistoricoService`, con el repositorio y el mediador inyectados por constructor.
  - [ ] 3.4 Implementar la generación: obtener los paneles y su energía con el mediador, evitar duplicados por panel y fecha, instanciar `Historico` y persistir los registros.
  - [ ] 3.5 Crear las pruebas unitarias del servicio: generación con paneles, generación sin paneles y no duplicar el registro del mismo día.

- [ ] **4. Proceso programado (cada 24 horas)**
  - [ ] 4.1 Crear `GeneradorHistoricoBackgroundService : BackgroundService` con un `PeriodicTimer` de 24 horas configurables.
  - [ ] 4.2 Crear un scope por ejecución e invocar `IHistoricoService.GenerarRegistrosDiariosAsync()`, registrando los errores con `ILogger` sin detener el servicio.
  - [ ] 4.3 Centralizar el registro del módulo en `RegistroModuloHistorico.AgregarModuloHistorico` e invocarlo desde `Program.cs`.
  - [ ] 4.4 Registrar el adaptador `MediadorHistoricoParqueSolar` en el punto de composición, sin crear dependencias entre los módulos Historico y ParqueSolar.

- [ ] **5. Capa Presentation (Blazor Server con Radzen)**
  - [ ] 5.1 No aplica: el alta es automática. No se crea componente, formulario ni ruta para este caso de uso.

- [ ] **6. Verificación de Calidad (Guardarraíles)**
  - [ ] 6.1 Ejecutar la suite de pruebas para validar que no existan violaciones de Clean Architecture ni comunicación directa entre módulos distintos (ParqueSolar, Historico, Usuario y Auth).
  - [ ] 6.2 Ejecutar las pruebas de ArchUnitNET (`ArquitectureTest`) para validar que los módulos no dependan entre sí ni tengan dependencias cíclicas.
  - [ ] 6.3 Ejecutar el análisis estático con SonarQube para validar code smells, bugs, vulnerabilidades y cumplimiento de estándares del módulo.
