using Taller_.NET.Modulos.Rol.Models;

public class Usuario
{
    public string Id { get; private set; }
    public string Nombre { get; private set; }
    public string Apellido { get; private set; }
    public string Correo { get; private set; }
    public string Telefono { get; private set; }
    
    private readonly List<Rol> _roles = new();
    public IReadOnlyCollection<Rol> Roles => _roles.AsReadOnly();

    public Usuario(string id, string nombre, string apellido, string correo, string telefono)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Correo = correo;
        Telefono = telefono;
    }
    
}