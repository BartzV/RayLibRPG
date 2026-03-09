using RayLibRPG.Clases.Config;
using RayLibRPG.Clases.Letras;
using System.Numerics;

namespace RayLibRPG.Clases.Inputs;

public class LectorInputLetra : LectorInput
{
    protected Letra Elemento;

    public LectorInputLetra(Letra elemento, int delayIni = 1, int delayRep = 1)
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
        Boolean zoomIn = InputConfig.A_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);
        Boolean zoomOut = InputConfig.B_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);

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
        if(zoomIn && !zoomOut)
        {
            this.Elemento.AplicarZoom(0.1F);
            res = true;
        }
        if (zoomOut && !zoomIn)
        {
            this.Elemento.AplicarZoom(-0.1F);
            res = true;
        }
        return res;
    }
}
