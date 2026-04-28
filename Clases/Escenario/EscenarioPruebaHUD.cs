using Raylib_cs;
using RayLibRPG.Clases.Config;
using RayLibRPG.Clases.Inputs;
using RayLibRPG.Clases.Letras;
using RayLibRPG.Clases.Trigo;
using RayLibRPG.Clases.Trigo.Barras;
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
    public List<Letra> letras = new();

    public override void Initialize()
    {
        this.InicializarCapas();
        //this.InicializarBench();
        this.InicializarPersonajes();
        //this.InicializarLetras();
        //this.InicializarHUD();
        this.DibujarMierda();
    }

    protected void InicializarBench()
    {
        this.Capas.Add(new("Main", 512, 288, new Vector2(0, 0))
        {
            Fondo = Color.RayWhite,
        });

        for(var x = 0; x < 64; x++)
        {
            for(var y = 0; y < 36; y++)
            {
                Letra a = new Letra('+', new Vector2(x * 8 + 4, y * 8 + 4), Vector2.One)
                {
                    Tinte = new Color(Math.Clamp(512 - x * 8, 0, 255), Math.Clamp(x * 8, 0, 255), y * 4),
                    CapaPrioridad = 100F,
                };
                Letra b = new Letra('~', new Vector2(x * 8 + 4, y * 8 + 4), Vector2.One)
                {
                    Tinte = new Color(64, 64, 64),
                    CapaPrioridad = 105F,
                    Rotacion = 45F
                };
                Letra c = new Letra('\uFF00', new Vector2(x * 8 + 4, y * 8 + 4), Vector2.One)
                {
                    Tinte = new Color(64, 64, 64),
                    CapaPrioridad = 105F,
                    Rotacion = 45F
                };

                letras.Add(a);
                this.Capas[0].InsertarElemento(a);

                letras.Add(b);
                this.Capas[0].InsertarElemento(b);

                letras.Add(c);
                this.Capas[0].InsertarElemento(c);
            }
        }
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
            Raza = Logic.Reglas.Tags.RazaTag.Humano,
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

    }

    protected void InicializarHUD()
    {
        Int32 w = 512, h = 24 * 3;
        (Vector2, Color)[] vecs =
        [
            (new Vector2(0, 0),     new Color(000, 192, 064)),
            (new Vector2(0, h),     new Color(000, 128, 016)),
            (new Vector2(w / 2, 0), new Color(000, 128, 016)),
            (new Vector2(w / 2, h), new Color(000, 164, 032)),
            (new Vector2(w, 0),     new Color(000, 164, 032)),
            (new Vector2(w, h),     new Color(000, 096, 000)),
        ];

        Poligono2DVertexColor vhud = new(vecs, new Vector2(0, 0), true);
        this.Capas[2].InsertarElemento(vhud);

        PersonajeHUD pjHUD = new(personajes[0], new Vector2(24, 16));
        this.Capas[2].InsertarElemento(pjHUD);
    }

    protected void DibujarMierda()
    {
        PersonajeHUD hud = new PersonajeHUD(this.personajes[0], new Vector2(16, 16));
        this.Capas[2].InsertarElemento(hud);
        Int32 w = 256, h = 24 * 2;

        (Vector2, Color)[] vecs =
        [
            (new Vector2(0, 0), new Color(255, 0, 0)),
            (new Vector2(0, h), new Color(192, 64, 0)),
            (new Vector2(w / 3, 0), new Color(192, 64, 0)),
            (new Vector2(w / 3, h), new Color(255, 255, 0)),
            (new Vector2(w * 2 / 3, 0), new Color(255, 255, 0)),
            (new Vector2(w * 2 / 3, h), new Color(0, 192, 32)),
            (new Vector2(w, 0), new Color(0, 192, 32)),
            (new Vector2(w, h), new Color(0, 128, 192)),

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
        LectorMovimiento<Poligono2DVertexColor> input = new(vhud);
        this.input.Push(input);
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
            String titulo = $"Sir Bartz Engine | FPS: {Raylib.GetFPS()}/{ScreenManager.FPS} | Draws: {counter,2}";
            Raylib.SetWindowTitle(titulo);
        }
        return counter;
    }
}
