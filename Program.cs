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
    public static InputSprite ControlSprite;
    public static List<IActualizable> Actualizables;

    public static void Main(string[] args)
    {
        Initialize();
        Double tiempoAcumulado = 0.0;
        while (!Raylib.WindowShouldClose())
        {
            Double frameTime = Raylib.GetFrameTime();
            tiempoAcumulado += frameTime; // 1. Primero sumamos TODO el tiempo nuevo

            // 2. Ejecutamos todos los Updates que quepan en ese tiempo
            while (tiempoAcumulado >= CONFIG.Tickrate)
            {
                Update(1.0f); // El alfa acá no importa mucho, es lógica pura
                CONFIG.TicksTranscurridos++;
                tiempoAcumulado -= CONFIG.Tickrate;
            }

            // 3. RECIÉN ACÁ calculamos el Alfa con lo que sobró
            // tiempoAcumulado es el "resto" de la división, entre 0 y Tickrate
            Single alfa = (Single)(tiempoAcumulado / CONFIG.Tickrate);

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

        pruebaCamara = new Sprite2D[1];
        pruebaCamara[0] = new Sprite2D(textura, (Rectangle)LetraManager.GetRectangle("~")!, new Rectangle(120, 200, 64, 64));
        pruebaCamara[0].ZBuffer = 2;

        Camara.Relativos.AddRange(pruebaCamara);

        foreach (Sprite2D limite in limites)
        {
            Camara.Absolutos.Add(limite);
        }

        //ControlRemoto = new InputCamara(Camara);
        ControlSprite = new InputSprite(pruebaCamara[0]);

        Actualizables = new();
        Actualizables.AddRange(limites);
        Actualizables.AddRange(pruebaCamara);
    }

    public static void Update(Single alfa)
    {
        Actualizables.ForEach(a => a.Update());
        pruebaCamara[0].Rotacion += 5.0f; 
        ControlSprite.Update(alfa);
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

public class InputSprite
{
    public Sprite2D Sprite;
    private Int64 _ultimoInput;
    private const Int64 DELAY = 4;
    public InputSprite(Sprite2D sprite)
    {
        this.Sprite = sprite;
        this._ultimoInput = -1;
    }
    // alfa sin usar
    public void Update(Single alfa)
    {
        if (this._ultimoInput + DELAY > CONFIG.TicksTranscurridos)
            return;
        if (Raylib.IsKeyDown(KeyboardKey.Up))
        {
            this.Sprite.MoverY(-2f);
        }
        if (Raylib.IsKeyDown(KeyboardKey.Down))
        {
            this.Sprite.MoverY(2f);
        }
        if (Raylib.IsKeyDown(KeyboardKey.Right))
        {
            this.Sprite.MoverX(2f);
        }
        if (Raylib.IsKeyDown(KeyboardKey.Left))
        {
            this.Sprite.MoverX(-2f);
        }
    }

}