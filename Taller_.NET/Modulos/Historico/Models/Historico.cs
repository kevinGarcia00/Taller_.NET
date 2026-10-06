using System.Reflection.Metadata;

namespace Taller_.NET.Modulos.Historico
{
    public class Historico
    {
        public int Id { set; get; }
        public string Nombre { set; get; }
        public string UbicacionParque { set; get; }
        public DateTime FechaCreacion { set; get; }
        public decimal EnergiaGenerada { set; get; }

        public Historico(int id, string nombre, string ubicacionParque, DateTime fechaCreacion, decimal energiaGenerada)
        {
            Id = id;
            Nombre = nombre;
            UbicacionParque = ubicacionParque;
            FechaCreacion = fechaCreacion;
            EnergiaGenerada = energiaGenerada;
        }
    }
}
