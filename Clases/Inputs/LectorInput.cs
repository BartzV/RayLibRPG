using Raylib_cs;
using RayLibRPG.Clases.Config;

namespace RayLibRPG.Clases.Inputs;

/// <summary>
/// Clase base para los lectores.
/// </summary>
//public abstract class LectorInput
//{
//    protected Int32 DELAY_INICIAL = 1;
//    protected Int32 DELAY_REPETICION = 1;
//    public virtual Boolean Procesar()
//    {
//        return false;
//    }
//}

public abstract class LectorInput
{
    // Delays configurables por cada lector (Menú vs Mundo)
    protected Int32 _delayInicial;
    protected Int32 _delayRepeticion;

    public LectorInput(Int32 delayIni = 1, Int32 delayRep = 1)
    {
        _delayInicial = delayIni;
        _delayRepeticion = delayRep;
    }

    /// <summary>
    /// Procesa el input. Retorna TRUE si el input fue consumido.
    /// </summary>
    public virtual Boolean Procesar()
    {
        // --- INPUTS DE SISTEMA / DEBUG ---
        // Estos se procesan SIEMPRE, sin importar los delays del hijo.

        // Ejemplo: Cambio de Resolución (F1, F2, F3...)
        if (Raylib.IsKeyDown(KeyboardKey.F1))
        {
            ScreenManager.Filtro = ScreenManager.Filtro == TextureFilter.Point ? TextureFilter.Trilinear : TextureFilter.Point;
            return true;
        }
        if (Raylib.IsKeyDown(KeyboardKey.F2))
        {
            //Config.CambiarEscala(2); 
            return true;
        }

        // Ejemplo: Toggle de información de Debug
        if (Raylib.IsKeyDown(KeyboardKey.F10))
        {
            //Config.DebugMode = !Config.DebugMode;
            return true;
        }

        return false; // Nadie consumió el input todavía
    }
}
