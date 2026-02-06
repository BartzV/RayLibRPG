using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases.Config;

internal static class CONFIG
{
    // Constantes. Resolución "interna" del juego.
    public const Int32 WIDTH = 512;
    public const Int32 HEIGHT = 288;
    // Constantes de lógica.
    public const Int32 TPS = 30; // Ticks per second, veces que Update se llama por segundo.
    public const Single Tickrate = 1.0f / TPS;
    // Escalado de la resolución interna a la ventana.
    public static Int32 PantallaX = 512 * 2;
    public static Int32 PantallaY = 288 * 2;
    public static Int32 FPS = 120; // Frames per second, veces que Draw se llama por segundo.
    // Contadores
    public static Int64 FramesTranscurridos = 0; // Para boludear.
    public static Int64 TicksTranscurridos = 0; // Más boludeo.
    // Texturas (mover a un TextureManager o huevadas así en un futuro)
    public static Dictionary<String, Texture2D> TexturasCargadas = new Dictionary<String, Texture2D>();
    public static RenderTexture2D Lienzo;
    public static Rectangle LienzoDest;

    public static void Inicializar()
    {
        Raylib.InitWindow(PantallaX, PantallaY, "Bartz RPG");
        // Esto no se toca.
        CONFIG.Lienzo = Raylib.LoadRenderTexture(WIDTH, HEIGHT);
        CONFIG.LienzoDest = new Rectangle(0, 0, PantallaX, PantallaY);

        Raylib.SetTextureFilter(CONFIG.Lienzo.Texture, TextureFilter.Point);
        Raylib.SetTargetFPS(FPS);

        InicializarTexturas();
    }

    public static void CambiarResolucion(Int32 ancho, Int32 alto)
    {
        CONFIG.PantallaX = ancho / WIDTH * WIDTH;
        CONFIG.PantallaY = alto / HEIGHT * HEIGHT;
        Int32 padX = (ancho - CONFIG.PantallaX) / 2;
        Int32 padY = (alto - CONFIG.PantallaY) / 2;

        CONFIG.LienzoDest = new Rectangle(padX, padY, CONFIG.PantallaX, CONFIG.PantallaY);
        Raylib.SetWindowSize(ancho, alto);
    }

    private static void InicializarTexturas()
    {
        CONFIG.TexturasCargadas.Add("Letra", Raylib.LoadTexture(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\FuentesRES.png")));
    }

}
