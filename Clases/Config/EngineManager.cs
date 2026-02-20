using Raylib_cs;

namespace RayLibRPG.Clases.Config;

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