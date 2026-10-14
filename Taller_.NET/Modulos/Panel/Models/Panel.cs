namespace Taller_.NET.Modulos.Panel
{
    public class Panel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public EstadoPanel EstadoPanel { get; set; }
         public decimal Potencia { get; set; }
        public string VidaUtil { get; set; }

        public Panel(int id, string nombre, DateTime fechaCreacion, EstadoPanel estado, decimal potencia, string vidaUtil)
        {
            Id = id;
            Nombre = nombre;
            FechaCreacion = fechaCreacion;
            EstadoPanel = estado;
            Potencia = potencia;
            VidaUtil = vidaUtil;
        }
    }
}
