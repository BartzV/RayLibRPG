using RayLibRPG.Clases.Config;
using System.Numerics;

namespace RayLibRPG.Clases.Inputs;

public class LectorInputCustom<T> : LectorInput where T : IRenderizable, ITransformable
{
    protected T Elemento;

    public LectorInputCustom(T elemento, int delayIni = 1, int delayRep = 1)
    {
        this.Elemento = elemento;
        this.DELAY_INICIAL = delayIni;
        this.DELAY_REPETICION = delayRep;

        this.ActArr = (obj) =>
        {
            obj.Mover(arr);
            return true;
        };
        this.ActAbj = (obj) =>
        {
            obj.Mover(abj);
            return true;
        };
        this.ActIzq = (obj) =>
        {
            obj.Mover(izq);
            return true;
        };
        this.ActDer = (obj) =>
        {
            obj.Mover(der);
            return true;
        };

        this.ActL = (obj, RP) =>
        {
            obj.Rotar(4F);
            return true;
        };
        this.ActR = (obj, LP) =>
        {
            obj.Rotar(-4F);
            return true;
        };
        this.ActA = (obj) =>
        {
            obj.AplicarZoom(0.05F);
            return true;
        };
        this.ActB = (obj) =>
        {
            obj.AplicarZoom(-0.05F);
            return true;
        };
        this.ActX = (obj) =>
        {
            obj.Amplificacion += new Vector2(0.1F, 0);
            return true;
        };
        this.ActY = (obj) =>
        {
            obj.Amplificacion += new Vector2(-0.1F, 0);
            return true;
        };
    }

    protected Vector2 izq = new Vector2(-1, 0);
    protected Vector2 der = new Vector2(1, 0);
    protected Vector2 arr = new Vector2(0, -1);
    protected Vector2 abj = new Vector2(0, 1);

    // Cursores 
    public Func<T, Boolean> ActIzq;
    public Func<T, Boolean> ActDer;

    public Func<T, Boolean> ActArr;
    public Func<T, Boolean> ActAbj;
    // Botones L y R
    public Func<T, Boolean, Boolean> ActL;
    public Func<T, Boolean, Boolean> ActR;
    // Botones A, B, X e Y
    public Func<T, Boolean> ActA; 
    public Func<T, Boolean> ActB; 
    public Func<T, Boolean> ActX; 
    public Func<T, Boolean> ActY; 

    public override Boolean Procesar()
    {
        Boolean res = false;    // Al pedo, pero por las dudas...
        Boolean izqP = InputConfig.IzquierdaPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean derP = InputConfig.DerechaPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean arrP = InputConfig.ArribaPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean abjP = InputConfig.AbajoPresionada(this.DELAY_INICIAL, this.DELAY_REPETICION);

        Boolean AP = InputConfig.A_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean BP = InputConfig.B_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean XP = InputConfig.X_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean YP = InputConfig.Y_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);

        Boolean LP = InputConfig.L_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean RP = InputConfig.R_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);

        if(arrP && !abjP)
        {
            res |= this.ActArr(this.Elemento);
        }
        if(abjP && !arrP)
        {
            res |= this.ActAbj(this.Elemento);
        }
        if(derP && !izqP)
        {
            res |= this.ActDer(this.Elemento);
        }
        if(izqP && !derP)
        {
            res |= this.ActIzq(this.Elemento);
        }
        if (LP)
        {
            res |= this.ActL(this.Elemento, RP);
        }
        if (RP)
        {
            res |= this.ActR(this.Elemento, LP);
        }
        if (AP)
        {
            res |= this.ActA(this.Elemento);
        }
        if (BP)
        {
            res |= this.ActB(this.Elemento);
        }
        if (XP)
        {
            res |= this.ActX(this.Elemento);
        }
        if (YP)
        {
            res |= this.ActY(this.Elemento);
        }

        return res;
    }

}
