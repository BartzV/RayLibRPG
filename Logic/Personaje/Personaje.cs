using Raylib_cs;
using RayLibRPG.Logic.Reglas.Tags;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayLibRPG.Logic.Personaje;

public class Personaje
{
    public String Id { get; set; }
    public String Nombre { get; set; }
    // Datos estéticos
    public Color ColorPrimario { get; set; }
    // Datos de Rol
    public RazaTag Raza { get; set; }

    // Registros comunes
    // PC = Puntos Corazón, o Puntos Cuarzo
    public Int32 PCMax { get; set; }

    // Por cada combate, se va perdiendo el "máximo" hasta descansar.
    public Int32 PCActual { get; set; }
    public Int32 PCMaxActual { get; set; }
    // TODO: Resto jaja



}


//public class PersonajeSprite : IDesplazable, IRenderizable, IActualizable
//{
//    public Sprite2D[] Sprites { get; set; }

//    public PersonajeSprite()
//    {

//    }

//}
