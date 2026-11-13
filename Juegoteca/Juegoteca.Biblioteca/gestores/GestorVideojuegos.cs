using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;
namespace Juegoteca.Biblioteca.Gestores;
public partial class GestorVideojuegos : IGestorVideojuegos
{
    public List<Videojuego> JuegosDisponibles { get; }
    public GestorVideojuegos() { JuegosDisponibles = new List<Videojuego>(); }
    public void AgregarJuego(Videojuego juego)
    {
        if (juego == null) throw new ArgumentNullException(nameof(juego));
        if (!JuegosDisponibles.Contains(juego)) JuegosDisponibles.Add(juego);
    }
    public bool EliminarJuego(Videojuego juego) => JuegosDisponibles.Remove(juego);
    public bool EliminarPorIndice(int indice)
    {
        if (indice < 0 || indice >= JuegosDisponibles.Count) return false;
        JuegosDisponibles.RemoveAt(indice);
        return true;
    }
    public List<Videojuego> BuscarPorTitulo(string texto)
    {
        List<Videojuego> resultado = new List<Videojuego>();
        if (string.IsNullOrWhiteSpace(texto)) return resultado;
        foreach (var j in JuegosDisponibles)
            if (j.Titulo.Contains(texto, StringComparison.OrdinalIgnoreCase))
                resultado.Add(j);
        return resultado;
    }
    public void LimpiarCatalogo() => JuegosDisponibles.Clear();

}