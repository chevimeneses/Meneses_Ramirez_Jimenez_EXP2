using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;
namespace Juegoteca.Tests.Datos;
public class ExportadorPrueba : IExportador
{
    public int VecesLlamado { get; private set; }
    public void ExportarATxt(Usuario u, string r) => VecesLlamado++;
}