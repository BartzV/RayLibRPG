using Raylib_cs;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace RayLibRPG.Clases.Config;

public enum Accion
{
    Arriba, Abajo, Izquierda, Derecha,
    Aceptar, Cancelar, Menu
}

internal static class InputConfig
{
    // Mapeo de Accion -> Tecla (Mañana esto puede venir de un .json)
    private static readonly Dictionary<Accion, KeyboardKey> MapeoTeclas = new()
    {
        { Accion.Arriba, KeyboardKey.Up },
        { Accion.Abajo, KeyboardKey.Down },
        { Accion.Izquierda, KeyboardKey.Left },
        { Accion.Derecha, KeyboardKey.Right },
        { Accion.Aceptar, KeyboardKey.S }, // Tu 'A'
        { Accion.Cancelar, KeyboardKey.A }, // Tu 'B'
        { Accion.Menu, KeyboardKey.D }      // Tu 'Start'
    };

    // Diccionario de timers (se queda igual, es eficiente)
    private static readonly Dictionary<Accion, int> _timersAcciones = new();

    public static void Actualizar()
    {
        foreach (var mapping in MapeoTeclas)
        {
            if (Raylib.IsKeyDown(mapping.Value))
            {
                if (!_timersAcciones.ContainsKey(mapping.Key))
                    _timersAcciones[mapping.Key] = 0;
                else
                    _timersAcciones[mapping.Key]++;
            }
            else
            {
                _timersAcciones.Remove(mapping.Key);
            }
        }
    }

    // La función mágica de repetición
    public static bool AccionTrigger(Accion accion, int init, int rep)
    {
        if (!_timersAcciones.TryGetValue(accion, out int ticks))
            return false;

        // Caso 1: El frame exacto donde se apretó
        if (ticks == 0) return true;

        // Caso 2: Pasó el delay inicial y cumple el intervalo de repetición
        if (ticks >= init)
        {
            return (ticks - init) % rep == 0;
        }

        return false;
    }
}

//internal static class InputConfig
//{
//    // Diccionario para seguir cuánto tiempo lleva cada tecla apretada
//    private static Dictionary<KeyboardKey, int> _timersControles = new();
//    // Array de controles, para cambiar en algún futuro, que se lea de un config o algo.
//    private static KeyboardKey[] _controles =
//    {
//        //  Cursores
//        KeyboardKey.Up,
//        KeyboardKey.Down,
//        KeyboardKey.Left,
//        KeyboardKey.Right,
//        //  Acciones Simples
//        KeyboardKey.S,
//        KeyboardKey.A,
//        KeyboardKey.X,
//        KeyboardKey.Z,
//        //  L y R
//        KeyboardKey.Q,
//        KeyboardKey.W,
//        // Start y Select
//        KeyboardKey.D,
//        KeyboardKey.E,
//        // Comandos de Control
//        KeyboardKey.F2,
//        KeyboardKey.F3,
//    };

//    // Cursores
//    public static Boolean IzquierdaPresionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.Left, init, rep);
//    public static Boolean DerechaPresionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.Right, init, rep);
//    public static Boolean ArribaPresionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.Up, init, rep);
//    public static Boolean AbajoPresionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.Down, init, rep);
//    // Botones
//    public static Boolean A_Presionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.S, init, rep);
//    public static Boolean B_Presionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.A, init, rep);
//    public static Boolean X_Presionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.X, init, rep);
//    public static Boolean Y_Presionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.Z, init, rep);

//    public static Boolean L_Presionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.Q, init, rep);
//    public static Boolean R_Presionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.W, init, rep);

//    public static Boolean Start_Presionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.D, init, rep);


//    public static void Actualizar()
//    {
//        foreach (var tecla in _controles)
//        {
//            if (Raylib.IsKeyDown(tecla))
//            {
//                if (!_timersControles.ContainsKey(tecla))
//                    _timersControles[tecla] = 0;
//                else
//                    _timersControles[tecla]++;
//            }
//            else
//            {
//                _timersControles.Remove(tecla);
//            }
//        }
//    }

//    public static Boolean AccionPresionada(KeyboardKey tecla, Int32 DELAY_INICIAL, Int32 DELAY_REPETICION)
//    {
//        if (!_timersControles.ContainsKey(tecla)) return false;

//        int ticks = _timersControles[tecla];

//        // 1. Se acaba de apretar (Tick 0)
//        if (ticks == 0) return true;

//        // 2. Pasó el delay inicial y estamos en el tick de repetición
//        if (ticks >= DELAY_INICIAL)
//        {
//            return (ticks - DELAY_INICIAL) % DELAY_REPETICION == 0;
//        }

//        return false;
//    }
//}
