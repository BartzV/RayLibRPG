namespace RayLibRPG.Clases.Config;

internal static class ConfigManager
{
    // Constantes. Resolución "interna" del juego.
    public const Int32 WIDTH = 512;
    public const Int32 HEIGHT = 288;
    // Constantes de lógica.
    public const Int32 TPS = 60; // Ticks per second, veces que Update se llama por segundo.
    public const Double TICKRATE = 1.0 / TPS;
    // Rutas de archivos. Agregar luego.
    public const String RUTA_LETRAS = @"Assets/FuentesRES.png";
    public const String RUTA_BG_BOSQUE = @"Assets/BackGroundBosque.PNG";

    public static DebugMode DEBUG = DebugMode.None;
}

[Flags]
public enum DebugMode
{
    None = 0,
    Centers = 1,

}