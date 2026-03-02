using Raylib_cs;
using RayLibRPG.Clases.Letras;
using System.Numerics;
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
    public static Boolean A_Presionada(Int32 init, Int32 rep) => AccionPresionada(KeyboardKey.A, init, rep);


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

    public static bool AccionPresionada(KeyboardKey tecla, Int32 DELAY_INICIAL, Int32 DELAY_REPETICION)
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
/// <summary>
/// Clase base para los lectores.
/// </summary>
public abstract class LectorInput
{
    protected Int32 DELAY_INICIAL = 1;
    protected Int32 DELAY_REPETICION = 1;
    public abstract Boolean Procesar();
}
/// <summary>
/// Ejemplo. No tomar en serio.
/// </summary>
public class LectorInputDebug : LectorInput
{
    protected IDesplazable Elemento;

    public LectorInputDebug(IDesplazable elemento, int delayIni = 1, int delayRep = 1)
    {
        this.Elemento = elemento;
        this.DELAY_INICIAL = delayIni;
        this.DELAY_REPETICION = delayRep;
    }

    // Para evitar alloc? Es un STRUCT!!! 
    protected Vector2 izq = new Vector2(-1, 0);
    protected Vector2 der = new Vector2(1, 0);
    protected Vector2 arr = new Vector2(0, -1);
    protected Vector2 abj = new Vector2(0, 1);

    public override Boolean Procesar()
    {
        Boolean res = false;    // Al pedo, pero por las dudas...
        Boolean izqP = InputConfig.IzquierdaPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean derP = InputConfig.DerechaPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean arrP = InputConfig.ArribaPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean abjP = InputConfig.AbajoPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);

        if (izqP && !derP)
        {
            this.Elemento.Mover(izq);
            res = true;
        }
        if (derP && !izqP)
        {
            this.Elemento.Mover(der);
            res = true;
        }
        if (arrP && !abjP)
        {
            this.Elemento.Mover(arr);
            res = true;
        }
        if (abjP && !arrP)
        {
            this.Elemento.Mover(abj);
            res = true;
        }
        return res;
    }
}

public class LectorInputRichText : LectorInputDebug
{
    protected RichText elemento;
    public LectorInputRichText(RichText elemento, int delayIni = 1, int delayRep = 1) : base(elemento, delayIni, delayRep)
    {
        this.elemento = elemento;
    }

    public override Boolean Procesar()
    {
        Boolean res = false;    // Al pedo, pero por las dudas...
        Boolean izqP = InputConfig.IzquierdaPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean derP = InputConfig.DerechaPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean arrP = InputConfig.ArribaPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean abjP = InputConfig.AbajoPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean accAP = InputConfig.A_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);

        if (izqP && !derP)
        {
            this.Elemento.Mover(izq);
            res = true;
        }
        if (derP && !izqP)
        {
            this.Elemento.Mover(der);
            res = true;
        }
        if (arrP && !abjP)
        {
            this.Elemento.Mover(arr);
            res = true;
        }
        if (abjP && !arrP)
        {
            this.Elemento.Mover(abj);
            res = true;
        }
        if (accAP)
        {
            this.elemento.Reiniciar();
            res = true;
        }
        return res;
    }
}