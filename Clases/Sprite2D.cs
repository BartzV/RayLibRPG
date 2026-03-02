using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases;

public class Sprite2D : IRenderizable, IActualizable, IDesplazable
{
    public Texture2D Textura;
    // OJO! Rectangle es de RayLib, no de System.Drawing!
    public Rectangle Fuente;
    public Rectangle Destino;
    public Single FactorProfundidad;
    // Posiciones en el mundo real
    public Vector2 PosicionActual;
    public Vector2 PosicionAnterior;
    // IDesplazable
    public Vector2 Posicion
    {
        get => PosicionActual;
        set => PosicionActual = value;
    }
    public Single ZBuffer
    {
        get => FactorProfundidad;
        set => FactorProfundidad = value;
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

    public Sprite2D(Texture2D textura, Rectangle fuente, Rectangle destino)
    {
        this.Textura = textura;
        this.Fuente = fuente;
        this.Destino = destino;
        this.Centro = new Vector2(destino.Width / 2, destino.Height / 2);
        this.Rotacion = 0.0f;
        this.Tinte = Color.White;
        this.ZBuffer = 1.0F;
        this.PosicionActual = new Vector2(destino.X, destino.Y);
        this.PosicionAnterior = new Vector2(destino.X, destino.Y);
    }
    public void Update()
    {
        this.PosicionAnterior = this.PosicionActual;
    }
    public void Draw(Single alfa)
    {
        this._posInterpolada = Vector2.Lerp(this.PosicionAnterior, this.PosicionActual, alfa);
        this.Destino.X = _posInterpolada.X;
        this.Destino.Y = _posInterpolada.Y;

        Raylib.DrawTexturePro(
            this.Textura,
            this.Fuente,
            this.Destino,
            this.Centro,
            this.Rotacion,
            this.Tinte);
    }
    // Esto me trajo demasiados problemas. Creo que funciona. Te odio función Draw.
    public void Draw(Single alfa, Vector2 desp, Single zbuf)
    {
        // Por qué no uso un auxiliar para evitar alloc? Porque eso se aloja en stack, no en heap.
        Single escalaFinal = zbuf / this.ZBuffer;
        this._posInterpolada = Vector2.Lerp(this.PosicionAnterior, this.PosicionActual, alfa);
        this.Destino.X = _posInterpolada.X;
        this.Destino.Y = _posInterpolada.Y;

        // Resulta que esto no andaba, lo dejo documentado por las dudas.
        //this._desplazado.X = (this.Destino.X + desp.X) * escalaFinal;
        //this._desplazado.Y = (this.Destino.Y + desp.Y) * escalaFinal;

        this._desplazado.X = this.Destino.X * escalaFinal + desp.X;
        this._desplazado.Y = this.Destino.Y * escalaFinal + desp.Y;

        this._desplazado.Width = this.Destino.Width * escalaFinal;
        this._desplazado.Height = this.Destino.Height * escalaFinal;

        this._centrado.X = this.Centro.X * escalaFinal;
        this._centrado.Y = this.Centro.Y * escalaFinal;

        Raylib.DrawTexturePro(
            this.Textura,
            this.Fuente,
            this._desplazado,
            this._centrado,
            this.Rotacion,
            this.Tinte);
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
        this.ZBuffer += zoom;
    }

}

public class MultiSprite2D : IRenderizable, IActualizable
{
    public Sprite2D Prototipo; // Usamos un sprite base para sacar los datos
    public Vector2[] Posiciones;
    public Vector2[] PosicionesAnteriores;

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

    public void Draw(Single alfa, Vector2 desp, Single zbuf)
    {
        // El truco está en "disfrazar" al prototipo en cada iteración
        for (int i = 0; i < Posiciones.Length; i++)
        {
            this.Prototipo.PosicionAnterior = this.PosicionesAnteriores[i];
            this.Prototipo.PosicionActual = this.Posiciones[i];

            // Reutilizamos tu función de Draw que tanto odiás (pero que anda)
            this.Prototipo.Draw(alfa, desp, zbuf);
        }
    }

    public void Draw(float alfa)
    {
        throw new NotImplementedException();
    }
}

public interface IRenderizable
{
    public abstract void Draw(Single alfa);
    public abstract void Draw(Single alfa, Vector2 desp, Single zbuf);
}

public interface IActualizable
{
    public abstract void Update();
}

public interface IDesplazable
{
    Vector2 Posicion { get; set; }
    Single ZBuffer { get; set; }
    public abstract void Mover(Vector2 mov);
    //{
    //    this.Posicion += mov;
    //}
    public abstract void Posicionar(Vector2 pos);
    //{
    //    this.Posicion = pos;
    //}
    public abstract void Zoom(Single zoom);
    //{
    //    this.ZBuffer += zoom;
    //}
}