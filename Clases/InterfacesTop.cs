using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayLibRPG.Clases;

public interface IRenderizable
{
    Boolean Activo { get; set; }
    Boolean Eliminado { get; set; }
    Single CapaPrioridad { get; set; } // Para el Sort dentro de la capa

    // El "grito" que van a escuchar las capas
    event Action? OnCambioPrioridad;

    Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible);
}

public interface IActualizable
{
    // No hay gran ciencia. Cada cosa renderizable que se mueva debe tener un update.
    public abstract void Update();
}

public interface ITransformable
{
    Vector2 Posicion { get; set; }
    Single ProfundidadZ { get; set; } // Lo que llamabas Escala (distancia)
    Single Rotacion { get; set; }
    Vector2 Amplificacion { get; set; }

    // Métodos de acción
    void Mover(Vector2 mov);
    void Posicionar(Vector2 pos);
    void Rotar(Single rad);
    void Estabilizar(Single rad); // Set directo

    // Transformaciones visuales
    void AplicarZoom(Single delta);
    void SetZoom(Single valor);
    void SetFlip(bool x, bool y);
}

public interface IEntidad : IRenderizable, IActualizable
{
    // Al heredar de ambas, ya tenés el contrato de vida y muerte asegurado.
}
