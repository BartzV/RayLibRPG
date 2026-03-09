using RayLibRPG.Clases.Config;
using RayLibRPG.Logic.Personaje;
using System.Numerics;

namespace RayLibRPG.Clases.Inputs;

/// <summary>
/// Ejemplo. No tomar en serio.
/// </summary>
public class LectorInputDebug<T> : LectorInput where T : IRenderizable, ITransformable
{
    protected T Elemento;

    public LectorInputDebug(T elemento, int delayIni = 1, int delayRep = 1)
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

        Boolean zoomInP = InputConfig.A_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean zoomOutP = InputConfig.B_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);

        Boolean rotIzP = InputConfig.L_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean rotDeP = InputConfig.R_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);

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
        if (zoomInP && !zoomOutP)
        {
            this.Elemento.AplicarZoom(0.1F);
            res = true;
        }
        if (zoomOutP && !zoomInP)
        {
            this.Elemento.AplicarZoom(-0.1F);
            res = true;
        }
        if (rotIzP && !rotDeP)
        {
            this.Elemento.Rotar(4F);
            res = true;
        }
        if (!rotIzP && rotDeP)
        {
            this.Elemento.Rotar(-4F);
            res = true;
        }
        return res;
    }
}

public class LectorInputHUD : LectorInput
{
    protected PersonajeHUD Elemento;

    public LectorInputHUD(PersonajeHUD elemento, int delayIni = 1, int delayRep = 1)
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

        Boolean zoomInP = InputConfig.A_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean zoomOutP = InputConfig.B_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);

        Boolean rotIzP = InputConfig.L_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean rotDeP = InputConfig.R_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);

        if (izqP && !derP)
        {
            this.Elemento.PJ.PCActual -= 1;
            res = true;
        }
        if (derP && !izqP)
        {
            this.Elemento.PJ.PCActual += 1;
            res = true;
        }
        if (arrP && !abjP)
        {
            res = true;
        }
        if (abjP && !arrP)
        {
            res = true;
        }
        if (zoomInP && !zoomOutP)
        {
            res = true;
        }
        if (zoomOutP && !zoomInP)
        {
            res = true;
        }
        if (rotIzP && !rotDeP)
        {
            res = true;
        }
        if (!rotIzP && rotDeP)
        {
            res = true;
        }
        return res;
    }
}