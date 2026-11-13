namespace Juegoteca.Biblioteca.Modelos;
public class Usuario
{
    public string Nombre { get; }
    public Dictionary<string, List<Videojuego>> Colecciones { get; }
    public Usuario(string nombre)
    {
        Nombre = nombre;
        Colecciones = new Dictionary<string, List<Videojuego>>();
    }
    public void CrearColeccion(string nombre)
    {
        if (Colecciones.ContainsKey(nombre))
            throw new InvalidOperationException($"La colección '{nombre}' ya existe.");
        Colecciones[nombre] = new List<Videojuego>();
    }
    public void AgregarJuegoAColeccion(string nombre, Videojuego juego)
    {
        if (!Colecciones.ContainsKey(nombre))
            throw new KeyNotFoundException($"La colección '{nombre}' no existe.");
        if (!Colecciones[nombre].Contains(juego))
            Colecciones[nombre].Add(juego);
    }
    public bool EliminarColeccion(string nombre)
    {
        return Colecciones.Remove(nombre);
    }
    public List<Videojuego> ObtenerColeccion(string nombre)
    {
        if (!Colecciones.ContainsKey(nombre))
            throw new KeyNotFoundException($"La colección no existe.");
        return Colecciones[nombre];
    }
    public int ContarJuegos()
    {
        int total = 0;
        foreach (var c in Colecciones) total += c.Value.Count;
        return total;
    }
    public void MostrarColecciones()
    {
        if (Colecciones.Count == 0) { Console.WriteLine("Vacío"); return; }
        foreach (var c in Colecciones)
        {
            Console.WriteLine($"{c.Key} ({c.Value.Count})");
        }
    }
}
