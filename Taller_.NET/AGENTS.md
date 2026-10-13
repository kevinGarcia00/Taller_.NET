# Taller .NET

Este es un proyecto para un taller de .NET.

## Project Structure

- `modulos/` - Contiene todos los modulos del sistema
- `Components/` - Contiene todo lo que es la parte visual del proyecto.

## Code Standards

- Usa .NET con el código estructurado y ordenado.
- Interfaces: Toda interfaz debe comenzar invariablemente con la letra I mayúscula (ej. IUsuarioRepository, ICorreoService).
- Excepciones: Cualquier clase personalizada que herede de System.Exception debe terminar con el sufijo Exception (ej. SaldoInsuficienteException).
- PascalCase y camelCase: PascalCase: Se utiliza para nombres de Clases, Records, Interfaces, Métodos y Propiedades públicas. camelCase: Se utiliza para variables locales y parámetros de métodos. _camelCase (con guion bajo): Se utiliza EXCLUSIVAMENTE para variables de instancia privadas, en especial aquellas inyectadas por dependencia en el constructor (ej. private readonly IUsuarioRepository _usuarioRepository;).
- Repositorios: Toda clase que gestione el acceso a los datos debe tener el sufijo Repository.
- Servicios: Las clases que coordinan la lógica de negocio o actúan como Casos de Uso deben terminar con el sufijo Service.
- Controladores: Los controladores deben terminar con el sufijo Controller.
- Si se utilizan acrónimos de dos letras (ej. ID, IP), ambas deben ir en mayúscula.
- Los nombres base de las entidades, variables, propiedades y métodos que representan reglas de negocio deben escribirse en Español.
- Debes respetar obligatoriamente los sufijos y prefijos técnicos en Inglés exigidos anteriormente.
- Si el agente detecta que un método hace dos cosas justificadas, debe exigir que el nombre lo refleje
- El agente debe marcar como incorrecto cualquier método que combine múltiples operaciones de negocio distintas.
- Los métodos deben hacer exactamente lo que su nombre indica y nada más. No deben tener efectos secundarios

## Reglas de arquitectura
- La capa de Domain es el núcleo. nunca debe importar paquetes de Entity Framework Core, ASP.NET, componentes de UI, ni hacer referencias a otras capas del proyecto. Solo usa C# puro.
- No puede conocer nada de Blazor ni detalles de acceso a datos (como DbContext o SQL).
- La capa de UI (Componentes de Blazor) NO DEBE acceder directamente a la capa de Infrastructure.
- Separación de módulos: Al tratarse de un monolito modular queda prohibido la comunicacion directa entre modulos, se tiene que si o si utilizar un canal de comunicación intermedio, ya sea mediante eventos o el patrón mediador.
- En caso de aplicar el patrón mediador se limitará cada mediador a dos módulos, es decir cada mediador que sea necesario funcionará como un canal de comunicación entre esos dos módulos.
- implementa las interfaces definidas en Application o Domain. Ninguna otra capa debe depender de Infrastructure directamente.
- Inversión de Dependencias (DIP): Todo acceso a la base de datos se hace mediante interfaces. Las interfaces de los repositorios (ej. IUsuarioRepository) se definen idealmente en Domain o Application, pero su implementación concreta (ej. UsuarioRepository) va obligatoriamente en Infrastructure.
- Inyección por Constructor: Las dependencias en los Servicios de Aplicación y Repositorios deben inyectarse solamente a través del constructor. No instanciar clases complejas con new si corresponden a servicios, repositorios o componentes de infraestructura.

## Reglas avanzadas
- Las clases en la capa de Domain deben modelarse pensando en objetos, comportamiento y lenguaje ubicuo, NO como un mero reflejo de tablas SQL para facilitar el trabajo
- Fomenta activamente el uso de propiedades de navegación ricas (referencias a objetos reales, ej: public Cliente Cliente { get; private set; }) y evita la "obsesión por los tipos primitivos", es decir, exponer ciegamente claves foráneas crudas


## Reglas de Agente
- Antes de escribir cualquier bloque de código C#, debes tener en cuenta las reglas estáticas y de diseño definidas en los otros archivos de esta carpeta (AGENTS.md o cualquier otro archivo .md que este dentro de los modulos).
- No asumas que tu código es perfecto. Cada vez que crees, modifiques o refactorices código (especialmente si agregas nuevas dependencias, clases, repositorios o servicios), tienes la OBLIGACIÓN de verificar que no has roto la arquitectura.
- Si al ejecutar dotnet test algún test de ArchUnitNET falla, NO le pidas al usuario que lo arregle ni des por terminada tu tarea.
- Lee el mensaje de error del test fallido (que indica qué regla arquitectónica rompiste).
- Modifica el código que generaste para solucionar el problema.
- Vuelve a ejecutar los tests. Solo debes reportar tu trabajo al usuario cuando todas las pruebas pasen en verde.
- No se deben de modificar los test para que pasen sin consultar al usuario explícitamente que autorice esta modificación.
- Obligatorio para el Dominio: Cada vez que generes o modifiques comportamiento o lógica dentro de la capa de Domain (ej. métodos de entidades, validaciones, cálculos), DEBES generar simultáneamente la prueba unitaria (xUnit) correspondiente para validar ese comportamiento.
- Silencio en otras capas: Para las capas de Application, Infrastructure o Presentation, NO generes pruebas unitarias. Además, NO debes preguntarle ni sugerirle al usuario la creación de tests en estas capas. Mantén tus respuestas enfocadas estrictamente en lo que el usuario solicitó.
-  Si tras analizar un error crees genuinamente que la regla de ArchUnitNET está mal planteada, está desactualizada, o se requiere una excepción justificada para el diseño actual, debes DETENERTE. Explica la situación al usuario humano y solicita su autorización explícita antes de editar cualquier archivo de pruebas de calidad.
- Si un test arquitectónico falla, tu obligación es SIEMPRE corregir el código fuente del sistema para que cumpla con el test. NUNCA debes relajar ni alterar el test para adaptarlo al código mal diseñado.