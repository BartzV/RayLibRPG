using Raylib_cs;
using RayLibRPG.Clases.Letras;
using System.Numerics;

namespace RayLibRPG.Clases.Config;

/// <summary>
/// Usada para fijar los controles del juego. No, no va a haber multijugador ni nada.
/// </summary>
internal static class InputConfig
{
    // Cursores:
    public static Boolean Arriba => Raylib.IsKeyDown(KeyboardKey.Up);
    public static Boolean Abajo => Raylib.IsKeyDown(KeyboardKey.Down);
    public static Boolean Izquierda => Raylib.IsKeyDown(KeyboardKey.Left);
    public static Boolean Derecha => Raylib.IsKeyDown(KeyboardKey.Right);

    // Comandos
    public static Boolean Aceptar => Raylib.IsKeyPressed(KeyboardKey.S);
    public static Boolean Cancelar => Raylib.IsKeyPressed(KeyboardKey.A);
    public static Boolean Start => Raylib.IsKeyPressed(KeyboardKey.D);

}

public static class InputBuffer
{
    public static HashSet<KeyboardKey> TeclasPresionadas = new();

    public static void Capturar()
    {
        // Raylib.GetKeyPressed devuelve la tecla que se apretó en este frame de renderizado
        int tecla;
        while ((tecla = Raylib.GetKeyPressed()) != 0)
        {
            TeclasPresionadas.Add((KeyboardKey)tecla);
        }
    }

    public static void Limpiar()
    {
        TeclasPresionadas.Clear();
    }
}

/// <summary>
/// Ejemplo burdo
/// </summary>
public abstract class LectorInput
{
    public Int64 UltimoInput = -1L;
    
    public abstract Boolean Procesar();
}


public class LectorInputDebug : LectorInput
{
    public const Int64 Delay = 30L;
    public IDesplazable Elemento;

    public LectorInputDebug(IDesplazable elemento)
    {
        this.Elemento = elemento;
    }
    // Para evitar alloc
    private Vector2 aux;
    public override Boolean Procesar()
    {
        if(InputConfig.Izquierda && !InputConfig.Derecha)
        {
            this.UltimoInput = EngineManager.TicksTranscurridos;
            aux.X = -1;
            aux.Y = 0;
            this.Elemento.Mover(aux);
            // Hacer algo...
            return true;
        }
        if (InputConfig.Aceptar && EngineManager.TicksTranscurridos > (this.UltimoInput + Delay))
        {
            
        }

        return false;
    }
}

public class LectorInputLetra : LectorInput
{
    public const Int64 Delay = 30L;
    public static Char[] Vocales = ['A', 'E', 'I', 'O', 'U'];

    public Letra Elemento;
    public Int32 VocalActual = 0;

    public LectorInputLetra(Letra elemento)
    {
        this.Elemento = elemento;
        this.Elemento.CambiarLetra(Vocales[0]);
    }

    private Vector2 aux;
    public override Boolean Procesar()
    {
        if (InputConfig.Izquierda && !InputConfig.Derecha)
        {
            this.UltimoInput = EngineManager.TicksTranscurridos;
            aux.X = -1;
            aux.Y = 0;
            this.Elemento.Mover(aux);
        }
        if (InputConfig.Derecha && !InputConfig.Izquierda)
        {
            this.UltimoInput = EngineManager.TicksTranscurridos;
            aux.X = 1;
            aux.Y = 0;
            this.Elemento.Mover(aux);
        }
        if (InputConfig.Aceptar && EngineManager.TicksTranscurridos > (this.UltimoInput + Delay))
        {
            this.UltimoInput = EngineManager.TicksTranscurridos;
            this.VocalActual = (this.VocalActual + 1) % Vocales.Length;
            this.Elemento.CambiarLetra(Vocales[VocalActual]);
        }
        if(EngineManager.TicksTranscurridos <= (this.UltimoInput + Delay))
        {
            this.Elemento.Tinte = Color.Gray;
        }
        else
        {
            this.Elemento.Tinte = Color.Red;

        }

        return true;
    }
}