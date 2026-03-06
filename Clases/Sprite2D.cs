using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases;

public class Sprite2D : IRenderizable, IActualizable, IDesplazable
{
    // Estado
    public Boolean _eliminado = false;
    public Boolean Eliminado
    {
        get => this._eliminado;
        set
        {
            this._eliminado = value;
            this.Activo = !value;
        }
    }
    public Boolean Activo = true;

    public Texture2D Textura;
    // OJO! Rectangle es de RayLib, no de System.Drawing!
    public Rectangle Fuente;
    public Rectangle Destino;
    protected Single _escala;
    protected Single _prioridad;
    // Posiciones en el mundo real
    public Vector2 PosicionActual;
    public Vector2 PosicionAnterior;
    // IDesplazable
    public Vector2 Posicion
    {
        get => this.PosicionActual;
        set => this.PosicionActual = value;
    }
    public Single Escala
    {
        get => this._escala;
        set => this._escala = value;
    }
    public Single Prioridad
    {
        get => this._prioridad;
        set => this._prioridad = value;
    }
    // Punto de origen para rotaciones y escalados
    public Vector2 Centro;
    public Single Rotacion;
    // Color. TODAS las texturas van a ser coloreadas. TODAS.
    // De nuevo, Color es de RayLib, no de System Drawing.
    public Color Tinte;
    // Auxiliares para evitar alloc
    private Rectangle _desplazado;
    private Vector2 _centrado;
    private Vector2 _posInterpolada;
    /// <summary>
    /// Legacy???
    /// </summary>
    /// <param name="textura">Textura del Atlas.</param>
    /// <param name="fuente">Lugar y tamaño de la textura que se va a usar</param>
    /// <param name="destino">El lugar donde estará el Sprite en pantalla. </param>
    public Sprite2D(Texture2D textura, Rectangle fuente, Rectangle destino)
    {
        this.Textura = textura;
        this.Fuente = fuente;
        this.Destino = destino;
        this.Centro = new Vector2(destino.Width / 2, destino.Height / 2);
        this.Rotacion = 0.0f;
        this.Tinte = Color.White;
        this.Escala = 1.0F;
        this.PosicionActual = new Vector2(destino.X, destino.Y);
        this.PosicionAnterior = new Vector2(destino.X, destino.Y);
    }
    public void Update()
    {
        if (this.Activo)
            this.PosicionAnterior = this.PosicionActual;
    }
    // Esto me trajo demasiados problemas. Te odio función Draw.
    public Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible)
    {
        if (!this.Activo)
            return 0;
        // Por qué no uso un auxiliar para evitar alloc? Porque eso se aloja en stack, no en heap.
        Single escalaFinal = zbuf / this.Escala;
        this._posInterpolada = Vector2.Lerp(this.PosicionAnterior, this.PosicionActual, alfa);
        this.Destino.X = this._posInterpolada.X;
        this.Destino.Y = this._posInterpolada.Y;

        this._desplazado.X = this.Destino.X * escalaFinal + desp.X;
        this._desplazado.Y = this.Destino.Y * escalaFinal + desp.Y;

        this._desplazado.Width = this.Destino.Width * escalaFinal;
        this._desplazado.Height = this.Destino.Height * escalaFinal;

        // LOGICA DE CULLING
        // Por qué no la mitad? Porque si el Sprite no tiene el centro en el medio, esto vuela a LCDSM
        areaVisible.Width += this.Destino.Width;
        areaVisible.Height += this.Destino.Height;
        // CheckCheck: ¿El rectángulo del sprite se toca con el de la capa?
        if (!Raylib.CheckCollisionRecs(this._desplazado, areaVisible))
            return 0;

        this._centrado.X = this.Centro.X * escalaFinal;
        this._centrado.Y = this.Centro.Y * escalaFinal;

        Raylib.DrawTexturePro(
            this.Textura,
            this.Fuente,
            this._desplazado,
            this._centrado,
            this.Rotacion,
            this.Tinte);

        return 1;
    }

    public void Mover(Vector2 mov)
    {
        this.Posicion += mov;
    }

    public void Posicionar(Vector2 pos)
    {
        this.Posicion = pos;
    }

    public void Zoom(Single zoom)
    {
        this.Escala += zoom;
    }

    public void Rotar(Single rad)
    {
        this.Rotacion += rad;
    }

    public void Estabilizar(Single rad)
    {
        this.Rotacion = rad;
    }
}

public class MultiSprite2D : IRenderizable, IActualizable
{
    public Boolean _eliminado;
    public Boolean Eliminado
    {
        get => this._eliminado;
        set
        {
            this._eliminado = value;
            this.Activo = !value;
        }
    }
    public Boolean Activo = true;

    public Sprite2D Prototipo; // Usamos un sprite base para sacar los datos
    public Vector2[] Posiciones;
    public Vector2[] PosicionesAnteriores;

    private Single _prioridad;
    public Single Prioridad
    {
        get => this._prioridad;
        set => this._prioridad = value;
    }

    public MultiSprite2D(Sprite2D prototipo, int cantidad)
    {
        this.Prototipo = prototipo;
        this.Posiciones = new Vector2[cantidad];
        this.PosicionesAnteriores = new Vector2[cantidad];
    }

    public void Update()
    {
        // Sincronizamos todas las posiciones para el Lerp
        for (int i = 0; i < Posiciones.Length; i++)
        {
            PosicionesAnteriores[i] = Posiciones[i];
        }
    }

    public Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible)
    {
        if (!this.Activo) return 0;
        Int32 counter = 0;
        // El truco está en "disfrazar" al prototipo en cada iteración
        for (int i = 0; i < Posiciones.Length; i++)
        {
            this.Prototipo.PosicionAnterior = this.PosicionesAnteriores[i];
            this.Prototipo.PosicionActual = this.Posiciones[i];

            // Reutilizamos tu función de Draw que tanto odiás (pero que anda)
            counter += this.Prototipo.Draw(alfa, desp, zbuf, areaVisible);
        }
        return counter;
    }


}

public interface IRenderizable
{
    // Ahora devuelve cuántos elementos dibujó realmente
    public abstract Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible);
    // Profundidad, ya que Draw no tiene la profundidad, y Raylib.DrawTexturePro no la toma.
    public abstract Single Prioridad { get; set; }
    public abstract Boolean Eliminado { get; set; }
}

public interface IActualizable
{
    public abstract void Update();
}

public interface IDesplazable
{
    Vector2 Posicion { get; set; }
    Single Escala { get; set; }
    public abstract void Mover(Vector2 mov);
    public abstract void Posicionar(Vector2 pos);
    public abstract void Zoom(Single zoom);
    // En Radianes, la concha de tu madre.
    public abstract void Rotar(Single ang);
    // Setea la Rotación, no agrega.
    public abstract void Estabilizar(Single ang);
}