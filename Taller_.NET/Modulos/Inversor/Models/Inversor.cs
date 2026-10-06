namespace Taller_.NET.Modulos.Inversor
{
    public class Inversor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }

        public Inversor(int id , string nombre, DateTime fechaCreacion)
        {
            Id = id;
            Nombre = nombre;
            FechaCreacion = fechaCreacion;
        }
        
    }
}

