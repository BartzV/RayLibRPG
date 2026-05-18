using Raylib_cs;
using RayLibRPG.Clases.Capas;
using RayLibRPG.Clases.Config;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayLibRPG.Clases.Escenario;

/// <summary>
/// Los escenarios son los estados del juego. Cada uno tiene sus propias capas, que se actualizan y renderizan a su ritmo.<br/>
/// Ejemplo rápido: HUD, menú de pausa, etc.
/// </summary>
public abstract class EscenarioEngine : IDisposable
{
    protected Boolean _eliminado = false;

    public List<Capa> Capas { get; set; } = new();
    public Int64 FramesTranscurridos { get; set; } = 0;
    public Int64 TicksTranscurridos { get; set; } = 0;

    /// <summary>
    /// Acá se actualiza la lógica del juego. El juego va a <see cref="ConfigManager.TPS"/> ticks por segundo.
    /// </summary>
    public virtual void Update()
    {
        for(Int32 i = 0; i < Capas.Count; i++)
        {
            this.Capas[i].Actualizar();
        }
        this.TicksTranscurridos++;
    }

    /// <summary>
    /// Acá se dibuja a <see cref="ScreenManager.FPS"/> FPS. Cada capa se renderiza a su ritmo, y luego se ensamblan todas las texturas en la pantalla.<br/>
    /// Como el juego va a <see cref="ConfigManager.TPS"/> ticks por segundo,
    /// cada capa puede renderizarse a más FPS de los TPS, o a la misma cantidad.<br/>
    /// Por ejemplo, se puede tener una capa a 120FPS y otras 2 a 60FPS.
    /// </summary>
    /// <param name="alfa"></param>
    /// <returns></returns>
    public virtual Int32 Draw(Single alfa)
    {
        Int32 cant = 0;
        // 1. Renderizado a texturas (cada una a su ritmo)
        for (Int32 i = 0; i < this.Capas.Count; i++)
        {
            // La capa Main es rápida!
            if (this.Capas[i].EsRapido || (this.FramesTranscurridos & ScreenManager.FPS_Ops) == 0)
            {
                cant += this.Capas[i].Renderizar(alfa, this.FramesTranscurridos);
            }
        }

        // 2. Ensamble final
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        // Pegamos las texturas usando sus Rectangles de destino
        for (Int32 i = 0; i < this.Capas.Count; i++)
        {
            PegarCapa(this.Capas[i]);
        }

        Raylib.EndDrawing();
        this.FramesTranscurridos++;
        return cant;
    }

    private static void PegarCapa(in Capa c)
    {
        Raylib.DrawTexturePro(
            c.TexturaInterna.Texture,
            new Rectangle(0, 0, c.TexturaInterna.Texture.Width, -c.TexturaInterna.Texture.Height),
            c.DestinoEnPantalla,
            Vector2.Zero, 0f, c.Tinte
        );
    }

    public virtual void Initialize()
    {

    }

    public virtual void LimpiarBasura()
    {
        for(Int32 i = 0; i < this.Capas.Count; i++)
        {
            this.Capas[i].LimpiarBasura();
        }
    }

    /// <summary>
    /// Acordate del Using!!!
    /// Por cierto, esto funciona. Tratá de no hacerle <see langword="override"></see> por favor.
    /// </summary>
    public void Dispose()
    {
        if (this._eliminado) return;
        for(Int32 i = 0; i < this.Capas.Count; i++)
        {
            this.Capas[i].Dispose();
        }
        this._eliminado = true;
        // Esto es para que el GC no llame al finalizador, ya que ya liberamos los recursos manualmente.
        GC.SuppressFinalize(this);
    }
}
