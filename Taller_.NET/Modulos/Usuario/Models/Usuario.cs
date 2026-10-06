namespace Taller_.NET.Modulos.Usuario.Models
{
    public class Usuario{
        public int Id{ get; set; }
        public string Nombre{ get; set; }
        public string Apellido{ get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }

    public Usuario(int id, string nombre, string apellido, string telefono, string correo)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Telefono = telefono;
        Correo = correo;
    }

    }
}

