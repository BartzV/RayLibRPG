using RayLibRPG.Clases.Config;
using RayLibRPG.Clases.Letras;

namespace RayLibRPG.Clases.Inputs;

public class LectorInputRichText : LectorInputDebug<RichText>
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
        Boolean accBP = InputConfig.B_Presionada(this.DELAY_INICIAL, this.DELAY_REPETICION);

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
        if (accBP)
        {
            this.elemento.Terminar();
            res = true;
        }
        return res;
    }
}