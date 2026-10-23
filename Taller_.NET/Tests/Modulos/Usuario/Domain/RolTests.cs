using Taller_.NET.Modulos.Usuario.Excepciones;
using Xunit;

// El namespace de los tests "Taller_.NET.Tests.Modulos.Usuario" tiene prioridad
// sobre el tipo "Rol", por lo que se usa un alias para referenciar la entidad.
using EntidadRol = Taller_.NET.Modulos.Usuario.Rol;

namespace Taller_.NET.Tests.Modulos.Usuario.Domain;

public class RolTests
{
    [Fact]
    public void CrearRol_ConDatosValidos_AsignaPropiedades()
    {
        var rol = new EntidadRol(1, EntidadRol.NombreAdministrador);

        Assert.Equal(1, rol.Id);
        Assert.Equal(EntidadRol.NombreAdministrador, rol.Nombre);
    }

    [Fact]
    public void CrearRol_RecortaLosEspaciosDelNombre()
    {
        var rol = new EntidadRol(1, "  Mantenimiento  ");

        Assert.Equal("Mantenimiento", rol.Nombre);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearRol_ConNombreInvalido_LanzaExcepcion(string nombre)
    {
        Assert.Throws<RolInvalidoException>(() => new EntidadRol(1, nombre));
    }

    [Fact]
    public void CrearRol_ConNombreDemasiadoLargo_LanzaExcepcion()
    {
        var nombre = new string('a', EntidadRol.LongitudMaximaNombre + 1);

        Assert.Throws<RolInvalidoException>(() => new EntidadRol(1, nombre));
    }
}
