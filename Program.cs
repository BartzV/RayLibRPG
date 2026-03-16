using Raylib_cs;
using RayLibRPG.Clases.Config;
using RayLibRPG.Clases.Escenario;
using RayLibRPG.Clases.Letras;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RayLibRPG;

public class Program
{
    public static EscenarioEngine EscenarioActual = null!;

    public static void Main(string[] args)
    {
        Initialize();
        Double tiempoAcumulado = 0.0;
        Double tiempoAnterior = Raylib.GetTime(); // Tiempo absoluto desde que arrancó el .exe

        while (!Raylib.WindowShouldClose())
        {
            Double tiempoActual = Raylib.GetTime();
            Double frameTime = tiempoActual - tiempoAnterior;
            tiempoAnterior = tiempoActual;

            if (frameTime > 0.25) frameTime = 0.25;

            tiempoAcumulado += frameTime;

            while (tiempoAcumulado >= ConfigManager.TICKRATE)
            {
                Update();

                EngineManager.TicksTranscurridos++;
                tiempoAcumulado -= ConfigManager.TICKRATE;
            }

            Single alfa = (Single)(tiempoAcumulado / ConfigManager.TICKRATE);
            Draw(alfa);
            EngineManager.FramesTranscurridos++;
        }

        ScreenManager.CerrarPantalla();
        Texture2DManager.UnloadAll();
        Raylib.CloseWindow();
    }

    public static void Initialize()
    {
        EngineManager.Inicializar();       // Importante. Maneja los ticks, la resolución y mucho más.
        LetraManager.Inicializar();        // Importante. Maneja las letras.

        ScreenManager.SetFPS(FPS_Options.FPS_120);
        //ScreenManager.LimpiarCapas();

        EscenarioActual = new EscenarioPruebaHUD();
        EscenarioActual.Initialize();
    }

    public static void Update()
    {
        EscenarioActual.Update();
    }


    public static void Draw(Single alfa)
    {
        EscenarioActual.Draw(alfa);

        //ScreenManager.DibujarTodo(alfa, EngineManager.FramesTranscurridos);
        
        //if (EngineManager.FramesTranscurridos % 30 == 0)
        //{
        //    string titulo = $"Sir Bartz Engine | FPS: {Raylib.GetFPS()}/{ScreenManager.FPS} | Ticks: {EngineManager.TicksTranscurridos} | Frames: {EngineManager.FramesTranscurridos}";
        //    Raylib.SetWindowTitle(titulo);
        //}
    }

}
