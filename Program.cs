using Raylib_cs;
using RayLibRPG.Clases;
using RayLibRPG.Clases.Config;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RaylibRPG;

public class Program
{
    public static Sprite2D ejemplo;
    public static void Main(string[] args)
    {
        Initialize();
        Double tiempoAcumulado = 0.0;

        while (!Raylib.WindowShouldClose())
        {
            Double frameTime = Raylib.GetFrameTime();
            tiempoAcumulado += frameTime; // 1. Primero sumamos TODO el tiempo nuevo

            // 2. Ejecutamos todos los Updates que quepan en ese tiempo
            while (tiempoAcumulado >= ConfigManager.TICKRATE)
            {
                Update();
                EngineManager.TicksTranscurridos++;
                tiempoAcumulado -= ConfigManager.TICKRATE;
            }

            Single alfa = (Single)(tiempoAcumulado / ConfigManager.TICKRATE);

            Draw(alfa);
        }

        Texture2DManager.UnloadAll();
        Raylib.CloseWindow();
    }

    public static void Initialize()
    {
        EngineManager.Inicializar();       // Importante. Maneja los ticks, la resolución y mucho más.
        ScreenManager.CambiarResolucion(512 * 2 + 16, 288 * 2);

        ejemplo = new Sprite2D(Texture2DManager.GetTexture("Letra"), new Rectangle(0, 0, 8, 8), new Rectangle(100, 100, 32, 32));
    }

    public static void Update()
    {

    }

    
    public static void Draw(Single alfa)
    {
        // Lienzo Veloz
        Raylib.BeginTextureMode(ScreenManager.Lienzo);
        Raylib.ClearBackground(Color.SkyBlue);
        FrameDraw(alfa);
        Raylib.EndTextureMode();

        // Main
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        Raylib.DrawTexturePro(
            ScreenManager.Lienzo.Texture,
            new Rectangle(0, 0, ConfigManager.WIDTH, -ConfigManager.HEIGHT),
            ScreenManager.LienzoDest,
            new Vector2(0, 0),
            0.0f,
            Color.White
        );

        Raylib.EndDrawing();
        EngineManager.FramesTranscurridos++;
    }

    // Esto quiero que esté telemetrizado en el título de la ventana.
    public static void FrameDraw(Single alfa)
    {
        //Raylib.DrawText($"Ticks: {CONFIG.TicksTranscurridos}", 10, 10, 20, Color.DarkGray);
        //Raylib.DrawText($"Frames: {CONFIG.FramesTranscurridos}", 10, 40, 20, Color.Maroon);
        //Raylib.DrawText($"Alfa (Interpolación): {alfa:F2}", 10, 70, 20, Color.Blue);

        ejemplo.Draw(alfa);

    }
}


/// <summary>
/// Esta clase sirve para manejar la cámara. No va a ser usado.
/// </summary>
//public class InputCamara
//{
//    public Camara2D Camara;
//    private Int64 _ultimoInput;
//    private const Int64 DELAY = 4;
//    public InputCamara(Camara2D camara)
//    {
//        this.Camara = camara;
//        this._ultimoInput = -1;
//    }
//    // alfa sin usar
//    public void Update(Single alfa)
//    {
//        if (this._ultimoInput + DELAY > CONFIG.TicksTranscurridos)
//            return;
//        if (Raylib.IsKeyDown(KeyboardKey.Up))
//        {
//            this.Camara.Desplazamiento.Y += 2;
//            _ultimoInput = CONFIG.TicksTranscurridos;
//        }
//        if (Raylib.IsKeyDown(KeyboardKey.Down))
//        {
//            this.Camara.Desplazamiento.Y -= 2;
//            _ultimoInput = CONFIG.TicksTranscurridos;
//        }
//        if (Raylib.IsKeyDown(KeyboardKey.Right))
//        {
//            this.Camara.Desplazamiento.X += 2;
//            _ultimoInput = CONFIG.TicksTranscurridos;
//        }
//        if (Raylib.IsKeyDown(KeyboardKey.Left))
//        {
//            this.Camara.Desplazamiento.X -= 2;
//            _ultimoInput = CONFIG.TicksTranscurridos;
//        }
//    }
//}
