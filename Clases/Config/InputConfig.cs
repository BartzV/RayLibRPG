using Raylib_cs;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace RayLibRPG.Clases.Config;

internal static class InputConfig
{
    // Diccionario para seguir cuánto tiempo lleva cada tecla apretada
    private static Dictionary<KeyboardKey, int> _timersControles = new();
    // Array de controles, para cambiar en algún futuro, que se lea de un config o algo.
    private static KeyboardKey[] _controles =
        [KeyboardKey.Up, KeyboardKey.Down, KeyboardKey.Left, KeyboardKey.Right,
            KeyboardKey.S, KeyboardKey.A, KeyboardKey.D];

    // Cursores
    public static Boolean IzquierdaPresionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.Left, init, rep);
    public static Boolean DerechaPresionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.Right, init, rep);
    public static Boolean ArribaPresionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.Up, init, rep);
    public static Boolean AbajoPresionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.Down, init, rep);
    // Botones
    public static Boolean A_Presionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.S, init, rep);
    public static Boolean B_Presionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.A, init, rep);


    public static void Actualizar()
    {
        foreach (var tecla in _controles)
        {
            if (Raylib.IsKeyDown(tecla))
            {
                if (!_timersControles.ContainsKey(tecla))
                    _timersControles[tecla] = 0;
                else
                    _timersControles[tecla]++;
            }
            else
            {
                _timersControles.Remove(tecla);
            }
        }
    }

    public static Boolean AccionPresionada(KeyboardKey tecla, Int32 DELAY_INICIAL, Int32 DELAY_REPETICION)
    {
        if (!_timersControles.ContainsKey(tecla)) return false;

        int ticks = _timersControles[tecla];

        // 1. Se acaba de apretar (Tick 0)
        if (ticks == 0) return true;

        // 2. Pasó el delay inicial y estamos en el tick de repetición
        if (ticks >= DELAY_INICIAL)
        {
            return (ticks - DELAY_INICIAL) % DELAY_REPETICION == 0;
        }

        return false;
    }
}
