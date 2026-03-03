using Raylib_cs;
using RayLibRPG.Clases.Config;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RayLibRPG.Clases.Letras;

public static class LetraManager
{
    private static Boolean _inicializado = false;
    private static Texture2D? _textura;
    // Por qué un Char? Porque soy así de pelotudo. Ahora va a costar un huevo agregar íconos.
    private static Dictionary<Char, Rectangle> Diccionario = new();

    public static Texture2D Textura
    {
        get => _textura ?? throw new InvalidOperationException("Inicializá el LetraManager!!!");
    }
    public static Rectangle? GetRectangle(Char c)
    {
        if (!_inicializado)
            throw new InvalidOperationException("Inicializá el LetraManager!!!");
        if (Diccionario.TryGetValue(c, out Rectangle rect))
        {
            return rect;
        }
        else
        {
            return null;
        }
    }
    public static Rectangle GetEspacio()
    {
        if (!_inicializado)
            throw new InvalidOperationException("Inicializá el LetraManager!!!");
        return Diccionario[' ']!;
    }

    public static void Inicializar()
    {
        if (_inicializado) return;

        _textura = Raylib.LoadTexture(ConfigManager.RUTA_LETRAS);

        // Definimos las filas tal cual están en tu PNG (8 píxeles de alto cada una)
        // Fila 0 (Y=0): Mayúsculas y tildes raras
        // Fila 1 (Y=8): Minúsculas y tildes raras
        // Fila 2 (Y=16): Números, símbolos matemáticos, etc.
        // Fila 3 (Y=24): Símbolos raros
        // Fila 4 (Y=32): Símbolos aún más raros jaja

        MapearFila("ABCDEFGHIJKLMNOPQRSTUVWXYZÑÁÉÍÓÚ", 0);
        MapearFila("abcdefghijklmnopqrstuvwxyzñáéíóú", 8);
        MapearFila("0123456789+-*/%=$#@            Ü", 16);
        MapearFila(".,:;_…\"'¿?¡!()[]<>✓            ü", 24);
        // Fila Y=32, Y=40 e Y=48 reservada para símbolos
        MapearSimbolos();
        MapearFila("~", 56);

        // El espacio siempre es especial (no ocupa lugar en el atlas)
        if (!Diccionario.ContainsKey(' ')) 
            Diccionario.Add(' ', new Rectangle(0, 0, 0, 0));

        _inicializado = true;
    }

    /// <summary>
    /// Recorre un string y asigna cada char a un rectángulo de 8x8 en la fila Y especificada.
    /// </summary>
    private static void MapearFila(string fila, int posY)
    {
        for (Int32 i = 0; i < fila.Length; i++)
        {
            Char c = fila[i];

            // Si es un espacio en el mapa, lo salteamos para no pisar el ' ' real
            // o si ya existe el carácter por alguna razón.
            if (c == ' ' || Diccionario.ContainsKey(c)) continue;

            Diccionario.Add(c, new Rectangle(i * 8, posY, 8, 8));
        }
    }

    private static void MapearSimbolos()
    {
        // Corazón
        Diccionario.Add('\uFF00', new Rectangle(0, 32, Letra.TAM_LETRA));
        // Escudo
        Diccionario.Add('\uFF01', new Rectangle(8, 32, Letra.TAM_LETRA));
        // Marco de Barra
        Diccionario.Add('\uFBF0', new Rectangle(16, 56, Letra.TAM_LETRA));
        Diccionario.Add('\uFBF1', new Rectangle(24, 56, new Vector2(1, 8)));
        Diccionario.Add('\uFBF2', new Rectangle(24, 56, Letra.TAM_LETRA));
        // Barra en cuestión
        Diccionario.Add('\uFBF3', new Rectangle(8, 56, new Vector2(1, 8)));
    }
}
