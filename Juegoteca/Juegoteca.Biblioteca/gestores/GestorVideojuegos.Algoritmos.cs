using Juegoteca.Biblioteca.Modelos;
namespace Juegoteca.Biblioteca.Gestores;
public partial class GestorVideojuegos
{
    public void QuickSort(List<Videojuego> lista)
    {
        if (lista.Count < 2) return;
        QuickSortRec(lista, 0, lista.Count - 1);
    }
    private void QuickSortRec(List<Videojuego> lista, int ini, int fin)
    {
        if (ini >= fin) return;
        int pivote = lista[fin].HorasJuego;
        int i = ini - 1;
        for (int j = ini; j < fin; j++)
        {
            if (lista[j].HorasJuego <= pivote)
            {
                i++;
                (lista[i], lista[j]) = (lista[j], lista[i]);
            }
        }
        (lista[i + 1], lista[fin]) = (lista[fin], lista[i + 1]);
        QuickSortRec(lista, ini, i);
        QuickSortRec(lista, i + 2, fin);
    }
    public List<Videojuego> MergeSort(List<Videojuego> lista)
    {
        if (lista.Count <= 1) return new List<Videojuego>(lista);
        int medio = lista.Count / 2;
        var izq = MergeSort(lista.GetRange(0, medio));
        var der = MergeSort(lista.GetRange(medio, lista.Count - medio));
        List<Videojuego> res = new List<Videojuego>();
        int i = 0, j = 0;
        while (i < izq.Count && j < der.Count)
            res.Add(izq[i].HorasJuego <= der[j].HorasJuego ? izq[i++] : der[j++]);
        while (i < izq.Count) res.Add(izq[i++]);
        while (j < der.Count) res.Add(der[j++]);
        return res;
    }
    public int BusquedaBinaria(string titulo)
    {
        var copia = new List<Videojuego>(JuegosDisponibles);
        copia.Sort((a, b) => string.Compare(a.Titulo, b.Titulo,
        StringComparison.OrdinalIgnoreCase));
        int ini = 0, fin = copia.Count - 1;
        while (ini <= fin)
        {
            int medio = (ini + fin) / 2;
            int comp = string.Compare(titulo, copia[medio].Titulo,
            StringComparison.OrdinalIgnoreCase);
            if (comp == 0) return JuegosDisponibles.IndexOf(copia[medio]);
            if (comp < 0) fin = medio - 1; else ini = medio + 1;
        }
        return -1;
    }
}