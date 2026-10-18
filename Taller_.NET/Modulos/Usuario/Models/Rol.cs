namespace Taller_.NET.Modulos.Rol.Models
{
    public class Rol{
        public int Id{ get; set; } 
        public string Nombre { get; set; }

        public Rol(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }
    }
}