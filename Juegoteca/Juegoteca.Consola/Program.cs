using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Biblioteca.Servicios;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ===== 1. Inicialización =====
GestorVideojuegos gestor = new GestorVideojuegos();
ExportadorTxt exportador = new ExportadorTxt();
ServicioVideojuegos servicio = new ServicioVideojuegos(gestor, exportador);

gestor.AgregarJuego(new Videojuego("Celeste", "Maddy Makes Games", 8));
gestor.AgregarJuego(new Videojuego("Hollow Knight", "Team Cherry", 40));
gestor.AgregarJuego(new Videojuego("Minecraft", "Mojang", 100));
gestor.AgregarJuego(new Videojuego("Tetris", "The Tetris Company", 5));
gestor.AgregarJuego(new Videojuego("Portal 2", "Valve", 10));
gestor.AgregarJuego(new Videojuego("Stardew Valley", "ConcernedApe", 60));
gestor.AgregarJuego(new Videojuego("Hades", "Supergiant", 25));
gestor.AgregarJuego(new Videojuego("Zelda: BOTW", "Nintendo", 120));

Console.WriteLine("Bienvenido al Sistema de Gestión de Videojuegos");
Console.WriteLine();

// ===== 2. Registro de usuario =====
Console.WriteLine("--- REGISTRO DE USUARIO ---");
string nombreUsuario = LeerTexto("Ingrese su nombre de usuario: ");
Usuario usuario = servicio.RegistrarUsuario(nombreUsuario);
Console.WriteLine($"Usuario '{usuario.Nombre}' registrado. ¡Bienvenido/a, {usuario.Nombre}!");

// ===== 3. Creación de la colección inicial =====
string coleccionActual = LeerTexto("Nombre de su primera colección: ");
usuario.CrearColeccion(coleccionActual);
Console.WriteLine($"Colección '{coleccionActual}' creada exitosamente.");

// ===== 4. Menú principal =====
bool salir = false;

while (!salir)
{
    Console.WriteLine();
    Console.WriteLine("--- MENÚ PRINCIPAL ---");
    Console.WriteLine($"Colección actual: '{coleccionActual}' ({usuario.ObtenerColeccion(coleccionActual).Count} juegos)");
    Console.WriteLine("1. Mostrar juegos disponibles");
    Console.WriteLine("2. Ordenar colección (QuickSort o MergeSort)");
    Console.WriteLine("3. Búsqueda binaria en catálogo");
    Console.WriteLine("4. Crear nueva colección");
    Console.WriteLine("5. Cambiar de colección actual");
    Console.WriteLine("6. Ver mis colecciones");
    Console.WriteLine("7. Exportar mis colecciones a .txt");
    Console.WriteLine("8. Salir");
    Console.Write("Seleccione una opción: ");

    if (!int.TryParse(Console.ReadLine(), out int opcion))
    {
        Console.WriteLine("Opción no válida. Ingrese un número del 1 al 8.");
        continue;
    }

    switch (opcion)
    {
        case 1:
            {
                // Juegos del catálogo que aún no están en la colección actual
                List<Videojuego> enColeccion = usuario.ObtenerColeccion(coleccionActual);
                List<Videojuego> disponibles = new List<Videojuego>();
                foreach (Videojuego juego in gestor.JuegosDisponibles)
                {
                    if (!enColeccion.Contains(juego))
                    {
                        disponibles.Add(juego);
                    }
                }

                if (disponibles.Count == 0)
                {
                    Console.WriteLine("No hay juegos disponibles para agregar a esta colección.");
                }
                else
                {
                    Console.WriteLine("Juegos que se pueden incluir en la colección actual:");
                    AgregarDesdeLista(disponibles, usuario, coleccionActual);
                }
                break;
            }

        case 2:
            {
                Console.WriteLine("1. QuickSort");
                Console.WriteLine("2. MergeSort");
                Console.Write("Elija el algoritmo: ");
                if (!int.TryParse(Console.ReadLine(), out int algoritmo) || (algoritmo != 1 && algoritmo != 2))
                {
                    Console.WriteLine("Algoritmo no válido.");
                    break;
                }

                servicio.OrdenarCatalogo(algoritmo == 1 ? "quick" : "merge");
                int totalHoras = servicio.OrdenarColeccionYCalcularHoras(usuario, coleccionActual);

                Console.WriteLine($"Colección '{coleccionActual}' ordenada por horas de juego (de menor a mayor):");
                foreach (Videojuego juego in usuario.ObtenerColeccion(coleccionActual))
                {
                    Console.WriteLine($"  - {juego}");
                }
                Console.WriteLine($"Total de horas de juego: {totalHoras} h");
                break;
            }

        case 3:
            {
                Console.Write("Título a buscar: ");
                string titulo = (Console.ReadLine() ?? "").Trim();

                List<Videojuego> opciones = new List<Videojuego>();
                int indice = gestor.BusquedaBinaria(titulo);
                if (indice >= 0)
                {
                    opciones.Add(gestor.JuegosDisponibles[indice]);
                }
                else
                {
                    opciones = gestor.BuscarPorTitulo(titulo);
                }

                if (opciones.Count == 0)
                {
                    Console.WriteLine($"No se encontraron coincidencias para '{titulo}'.");
                }
                else
                {
                    AgregarDesdeLista(opciones, usuario, coleccionActual);
                }
                break;
            }

        case 4:
            {
                string nueva = LeerTexto("Nombre de la nueva colección: ");
                try
                {
                    usuario.CrearColeccion(nueva);
                    Console.WriteLine($"Colección '{nueva}' creada exitosamente.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                break;
            }

        case 5:
            {
                Console.WriteLine("Sus colecciones:");
                foreach (var coleccion in usuario.Colecciones)
                {
                    Console.WriteLine($"  - {coleccion.Key} ({coleccion.Value.Count} juegos)");
                }

                string elegida = LeerTexto("Nombre de la colección que será la actual: ");
                if (usuario.Colecciones.ContainsKey(elegida))
                {
                    coleccionActual = elegida;
                    Console.WriteLine($"Ahora la colección actual es '{coleccionActual}'.");
                }
                else
                {
                    Console.WriteLine($"La colección '{elegida}' no existe.");
                }
                break;
            }

        case 6:
            usuario.MostrarColecciones();
            break;

        case 7:
            {
                Console.Write("Ruta del archivo (Enter para 'colecciones.txt'): ");
                string ruta = (Console.ReadLine() ?? "").Trim();
                if (ruta == "")
                {
                    ruta = "colecciones.txt";
                }

                try
                {
                    servicio.ExportarColecciones(usuario, ruta);
                    Console.WriteLine($"Archivo creado en: {Path.GetFullPath(ruta)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"No se pudo exportar: {ex.Message}");
                }
                break;
            }

        case 8:
            salir = true;
            Console.WriteLine("¡Hasta luego!");
            break;

        default:
            Console.WriteLine("Opción no válida. Ingrese un número del 1 al 8.");
            break;
    }
}

// ===== Funciones auxiliares =====

// Pide un texto hasta que no esté vacío
static string LeerTexto(string mensaje)
{
    string texto = "";
    while (texto == "")
    {
        Console.Write(mensaje);
        texto = (Console.ReadLine() ?? "").Trim();
        if (texto == "")
        {
            Console.WriteLine("El texto no puede estar vacío.");
        }
    }
    return texto;
}

// Muestra una lista numerada y agrega el juego elegido a la colección (0 cancela)
static void AgregarDesdeLista(List<Videojuego> opciones, Usuario jugador, string nombreColeccion)
{
    for (int i = 0; i < opciones.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {opciones[i]}");
    }

    Console.Write("Ingrese el número del juego a agregar (0 para cancelar): ");
    if (!int.TryParse(Console.ReadLine(), out int numero))
    {
        Console.WriteLine("Entrada no válida.");
        return;
    }

    if (numero == 0)
    {
        Console.WriteLine("Operación cancelada.");
        return;
    }

    if (numero < 1 || numero > opciones.Count)
    {
        Console.WriteLine("Número fuera de rango.");
        return;
    }

    Videojuego elegido = opciones[numero - 1];
    if (jugador.ObtenerColeccion(nombreColeccion).Contains(elegido))
    {
        Console.WriteLine($"'{elegido.Titulo}' ya estaba en '{nombreColeccion}'.");
        return;
    }

    jugador.AgregarJuegoAColeccion(nombreColeccion, elegido);
    Console.WriteLine($"'{elegido.Titulo}' agregado a '{nombreColeccion}'.");
}