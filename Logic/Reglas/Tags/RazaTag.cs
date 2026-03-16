using System;
using System.Collections.Generic;
using System.Text;

namespace RayLibRPG.Logic.Reglas.Tags
{
    [Flags]
    public enum RazaTag
    {
        Desconocido = 0,
        Humano = 1,
        Bestia = 2,
        NoMuerto = 4,
        Espectro = 8,
    }
}
