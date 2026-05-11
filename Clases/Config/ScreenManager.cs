using Raylib_cs;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RayLibRPG.Clases.Config;

internal static class ScreenManager
{
    private static Boolean _inicializado = false;
    // Lienzos donde se dibuja todo. Se escala a la resolución de la ventana al final de cada Draw.
    //private static List<Capa> _capas = new();
    public static TextureFilter Filtro
    {
        get => _filtro;
        set
        {
            CambiarFiltro(value);
        }
    }
    private static TextureFilter _filtro = TextureFilter.Point;
    // Escalado de la resolución interna a la ventana.
    public static Int32 PantallaX = 512 * 2;
    public static Int32 PantallaY = 288 * 2;
    private static Int32 _padX = 0;
    private static Int32 _padY = 0;
    public static Int32 PadX { get { return _padX; } }
    public static Int32 PadY { get { return _padY; } }
    // Tamaño de cada pixel.
    // Si es 2, cada pixel de la resolución interna se dibuja como un cuadrado de 2x2 en la pantalla. Si es 3, como un cuadrado de 3x3, etc.
    private static Int32 _tamPixel = 2;
    public static Int32 TamPixel { get { return _tamPixel; } }
    // FPS
    private static Int32 _fps = 60; // Frames per second, veces que Draw se llama por segundo.
    private static FPS_Options _fpsOpts = FPS_Options.FPS_60;
    public static Int64 FPS_Ops = (Int64)_fpsOpts;

    public static Int32 FPS { get { return _fps; } }

    private static readonly (Int32 X, Int32 Y)[] _resoluciones = [(512, 288), (512 * 2, 288 * 2)];

    public static void SetFPS(FPS_Options nuevoFPS)
    {
        Int32 fps;
        switch (nuevoFPS)
        {
            case FPS_Options.FPS_60:
                fps = 60;
                break;
            case FPS_Options.FPS_120:
                fps = 120;
                break;
            case FPS_Options.FPS_240:
                fps = 240;
                break;
            default:
                Console.WriteLine("FPS no válidos!");
                return;
        }
        ScreenManager._fpsOpts = nuevoFPS;
        ScreenManager._fps = fps;
        Raylib.SetTargetFPS(_fps);
    }

    internal static void InicializarPantalla()
    {
        // 1. Usamos C# puro para saber el tamaño del monitor principal
        // Esto funciona ANTES de cualquier ventana abierta.
        int monitorAncho = 512 * 2 + 10;
        int monitorAlto = 288 * 2 + 10;

        // 2. Configuramos los Flags para que no tenga bordes
        //Raylib.SetConfigFlags(ConfigFlags.MaximizedWindow);

        // 3. Inicializamos con el tamaño real del monitor
        Raylib.InitWindow(monitorAncho, monitorAlto, "Bartz RPG");

        // 4. Forzamos la posición arriba a la izquierda para que no quede desplazada
        //Raylib.SetWindowPosition(0, 0);

        // 5. Corremos tu lógica de escalado
        CambiarResolucion(monitorAncho, monitorAlto);

        Raylib.SetTargetFPS(ScreenManager.FPS);
        _inicializado = true;
    }

    //internal static void LimpiarCapas()
    //{
    //    for(int i = 0; i < ScreenManager._capas.Count; i++)
    //    {
    //        ScreenManager._capas[i].Dispose();
    //    }
    //    ScreenManager._capas.Clear();
    //}
    //internal static Capa ObtenerCapa(String nombre)
    //{
    //    return ScreenManager._capas.First(c => c.Nombre == nombre);
    //}

    internal static void CambiarResolucion(Int32 ancho, Int32 alto)
    {
        // Algoritmo para el escalado. Se mantiene la relación de aspecto y se agregan barras negras si es necesario.
        ScreenManager.PantallaX = ancho / ConfigManager.WIDTH * ConfigManager.WIDTH;
        ScreenManager.PantallaY = alto / ConfigManager.HEIGHT * ConfigManager.HEIGHT;
        ScreenManager._tamPixel = ancho / ConfigManager.WIDTH;
        ScreenManager._padX = (ancho - ScreenManager.PantallaX) / 2;
        ScreenManager._padY = (alto - ScreenManager.PantallaY) / 2;

        // Cambiar el tamaño de la ventana.
        Raylib.SetWindowSize(ancho, alto);
    }

    internal static void CambiarFiltro(TextureFilter filtro)
    {
        _filtro = filtro;
    }
    
    public static void CerrarPantalla()
    {
        if(_inicializado == false)
        {
            Console.WriteLine("ScreenManager2 no inicializado!");
            return;
        }

    }
}

/// <summary>
/// Respuesta simple. Las capas rápidas renderizan siempre a los FPS configurados. Las lentas van siempre a 60 FPS (o lo intentan).<br/>
/// A FPS_60, el FrameActual & 0 siempre dará 0.
/// A FPS_120, el FrameActual & 1 dará 0 la mitad de las veces.
/// A FPS_240, el FrameActual & 2 dará 0 cada 4 frames.
/// 
/// </summary>
public enum FPS_Options : Int64
{
    FPS_60 = 0,
    FPS_120 = 1,
    FPS_240 = 3,
}
