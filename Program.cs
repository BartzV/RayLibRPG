using Raylib_cs;
using RayLibRPG.Clases;
using RayLibRPG.Clases.Config;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RaylibRPG;

public class Program
{
    public static Letra? letraPrueba;
    public static Sprite2D[] limites;
    public static Sprite2D[] pruebaCamara;
    public static Camara2D Camara;
    public static InputCamara ControlRemoto;
    public static List<IActualizable> Actualizables;

    public static void Main(string[] args)
    {
        Initialize();
        Double tiempoAcumulado = 0.0;
        while (!Raylib.WindowShouldClose())
        {
            // DeltaTime es el tiempo que pasó desde el frame anterior (ej: 0.016s)
            Double frameTime = Raylib.GetFrameTime();
            // Calculamos el alfa: qué porcentaje del camino al siguiente tick recorrimos
            // Esto es lo que usás para el LERP en el Draw
            Single alfa = (Single)(tiempoAcumulado * CONFIG.TPS);

            // Si el tiempo acumulado superó el "Tickrate", es hora de actualizar la lógica
            tiempoAcumulado += frameTime;
            while (tiempoAcumulado >= CONFIG.Tickrate)
            {
                Update(alfa);
                CONFIG.TicksTranscurridos++;
                tiempoAcumulado -= CONFIG.Tickrate;
            }

            Draw(alfa);
        }

        foreach ((_, Texture2D tex) in CONFIG.TexturasCargadas)
        {
            Raylib.UnloadTexture(tex);

        }
        foreach (IRenderizable renderizable in Camara.Absolutos)
        {
            if (renderizable is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        foreach (IRenderizable renderizable in Camara.Relativos)
        {
            if (renderizable is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        Raylib.CloseWindow();
    }

    public static void Initialize()
    {
        // Olvidate del Async, hacela simple y directa
        CONFIG.Inicializar();
        LetraManager.Inicializar();
        CONFIG.CambiarResolucion(512 * 2 + 16, 288 * 2);
        Texture2D textura;
        Camara = new Camara2D();

        CONFIG.TexturasCargadas.TryGetValue("Letra", out textura);

        limites = new Sprite2D[4];
        limites[0] = new Sprite2D(textura, (Rectangle)LetraManager.GetRectangle("~")!, new Rectangle(4, 4, 8, 8));
        limites[1] = new Sprite2D(textura, (Rectangle)LetraManager.GetRectangle("~")!, new Rectangle(4, CONFIG.HEIGHT - 4, 8, 8));
        limites[2] = new Sprite2D(textura, (Rectangle)LetraManager.GetRectangle("~")!, new Rectangle(CONFIG.WIDTH - 4, 4, 8, 8));
        limites[3] = new Sprite2D(textura, (Rectangle)LetraManager.GetRectangle("~")!, new Rectangle(CONFIG.WIDTH - 4, CONFIG.HEIGHT - 4, 8, 8));

        pruebaCamara = new Sprite2D[2];
        pruebaCamara[0] = new Sprite2D(textura, (Rectangle)LetraManager.GetRectangle("F")!, new Rectangle(150, 100, 32, 32));
        pruebaCamara[0].ZBuffer = 2;
        pruebaCamara[1] = new Sprite2D(textura, (Rectangle)LetraManager.GetRectangle("F")!, new Rectangle(150, 100, 32, 32));
        pruebaCamara[1].ZBuffer = 4;

        Camara.Relativos.Add(pruebaCamara[0]);
        Camara.Relativos.Add(pruebaCamara[1]);

        foreach (Sprite2D limite in limites)
        {
            Camara.Absolutos.Add(limite);
        }

        ControlRemoto = new InputCamara(Camara);

        Actualizables = new();
        Actualizables.AddRange(limites);
        Actualizables.AddRange(pruebaCamara);
    }

    public static void Update(Single alfa)
    {
        Actualizables.ForEach(a => a.Update());

        ControlRemoto.Update(alfa);
        pruebaCamara[0].Rotacion += 5.0f;
        pruebaCamara[1].Rotacion += 5.0f;
    }

    public static void Draw(Single alfa)
    {
        Raylib.BeginTextureMode(CONFIG.Lienzo);

        Raylib.ClearBackground(Color.SkyBlue);
        FrameDraw(alfa);
        Raylib.EndTextureMode();

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        Raylib.DrawTexturePro(
            CONFIG.Lienzo.Texture,
            new Rectangle(0, 0, CONFIG.WIDTH, -CONFIG.HEIGHT),
            CONFIG.LienzoDest,
            //new Rectangle(0, 0, CONFIG.PantallaX, CONFIG.PantallaY),
            new Vector2(0, 0),
            0.0f,
            Color.White
        );

        Raylib.EndDrawing();
        CONFIG.FramesTranscurridos++;
    }

    public static void FrameDraw(Single alfa)
    {
        // El 'alfa' sirve para predecir la posición y que se vea fluido
        // Ejemplo: posicionDibujado = posicionActual + (velocidad * alfa);
        //Raylib.DrawText($"Ticks: {CONFIG.TicksTranscurridos}", 10, 10, 20, Color.DarkGray);
        //Raylib.DrawText($"Frames: {CONFIG.FramesTranscurridos}", 10, 40, 20, Color.Maroon);
        //Raylib.DrawText($"Alfa (Interpolación): {alfa:F2}", 10, 70, 20, Color.Blue);
        
        Camara.DibujarRelativos(alfa);
        Camara.DibujarAbsolutos(alfa);
    }
}


/// <summary>
/// Esta clase sirve para manejar la cámara. No va a ser usado.
/// </summary>
public class InputCamara
{
    public Camara2D Camara;
    private Int64 _ultimoInput;
    private const Int64 DELAY = 4;
    public InputCamara(Camara2D camara)
    {
        this.Camara = camara;
        this._ultimoInput = -1;
    }
    // alfa sin usar
    public void Update(Single alfa)
    {
        if (this._ultimoInput + DELAY > CONFIG.TicksTranscurridos)
            return;
        if (Raylib.IsKeyDown(KeyboardKey.Up))
        {
            this.Camara.Desplazamiento.Y += 2;
        }
        if (Raylib.IsKeyDown(KeyboardKey.Down))
        {
            this.Camara.Desplazamiento.Y -= 2;
        }
        if (Raylib.IsKeyDown(KeyboardKey.Right))
        {
            this.Camara.Desplazamiento.X += 2;
        }
        if (Raylib.IsKeyDown(KeyboardKey.Left))
        {
            this.Camara.Desplazamiento.X -= 2;
        }
    }
}