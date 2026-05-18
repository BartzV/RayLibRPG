using Raylib_cs;
using RayLibRPG.Clases.Config;

namespace RayLibRPG.Clases.Inputs;

/// <summary>
/// Clase base para los lectores.
/// </summary>
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

        //if (InputConfig.AccionTrigger(Accion.F2, 60, -1) && InputConfig.AccionTrigger(Accion.Arriba, 60, -1))
        if (Raylib.IsKeyDown(KeyboardKey.F2) && InputConfig.AccionTrigger(Accion.Arriba, 60, -1))
        {
            Int32 tam = Math.Min(ScreenManager.TamPixel + 1, 4);
            (Int32 W, Int32 H) = (512 * tam, 288 * tam);
            ScreenManager.CambiarResolucion(W, H);
            Program.EscenarioActual?.Capas.ForEach((x) => x.RecargarResolucion());
            return true;
        }
        if (Raylib.IsKeyDown(KeyboardKey.F2) && InputConfig.AccionTrigger(Accion.Abajo, 60, -1))
        {
            Int32 tam = Math.Max(ScreenManager.TamPixel - 1, 1);
            (Int32 W, Int32 H) = (512 * tam, 288 * tam);
            ScreenManager.CambiarResolucion(W, H);
            Program.EscenarioActual?.Capas.ForEach((x) => x.RecargarResolucion());
            return true;
        }
        if (Raylib.IsKeyDown(KeyboardKey.F3))
        {
            Console.WriteLine("Escriba un comando:");
            String? cmd = Console.ReadLine();
            if(cmd is null)
                return false;
            Console.WriteLine("Comando detectado!");
        }

        // Ejemplo: Toggle de información de Debug
        if (InputConfig.AccionTrigger(Accion.F1, 60, -1))
        {
            ConfigManager.DEBUG ^= DebugMode.Centers;
            return true;
        }

        return false; // Nadie consumió el input todavía
    }

    protected Int32 Comandos(String cmd)
    {
        switch (cmd)
        {
            case "debug_c":
                ConfigManager.DEBUG ^= DebugMode.Centers;
                return 1;
            case "all_dirty":
                Program.EscenarioActual?.Capas.ForEach((x) => x.DebeReordenar = true);
                return Program.EscenarioActual?.Capas.Count ?? 0;
            default:
                return 0;
        }
    }
}
