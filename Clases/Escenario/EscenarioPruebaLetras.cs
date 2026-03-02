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
        public List<RichText> Richs = new();

        public List<IRenderizable> EscenarioS = new();
        public List<IActualizable> EscenarioU = new();

        public override void Initialize()
        {
            this.Capas = ScreenManager.InsertarCapas(CapaFactory.CrearCapasPrueba());
            this.InicializarEscenario();
            //Color[] arcoiris = [Color.Red, Color.Orange, Color.Gold, Color.Lime, Color.Green, Color.SkyBlue, Color.Blue, Color.Magenta];
            //Color[] defecto = [Color.White, new Color(192, 64, 0), new Color(0, 192, 128), new Color(64, 0, 200)];

            //RichText rich = new RichText(cadPrueba, defecto, [arcoiris], Vector2.Zero, Vector2.One);
            //rich.VelTexto = 2;

            //this.Lector = new LectorInputRichText(rich);

            //this.Richs.Add(rich);
            //this.Capas[0].InsertarElemento(rich);
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

        public void InicializarEscenario()
        {
            Texture2D textura = Texture2DManager.LoadTexture(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigManager.RUTA_BG_BOSQUE), "BGBosque");
            Sprite2D montaña1 = 
                new Sprite2D(textura, 
                    new Rectangle(0, 16, 16 * 6, 16 * 3), // Fuente
                    new Rectangle(-ConfigManager.WIDTH / 2, 0, 16 * 6, 16 * 3)); // Destino
            MultiSprite2D multiMontaña1 = new(montaña1, 4);

            this.EscenarioU.Add(montaña1);
            this.Capas[0].InsertarElemento(montaña1);
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
            "{c1}Albion Online es un rpg sandbox en el que{s}" +
            "{c1}escribes tu propia historia{c0}, en vez de{s}" +
            "{c1}seguir un camino prefijado.{s}" +
            "{s}" +
            "{e1}¡Explora un amplio mundo abierto!{e0}{s}" +
            "{s}" +
            "{c2}Hay cinco biomas únicos.{c0} Todo lo que{s}" +
            "{c2}haces tiene su repercusión en el mundo,{s}" +
            "{c0}con la economía orientada al jugador de{s}" +
            "{p1}Albion.{p0}{s}" +
            "{s}" +
            "{e2}¡Tú eres lo que llevas puesto!{e0}{s}" +
            "{s}" +
            "{c3}Si eres lo que llevas puesto, entonces{s}" +
            "{c3}eres lo que haces.{c0} ¡Y lo que haces{s}" +
            "{e1}ES JUGAR ALBION ONLINE!{e0}";
    }
}
