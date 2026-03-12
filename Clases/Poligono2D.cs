using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases;

public class Poligono2DPlano : IEntidad, ITransformable
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

    protected Boolean _activo = true;
    public Boolean Activo
    {
        get => this._activo;
        set => this._activo = value;
    }

    public Vector2[] VerticesOriginales; // La forma base
    private Vector2[] _verticesProcesados; // Para no tocar los originales

    public Vector2 PosicionActual;
    public Vector2 PosicionAnterior;

    private Vector2 _posInterpolada;
    protected Single _escala;
    protected Single _prioridad;
    protected Vector2 _amplificacion;
    protected Single _rotacion;

    // IDesplazable
    public Vector2 Posicion
    {
        get => this.PosicionActual;
        set => this.PosicionActual = value;
    }

    // Para agrandar o achicar sin afectar la profundidad
    // Se puede "aplastar" disminuyendo el Y, dejando el X en 1.
    public Vector2 Amplificacion
    {
        get => this._amplificacion;
        set => this._amplificacion = Vector2.Abs(value);
    }

    public Single ProfundidadZ
    {
        get => this._prioridad;
        set => this._prioridad = value;
    }

    public event Action? OnCambioPrioridad; // Implementación del interfaz
    private Single _capaPrioridad;
    public Single CapaPrioridad
    {
        get => this._capaPrioridad;
        set
        {
            if (this._capaPrioridad != value)
            {
                this._capaPrioridad = value;
                this.OnCambioPrioridad?.Invoke();
            }
        }
    }

    public Single Rotacion
    {
        get => _rotacion;
        set => this._rotacion = value;
    }


    public Color Tinte;
    public Boolean EsStrip; // True para Strip, False para Fan

    public Single RadioMaximo;

    public Poligono2DPlano(Vector2[] vertices, Vector2 pos, Color color, Boolean esStrip = true)
    {
        if (vertices == null || vertices.Length < 3)
            throw new ArgumentException("¡Vértices insuficientes! ¡Esto no forma un triángulo!");

        this.VerticesOriginales = vertices;
        this._verticesProcesados = new Vector2[vertices.Length];
        this.PosicionActual = pos;
        this.PosicionAnterior = pos;
        this.ProfundidadZ = 1F;
        this.Amplificacion = Vector2.One;

        this.Tinte = color;
        this.EsStrip = esStrip;

        // Calculamos el radio máximo una sola vez
        // Es la distancia del vértice más lejano respecto al centro (0,0) local
        this.RadioMaximo = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            // Usamos el origen (0,0) porque se supone que VerticesOriginales son locales
            float distanciaAlCentro = vertices[i].Length();
            if (distanciaAlCentro > this.RadioMaximo)
                this.RadioMaximo = distanciaAlCentro;
        }
    }
    public const float DEG_TO_RAD = 0.0174532925f;
    public Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible)
    {
        if (!this.Activo) return 0;

        Single escalaFinal = zbuf / this.ProfundidadZ;
        this._posInterpolada = Vector2.Lerp(this.PosicionAnterior, this.PosicionActual, alfa);

        // 1. CULLING INTELIGENTE
        Vector2 posEnPantalla = (this._posInterpolada * escalaFinal) + desp;

        // El radio máximo ahora debe considerar la amplificación más grande para el culling
        Single maxAmp = Math.Max(this.Amplificacion.X, this.Amplificacion.Y);
        Single radioEnPantalla = this.RadioMaximo * escalaFinal * maxAmp;

        if (!Raylib.CheckCollisionCircleRec(posEnPantalla, radioEnPantalla, areaVisible))
            return 0;

        // 2. TRANSFORMACIONES
        Single rotGrados = this.Rotacion * DEG_TO_RAD;
        Single cos = (Single)Math.Cos(rotGrados);
        Single sin = (Single)Math.Sin(rotGrados);

        for (int i = 0; i < VerticesOriginales.Length; i++)
        {
            // --- LA MAGIA ESTÁ ACÁ ---
            // A. Aplicamos Amplificación LOCAL (Squash & Stretch)
            Single sX = this.VerticesOriginales[i].X * this.Amplificacion.X;
            Single sY = this.VerticesOriginales[i].Y * this.Amplificacion.Y;

            // B. Rotamos el punto ya "amplificado"
            Single rx = sX * cos - sY * sin;
            Single ry = sX * sin + sY * cos;

            // C. Posicionamiento final (Mundo + Cámara + Z-Scale)
            this._verticesProcesados[i].X = (rx + this._posInterpolada.X) * escalaFinal + desp.X;
            this._verticesProcesados[i].Y = (ry + this._posInterpolada.Y) * escalaFinal + desp.Y;
        }

        // 3. RENDER
        if (EsStrip)
            Raylib.DrawTriangleStrip(_verticesProcesados, _verticesProcesados.Length, Tinte);
        else
            Raylib.DrawTriangleFan(_verticesProcesados, _verticesProcesados.Length, Tinte);

        return 1;
    }

    public void Update()
    {
        if (this.Activo)
            this.PosicionAnterior = this.PosicionActual;
    }


    public void Mover(Vector2 mov) => this.Posicion += mov;
    public void Posicionar(Vector2 pos) => this.Posicion = pos;
    public void AplicarZoom(Single zoom) => this.ProfundidadZ = Math.Max(this.ProfundidadZ, 0F);
    public void SetZoom(Single zoom) => this.ProfundidadZ = zoom;
    public void Rotar(Single rad) => this.Rotacion += rad;
    public void Estabilizar(Single rad) => this.Rotacion = rad;

    public void SetFlip(Boolean x, Boolean y)
    {
        if (x)
        {
            for (Int32 i = 0; i < this.VerticesOriginales.Length; i++)
            {
                this.VerticesOriginales[i].X = -this.VerticesOriginales[i].X;
            }
        }
        if (y)
        {
            for (Int32 i = 0; i < this.VerticesOriginales.Length; i++)
            {
                this.VerticesOriginales[i].Y = -this.VerticesOriginales[i].Y;
            }
        }
    }
}

public class Circulo2D : IEntidad, ITransformable
{
    public Boolean Activo { get; set; } = true;
    public Boolean Eliminado { get; set; } = false;

    public event Action? OnCambioPrioridad; // Implementación del interfaz

    private Single _capaPrioridad;
    public Single CapaPrioridad
    {
        get => this._capaPrioridad;
        set
        {
            if (this._capaPrioridad != value)
            {
                this._capaPrioridad = value;
                this.OnCambioPrioridad?.Invoke();
            }
        }
    }

    // ITransformable
    public Vector2 Posicion { get; set; }
    public Vector2 PosicionAnterior;
    public Single ProfundidadZ { get; set; } = 1.0f;
    public Single Rotacion { get; set; } // El círculo no "parece" rotar, pero para el Squash & Stretch importa!
    public Vector2 Amplificacion { get; set; } = Vector2.One;

    public Single Radio;
    public Color Tinte;

    public Circulo2D(Vector2 pos, Single radio, Color color)
    {
        this.Posicion = pos;
        this.PosicionAnterior = pos;
        this.Radio = radio;
        this.Tinte = color;
    }

    public void Update()
    {
        if (this.Activo)
            this.PosicionAnterior = this.Posicion;
    }

    public Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible)
    {
        if (!this.Activo) return 0;

        Single escalaFinal = zbuf / this.ProfundidadZ;
        Vector2 posInterpolada = Vector2.Lerp(this.PosicionAnterior, this.Posicion, alfa);

        // Transformación a pantalla
        Vector2 posPantalla = (posInterpolada * escalaFinal) + desp;
        Single radioFinal = this.Radio * escalaFinal;

        // CULLING CIRCULAR: El más eficiente de todos
        // Expandimos el radio por la mayor amplificación para que no desaparezca de golpe
        Single maxAmp = Math.Max(this.Amplificacion.X, this.Amplificacion.Y);
        if (!Raylib.CheckCollisionCircleRec(posPantalla, radioFinal * maxAmp, areaVisible))
            return 0;

        Raylib.DrawCircleV(posPantalla, radioFinal, this.Tinte);

        return 1;
    }

    // Métodos de ITransformable (Motosierra ready)
    public void Mover(Vector2 mov) => this.Posicion += mov;
    public void Posicionar(Vector2 pos) => this.Posicion = pos;
    public void Rotar(Single rad) => this.Rotacion += rad;
    public void Estabilizar(Single rad) => this.Rotacion = rad;
    public void AplicarZoom(Single delta) => this.ProfundidadZ = Math.Max(0, this.ProfundidadZ + delta);
    public void SetZoom(Single valor) => this.ProfundidadZ = valor;
    public void SetFlip(bool x, bool y) { /* Un círculo espejado es... un círculo */ }
}