namespace RayLibRPG.Logic.Reglas.Tags;

[Flags]
public enum ItemTag : UInt64
{
    Ninguno = 0,
    // Naturaleza elemental
    Animal = 1L << 0,
    Vegetal = 1L << 1,
    Fungi = 1L << 2,
    Mineral = 1L << 3,
    // Usos prácticos
    Medicinal = 1L << 4,
    Alimento = 1L << 5,
    Conservante = 1L << 6,
    Consumible = 1L << 7,
    Equipable = 1L << 8,
    Combustible = 1L << 9,
    Hidratante = 1L << 10,
    Toxico = 1L << 11,
    // Alimento Animal
    Carne_Roja = 1L << 12,
    Carne_Blanca = 1L << 13,
    Pescado = 1L << 14,
    Marisco = 1L << 15,
    Carne_Invertebrado = 1L << 16,
    Huevo = 1L << 17,
    // Alimento Vegetal
    Fruta_Acida = 1L << 18,
    Fruta_Dulce = 1L << 19,
    Fruta_Neutra = 1L << 20,
    Verdura = 1L << 21,
    Grano = 1L << 22,
    // Falta Semilla...?
    // Plantas
    Hierba = 1L << 23,
    Flor = 1L << 24,
    Raiz = 1L << 25,
    // Minerales
    Metal = 1L << 26,
    Piedra = 1L << 27,
    Cristal = 1L << 28,
    // Materia Prima
    Madera = 1L << 29,
    Tela = 1L << 30,
    Cuero = 1L << 31,
    Algodón = 1L << 32,
    Lana = 1L << 33,
    Pluma = 1L << 34,
    Hueso = 1L << 35,
    Organo = 1L << 36,
    // Cultural
    Arcano = 1L << 37,
    Artificial_Quimico = 1L << 38,
    Artificial_Mecanico = 1L << 39,
    Artificial_Artesano = 1L << 40,
    Fermentado = 1L << 41,
    Maldito = 1L << 42,
    Sagrado = 1L << 43,
    Tecnologico = 1L << 44,
    Brillante = 1L << 45,
    Lujoso = 1L << 46,
    Aromatico = 1L << 47,
    Vital = 1L << 48,

    // Combinaciones comunes
    Alimento_Carne = Alimento | Carne_Blanca | Carne_Roja,
    Alimento_Espectro = Alimento | Carne_Blanca | Carne_Roja | Carne_Invertebrado | Vital, // Nótese el "Vital"
    Alimento_Pescado = Alimento | Pescado | Marisco,
    Alimento_Carnivoro = Alimento | Alimento_Carne | Alimento_Pescado | Huevo, // Nótese la falta de la carne de insecto
    Alimento_Herbivoro = Alimento | Fruta_Dulce | Fruta_Neutra | Fruta_Acida | Verdura | Grano,

    Alimento_Fruta = Alimento | Fruta_Dulce | Fruta_Neutra | Fruta_Acida,
    Alimento_Verdura = Alimento | Verdura,
    Alimento_Grano = Alimento | Grano,
    Hierba_Medicinal = Hierba | Medicinal,
}
