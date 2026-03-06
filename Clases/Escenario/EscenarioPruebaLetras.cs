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

        public List<IActualizable> EscenarioU = new();

        public override void Initialize()
        {
            this.Capas = ScreenManager.InsertarCapas(CapaFactory.CrearCapasPrueba());
            //this.InicializarEscenario();
            //this.InicializarRich();
            //this.InicializarLetra();
            //this.InicializarBarra();
            this.InicializarPolys();
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

        public void InicializarPolys()
        {
            Poligono2D pol1 = new([new Vector2(10, 0), new Vector2(0, 10), new Vector2(110, 0), new Vector2(100, 10)], Vector2.Zero, new Color(255, 0, 0, 255));
            this.EscenarioU.Add(pol1);
            this.Capas[0].InsertarElemento(pol1);
            this.Lector = new LectorInputDebug<Poligono2D>(pol1);
        }

        public void InicializarBarra()
        {
            BarraProgreso bar1 = new(new Vector2(0, 0), 128, Color.Green, Color.Red, Color.Gold);
            bar1.Porcentaje = 0.75f;
            bar1.Prioridad = 1000;
            this.Barras.Add(bar1);

            BarraProgreso bar2 = new(new Vector2(0, 20), 8, Color.SkyBlue, Color.DarkBlue, Color.Beige);
            bar2.Porcentaje = 0.33f;
            bar2.Prioridad = 1000;
            this.Barras.Add(bar2);

            RichText rich = new("{c1}100{c0}/{c2}100", [new Color(255, 255, 255), new Color(204, 255, 204), new Color(255, 204, 255)], null, new Vector2(0, 0), Vector2.One, 1);
            rich.Prioridad = 101;

            this.EscenarioU.Add(rich);
            this.EscenarioU.Add(bar1);
            this.EscenarioU.Add(bar2);

            this.Capas[0].InsertarElemento(rich);
            this.Capas[0].InsertarElemento(bar1);
            this.Capas[0].InsertarElemento(bar2);
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
            l.Escala = 1;

            this.Letras.Add(l);
            this.Lector = new LectorInputLetra(l);
            this.EscenarioU.Add(l);
            this.Capas[0].InsertarElemento(l);
        }

        public override void Update()
        {
            for (Int32 i = 0; i < EscenarioU.Count; i++)
            {
                this.EscenarioU[i].Update();
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
