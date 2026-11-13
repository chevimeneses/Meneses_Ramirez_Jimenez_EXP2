using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;
namespace Juegoteca.Biblioteca.Servicios;
public class ExportadorTxt : IExportador
{
    public void ExportarATxt(Usuario usuario, string rutaArchivo)
    {
        List<string> lineas = new List<string> { $"Jugador: {usuario.Nombre}" };
        foreach (var c in usuario.Colecciones)
        {
            lineas.Add($"Coleccion: {c.Key}");
            foreach (var j in c.Value) lineas.Add($" - {j.Titulo}");
        }
        File.WriteAllLines(rutaArchivo, lineas);
    }
}