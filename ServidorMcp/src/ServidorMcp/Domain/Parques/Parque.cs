using Domain.Parques.Excepciones;

namespace Domain.Parques;

public sealed class Parque
{
    public const int LongitudMaximaNombre = 100;
    public const int LongitudMaximaUbicacion = 100;

    public int Id { get; }
    public string Nombre { get; }
    public string Ubicacion { get; }
    public DateTime FechaCreacion { get; }
    public int CantMaxNodos { get; }

    public Parque(int id, string nombre, string ubicacion, DateTime fechaCreacion, int cantMaxNodos)
    {
        ValidarId(id);
        ValidarNombre(nombre);
        ValidarUbicacion(ubicacion);
        ValidarCantMaxNodos(cantMaxNodos);

        Id = id;
        Nombre = nombre.Trim();
        Ubicacion = ubicacion.Trim();
        FechaCreacion = fechaCreacion;
        CantMaxNodos = cantMaxNodos;
    }

    public bool TieneMayorCapacidadQue(Parque otroParque)
    {
        ArgumentNullException.ThrowIfNull(otroParque);

        return CantMaxNodos > otroParque.CantMaxNodos;
    }

    private static void ValidarId(int id)
    {
        if (id <= 0)
        {
            throw new ParqueInvalidoException("El identificador del parque debe ser mayor a cero.");
        }
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ParqueInvalidoException("El nombre del parque es obligatorio.");
        }

        if (nombre.Length > LongitudMaximaNombre)
        {
            throw new ParqueInvalidoException($"El nombre del parque no puede superar los {LongitudMaximaNombre} caracteres.");
        }
    }

    private static void ValidarUbicacion(string ubicacion)
    {
        if (string.IsNullOrWhiteSpace(ubicacion))
        {
            throw new ParqueInvalidoException("La ubicacion del parque es obligatoria.");
        }

        if (ubicacion.Length > LongitudMaximaUbicacion)
        {
            throw new ParqueInvalidoException($"La ubicacion del parque no puede superar los {LongitudMaximaUbicacion} caracteres.");
        }
    }

    private static void ValidarCantMaxNodos(int cantMaxNodos)
    {
        if (cantMaxNodos <= 0)
        {
            throw new ParqueInvalidoException("La cantidad maxima de nodos debe ser mayor a cero.");
        }
    }
}
