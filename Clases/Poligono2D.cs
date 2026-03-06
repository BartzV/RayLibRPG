using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases;

public class Poligono2D : IRenderizable, IActualizable, IDesplazable
{
    private Boolean _eliminado = false;
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
    public Vector2[] VerticesOriginales; // La forma base
    private Vector2[] _verticesProcesados; // Para no tocar los originales

    public Vector2 PosicionActual;
    public Vector2 PosicionAnterior;
    private Vector2 _posInterpolada;

    // En Radianes!!!
    private Single _rotacion;
    public Single Rotacion
    {
        get => _rotacion;
        set => this._rotacion = value;
    }

    public Single Escala { get; set; } = 1.0f;
    public Single Prioridad { get; set; }

    public Color Tinte;
    public Boolean EsStrip; // True para Strip, False para Fan

    public Poligono2D(Vector2[] vertices, Vector2 pos, Color color, bool esStrip = true)
    {
        this.VerticesOriginales = vertices;
        this._verticesProcesados = new Vector2[vertices.Length];
        this.PosicionActual = pos;
        this.PosicionAnterior = pos;
        this.Tinte = color;
        this.EsStrip = esStrip;
    }

    public void Update()
    {
        if (this.Activo)
            this.PosicionAnterior = this.PosicionActual;
    }

    public Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible)
    {
        if (!this.Activo) return 0;

        Single escalaFinal = zbuf / this.Escala;
        this._posInterpolada = Vector2.Lerp(this.PosicionAnterior, this.PosicionActual, alfa);

        // Pre-calculamos el seno y coseno para no hacerlo mil veces en el loop
        Single cos = (Single)Math.Cos(this.Rotacion);
        Single sin = (Single)Math.Sin(this.Rotacion);

        for (int i = 0; i < VerticesOriginales.Length; i++)
        {
            // 1. Rotación local (sobre el origen 0,0 del polígono)
            Single rx = VerticesOriginales[i].X * cos - VerticesOriginales[i].Y * sin;
            Single ry = VerticesOriginales[i].X * sin + VerticesOriginales[i].Y * cos;

            // 2. Escalado y Traslación a la posición del mundo + Desplazamiento de cámara
            _verticesProcesados[i].X = (rx + _posInterpolada.X) * escalaFinal + desp.X;
            _verticesProcesados[i].Y = (ry + _posInterpolada.Y) * escalaFinal + desp.Y;
        }

        // Culling (usamos el primer punto procesado con un margen generoso)
        // Ojo: Si el polígono es enorme, quizás necesites un margen mayor a 200
        if (!Raylib.CheckCollisionPointRec(_verticesProcesados[0],
            new Rectangle(areaVisible.X - 200, areaVisible.Y - 200, areaVisible.Width + 400, areaVisible.Height + 400)))
            return 0;

        if (EsStrip)
            Raylib.DrawTriangleStrip(_verticesProcesados, _verticesProcesados.Length, Tinte);
        else
            Raylib.DrawTriangleFan(_verticesProcesados, _verticesProcesados.Length, Tinte);

        return 1;
    }

    // Implementación de IDesplazable (igual que Sprite2D)
    public Vector2 Posicion { get => PosicionActual; set => PosicionActual = value; }
    public void Mover(Vector2 mov) => this.Posicion += mov;
    public void Posicionar(Vector2 pos) => this.Posicion = pos;
    public void Zoom(Single zoom) => this.Escala += zoom;
    public void Rotar(Single rad) => this.Rotacion += rad;
    public void Estabilizar(Single rad) => this.Rotacion = rad;

}
