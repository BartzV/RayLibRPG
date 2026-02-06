using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases.Config;

internal static class ConfigManager
{
    // Constantes. Resolución "interna" del juego.
    public const Int32 WIDTH = 512;
    public const Int32 HEIGHT = 288;
    // Constantes de lógica.
    public const Int32 TPS = 30; // Ticks per second, veces que Update se llama por segundo.
    public const Single TICKRATE = 1.0f / TPS;
    // Rutas de archivos. Agregar luego.
    public const String RUTA_LETRAS = @"Assets/FuentesRES.png";
    
}

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

internal static class ScreenManager
{
    private static Boolean _inicializado = false;
    // Lienzos donde se dibuja todo. Se escala a la resolución de la ventana al final de cada Draw.
    public static RenderTexture2D Lienzo;
    public static Rectangle LienzoDest;
    // Escalado de la resolución interna a la ventana.
    public static Int32 PantallaX = 512 * 2;
    public static Int32 PantallaY = 288 * 2;
    public static Int32 FPS = 120; // Frames per second, veces que Draw se llama por segundo.

    internal static void InicializarPantalla()
    {
        // Crear la ventana. Se escala a la resolución interna al final de cada Draw.
        Raylib.InitWindow(PantallaX, PantallaY, "Bartz RPG");
        // Esto no se toca.
        ScreenManager.Lienzo = Raylib.LoadRenderTexture(ConfigManager.WIDTH, ConfigManager.HEIGHT);
        ScreenManager.LienzoDest = new Rectangle(0, 0, ScreenManager.PantallaX, ScreenManager.PantallaY);

        Raylib.SetTextureFilter(Lienzo.Texture, TextureFilter.Point);
        Raylib.SetTargetFPS(ScreenManager.FPS);
        // Marcar como inicializado.
        _inicializado = true;
    }

    internal static void CambiarResolucion(Int32 ancho, Int32 alto)
    {
        ScreenManager.PantallaX = ancho / ConfigManager.WIDTH * ConfigManager.WIDTH;
        ScreenManager.PantallaY = alto / ConfigManager.HEIGHT * ConfigManager.HEIGHT;
        Int32 padX = (ancho - ScreenManager.PantallaX) / 2;
        Int32 padY = (alto - ScreenManager.PantallaY) / 2;

        ScreenManager.LienzoDest = new Rectangle(padX, padY, ScreenManager.PantallaX, ScreenManager.PantallaY);
        Raylib.SetWindowSize(ancho, alto);
    }

    public static void CerrarPantalla()
    {
        Raylib.UnloadRenderTexture(ScreenManager.Lienzo);
    }
}

public static class EngineManager
{
    // Contadores
    public static Int64 FramesTranscurridos = 0; // Para boludear.
    public static Int64 TicksTranscurridos = 0; // Más boludeo.

    public static void Inicializar()
    {
        ScreenManager.InicializarPantalla();
        // Guarda! Texture2DManager debe ser inicializado después de ScreenManager, por el InitWindow
        Texture2DManager.InicializarTexturas();
    }
}