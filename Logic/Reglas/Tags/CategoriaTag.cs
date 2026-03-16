namespace RayLibRPG.Logic.Reglas.Tags;

[Flags]
public enum CategoriaTag : Int32
{
    // Neutro, nulo, usado para habilidades imbloqueables o sin categoría (Movimiento "Bloquear")
    Neutral = 1 << 0,
    // Ataques físicos
    Fisico = Incisivo | Contundente | Perforante,
    Incisivo = 1 << 1,          // Cortes, como espadas o garras
    Contundente = 1 << 2,       // Golpes, como martillos o piedrazos
    Perforante = 1 << 3,        // Puntadas, como flechas, apuñaladas o colmillos
    // Buffs o categorías abstractas
    Curativo = 1 << 4,          // Habilidades/Items sanadores, como Aloe Vera o Luz Reparadora
    Magico = 1 << 5,            // Autoexplicativo, pero abarcativo. Lanzallamas, rayos, barreras...
    Alquimico = 1 << 6,         // Bombas químicas o reacciones químicas, como la del Escarabajo Bombarda
    Biologico = 1 << 7,         // Esporas o saliva
    Psionico = 1 << 8,          // "Magia" psíquica, que no es magia. Intimidaciones o somníferos
    Desplazamiento = 1 << 9,    // Empujes
    // ...
    Adaptable = 1 << 31         // Reservado para habilidades especiales
}
