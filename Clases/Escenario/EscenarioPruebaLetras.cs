using Raylib_cs;
using RayLibRPG.Clases.Config;
using RayLibRPG.Clases.Inputs;
using RayLibRPG.Clases.Letras;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayLibRPG.Clases.Escenario
{
    internal class EscenarioPruebaLetras : EscenarioEngine
    {
        public Capa[] Capas = new Capa[2];
        public LectorInput Lector;
        public List<RichText> Richs = new();
        public List<Letra> Letras = new();
        public List<BarraProgreso> Barras = new();

        public List<IRenderizable> EscenarioS = new();
        public List<IActualizable> EscenarioU = new();

        public override void Initialize()
        {
            this.Capas = ScreenManager.InsertarCapas(CapaFactory.CrearCapasPrueba());
            //this.InicializarEscenario();
            //this.InicializarRich();
            //this.InicializarLetra();
            this.InicializarBarra();
        }

        public override void Draw(float alfa)
        {
            Int32 counter = 0;
            for (Int32 i = 0; i < Capas.Length; i++)
            {
                counter += this.Capas[i].Renderizar(alfa, EngineManager.FramesTranscurridos);
            }

            base.Draw(alfa);

            if (EngineManager.FramesTranscurridos % 30 == 0)
            {
                //string titulo = $"Sir Bartz Engine | FPS: {Raylib.GetFPS()}/{ScreenManager.FPS} | Ticks: {EngineManager.TicksTranscurridos} | Frames: {EngineManager.FramesTranscurridos}";
                String titulo = $"Sir Bartz Engine | FPS: {Raylib.GetFPS()}/{ScreenManager.FPS} | Draws: {counter,2}";
                Raylib.SetWindowTitle(titulo);
            }
        }

        public void InicializarBarra()
        {
            BarraProgreso bar = new(Vector2.One, new Vector2(64, 4), Color.Green, Color.Red);
            bar.Porcentaje = 0.5f;
            this.Barras.Add(bar);

            this.EscenarioU.Add(bar);
            this.Capas[0].InsertarElemento(bar);
        }

        public void InicializarEscenario()
        {
            Texture2D textura = Texture2DManager.LoadTexture(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigManager.RUTA_BG_BOSQUE), "BGBosque");
            Sprite2D montaña1 = 
                new Sprite2D(textura, 
                    // Fuente
                    new Rectangle(0, 16, 16 * 6, 16 * 3), 
                    // Destino
                    new Rectangle(-ConfigManager.WIDTH / 2, 0, 16 * 6, 16 * 3)); 
            MultiSprite2D multiMontaña1 = new(montaña1, 4);

            this.EscenarioU.Add(montaña1);
            this.Capas[0].InsertarElemento(montaña1);
        }

        public void InicializarRich()
        {
            Color[] arcoiris = [Color.Red, Color.Orange, Color.Gold, Color.Lime, Color.Green, Color.SkyBlue, Color.Blue, Color.Magenta];
            Color[] defecto = [Color.White, new Color(192, 64, 0), new Color(0, 192, 128), new Color(64, 0, 200)];

            RichText rich = new RichText(cadPrueba, defecto, [arcoiris], Vector2.Zero, Vector2.One, 1);
            this.Lector = new LectorInputRichText(rich);

            this.Richs.Add(rich);
            this.Capas[0].InsertarElemento(rich);
        }

        public void InicializarLetra()
        {
            Letra l = new Letra('A', Vector2.Zero, Vector2.One, Color.Red);
            l.ZBuffer = 1;

            this.Letras.Add(l);
            this.Lector = new LectorInputLetra(l);
            this.Capas[0].InsertarElemento(l);
        }

        public override void Update()
        {
            for(Int32 i = 0; i < EscenarioS.Count; i++)
            {
                this.EscenarioU[i].Update();
            }
            for (Int32 i = 0; i < Letras.Count; i++)
            {
                this.Letras[i].Update();
            }
            for (Int32 i = 0; i < Richs.Count; i++)
            {
                this.Richs[i].Update();
            }

            // 2. Actualizamos los contadores de las teclas
            InputConfig.Actualizar();
            // 3. El LectorInput ahora usa el nuevo método
            this.Lector?.Procesar();

            base.Update();
        }


        private static String cadPrueba =
            "{c1}{i01}Agrass{c0}:{s}" +
            "{t04}Espero estés listo para lo que viene{w01}…{w01}…{w01}…{s}" +
            "{t04}{p0}{e2}{w05}¡TRANSFORMACIÓN!";
    }
}
