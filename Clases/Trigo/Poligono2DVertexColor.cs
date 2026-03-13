using Raylib_cs;
using RayLibRPG.Clases.Config;
using RayLibRPG.Clases.Letras;
using System.Numerics;

namespace RayLibRPG.Clases.Trigo;

public class Poligono2DVertexColor : IEntidad, ITransformable
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
    private Letra[] _debugVertices;
    private Vector2[] _verticesProcesados; // Para no tocar los originales
    private Color[] _colores;

    public Vector2 PosicionActual;
    public Vector2 PosicionAnterior;
    private Vector2 _posInterpolada;

    // Implementación de IDesplazable (igual que Sprite2D)
    public Vector2 Posicion
    {
        get => PosicionActual;
        set
        {
            Vector2 dif = value - PosicionActual;
            this.PosicionActual = value;
            for (Int32 i = 0; i < this._debugVertices.Length; i++)
            {
                this._debugVertices[i].Posicion += dif;
            }
        }
    }

    private Vector2 _amplificacion;
    public Vector2 Amplificacion
    {
        get => this._amplificacion;
        set => this._amplificacion = value;
    }

    protected Single _rotacion;
    public Single Rotacion
    {
        get => _rotacion;
        set => this._rotacion = value;
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

    protected Single _profundidad;
    public Single ProfundidadZ
    {
        get => this._profundidad;
        set
        {
            this._profundidad = value;
        }
    }

    public Boolean EsStrip; // True para Strip, False para Fan
    public Single RadioMaximo;

    public Poligono2DVertexColor((Vector2 vertex, Color color)[] vertices, Vector2 pos, Boolean esStrip = true)
    {
        if (vertices == null || vertices.Length < 3)
            throw new ArgumentException("¡Vértices insuficientes! ¡Esto no forma un triángulo!");
        this.VerticesOriginales = new Vector2[vertices.Length];
        this._debugVertices = new Letra[vertices.Length];
        this._verticesProcesados = new Vector2[vertices.Length];
        this._colores = new Color[vertices.Length];
        this.ProfundidadZ = 1F;
        this.Amplificacion = Vector2.One;

        for (Int32 i = 0; i < vertices.Length; i++)
        {
            this._debugVertices[i] = new Letra('~', pos + vertices[i].vertex, Vector2.One, vertices[i].color);
            this.VerticesOriginales[i] = vertices[i].vertex;
            this._colores[i] = vertices[i].color;
        }

        this.PosicionActual = pos;
        this.PosicionAnterior = pos;
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
        if ((ConfigManager.DEBUG & DebugMode.Centers) != 0)
        {
            for (Int32 i = 0; i < this._debugVertices.Length; i++)
            {
                this._debugVertices[i].Update();
            }
        }
    }

    public Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible)
    {
        if (!this.Activo) return 0;

        Single escalaFinal = zbuf / this.ProfundidadZ;
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
            // Para simular un STRIP con TRIANGLES manuales:
            Rlgl.Begin(DrawMode.Triangles);
            for (int i = 0; i < _verticesProcesados.Length - 2; i++)
            {
                // Triángulo i, i+1, i+2
                // Ojo: en los Strips, el orden de los vértices se invierte en cada paso para mantener el "front face"
                if (i % 2 == 0)
                {
                    DibujarVertice(i);
                    DibujarVertice(i + 1);
                    DibujarVertice(i + 2);
                }
                else
                {
                    DibujarVertice(i + 1);
                    DibujarVertice(i);
                    DibujarVertice(i + 2);
                }
            }
        }
        else
        {
            // Para un FAN (estilo PS1 total)
            Rlgl.Begin(DrawMode.Triangles);
            for (int i = 1; i < _verticesProcesados.Length - 1; i++)
            {
                DibujarVertice(0); // El centro del abanico
                DibujarVertice(i);
                DibujarVertice(i + 1);
            }
        }

        Rlgl.End();

        if ((ConfigManager.DEBUG & DebugMode.Centers) != 0)
        {
            for (Int32 i = 0; i < this._debugVertices.Length; i++)
            {
                this._debugVertices[i].Draw(alfa, desp, zbuf, areaVisible);
            }
        }
        return 1;
    }

    // Un helper para no repetir código como un loco
    private void DibujarVertice(int index)
    {
        Rlgl.Color4ub(_colores[index].R, _colores[index].G, _colores[index].B, _colores[index].A);
        Rlgl.Vertex2f(_verticesProcesados[index].X, _verticesProcesados[index].Y);
    }


    public void Mover(Vector2 mov) => this.Posicion += mov;
    public void Posicionar(Vector2 pos) => this.Posicion = pos;
    public void AplicarZoom(Single zoom) => this.ProfundidadZ = Math.Max(0, this.ProfundidadZ + zoom);
    public void SetZoom(Single zoom) => this.ProfundidadZ += zoom;
    public void Rotar(Single rad) => this.Rotacion += rad;
    public void Estabilizar(Single rad) => this.Rotacion = rad;

    public void SetFlip(Boolean x, Boolean y)
    {

    }
}

