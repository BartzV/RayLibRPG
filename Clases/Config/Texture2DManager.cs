using Raylib_cs;

namespace RayLibRPG.Clases.Config;

internal static class Texture2DManager
{
    private static Boolean _inicializado = false;
    private static Dictionary<String, Texture2D> _texturasCargadas = new Dictionary<String, Texture2D>();

    internal static void InicializarTexturas()
    {
        // Obligatorio. Siempre usado. En ningún momento se descarga.
        _texturasCargadas.Add("Letra",
            Raylib.LoadTexture(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigManager.RUTA_LETRAS)));
        // Agregar más "indispensables" luego...

        // Marcar como inicializado.
        _inicializado = true;
    }

    public static Texture2D GetTexture(String nombre)
    {
        if (!_inicializado)
        {
            throw new InvalidOperationException("Texture2DManager no ha sido inicializado. Llama a InicializarTexturas() primero.");
        }
        if (_texturasCargadas.TryGetValue(nombre, out Texture2D tex))
        {
            return tex;
        }
        else
        {
            throw new InvalidOperationException($"La textura '{nombre}' no está cargada.");
        }
    }

    public static Texture2D LoadTexture(String ruta, String nombre)
    {
        if (!_inicializado)
        {
            throw new InvalidOperationException("Texture2DManager no ha sido inicializado. Llama a InicializarTexturas() primero.");
        }

        ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ruta);

        if (_texturasCargadas.TryGetValue(nombre, out Texture2D value))
            return value;

        Texture2D tex = Raylib.LoadTexture(ruta);
        _texturasCargadas.Add(nombre, tex);
        return tex;
    }

    public static void UnloadTexture(String nombre)
    {
        if (_texturasCargadas.TryGetValue(nombre, out Texture2D tex))
        {
            Raylib.UnloadTexture(tex);
            _texturasCargadas.Remove(nombre);
        }
        else
        {
            throw new InvalidOperationException($"La textura '{nombre}' no está cargada.");
        }
    }

    public static void UnloadAll()
    {
        foreach (Texture2D tex in _texturasCargadas.Values)
        {
            Raylib.UnloadTexture(tex);
        }
        _texturasCargadas.Clear();
    }
}
