using Taller_.NET.Modulos.Usuario.Excepciones;
using Xunit;

// El namespace de los tests "Taller_.NET.Tests.Modulos.Usuario" tiene prioridad
// sobre el tipo "Usuario", por lo que se usa un alias para referenciar la entidad.
using EntidadUsuario = Taller_.NET.Modulos.Usuario.Usuario;

namespace Taller_.NET.Tests.Modulos.Usuario.Domain;

public class UsuarioTests
{
    [Fact]
    public void CrearUsuario_ConDatosValidos_AsignaPropiedades()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");

        Assert.NotEqual(Guid.Empty, usuario.Id);
        Assert.Equal("Ana", usuario.Nombre);
        Assert.Equal("Perez", usuario.Apellido);
        Assert.Equal("ana@solaryu.com", usuario.Correo);
        Assert.Equal("099123456", usuario.Telefono);
    }

    [Fact]
    public void CrearUsuario_NormalizaCorreoALowercase()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "Ana@SolarUY.com", "099123456");

        Assert.Equal("ana@solaruy.com", usuario.Correo);
    }

    [Fact]
    public void CrearUsuario_SinTelefono_DejaElTelefonoVacio()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "   ");

        Assert.Equal(string.Empty, usuario.Telefono);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearUsuario_ConNombreInvalido_LanzaExcepcion(string nombre)
    {
        Assert.Throws<UsuarioInvalidoException>(() => new EntidadUsuario(nombre, "Perez", "ana@solaryu.com", "099123456"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearUsuario_ConApellidoInvalido_LanzaExcepcion(string apellido)
    {
        Assert.Throws<UsuarioInvalidoException>(() => new EntidadUsuario("Ana", apellido, "ana@solaryu.com", "099123456"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ana-solaryu.com")]
    [InlineData("ana@")]
    [InlineData("@solaryu.com")]
    public void CrearUsuario_ConCorreoInvalido_LanzaExcepcion(string correo)
    {
        Assert.Throws<UsuarioInvalidoException>(() => new EntidadUsuario("Ana", "Perez", correo, "099123456"));
    }

    [Fact]
    public void CrearUsuario_ConNombreDemasiadoLargo_LanzaExcepcion()
    {
        var nombre = new string('a', EntidadUsuario.LongitudMaximaNombre + 1);

        Assert.Throws<UsuarioInvalidoException>(() => new EntidadUsuario(nombre, "Perez", "ana@solaryu.com", "099123456"));
    }

    [Fact]
    public void CrearUsuario_ConApellidoDemasiadoLargo_LanzaExcepcion()
    {
        var apellido = new string('a', EntidadUsuario.LongitudMaximaApellido + 1);

        Assert.Throws<UsuarioInvalidoException>(() => new EntidadUsuario("Ana", apellido, "ana@solaryu.com", "099123456"));
    }

    [Fact]
    public void CrearUsuario_ConCorreoDemasiadoLargo_LanzaExcepcion()
    {
        var correo = new string('a', EntidadUsuario.LongitudMaximaCorreo) + "@solaryu.com";

        Assert.Throws<UsuarioInvalidoException>(() => new EntidadUsuario("Ana", "Perez", correo, "099123456"));
    }

    [Fact]
    public void CrearUsuario_ConTelefonoDemasiadoLargo_LanzaExcepcion()
    {
        var telefono = new string('9', EntidadUsuario.LongitudMaximaTelefono + 1);

        Assert.Throws<UsuarioInvalidoException>(() => new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", telefono));
    }

    [Fact]
    public void CrearUsuario_RecortaLosEspaciosDeNombreApellidoYTelefono()
    {
        var usuario = new EntidadUsuario("  Ana ", "  Perez  ", " ana@solaryu.com ", " 099123456 ");

        Assert.Equal("Ana", usuario.Nombre);
        Assert.Equal("Perez", usuario.Apellido);
        Assert.Equal("099123456", usuario.Telefono);
    }

    [Fact]
    public void ActualizarDatosPersonales_ConDatosValidos_ReemplazaLosDatos()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");

        usuario.ActualizarDatosPersonales("Lucia", "Gomez", "Lucia@SolarUY.com", "099999888");

        Assert.Equal("Lucia", usuario.Nombre);
        Assert.Equal("Gomez", usuario.Apellido);
        Assert.Equal("lucia@solaruy.com", usuario.Correo);
        Assert.Equal("099999888", usuario.Telefono);
    }

    [Fact]
    public void ActualizarDatosPersonales_SinTelefono_DejaElTelefonoVacio()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");

        usuario.ActualizarDatosPersonales("Ana", "Perez", "ana@solaryu.com", "   ");

        Assert.Equal(string.Empty, usuario.Telefono);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ActualizarDatosPersonales_ConNombreInvalido_LanzaExcepcion(string nombre)
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");

        Assert.Throws<UsuarioInvalidoException>(() => usuario.ActualizarDatosPersonales(nombre, "Perez", "ana@solaryu.com", "099123456"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ActualizarDatosPersonales_ConApellidoInvalido_LanzaExcepcion(string apellido)
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");

        Assert.Throws<UsuarioInvalidoException>(() => usuario.ActualizarDatosPersonales("Ana", apellido, "ana@solaryu.com", "099123456"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ana-solaryu.com")]
    public void ActualizarDatosPersonales_ConCorreoInvalido_LanzaExcepcion(string correo)
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");

        Assert.Throws<UsuarioInvalidoException>(() => usuario.ActualizarDatosPersonales("Ana", "Perez", correo, "099123456"));
    }

    [Fact]
    public void ActualizarDatosPersonales_ConCorreoDemasiadoLargo_LanzaExcepcion()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");
        var correo = new string('a', EntidadUsuario.LongitudMaximaCorreo) + "@solaryu.com";

        Assert.Throws<UsuarioInvalidoException>(() => usuario.ActualizarDatosPersonales("Ana", "Perez", correo, "099123456"));
    }

    [Fact]
    public void ActualizarDatosPersonales_ConTelefonoDemasiadoLargo_LanzaExcepcion()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");
        var telefono = new string('9', EntidadUsuario.LongitudMaximaTelefono + 1);

        Assert.Throws<UsuarioInvalidoException>(() => usuario.ActualizarDatosPersonales("Ana", "Perez", "ana@solaryu.com", telefono));
    }

    [Fact]
    public void ActualizarDatosPersonales_CorreoInvalido_NoModificaLosDatosAnteriores()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");

        Assert.Throws<UsuarioInvalidoException>(() => usuario.ActualizarDatosPersonales("Lucia", "Gomez", "correo-invalido", "099999888"));

        Assert.Equal("Ana", usuario.Nombre);
        Assert.Equal("Perez", usuario.Apellido);
        Assert.Equal("ana@solaryu.com", usuario.Correo);
    }

    [Fact]
    public void VincularConAuth0_ConIdentificadorValido_GuardaElIdentificador()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");

        usuario.VincularConAuth0("auth0|65f1a2b3c4d5e6f7g8");

        Assert.Equal("auth0|65f1a2b3c4d5e6f7g8", usuario.Auth0UserId);
    }

    [Fact]
    public void VincularConAuth0_RecortaLosEspaciosDelIdentificador()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");

        usuario.VincularConAuth0("  auth0|65f1a2b3c4d5e6f7g8  ");

        Assert.Equal("auth0|65f1a2b3c4d5e6f7g8", usuario.Auth0UserId);
    }

    [Fact]
    public void VincularConAuth0_SinIdentificador_LanzaExcepcion()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");

        Assert.Throws<UsuarioInvalidoException>(() => usuario.VincularConAuth0("   "));
    }

    [Fact]
    public void VincularConAuth0_ConIdentificadorDemasiadoLargo_LanzaExcepcion()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");
        var auth0UserId = new string('a', EntidadUsuario.LongitudMaximaAuth0UserId + 1);

        Assert.Throws<UsuarioInvalidoException>(() => usuario.VincularConAuth0(auth0UserId));
    }

    [Fact]
    public void Usuario_SinVincularConAuth0_TieneElIdentificadorEnNull()
    {
        var usuario = new EntidadUsuario("Ana", "Perez", "ana@solaryu.com", "099123456");

        Assert.Null(usuario.Auth0UserId);
    }
}
