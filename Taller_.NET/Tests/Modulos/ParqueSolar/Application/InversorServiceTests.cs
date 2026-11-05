using System.Reflection;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Application.Servicios;
using Taller_.NET.Modulos.ParqueSolar.Application.Validadores;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Xunit;

namespace Taller_.NET.Tests.Modulos.ParqueSolar.Application;

public class InversorServiceTests
{
    [Fact]
    public async Task CrearInversorAsync_ConNodoInexistente_LanzaExcepcion()
    {
        var servicio = CrearServicio(new NodoRepositoryFalso(null), new InversorRepositoryFalso());
        var dto = CrearDto();

        await Assert.ThrowsAsync<NodoNoEncontradoException>(() => servicio.CrearInversorAsync(dto));
    }

    [Fact]
    public async Task CrearInversorAsync_ConNodoDeOtroParque_LanzaExcepcion()
    {
        var nodo = CrearNodo(parqueSolarId: 9);
        var inversorRepository = new InversorRepositoryFalso();
        var servicio = CrearServicio(new NodoRepositoryFalso(nodo), inversorRepository);

        await Assert.ThrowsAsync<NodoNoEncontradoException>(() => servicio.CrearInversorAsync(CrearDto()));
        Assert.False(inversorRepository.Agrego);
    }

    [Fact]
    public async Task CrearInversorAsync_ConNodoQueYaTieneInversor_LanzaExcepcion()
    {
        var nodo = new Nodo("Nodo Norte", "Sector A", 7);
        nodo.AsignarContexto(new ContextoParque(3));
        var inversorRepository = new InversorRepositoryFalso();
        var nodoRepository = new NodoRepositoryFalso(nodo);
        var servicio = CrearServicio(nodoRepository, inversorRepository);

        await Assert.ThrowsAsync<NodoYaTieneInversorException>(() => servicio.CrearInversorAsync(CrearDto()));

        Assert.False(inversorRepository.Agrego);
        Assert.False(nodoRepository.Actualizo);
        Assert.Equal(7, nodo.InversorId);
    }

    [Fact]
    public async Task CrearInversorAsync_ConDatosValidos_PersisteElInversorYLoAsignaAlNodo()
    {
        var nodo = CrearNodo(parqueSolarId: 3);
        var inversorRepository = new InversorRepositoryFalso();
        var nodoRepository = new NodoRepositoryFalso(nodo);
        var servicio = CrearServicio(nodoRepository, inversorRepository);

        var id = await servicio.CrearInversorAsync(CrearDto());

        Assert.Equal(15, id);
        Assert.True(inversorRepository.Agrego);
        Assert.True(nodoRepository.Actualizo);
        Assert.NotNull(nodo.Inversor);
        Assert.Equal("Inversor Central", nodo.Inversor.Nombre);
        Assert.Equal(15, nodo.InversorId);
    }

    [Fact]
    public async Task CrearInversorAsync_SinRolAdministrador_LanzaExcepcion()
    {
        var nodo = CrearNodo(parqueSolarId: 3);
        var inversorRepository = new InversorRepositoryFalso();
        var servicio = CrearServicio(
            new NodoRepositoryFalso(nodo),
            inversorRepository,
            esAdministrador: false);

        await Assert.ThrowsAsync<UsuarioNoAutorizadoException>(() => servicio.CrearInversorAsync(CrearDto()));
        Assert.False(inversorRepository.Agrego);
    }

    [Fact]
    public async Task ObtenerInversorDelNodoAsync_ConInversorAsignado_RetornaElDetalleSinModificarlo()
    {
        var inversor = CrearInversorPersistido(15, "Inversor Central");
        var nombre = inversor.Nombre;
        var fechaCreacion = inversor.FechaCreacion;
        var servicio = CrearServicio(new NodoRepositoryFalso(CrearNodo(parqueSolarId: 3)), new InversorRepositoryFalso
        {
            InversorDelNodo = inversor
        });

        var detalle = await servicio.ObtenerInversorDelNodoAsync(8, 3);

        Assert.NotNull(detalle);
        Assert.Equal(15, detalle.Id);
        Assert.Equal("Inversor Central", detalle.Nombre);
        Assert.Equal(fechaCreacion, detalle.FechaCreacion);
        Assert.Equal(8, detalle.NodoId);
        Assert.Equal(3, detalle.ParqueSolarId);
        Assert.Equal(nombre, inversor.Nombre);
        Assert.Equal(fechaCreacion, inversor.FechaCreacion);
    }

    [Fact]
    public async Task ObtenerInversorDelNodoAsync_ConNodoSinInversor_RetornaVacio()
    {
        var servicio = CrearServicio(new NodoRepositoryFalso(CrearNodo(parqueSolarId: 3)), new InversorRepositoryFalso());

        var detalle = await servicio.ObtenerInversorDelNodoAsync(8, 3);

        Assert.Null(detalle);
    }

    [Fact]
    public async Task ObtenerInversorDelNodoAsync_ConNodoInvalido_LanzaExcepcion()
    {
        var servicio = CrearServicio(new NodoRepositoryFalso(null), new InversorRepositoryFalso
        {
            InversorDelNodo = CrearInversorPersistido(15)
        });

        await Assert.ThrowsAsync<NodoNoEncontradoException>(() => servicio.ObtenerInversorDelNodoAsync(8, 3));
    }

    [Fact]
    public async Task ObtenerInversorDelNodoAsync_SinRolAdministrador_LanzaExcepcion()
    {
        var servicio = CrearServicio(
            new NodoRepositoryFalso(CrearNodo(parqueSolarId: 3)),
            new InversorRepositoryFalso { InversorDelNodo = CrearInversorPersistido(15) },
            esAdministrador: false);

        await Assert.ThrowsAsync<UsuarioNoAutorizadoException>(() => servicio.ObtenerInversorDelNodoAsync(8, 3));
    }

    [Fact]
    public async Task ObtenerParaEdicionAsync_ConInversorAsignado_RetornaElFormularioPrecargado()
    {
        var inversor = CrearInversorPersistido(15, "Inversor Central");
        var servicio = CrearServicio(new NodoRepositoryFalso(CrearNodo(parqueSolarId: 3)), new InversorRepositoryFalso
        {
            InversorDelNodo = inversor
        });

        var dto = await servicio.ObtenerParaEdicionAsync(8, 3);

        Assert.Equal(15, dto.Id);
        Assert.Equal("Inversor Central", dto.Nombre);
        Assert.Equal(inversor.FechaCreacion, dto.FechaCreacion);
        Assert.Equal(8, dto.NodoId);
        Assert.Equal(3, dto.ParqueSolarId);
    }

    [Fact]
    public async Task ObtenerParaEdicionAsync_ConNodoSinInversor_LanzaExcepcion()
    {
        var servicio = CrearServicio(new NodoRepositoryFalso(CrearNodo(parqueSolarId: 3)), new InversorRepositoryFalso());

        var excepcion = await Assert.ThrowsAsync<InversorNoEncontradoException>(() => servicio.ObtenerParaEdicionAsync(8, 3));

        Assert.Equal("El nodo no tiene un inversor asignado.", excepcion.Message);
    }

    [Fact]
    public async Task ModificarInversorAsync_ConNodoInvalido_LanzaExcepcion()
    {
        var inversor = CrearInversorPersistido(15);
        var inversorRepository = new InversorRepositoryFalso { InversorDelNodo = inversor };
        var servicio = CrearServicio(new NodoRepositoryFalso(CrearNodo(parqueSolarId: 9)), inversorRepository);

        await Assert.ThrowsAsync<NodoNoEncontradoException>(() => servicio.ModificarInversorAsync(CrearModificacion()));

        Assert.False(inversorRepository.Actualizo);
        Assert.Equal("Inversor Central", inversor.Nombre);
    }

    [Fact]
    public async Task ModificarInversorAsync_ConNodoSinInversor_LanzaExcepcion()
    {
        var inversorRepository = new InversorRepositoryFalso();
        var servicio = CrearServicio(new NodoRepositoryFalso(CrearNodo(parqueSolarId: 3)), inversorRepository);

        await Assert.ThrowsAsync<InversorNoEncontradoException>(() => servicio.ModificarInversorAsync(CrearModificacion()));
        Assert.False(inversorRepository.Actualizo);
    }

    [Fact]
    public async Task ModificarInversorAsync_ConIdentificadorDistinto_LanzaExcepcion()
    {
        var inversor = CrearInversorPersistido(15);
        var inversorRepository = new InversorRepositoryFalso { InversorDelNodo = inversor };
        var servicio = CrearServicio(new NodoRepositoryFalso(CrearNodo(parqueSolarId: 3)), inversorRepository);
        var dto = CrearModificacion();
        dto.Id = 99;

        await Assert.ThrowsAsync<InversorNoEncontradoException>(() => servicio.ModificarInversorAsync(dto));

        Assert.False(inversorRepository.Actualizo);
        Assert.Equal("Inversor Central", inversor.Nombre);
    }

    [Fact]
    public async Task ModificarInversorAsync_ConDatosValidos_ActualizaSoloElNombre()
    {
        var inversor = CrearInversorPersistido(15);
        var fechaCreacion = inversor.FechaCreacion;
        var nodo = CrearNodo(parqueSolarId: 3);
        nodo.AsignarInversor(inversor);
        var inversorRepository = new InversorRepositoryFalso { InversorDelNodo = inversor };
        var nodoRepository = new NodoRepositoryFalso(nodo);
        var servicio = CrearServicio(nodoRepository, inversorRepository);

        await servicio.ModificarInversorAsync(CrearModificacion(nombre: "Inversor Norte"));

        Assert.True(inversorRepository.Actualizo);
        Assert.False(nodoRepository.Actualizo);
        Assert.Equal("Inversor Norte", inversor.Nombre);
        Assert.Equal(15, inversor.Id);
        Assert.Equal(fechaCreacion, inversor.FechaCreacion);
        Assert.Equal(15, nodo.InversorId);
    }

    [Fact]
    public async Task ModificarInversorAsync_SinRolAdministrador_LanzaExcepcion()
    {
        var inversor = CrearInversorPersistido(15);
        var inversorRepository = new InversorRepositoryFalso { InversorDelNodo = inversor };
        var servicio = CrearServicio(
            new NodoRepositoryFalso(CrearNodo(parqueSolarId: 3)),
            inversorRepository,
            esAdministrador: false);

        await Assert.ThrowsAsync<UsuarioNoAutorizadoException>(() => servicio.ModificarInversorAsync(CrearModificacion()));
        Assert.False(inversorRepository.Actualizo);
    }

    [Fact]
    public async Task EliminarInversorAsync_ConNodoInvalido_LanzaExcepcion()
    {
        var inversorRepository = new InversorRepositoryFalso { InversorDelNodo = CrearInversorPersistido(15) };
        var nodoRepository = new NodoRepositoryFalso(null);
        var servicio = CrearServicio(nodoRepository, inversorRepository);

        await Assert.ThrowsAsync<NodoNoEncontradoException>(() => servicio.EliminarInversorAsync(8, 3));

        Assert.False(inversorRepository.Elimino);
        Assert.False(nodoRepository.Actualizo);
    }

    [Fact]
    public async Task EliminarInversorAsync_ConNodoSinInversor_LanzaExcepcion()
    {
        var nodoRepository = new NodoRepositoryFalso(CrearNodo(parqueSolarId: 3));
        var inversorRepository = new InversorRepositoryFalso();
        var servicio = CrearServicio(nodoRepository, inversorRepository);

        await Assert.ThrowsAsync<InversorNoEncontradoException>(() => servicio.EliminarInversorAsync(8, 3));

        Assert.False(inversorRepository.Elimino);
        Assert.False(nodoRepository.Actualizo);
    }

    [Fact]
    public async Task EliminarInversorAsync_ConInversorAsignado_QuitaLaRelacionYEliminaElInversor()
    {
        var inversor = CrearInversorPersistido(15);
        var nodo = CrearNodo(parqueSolarId: 3);
        nodo.AsignarInversor(inversor);
        var inversorRepository = new InversorRepositoryFalso { InversorDelNodo = inversor };
        var nodoRepository = new NodoRepositoryFalso(nodo);
        var servicio = CrearServicio(nodoRepository, inversorRepository);

        await servicio.EliminarInversorAsync(8, 3);

        Assert.True(nodoRepository.Actualizo);
        Assert.True(inversorRepository.Elimino);
        Assert.Null(nodo.Inversor);
        Assert.Null(nodo.InversorId);
        Assert.False(nodo.TieneInversorAsignado());
    }

    [Fact]
    public async Task EliminarInversorAsync_SinRolAdministrador_LanzaExcepcion()
    {
        var nodo = CrearNodo(parqueSolarId: 3);
        nodo.AsignarInversor(CrearInversorPersistido(15));
        var inversorRepository = new InversorRepositoryFalso { InversorDelNodo = nodo.Inversor };
        var nodoRepository = new NodoRepositoryFalso(nodo);
        var servicio = CrearServicio(nodoRepository, inversorRepository, esAdministrador: false);

        await Assert.ThrowsAsync<UsuarioNoAutorizadoException>(() => servicio.EliminarInversorAsync(8, 3));

        Assert.False(inversorRepository.Elimino);
        Assert.False(nodoRepository.Actualizo);
        Assert.Equal(15, nodo.InversorId);
    }

    private static Inversor CrearInversorPersistido(int id, string nombre = "Inversor Central")
    {
        var inversor = new Inversor(nombre);
        typeof(Inversor)
            .GetProperty(nameof(Inversor.Id), BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(inversor, id);

        return inversor;
    }

    private static ModificarInversorDto CrearModificacion(string nombre = "Inversor Norte")
    {
        return new ModificarInversorDto
        {
            Id = 15,
            Nombre = nombre,
            NodoId = 8,
            ParqueSolarId = 3
        };
    }

    private static InversorService CrearServicio(
        INodoRepository nodoRepository,
        IInversorRepository inversorRepository,
        bool esAdministrador = true)
    {
        return new InversorService(
            inversorRepository,
            nodoRepository,
            new UsuarioActualProviderFalso(esAdministrador),
            new CrearInversorDtoValidator(),
            new ModificarInversorDtoValidator());
    }

    private static CrearInversorDto CrearDto()
    {
        return new CrearInversorDto
        {
            Nombre = "Inversor Central",
            NodoId = 8,
            ParqueSolarId = 3
        };
    }

    private static Nodo CrearNodo(int parqueSolarId)
    {
        var nodo = new Nodo("Nodo Norte", "Sector A");
        nodo.AsignarContexto(new ContextoParque(parqueSolarId));
        return nodo;
    }

    private sealed class InversorRepositoryFalso : IInversorRepository
    {
        public bool Agrego { get; private set; }
        public bool Actualizo { get; private set; }
        public bool Elimino { get; private set; }
        public Inversor? InversorDelNodo { get; set; }

        public Task<int> AgregarAsync(Inversor inversor, CancellationToken cancellationToken = default)
        {
            Agrego = true;
            typeof(Inversor)
                .GetProperty(nameof(Inversor.Id), BindingFlags.Instance | BindingFlags.Public)!
                .SetValue(inversor, 15);

            return Task.FromResult(15);
        }

        public Task<Inversor?> ObtenerPorNodoAsync(int nodoId, CancellationToken cancellationToken = default)
            => Task.FromResult(InversorDelNodo);

        public Task ActualizarAsync(Inversor inversor, CancellationToken cancellationToken = default)
        {
            Actualizo = true;
            return Task.CompletedTask;
        }

        public Task EliminarAsync(Inversor inversor, CancellationToken cancellationToken = default)
        {
            Elimino = true;
            return Task.CompletedTask;
        }
    }

    private sealed class NodoRepositoryFalso : INodoRepository
    {
        private readonly Nodo? _nodo;

        public NodoRepositoryFalso(Nodo? nodo)
        {
            _nodo = nodo;
        }

        public bool Actualizo { get; private set; }

        public Task<bool> ExisteNodoConNombreAsync(string nombre, int parqueSolarId, CancellationToken cancellationToken = default)
            => Task.FromResult(false);

        public Task<bool> ExisteInversorEnUsoAsync(int inversorId, CancellationToken cancellationToken = default)
            => Task.FromResult(false);

        public Task<bool> ExisteInversorEnOtroNodoAsync(int inversorId, int nodoIdActual, CancellationToken cancellationToken = default)
            => Task.FromResult(false);

        public Task<bool> ExisteOtroNodoConNombreAsync(string nombre, int parqueSolarId, int nodoIdActual, CancellationToken cancellationToken = default)
            => Task.FromResult(false);

        public Task<IReadOnlyList<Nodo>> ObtenerPorParqueAsync(int parqueSolarId, CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<Nodo>>(Array.Empty<Nodo>());

        public Task<Nodo?> ObtenerPorIdAsync(int nodoId, CancellationToken cancellationToken = default)
            => Task.FromResult(_nodo);

        public Task<int> AgregarAsync(Nodo nodo, CancellationToken cancellationToken = default)
            => Task.FromResult(0);

        public Task ActualizarAsync(Nodo nodo, CancellationToken cancellationToken = default)
        {
            Actualizo = true;
            return Task.CompletedTask;
        }

        public Task EliminarAsync(Nodo nodo, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }

    private sealed class UsuarioActualProviderFalso : IUsuarioActualProvider
    {
        private readonly bool _esAdministrador;

        public UsuarioActualProviderFalso(bool esAdministrador)
        {
            _esAdministrador = esAdministrador;
        }

        public Task<bool> EsAdministradorAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(_esAdministrador);
    }
}
