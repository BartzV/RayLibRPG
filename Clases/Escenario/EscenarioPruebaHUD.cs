using Raylib_cs;
using RayLibRPG.Clases.Config;
using RayLibRPG.Clases.Inputs;
using RayLibRPG.Clases.Letras;
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
        //this.InicializarLetras();
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
            Fondo = Color.SkyBlue,
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

        Letra a = new Letra('A', new Vector2(10, 10), Vector2.One);
        this.Capas[1].InsertarElemento(a);

        input.Push(new LectorInputDebug<Letra>(a));
    }

    protected void InicializarCapaHUD()
    {
        PersonajeHUD hud = new PersonajeHUD(this.personajes[0], new Vector2(16, 16));
        this.Capas[2].InsertarElemento(hud);

        (Vector2, Color)[] vecs =
            [
                (new Vector2(0, 0), new Color(0, 64, 192)),
                (new Vector2(0, 24 * 3), new Color(0, 16, 64)),
                (new Vector2(512, 0), new Color(0, 96, 255)),
                (new Vector2(512, 24 * 3), new Color(0, 48, 192))
            ];

        Poligono2DVertexColor vhud = new(vecs, Vector2.Zero)
        {
            CapaPrioridad = 100
        };
        this.Capas[2].InsertarElemento(vhud);
        input.Push(new LectorInputDebug<Poligono2DVertexColor>(vhud));
    }

    public override void Update()
    {
        // 1. Base. Actualizo todo lo de las capas.
        base.Update();
        // 2. Actualizamos los contadores de las teclas
        InputConfig.Actualizar();
        // 3. El LectorInput ahora usa el nuevo método
        this.input.Peek()?.Procesar();
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
