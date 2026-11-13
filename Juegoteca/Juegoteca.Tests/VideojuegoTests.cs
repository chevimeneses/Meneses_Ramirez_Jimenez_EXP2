using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;
namespace Juegoteca.Tests;
public class VideojuegoTests
{
    [Fact]
    public void Constructor_DatosValidos_AsignaLasPropiedades()
    {
        var juego = new Videojuego("Hades", "Supergiant", 25);
        Assert.Equal("Hades", juego.Titulo);
        Assert.Equal("Supergiant", juego.Estudio);
        Assert.Equal(25, juego.HorasJuego);
    }
    [Fact]
    public void ToString_HadesSupergiant25_DevuelveElFormatoExacto()
    {
        var juego = new Videojuego("Hades", "Supergiant", 25);
        Assert.Equal("Hades - Supergiant (25 h)", juego.ToString());
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_HorasInvalidas_LanzaExcepcion(int horas)
    {
        Assert.Throws<ArgumentException>(() => new Videojuego("Hades", "Studio", horas));
    }
}