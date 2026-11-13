using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Biblioteca.Servicios;
using Juegoteca.Tests.Datos;

namespace Juegoteca.Tests;

public class ServicioVideojuegosTests
{
    private readonly ExportadorPrueba _exportador;
    private readonly ServicioVideojuegos _servicio;

    public ServicioVideojuegosTests()
    {
        _exportador = new ExportadorPrueba();
        _servicio = new ServicioVideojuegos(DatosPrueba.GestorPoblado(), _exportador);
    }

    [Fact]
    public void Constructor_DependenciasValidas_AsignaGestorExportadorYUsuariosVacia()
    {
        Assert.NotNull(_servicio.Gestor);
        Assert.Same(_exportador, _servicio.Exportador);
        Assert.Empty(_servicio.Usuarios);
    }

    [Fact]
    public void Constructor_GestorNull_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ServicioVideojuegos(null!, _exportador));
    }

    [Fact]
    public void Constructor_ExportadorNull_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ServicioVideojuegos(DatosPrueba.GestorPoblado(), null!));
    }

    [Fact]
    public void RegistrarUsuario_NombreValido_RetornaUsuarioLoGuardaYContarPasaDe0A1()
    {
        Assert.Equal(0, _servicio.ContarUsuarios());

        Usuario usuario = _servicio.RegistrarUsuario("Ana");

        Assert.Equal("Ana", usuario.Nombre);
        Assert.Contains(usuario, _servicio.Usuarios);
        Assert.Equal(1, _servicio.ContarUsuarios());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void RegistrarUsuario_NombreVacio_LanzaArgumentException(string nombre)
    {
        Assert.Throws<ArgumentException>(() => _servicio.RegistrarUsuario(nombre));
    }

    [Fact]
    public void BuscarUsuario_NombreEnMayusculas_EncuentraAlUsuario()
    {
        _servicio.RegistrarUsuario("Ana");

        Usuario? encontrado = _servicio.BuscarUsuario("ANA");

        Assert.NotNull(encontrado);
        Assert.Equal("Ana", encontrado!.Nombre);
    }

    [Fact]
    public void BuscarUsuario_NombreNoRegistrado_RetornaNull()
    {
        Assert.Null(_servicio.BuscarUsuario("Nadie"));
    }

    [Fact]
    public void EliminarUsuario_UsuarioExistente_RetornaTrueYElConteoBajaA0()
    {
        _servicio.RegistrarUsuario("Ana");

        bool resultado = _servicio.EliminarUsuario("Ana");

        Assert.True(resultado);
        Assert.Equal(0, _servicio.ContarUsuarios());
    }

    [Fact]
    public void EliminarUsuario_UsuarioInexistente_RetornaFalse()
    {
        Assert.False(_servicio.EliminarUsuario("Nadie"));
    }

    [Theory]
    [InlineData("quick")]
    [InlineData("merge")]
    public void OrdenarCatalogo_AlgoritmoValido_TetrisQuedaPrimero(string algoritmo)
    {
        _servicio.OrdenarCatalogo(algoritmo);

        Assert.Equal("Tetris", _servicio.Gestor.JuegosDisponibles[0].Titulo);
    }

    [Fact]
    public void OrdenarCatalogo_AlgoritmoBurbuja_LanzaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _servicio.OrdenarCatalogo("burbuja"));
    }

    [Fact]
    public void OrdenarColeccionYCalcularHoras_HadesYCeleste_Retorna33YCelesteQuedaPrimero()
    {
        // Arrange
        Usuario usuario = _servicio.RegistrarUsuario("Ana");
        usuario.CrearColeccion("Mix");
        usuario.AgregarJuegoAColeccion("Mix", new Videojuego("Hades", "Supergiant", 25));
        usuario.AgregarJuegoAColeccion("Mix", new Videojuego("Celeste", "Maddy Makes Games", 8));

        // Act
        int total = _servicio.OrdenarColeccionYCalcularHoras(usuario, "Mix");

        // Assert
        Assert.Equal(33, total);
        Assert.Equal("Celeste", usuario.ObtenerColeccion("Mix")[0].Titulo);
    }

    [Fact]
    public void OrdenarColeccionYCalcularHoras_ColeccionInexistente_LanzaKeyNotFoundException()
    {
        Usuario usuario = _servicio.RegistrarUsuario("Ana");

        Assert.Throws<KeyNotFoundException>(() => _servicio.OrdenarColeccionYCalcularHoras(usuario, "NoExiste"));
    }

    [Fact]
    public void OrdenarColeccionYCalcularHoras_UsuarioNull_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _servicio.OrdenarColeccionYCalcularHoras(null!, "Mix"));
    }

    [Fact]
    public void ExportarColecciones_ConExportadorPrueba_LlamaUnaVezConElMismoUsuarioYRuta()
    {
        // Arrange
        Usuario usuario = DatosPrueba.UsuarioConColecciones();

        // Act
        _servicio.ExportarColecciones(usuario, "salida.txt");

        // Assert
        Assert.Equal(1, _exportador.VecesLlamado);
        Assert.Same(usuario, _exportador.UltimoUsuario);
        Assert.Equal("salida.txt", _exportador.UltimaRuta);
    }

    [Fact]
    public void ExportarColecciones_UsuarioNull_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _servicio.ExportarColecciones(null!, "salida.txt"));
    }

    [Fact]
    public void ExportarColecciones_RutaVacia_LanzaArgumentException()
    {
        Usuario usuario = DatosPrueba.UsuarioConColecciones();

        Assert.Throws<ArgumentException>(() => _servicio.ExportarColecciones(usuario, ""));
    }
}