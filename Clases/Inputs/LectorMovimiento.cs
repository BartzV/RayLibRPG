using RayLibRPG.Clases.Config;
using System.Numerics;

namespace RayLibRPG.Clases.Inputs;

public class LectorMovimiento<T> : LectorInput where T : ITransformable
{
    private T _elemento;

    // Para evitar allocs de Vector2 en cada tick
    private static readonly Vector2 DIR_IZQ = new(-1, 0);
    private static readonly Vector2 DIR_DER = new(1, 0);
    private static readonly Vector2 DIR_ARR = new(0, -1);
    private static readonly Vector2 DIR_ABJ = new(0, 1);

    public LectorMovimiento(T elemento, int ini = 1, int rep = 1)
        : base(ini, rep)
    {
        _elemento = elemento;
    }

    public override bool Procesar()
    {
        // 1. ¿El sistema consumió el input (Debug/Resolución)?
        if (base.Procesar()) return true;

        bool consumido = false;

        // 2. Procesar movimiento usando los Timers de AccionTrigger
        // Usamos if separados para permitir diagonales si querés, 
        // o else-if para movimiento ortogonal puro.


        if (InputConfig.AccionTrigger(Accion.Izquierda, _delayInicial, _delayRepeticion))
        {
            _elemento.Mover(DIR_IZQ);
            consumido = true;
        }
        else if (InputConfig.AccionTrigger(Accion.Derecha, _delayInicial, _delayRepeticion))
        {
            _elemento.Mover(DIR_DER);
            consumido = true;
        }

        if (InputConfig.AccionTrigger(Accion.Arriba, _delayInicial, _delayRepeticion))
        {
            _elemento.Mover(DIR_ARR);
            consumido = true;
        }
        else if (InputConfig.AccionTrigger(Accion.Abajo, _delayInicial, _delayRepeticion))
        {
            _elemento.Mover(DIR_ABJ);
            consumido = true;
        }

        return consumido;
    }
}