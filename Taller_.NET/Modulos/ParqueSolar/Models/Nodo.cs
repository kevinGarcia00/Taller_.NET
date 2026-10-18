namespace Taller_.NET.Modulos.Nodo
{
    public class Nodo
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public string UbicacionParque { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal EnergiaGenerada { get; set; }

        public Nodo(int id, string nombre, string ubicacionParque, DateTime fechaCreacion, decimal energiaGenerada)
        {
            Id = id;
            Nombre = nombre;
            UbicacionParque = ubicacionParque;
            FechaCreacion = fechaCreacion;
            EnergiaGenerada = energiaGenerada;
        }
    }
}