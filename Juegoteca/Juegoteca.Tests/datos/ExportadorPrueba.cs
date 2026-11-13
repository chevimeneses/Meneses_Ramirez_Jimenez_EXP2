using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Tests.Datos;

public class ExportadorPrueba : IExportador
{
    public int VecesLlamado { get; private set; }
    public Usuario? UltimoUsuario { get; private set; }
    public string? UltimaRuta { get; private set; }

    public void ExportarATxt(Usuario usuario, string rutaArchivo)
    {
        VecesLlamado++;
        UltimoUsuario = usuario;
        UltimaRuta = rutaArchivo;
    }
}