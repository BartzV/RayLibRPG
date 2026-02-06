using Raylib_cs;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace RayLibRPG.Clases;

//public class Letra : IDisposable, IRenderizable
//{
//    private Boolean disposed = false;
//    public Sprite2D Sprite;
//    public String Caracter;
//    private Dictionary<String, Rectangle>.Enumerator enumerator;

//    public Letra(Sprite2D sprite, String caracter)
//    {
//        this.Sprite = sprite;
//        this.Caracter = caracter;
//        this.enumerator = LetraManager.Diccionario.GetEnumerator();

//        this.Sprite.Fuente = LetraManager.GetRectangle(caracter) ?? LetraManager.GetEspacio();
//    }

//    public void Siguiente()
//    {
//        if (!this.enumerator.MoveNext() || this.enumerator.Current.Key is null)
//        {
//            this.enumerator.Dispose(); // Buena práctica limpiar el anterior
//            this.enumerator = LetraManager.Diccionario.GetEnumerator();
//            this.enumerator.MoveNext(); // CRUCIAL: Movete al primer elemento!
//        }
//        this.Caracter = this.enumerator.Current.Key;
//        this.Sprite.Fuente = this.enumerator.Current.Value;
//    }

//    public void Draw(Single alfa)
//    {
//        this.Sprite.Draw(alfa);
//    }

//    public void Draw(Single alfa, Vector2 desp, Single zbuf)
//    {
//        this.Sprite.Draw(alfa, desp, zbuf);
//    }

//    // Implementación del patrón Dispose
//    public void Dispose()
//    {
//        this.Dispose(true);
//        GC.SuppressFinalize(this); // Le decimos al GC que no llame al finalizador
//    }

//    protected virtual void Dispose(Boolean disposing)
//    {
//        if (!this.disposed)
//        {
//            if (disposing)
//            {
//                // Liberar recursos administrados
//                this.enumerator.Dispose();
//            }
//            // Acá irían recursos no administrados si tuvieras (handles, etc.)
//            this.disposed = true;
//        }
//    }
//}

public static class LetraManager
{
    private static Boolean _inicializado = false;
    public static Rectangle? GetRectangle(String s)
    {
        if (!_inicializado)
            throw new InvalidOperationException("Inicializalo!!!");
        if (Diccionario.TryGetValue(s, out Rectangle rect))
        {
            return rect;
        }
        else
        {
            return null;
        }
    }
    public static Rectangle GetEspacio()
    {
        if (!_inicializado)
            throw new InvalidOperationException("Inicializalo!!!");
        return Diccionario[" "]!;
    }

    // Por qué un String? Porque así podemos usar más fácilmente símbolos y no solo letras.
    private static Dictionary<String, Rectangle> Diccionario = new()
    {
        // Agregar el resto de letras y símbolos según la fuente.
    };

    public static void Inicializar()
    {
        // Letras de la A a la Z
        for (Int32 i = (Int32)'A'; i < (Int32)'Z' + 1; i++)
        {
            String letra = ((Char)i).ToString();
            Int32 index = i - (Int32)'A';
            Diccionario.Add(letra, new Rectangle(index * 8, 0, 8, 8));
        }
        // Letras de la 'a' a la 'z'
        for (Int32 i = (Int32)'a'; i < (Int32)'z' + 1; i++)
        {
            String letra = ((Char)i).ToString();
            Int32 index = i - (Int32)'a' + 26;
            Diccionario.Add(letra, new Rectangle(index * 8, 8, 8, 8));
        }

        Diccionario.Add("~", new Rectangle(0, 16, 8, 8));   // Cuadrado horrible que uso para debug
        Diccionario.Add(" ", new Rectangle(0, 0, 0, 0));     // Espacio
        _inicializado = true;
    }
}