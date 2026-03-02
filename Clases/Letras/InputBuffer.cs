using Raylib_cs;

namespace RayLibRPG.Clases.Letras;

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

    public static Boolean FuePresionado(KeyboardKey tecla)
    {
        return TeclasPresionadas.Contains(tecla);
    }

    public static void Limpiar()
    {

        TeclasPresionadas.Clear();
    }
}
