using Raylib_cs;
using RayLibRPG.Clases.Config;
using RayLibRPG.Clases.Letras;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayLibRPG.Clases.Escenario
{
    internal class EscenarioPruebaLetras : EscenarioEngine
    {
        public List<Letra> Letras = new();
        public Capa[] Capas = new Capa[2];
        public LectorInput Lector;


        public override void Initialize()
        {
            this.Capas = ScreenManager.InsertarCapas(CapaFactory.CrearCapasPrueba());
            Color[] arcoiris = [Color.Red, Color.Orange, Color.Gold, Color.Lime, Color.Green, Color.SkyBlue, Color.Blue, Color.Magenta];
            Color[] defecto = [Color.White, new Color(164, 0, 164)];

            Letra l = new Letra('L', Vector2.Zero, Vector2.One, Color.Red);
            this.Lector = new LectorInputLetra(l);

            this.Letras.Add(l);
            this.Capas[0].InsertarElemento(l);
        }

        public override void Draw(float alfa)
        {
            for (Int32 i = 0; i < Capas.Length; i++)
            {
                Capas[i].Renderizar(alfa, EngineManager.FramesTranscurridos);
            }

            base.Draw(alfa);

            if (EngineManager.FramesTranscurridos % 30 == 0)
            {
                string titulo = $"Sir Bartz Engine | FPS: {Raylib.GetFPS()}/{ScreenManager.FPS} | Ticks: {EngineManager.TicksTranscurridos} | Frames: {EngineManager.FramesTranscurridos}";
                Raylib.SetWindowTitle(titulo);
            }
        }

        public override void Update()
        {
            // Letras[0].Mover(Vector2.One); //Esto lo mueve fluido!!!!
            
            for (Int32 i = 0; i < Letras.Count; i++)
            {
                Letras[0].Update();
            }

            this.Lector.Procesar();
            base.Update();
        }

    }
}
