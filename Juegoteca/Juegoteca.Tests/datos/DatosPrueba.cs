using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Modelos;
namespace Juegoteca.Tests.Datos;
public static class DatosPrueba
{
    public static List<Videojuego> Catalogo()
    {
        return new List<Videojuego> {
new Videojuego("Celeste", "Maddy", 8),
new Videojuego("Tetris", "TetrisCo", 5),
new Videojuego("Hades", "Supergiant", 25)
};
    }
    public static GestorVideojuegos GestorPoblado()
    {
        var g = new GestorVideojuegos();
        foreach (var j in Catalogo()) g.AgregarJuego(j);
        return g;
    }
    public static Usuario UsuarioConColecciones()
    {
        var u = new Usuario("Ana");
        u.CrearColeccion("Mix");
        u.AgregarJuegoAColeccion("Mix", new Videojuego("Celeste", "Maddy", 8));
        return u;
    }
}