using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases;

public class Sprite2D : IRenderizable, IActualizable
{
    public Texture2D Textura;
    // OJO! Rectangle es de RayLib, no de System.Drawing!
    public Rectangle Fuente;
    public Rectangle Destino;
    public Single ZBuffer;
    // Posiciones en el mundo real
    protected Vector2 PosicionActual;
    protected Vector2 PosicionAnterior;
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
        this.Centro = new Vector2(fuente.Width / 2, fuente.Height / 2);
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
    // alfa sin usar de momento.
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
    public void Draw(Single alfa, Vector2 desp, Single zbuf)
    {
        Single escalaFinal = zbuf / this.ZBuffer;

        this._desplazado.X = (this.Destino.X + desp.X) * escalaFinal;
        this._desplazado.Y = (this.Destino.Y + desp.Y) * escalaFinal;

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
}

public class Camara2D
{
    public Vector2 Desplazamiento;
    public Single ZBuffer;
    public List<IRenderizable> Relativos;
    public List<IRenderizable> Absolutos;

    public Camara2D()
    {
        this.Relativos = new();
        this.Absolutos = new();
        this.ZBuffer = 1f;
    }

    public void DibujarAbsolutos(Single alfa)
    {
        foreach(IRenderizable r in this.Absolutos)
        {
            r.Draw(alfa);
        }
    }
    public void DibujarRelativos(Single alfa)
    {
        foreach(IRenderizable r in this.Relativos)
        {
            r.Draw(alfa, this.Desplazamiento, this.ZBuffer);
        }
    }

}

public interface IRenderizable
{
    public void Draw(Single alfa);
    public void Draw(Single alfa, Vector2 desp, Single zbuf);
}

public interface IActualizable
{
    public void Update();
}