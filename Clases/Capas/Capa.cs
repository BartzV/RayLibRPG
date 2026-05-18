using Raylib_cs;
using RayLibRPG.Clases.Config;
using RayLibRPG.Clases.Letras;
using System.Numerics;

namespace RayLibRPG.Clases.Capas;

public class Capa : IDisposable
{
    // Forma de identificarlo
    public String Nombre;
    private Boolean _sucio = false;         // Se movió algo para el frente?
    public Boolean DebeReordenar { get => this._sucio; set => this._sucio = value;}

    private Boolean _disposed = false;
    // Profundizar...
    public Boolean Activa = true;           // Si la capa está activa, se dibuja. Sino, no se dibuja ni se actualiza. Útil para cosas como el menú de pausa.
    // Para cada capa, tenemos una textura interna donde se dibuja todo. Al final de cada Draw, se pega esa textura en el Main.
    public RenderTexture2D TexturaInterna;
    public Vector2 Posicion;                // Guardar para cuando se cambia la resolución.
    public Rectangle DestinoEnPantalla;     // Dónde se pega en el Main (ej: el 1/3 de abajo)
    public Int32 Ancho;
    public Int32 Alto;

    public List<IEntidad> Elementos;

    // Coordenadas de Desplazamiento y ZBuffer para esta capa
    public Vector2 DesplazamientoCamara;    // El desplazamiento de todo lo que se renderice acá.
    public Single FactorProfundidad;        // El ZBuffer global de esta capa.
    // Extra
    public Boolean EsRapido = true;         // Si es rápido, se renderiza a los frames actuales. Sino, va a 60 FPS.
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

        Rectangle destino = new Rectangle(
            (posicion.X * ScreenManager.TamPixel) + ScreenManager.PadX,
            (posicion.Y * ScreenManager.TamPixel) + ScreenManager.PadY,
            ancho * ScreenManager.TamPixel,
            alto * ScreenManager.TamPixel);
        this.DestinoEnPantalla = destino;

        this.Elementos = new List<IEntidad>();
        this.Fondo = Color.White;
        this.Tinte = Color.White;
    }

    /// <summary>
    /// Con esto me refiero a la resolución interna de la capa, no de la pantalla.
    /// </summary>
    /// <param name="ancho">Tratá que no sea mayor a <see cref="ConfigManager.WIDTH"/></param>
    /// <param name="alto">Tratá que no sea mayor a <see cref="ConfigManager.HEIGHT"/></param>
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

    public void RecargarResolucion()
    {
        Raylib.UnloadRenderTexture(this.TexturaInterna);
        this.TexturaInterna = Raylib.LoadRenderTexture(this.Ancho, this.Alto);
        Raylib.SetTextureFilter(this.TexturaInterna.Texture, ScreenManager.Filtro);
        
        // Reorganizando valores.
        this.DestinoEnPantalla = new Rectangle(
            (this.Posicion.X * ScreenManager.TamPixel) + ScreenManager.PadX,
            (this.Posicion.Y * ScreenManager.TamPixel) + ScreenManager.PadY,
            this.Ancho * ScreenManager.TamPixel,
            this.Alto * ScreenManager.TamPixel);
    }

    public void CambiarFiltro()
    {
        Raylib.SetTextureFilter(this.TexturaInterna.Texture, ScreenManager.Filtro);
    }

    public void Reposicionar(Vector2 nuevaPosicion)
    {
        this.Posicion = nuevaPosicion;
        this.CambiarResolucion(this.Ancho, this.Alto);
    }

    public Int32 Renderizar(Single alfa, Int64 framesTotales)
    {
        if (!this.Activa) 
            return 0;

        if (_sucio)
        {
            this.Elementos.Sort((x, y) => x.CapaPrioridad.CompareTo(y.CapaPrioridad));
            this._sucio = false;
        }

        Int32 counter = 0;
        Raylib.BeginTextureMode(this.TexturaInterna);
        Raylib.ClearBackground(this.Fondo);

        // Definimos el área visible de la capa (su tamaño interno)
        Rectangle areaVisible = new Rectangle(0, 0, this.Ancho, this.Alto);

        for (Int32 i = 0; i < this.Elementos.Count; i++)
        {
            counter += this.Elementos[i].Draw(alfa, this.DesplazamientoCamara, this.FactorProfundidad, areaVisible);
        }
        this.FramesTranscurridos++;
        Raylib.EndTextureMode();

        return counter;
    }

    public void Actualizar()
    {
        for(Int32 i = 0; i < this.Elementos.Count; i++)
        {
            this.Elementos[i].Update();
        }
    }
    private void MarcarCapaComoSucia()
    {
        this._sucio = true;
    }

    public void InsertarElemento(IEntidad elemento)
    {
        elemento.OnCambioPrioridad += MarcarCapaComoSucia;
        this.Elementos.Add(elemento);
        this._sucio = true;
    }

    /// <summary>
    /// Llamado arbitrario en la clase Escenario de turno. Usala, por ejemplo, luego que termina el turno de los enemigos.
    /// </summary>
    public void LimpiarBasura()
    {
        this.Elementos.RemoveAll((x) => 
        {
            if (x.Eliminado)
            {
                x.OnCambioPrioridad -= MarcarCapaComoSucia;
                return x.Eliminado; 
            }
            return false;
        });
    }

    public void Dispose()
    {
        if (!this._disposed)
        {
            Raylib.UnloadRenderTexture(this.TexturaInterna);
            for (Int32 i = 0; i < this.Elementos.Count; i++)
            {
                if (this.Elementos[i] is IDisposable d)
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