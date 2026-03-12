using Raylib_cs;
using RayLibRPG.Clases.Config;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayLibRPG.Clases.Escenario
{
    public abstract class EscenarioEngine
    {
        public List<Capa> Capas { get; set; } = new();
        public Int64 FramesTranscurridos { get; set; } = 0;
        public Int64 TicksTranscurridos { get; set; } = 0;

        public virtual void Update()
        {
            for(Int32 i = 0; i < Capas.Count; i++)
            {
                this.Capas[i].Actualizar();
            }
            this.TicksTranscurridos++;
        }

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
    }
}
