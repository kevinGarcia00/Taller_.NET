namespace Taller_.NET.Modulos.ParqueSolar.Models
{
    
    public class ParqueSolar
    {
        public int Id { get; set;}
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CantNodos { get; set;}

    public ParqueSolar(int id, string nombre, string ubicacion, DateTime fechaCreacion, int cantNodos)
        {
            Id = id;
            Nombre = nombre;
            Ubicacion = ubicacion;
            FechaCreacion = fechaCreacion;
            CantNodos = cantNodos;
        }
    }
}