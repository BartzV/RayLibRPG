using System;
using System.Collections.Generic;
using System.Text;

namespace RayLibRPG.Logic.Reglas.Tags;

[Flags]
public enum ElementoTag : Int32
{
    Neutral = 1 << 0,           // Sin elemento. El corte del acero, el golpe de un cuerno...
    // Elementos clásicos
    Fuego = 1 << 1,             // Caliente. No necesariamente relacionado con la luz o lo sagrado
    Hielo = 1 << 2,             // Frío
    Rayo = 1 << 3,              // Shock de alguilas, rayos eléctricos, cables de ruinas antiguas
    Tierra = 1 << 4,            // También abarca piedras. "Terremoto" pregunta también sobre la altura del enemigo, mucho ojo
    Agua = 1 << 5,              // Puede cortar, empujar, golpear, curar...
    Viento = 1 << 6,            // Puede cortar, empujar, purificar...
    // Ambos usados mucho por espectros
    Umbreo = 1 << 7,            // Ataques obscuros
    Sacro = 1 << 8,             // Ataques sagrados
    // Usado por animales/vegetales salvajes
    Sangre = 1 << 9,            // Desangrados o drenajes de vida. Aureon no puede ser desangrado
    Veneno = 1 << 10,           // Veneno o virus. Aureon no puede ser envenenado
    Acido = 1 << 11,            // Sustancia que derrite, disuelve, debilita...
    // Tanto más exclusivos
    Explosivo = 1 << 12,        // También abarca sonidos, pero el nombre "Trueno" (según las encuestas) es confundido con Rayo
    // ...
    Adaptable = 1 << 31         // Reservado para habilidades especiales
}
