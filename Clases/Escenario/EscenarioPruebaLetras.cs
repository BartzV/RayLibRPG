using Raylib_cs;
using RayLibRPG.Clases.Config;
using RayLibRPG.Clases.Inputs;
using RayLibRPG.Clases.Letras;
using RayLibRPG.Clases.Trigo;
using RayLibRPG.Logic.Personaje;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayLibRPG.Clases.Escenario
{
    internal class EscenarioPruebaLetras : EscenarioEngine
    {
        public LectorInput Lector;
        public List<RichText> Richs = new();
        public List<Letra> Letras = new();

        public List<IActualizable> EscenarioU = new();

        public override void Initialize()
        {
            this.Capas.Add(new Capa("Main", 512, 288, Vector2.Zero));
            //this.Capas = ScreenManager.InsertarCapas(CapaFactory.CrearCapasPrueba());
            //this.InicializarSprite();
            //this.InicializarEscenario();
            //this.InicializarRich();
            this.InicializarLetra();
            //this.InicializarPolys();
            //this.InicializarCirculo();
            //this.InicializarPolysColoridos();
            //this.InicializarPersonajeHUD();
        }

        public override Int32 Draw(Single alfa)
        {
            Int32 counter = 0;
            //for (Int32 i = 0; i < Capas.Length; i++)
            //{
            //    counter += this.Capas[i].Renderizar(alfa, EngineManager.FramesTranscurridos);
            //}

            counter += base.Draw(alfa);

            if (EngineManager.FramesTranscurridos % 30 == 0)
            {
                //string titulo = $"Sir Bartz Engine | FPS: {Raylib.GetFPS()}/{ScreenManager.FPS} | Ticks: {EngineManager.TicksTranscurridos} | Frames: {EngineManager.FramesTranscurridos}";
                String titulo = $"Sir Bartz Engine | FPS: {Raylib.GetFPS()}/{ScreenManager.FPS} | Draws: {counter,2}";
                Raylib.SetWindowTitle(titulo);
            }
            return counter;
        }

        public void InicializarCirculo()
        {
            Circulo2D c = new(Vector2.Zero, 12, Color.Red);
            this.EscenarioU.Add(c);
            this.Capas[0].InsertarElemento(c);
        }

        public void InicializarPersonajeHUD()
        {
            Personaje p1 = new()
            {
                Id = "personaje::agrass_knights",
                Nombre = "Agrass Knights",
                PCMax = 1000,
                PCMaxActual = 1000,
                PCActual = 1000,
                ColorPrimario = new Color(128, 0, 192)
            };

            PersonajeHUD hud = new(p1, new Vector2(-200, -100));
            this.EscenarioU.Add(hud);
            this.Capas[0].InsertarElemento(hud);
        }

        public void InicializarPolysColoridos()
        {
            Poligono2DVertexColor pol1 =
                new Poligono2DVertexColor(
                    [
                        (new Vector2(10, 0), new Color(0, 192, 64)),
                        (new Vector2(0, 10), new Color(0, 128, 0)),
                        (new Vector2(110, 0), new Color(0, 128, 32)),
                        (new Vector2(100, 10), new Color(0, 64, 16))
                    ],
                    Vector2.Zero);

            Poligono2DWireVC pol2 =
                new Poligono2DWireVC(
                    [
                        (new Vector2(10, 0), Color.Red),
                        (new Vector2(0, 10), Color.DarkPurple),
                        (new Vector2(100, 10), Color.Gold),
                        (new Vector2(110, 0), Color.Green),
                    ],
                    new Vector2(0, 25),
                    true, false);

            this.EscenarioU.Add(pol1);
            this.EscenarioU.Add(pol2);
            this.Capas[0].InsertarElemento(pol1);
            this.Capas[0].InsertarElemento(pol2);
            //this.Lector = new LectorInputDebug<Poligono2DVertexColor>(pol1);
        }

        public void InicializarPolys()
        {
            Poligono2DPlano pol1 = new([new Vector2(10, 0), new Vector2(0, 10), new Vector2(110, 0), new Vector2(100, 10)], Vector2.Zero, new Color(255, 0, 0, 255));
            pol1.Rotacion = 90;
            this.EscenarioU.Add(pol1);
            this.Capas[0].InsertarElemento(pol1);
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

            RichText rich = new RichText(cadPrueba, defecto, [arcoiris], Vector2.Zero, Vector2.One, 0);
            //this.Lector = new LectorInputRichText(rich);

            this.Richs.Add(rich);
            this.EscenarioU.Add(rich);
            this.Capas[0].InsertarElemento(rich);
        }

        public void InicializarLetra()
        {
            Letra l = new Letra('A', Vector2.Zero, Vector2.One, Color.Red);

            this.Letras.Add(l);
            this.EscenarioU.Add(l);
            this.Capas[0].InsertarElemento(l);
        }

        public void InicializarSprite()
        {
            Sprite2D l = new Sprite2D(Texture2DManager.GetTexture("Letra"), new Rectangle(8, 0, 8, 8), Vector2.Zero, Vector2.One * 8, new Vector2(2, 1));
            l.Rotacion = 90F;

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
