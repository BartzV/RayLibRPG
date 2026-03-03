using Raylib_cs;
using RayLibRPG.Clases.Config;
using RayLibRPG.Clases.Letras;
using System.Numerics;

namespace RayLibRPG.Clases;

public class Capa : IDisposable
{
    // Forma de identificarlo
    public String Nombre;
    private bool _sucio = false; // Se movió algo para el frente?
    public Boolean DebeReordenar { get => this._sucio; }

    private Boolean _disposed = false;
    // Profundizar...
    public Boolean Activa = true;              // Si la capa está activa, se dibuja. Sino, no se dibuja ni se actualiza. Útil para cosas como el menú de pausa.
    // Para cada capa, tenemos una textura interna donde se dibuja todo. Al final de cada Draw, se pega esa textura en el Main.
    public RenderTexture2D TexturaInterna;
    public Vector2 Posicion;                // Guardar para cuando se cambia la resolución.
    public Rectangle DestinoEnPantalla;     // Dónde se pega en el Main (ej: el 1/3 de abajo)
    public Int32 Ancho;
    public Int32 Alto;

    public List<IRenderizable> Elementos;
    // Coordenadas de Desplazamiento y ZBuffer para esta capa
    public Vector2 DesplazamientoCamara;    // El desplazamiento de todo lo que se renderice acá.
    public Single FactorProfundidad;        // El ZBuffer global de esta capa.
    // Extra
    public Boolean EsRapido = true;         // Si es rápido, se renderiza a los frames actuales. Sino, va a 30 FPS.
    public Color Tinte;                     // Tinte para efectos como luz.
    public Color Fondo;                     // Color para debuggear, no se va a usar en el producto final.
    public Int64 FramesTranscurridos = 0;

    public Capa(String nombre, Int32 ancho, Int32 alto, Vector2 posicion)
    {
        this.Nombre = nombre;
        // Datos útiles que no deben perderse.
        this.Posicion = posicion;
        this.Ancho = ancho;
        this.Alto = alto;
        // Datos de desplazamiento
        this.DesplazamientoCamara = Vector2.Zero;
        this.FactorProfundidad = 1f;

        // Crear la textura interna. Es donde se va a dibujar TODO lo de esta capa. Al final de cada Draw, se pega esa textura en el Main.
        this.TexturaInterna = Raylib.LoadRenderTexture(ancho, alto);
        Raylib.SetTextureFilter(this.TexturaInterna.Texture, ScreenManager.Filtro);

        // El pad de cada lado es la diferencia entre el tamaño de la ventana y el tamaño de la resolución interna, dividido por 2.
        Rectangle destino = new Rectangle(
            (posicion.X * ScreenManager.TamPixel) + ScreenManager.PadX,
            (posicion.Y * ScreenManager.TamPixel) + ScreenManager.PadY,
            ancho * ScreenManager.TamPixel,
            alto * ScreenManager.TamPixel);
        this.DestinoEnPantalla = destino;

        this.Elementos = new List<IRenderizable>();
        this.Fondo = Color.White;
        this.Tinte = Color.White;
    }

    public void CambiarResolucion(Int32 ancho, Int32 alto)
    {
        Raylib.UnloadRenderTexture(this.TexturaInterna);
        this.TexturaInterna = Raylib.LoadRenderTexture(ancho, alto);
        Raylib.SetTextureFilter(this.TexturaInterna.Texture, ScreenManager.Filtro);
        // Reorganizando valores.
        this.Ancho = ancho;
        this.Alto = alto;

        this.DestinoEnPantalla = new Rectangle(
            (this.Posicion.X * ScreenManager.TamPixel) + ScreenManager.PadX,
            (this.Posicion.Y * ScreenManager.TamPixel) + ScreenManager.PadY,
            ancho * ScreenManager.TamPixel,
            alto * ScreenManager.TamPixel);
    }
    public void Reposicionar(Vector2 nuevaPosicion)
    {
        this.Posicion = nuevaPosicion;
        CambiarResolucion(this.Ancho, this.Alto);
    }
    public Int32 Renderizar(Single alfa, Int64 framesTotales)
    {
        if (!this.Activa) return 0;
        Int32 counter = 0;
        Raylib.BeginTextureMode(this.TexturaInterna);
        //Raylib.ClearBackground(this.Fondo);

        // Definimos el área visible de la capa (su tamaño interno)
        Rectangle areaVisible = new Rectangle(0, 0, this.Ancho, this.Alto);

        for (Int32 i = 0; i < this.Elementos.Count; i++)
        {
            counter += this.Elementos[i].Draw(alfa, this.DesplazamientoCamara, this.FactorProfundidad, areaVisible);
        }
        this.FramesTranscurridos++;
        Raylib.EndTextureMode();

        if (_sucio)
        {
            this.Elementos.Sort((x, y) => y.Prioridad.CompareTo(x.Prioridad));
            _sucio = false;
        }

        return counter;
    }

    public void InsertarElemento(IRenderizable elemento)
    {
        this.Elementos.Add(elemento);
        this._sucio = true;
    }
    public void InsertarElementos(IRenderizable[] elemento)
    {
        this.Elementos.AddRange(elemento);
    }

    public void DebugCorners()
    {
        Sprite2D[] sprites = new Sprite2D[4];
        sprites[0] = new Sprite2D(Texture2DManager.GetTexture("Letra"),
            (Rectangle)LetraManager.GetRectangle('~')!,
            new Rectangle(this.Posicion.X + 4, this.Posicion.Y + 4, 8, 8));
        sprites[1] = new Sprite2D(Texture2DManager.GetTexture("Letra"),
            (Rectangle)LetraManager.GetRectangle('~')!,
            new Rectangle(this.Ancho - 4, this.Posicion.Y + 4, 8, 8));
        sprites[2] = new Sprite2D(Texture2DManager.GetTexture("Letra"),
            (Rectangle)LetraManager.GetRectangle('~')!,
            new Rectangle(this.Posicion.X + 4, this.Alto - 4, 8, 8));
        sprites[3] = new Sprite2D(Texture2DManager.GetTexture("Letra"),
            (Rectangle)LetraManager.GetRectangle('~')!,
            new Rectangle(this.Ancho - 4, this.Alto - 4, 8, 8));
        Elementos.AddRange(sprites);
    }

    public void Dispose()
    {
        if (!this._disposed)
        {
            Raylib.UnloadRenderTexture(this.TexturaInterna);
            for (Int32 i = 0; i < Elementos.Count; i++)
            {
                if (Elementos[i] is IDisposable d)
                {
                    d.Dispose();
                }
            }
            this._disposed = true;
        }
    }

    
}

public static class CapaFactory
{
    public static Capa[] CrearCapasBatalla()
    {
        Capa titulo = new Capa("TITULO", ConfigManager.WIDTH, 24, Vector2.Zero);
        titulo.EsRapido = false;
        titulo.Fondo = new Color(204, 153, 255); // Rosita

        Capa batalla = new Capa("BATALLA", ConfigManager.WIDTH, 288 - 24 - 64, new Vector2(0, 24));
        batalla.Fondo = new Color(102, 204, 255); // Celeste
        batalla.DesplazamientoCamara = new Vector2(ConfigManager.WIDTH / 2, ConfigManager.HEIGHT / 2 - 24);

        Capa hud = new Capa("HUD", ConfigManager.WIDTH, 64, new Vector2(0, 288 - 64));
        hud.EsRapido = false;
        hud.Fondo = new Color(255, 153, 102); // Naranja

        return new Capa[] { titulo, batalla, hud };
    }

    public static Capa[] CrearCapasSupramundo()
    {
        Capa dinamico = new Capa("DINAMICO", ConfigManager.WIDTH, ConfigManager.HEIGHT, Vector2.Zero);
        dinamico.Fondo = new Color(102, 204, 255); // Celeste
        dinamico.DesplazamientoCamara = new Vector2(dinamico.Ancho / 2, dinamico.Alto / 2);

        return new Capa[] { dinamico };
    }

    public static Capa[] CrearCapasPrueba()
    {
        Capa principal = new Capa("Main", ConfigManager.WIDTH, ConfigManager.HEIGHT, Vector2.Zero);
        principal.Fondo = Color.SkyBlue;
        principal.DesplazamientoCamara = new Vector2(ConfigManager.WIDTH / 2, ConfigManager.HEIGHT / 2);
        principal.FactorProfundidad = 1F;
        //secundario.Fondo = new Color(0, 0, 0, 0);

        return new Capa[] { principal };
    }
}