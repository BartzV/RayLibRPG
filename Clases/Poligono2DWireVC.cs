using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases;

public class Poligono2DWireVC : IRenderizable, IActualizable, IDesplazable
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
    private Color[] _colores;

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

    public Boolean EsCerrado;
    public Boolean EsStrip; // True para Strip, False para Fan
    public Single RadioMaximo;

    public Poligono2DWireVC((Vector2 vertex, Color color)[] vertices, Vector2 pos, Boolean esCerrado = false, Boolean esStrip = true)
    {
        if (vertices == null || vertices.Length < 2)
            throw new ArgumentException("¡Vértices insuficientes! ¡Esto no forma un wire!");
        this.VerticesOriginales = new Vector2[vertices.Length];
        this._verticesProcesados = new Vector2[vertices.Length];
        this._colores = new Color[vertices.Length];

        for (Int32 i = 0; i < vertices.Length; i++)
        {
            this.VerticesOriginales[i] = vertices[i].vertex;
            this._colores[i] = vertices[i].color;
        }

        this.PosicionActual = pos;
        this.PosicionAnterior = pos;
        this.EsCerrado = esCerrado;
        this.EsStrip = esStrip;

        // Calculamos el radio máximo una sola vez
        // Es la distancia del vértice más lejano respecto al centro (0,0) local
        this.RadioMaximo = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            // Usamos el origen (0,0) porque se supone que VerticesOriginales son locales
            float distanciaAlCentro = vertices[i].vertex.Length();
            if (distanciaAlCentro > this.RadioMaximo)
                this.RadioMaximo = distanciaAlCentro;
        }
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

        // 1. CULLING INTELIGENTE
        // Calculamos la posición final en pantalla de nuestro "centro"
        Vector2 posEnPantalla = (this._posInterpolada * escalaFinal) + desp;
        // El radio también se ve afectado por la escala
        float radioEnPantalla = this.RadioMaximo * escalaFinal;

        // Si el círculo que envuelve al polígono no toca el área visible, nos tomamos el palo
        if (!Raylib.CheckCollisionCircleRec(posEnPantalla, radioEnPantalla, areaVisible))
            return 0;

        // 2. TRANSFORMACIONES (Solo si pasó el Culling)
        Single cos = (Single)Math.Cos(this.Rotacion);
        Single sin = (Single)Math.Sin(this.Rotacion);

        for (int i = 0; i < VerticesOriginales.Length; i++)
        {
            // Rotación
            Single rx = VerticesOriginales[i].X * cos - VerticesOriginales[i].Y * sin;
            Single ry = VerticesOriginales[i].X * sin + VerticesOriginales[i].Y * cos;

            // Posicionamiento final
            _verticesProcesados[i].X = (rx + _posInterpolada.X) * escalaFinal + desp.X;
            _verticesProcesados[i].Y = (ry + _posInterpolada.Y) * escalaFinal + desp.Y;
        }


        if (EsStrip)
        {
            Rlgl.Begin(DrawMode.Lines);
            for (int i = 0; i < _verticesProcesados.Length - 1; i++)
            {
                DibujarVertice(i);
                DibujarVertice(i + 1);

            }
            if (this.EsCerrado)
            {
                DibujarVertice(_verticesProcesados.Length - 1);
                DibujarVertice(0);
            }
        }
        else
        {
            // Para un FAN (estilo PS1 total)
            Rlgl.Begin(DrawMode.Lines);
            for (int i = 1; i < _verticesProcesados.Length; i++)
            {
                DibujarVertice(0); // El centro del abanico
                DibujarVertice(i);
            }
        }

        Rlgl.End();
        return 1;
    }

    // Un helper para no repetir código como un loco
    private void DibujarVertice(int index)
    {
        Rlgl.Color4ub(_colores[index].R, _colores[index].G, _colores[index].B, _colores[index].A);
        Rlgl.Vertex2f(_verticesProcesados[index].X, _verticesProcesados[index].Y);
    }

    // Implementación de IDesplazable (igual que Sprite2D)
    public Vector2 Posicion { get => PosicionActual; set => PosicionActual = value; }
    public void Mover(Vector2 mov) => this.Posicion += mov;
    public void Posicionar(Vector2 pos) => this.Posicion = pos;
    public void Zoom(Single zoom) => this.Escala += zoom;
    public void Rotar(Single rad) => this.Rotacion += rad;
    public void Estabilizar(Single rad) => this.Rotacion = rad;

}

