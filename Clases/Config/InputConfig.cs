using Raylib_cs;

namespace RayLibRPG.Clases.Config;

/// <summary>
/// Usada para el input del juego. No, no va a haber multijugador ni nada, que se pasen el teclado o mouse.
/// </summary>
internal static class InputConfig
{
    // A = Aceptar
    public static KeyboardKey A = KeyboardKey.S;
    // B = Cancelar
    public static KeyboardKey B = KeyboardKey.A;
    // Cursores:
    public static KeyboardKey Arriba = KeyboardKey.Up;
    public static KeyboardKey Abajo = KeyboardKey.Down;
    public static KeyboardKey Izquierda = KeyboardKey.Left;
    public static KeyboardKey Derecha = KeyboardKey.Right;

    // Aquí van métodos y propiedades para manejar el input.
    // Por ejemplo:
    public static Boolean TeclaPresionada(KeyboardKey tecla)
    {
        return Raylib.IsKeyPressed(tecla);
    }
    public static Boolean TeclaSostenida(KeyboardKey tecla)
    {
        return Raylib.IsKeyDown(tecla);
    }
}
