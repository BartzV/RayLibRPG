using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases.Letras;

public class LetraPaleta : Letra
{
    public Color[] Colores;

    public LetraPaleta(Char caracter, Vector2 posicion, Vector2 amplitudes, Color[] colores, Int32 delayUpdate = 1, Int32 alfaUpdate = 0)
        : base(caracter, posicion, amplitudes, null, alfaUpdate)
    {
        if (colores.Length == 0)
            throw new ArgumentException("Una letra arcoiris no tiene colores cargados!");
        if (delayUpdate < 1)
            delayUpdate = 1;
        this.Colores = colores;

        this.DelayUpdate = delayUpdate;
        this.AlfaUpdate = alfaUpdate % colores.Length;
    }

    public override void Update()
    {
        this._actualAlfaUpdate++;
        if (this._actualAlfaUpdate >= this.DelayUpdate)
        {
            this._actualAlfaUpdate = 0;
            this.AlfaUpdate++;
        }
        // Evitemos overflow...
        if (this.AlfaUpdate >= Colores.Length)
        {
            this.AlfaUpdate = 0;
        }
        this.Sprite.Tinte = this.Colores[AlfaUpdate];
        base.Update();
    }
}

public static class LetraPaletaHelper
{
    public static Color[] Arcoiris() => [Color.Red, Color.Orange, Color.Gold, Color.Lime, Color.Green, Color.SkyBlue, Color.Blue, Color.Magenta];

}