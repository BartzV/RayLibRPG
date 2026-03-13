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

namespace RayLibRPG.Clases.Escenario;

public class EscenarioPruebaHUD : EscenarioEngine
{
    public Stack<LectorInput> input = new();
    public List<Personaje> personajes = new();

    public override void Initialize()
    {
        this.InicializarCapas();
        this.InicializarPersonajes();
        this.InicializarLetras();
        this.InicializarCapaHUD();
    }

    protected void InicializarCapas()
    {
        Capas.Add(new("Titulo", 512, 24, new Vector2(0, 0))
        {
            Fondo = Color.Maroon,
            EsRapido = false
        });
        Capas.Add(new("Main", 512, 288 - 24 * 4, new Vector2(0, 24))
        {
            Fondo = Color.RayWhite,
        });
        Capas.Add(new("HUD", 512, 24 * 3, new Vector2(0, 288 - 24 * 3))
        {
            Fondo = Color.Beige,
            EsRapido = false
        });
    }

    protected void InicializarPersonajes()
    {
        personajes.Add(new()
        {
            Nombre = "Agrass",
            ColorPrimario = new Color(192, 0, 128),
            PCActual = 1000,
            PCMax = 1000,
            PCMaxActual = 1000,
        });


    }

    protected void InicializarLetras()
    {
        RichText rich = new("Praderas", [Color.White], null, new Vector2(24, 12), new Vector2(2, 2), 1, null);
        this.Capas[0].InsertarElemento(rich);

        Letra a = new Letra('O', new Vector2(10, 10), Vector2.One);
        this.Capas[1].InsertarElemento(a);

        input.Push(new LectorInputDebug<Letra>(a));
    }

    protected void InicializarCapaHUD()
    {
        PersonajeHUD hud = new PersonajeHUD(this.personajes[0], new Vector2(16, 16));
        this.Capas[2].InsertarElemento(hud);
        Int32 w = 256, h = 24 * 3;

        (Vector2, Color)[] vecs =
        [
            (new Vector2(0, 0), new Color(0, 64, 192)),
            (new Vector2(0, h / 2), new Color(0, 16, 64)),
            (new Vector2(w / 2, 0), new Color(0, 96, 255)),
            (new Vector2(w / 2, h / 2), new Color(0, 48, 192)),
            (new Vector2(w, 0), new Color(0, 255, 192)),
            (new Vector2(w, h / 2), new Color(0, 192, 164)),
            // Segunda fila???
            (new Vector2(0, h / 2), new Color(0, 16, 64)),
            (new Vector2(0, h), new Color(192, 64, 0)),
            (new Vector2(w / 2, h / 2), new Color(164, 64, 104)),

        ];

        (Vector2, Color)[] arcoirisFan =
        [
            // Centro (El pivot)
            (new Vector2(w / 2f, h / 2f), Color.Green),    
            // Centro Arriba
            (new Vector2(w / 2f, 0f), Color.Yellow),
            // Parte Izquierda
            (new Vector2(0f, 0f), Color.Orange),
            (new Vector2(0f, h / 2f), Color.Yellow),
            (new Vector2(0f, h), Color.Green),
            // Centro Abajo
            (new Vector2(w / 2f, h), Color.SkyBlue),
            // Parte Derecha
            (new Vector2(w, h), Color.Blue),
            (new Vector2(w, h / 2f), Color.SkyBlue),
            (new Vector2(w, 0f), Color.Green),
            // Gran Cierre
            (new Vector2(w / 2f, 0f), Color.Yellow),
        ];


        Poligono2DVertexColor vhud = new(vecs, new Vector2(w / 4, h / 4), true);
        this.Capas[1].InsertarElemento(vhud);
        input.Push(new LectorInputDebug<Poligono2DVertexColor>(vhud));
    }

    public override void Update()
    {
        // 1. Base. Actualizo todo lo de las capas.
        base.Update();
        // 2. Actualizamos los contadores de las teclas
        InputConfig.Actualizar();
        // 3. El LectorInput ahora usa el nuevo método
        if (this.input.Count != 0)
        {
            this.input.Peek().Procesar();
        }
    }

    public override Int32 Draw(Single alfa)
    {
        Int32 counter = base.Draw(alfa);

        if (EngineManager.FramesTranscurridos % 30 == 0)
        {
            //string titulo = $"Sir Bartz Engine | FPS: {Raylib.GetFPS()}/{ScreenManager.FPS} | Ticks: {EngineManager.TicksTranscurridos} | Frames: {EngineManager.FramesTranscurridos}";
            String titulo = $"Sir Bartz Engine | FPS: {Raylib.GetFPS()}/{ScreenManager.FPS} | Draws: {counter,2}";
            Raylib.SetWindowTitle(titulo);
        }
        return counter;
    }
}
