using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Biblioteca.Servicios;

public class ServicioVideojuegos
{
    public IGestorVideojuegos Gestor { get; }
    public IExportador Exportador { get; }
    public List<Usuario> Usuarios { get; }

    public ServicioVideojuegos(IGestorVideojuegos gestor, IExportador exportador)
    {
        if (gestor == null)
            throw new ArgumentNullException(nameof(gestor));
        if (exportador == null)
            throw new ArgumentNullException(nameof(exportador));

        Gestor = gestor;
        Exportador = exportador;
        Usuarios = new List<Usuario>();
    }

    public Usuario RegistrarUsuario(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del usuario no puede estar vacío.", nameof(nombre));

        Usuario usuario = new Usuario(nombre);
        Usuarios.Add(usuario);
        return usuario;
    }

    public Usuario? BuscarUsuario(string nombre)
    {
        foreach (Usuario usuario in Usuarios)
        {
            if (string.Equals(usuario.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
            {
                return usuario;
            }
        }
        return null;
    }

    public bool EliminarUsuario(string nombre)
    {
        Usuario? usuario = BuscarUsuario(nombre);
        if (usuario == null)
            return false;

        return Usuarios.Remove(usuario);
    }

    public int ContarUsuarios()
    {
        return Usuarios.Count;
    }

    public void OrdenarCatalogo(string algoritmo)
    {
        string opcion = (algoritmo ?? "").Trim().ToLower();

        if (opcion == "quick")
        {
            Gestor.QuickSort(Gestor.JuegosDisponibles);
            Console.WriteLine("Catálogo ordenado con QuickSort.");
        }
        else if (opcion == "merge")
        {
            List<Videojuego> ordenada = Gestor.MergeSort(Gestor.JuegosDisponibles);
            Gestor.JuegosDisponibles.Clear();
            Gestor.JuegosDisponibles.AddRange(ordenada);
            Console.WriteLine("Catálogo ordenado con MergeSort.");
        }
        else
        {
            throw new ArgumentException($"Algoritmo desconocido: '{algoritmo}'. Use 'quick' o 'merge'.", nameof(algoritmo));
        }
    }

    public int OrdenarColeccionYCalcularHoras(Usuario usuario, string coleccion)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));

        List<Videojuego> juegos = usuario.ObtenerColeccion(coleccion);
        Gestor.QuickSort(juegos);

        int totalHoras = 0;
        foreach (Videojuego juego in juegos)
        {
            totalHoras += juego.HorasJuego;
        }
        return totalHoras;
    }

    public void ExportarColecciones(Usuario usuario, string ruta)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));
        if (string.IsNullOrWhiteSpace(ruta))
            throw new ArgumentException("La ruta no puede estar vacía.", nameof(ruta));

        Exportador.ExportarATxt(usuario, ruta);
    }
}