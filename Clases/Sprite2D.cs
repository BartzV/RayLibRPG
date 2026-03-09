using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases;

public class Sprite2D : IRenderizable, IActualizable, ITransformable
{
    // Estado
    protected Boolean _eliminado = false;
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

    public Texture2D Textura;
    // OJO! Rectangle es de RayLib, no de System.Drawing!
    public Rectangle Fuente;
    public Rectangle Destino;
    protected Single _escala;
    protected Single _prioridad;
    protected Vector2 _amplificacion;
    protected Single _rotacion;
    // Posiciones en el mundo real
    public Vector2 PosicionActual;
    public Vector2 PosicionAnterior;
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
    public Single CapaPrioridad
    {
        get => this._prioridad;
        set => this._prioridad = value;
    }
    // Punto de origen para rotaciones y escalados
    public Vector2 Centro;
    public Single Rotacion
    {
        get => this._rotacion;
        set => this._rotacion = value;
    }
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
        this.ProfundidadZ = 1.0F;
        this.Amplificacion = Vector2.One;
        this.PosicionActual = new Vector2(destino.X, destino.Y);
        this.PosicionAnterior = new Vector2(destino.X, destino.Y);
    }
    public Sprite2D(Texture2D textura, Rectangle fuente, Vector2 destino, Vector2 tamReal, Vector2? amp)
    {
        this.Textura = textura;
        this.Fuente = fuente;
        this.Destino = new Rectangle(destino, tamReal);
        this.Centro = new Vector2(this.Destino.Width / 2, this.Destino.Height / 2);
        this.Rotacion = 0.0f;
        this.Tinte = Color.White;
        this.ProfundidadZ = 1.0F;
        if (amp is null)
        {
            this.Amplificacion = Vector2.One;
        }
        else
        {
            this.Amplificacion = (Vector2)amp;
        }
        this.PosicionActual = new Vector2(destino.X, destino.Y);
        this.PosicionAnterior = new Vector2(destino.X, destino.Y);
    }
    public void Update()
    {
        if (this.Activo)
            this.PosicionAnterior = this.PosicionActual;
    }
    // Esto me trajo demasiados problemas. Te odio función Draw. Te sigo odiando función de mierda.
    public Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible)
    {
        if (!this.Activo)
            return 0;
        // Por qué no uso un auxiliar para evitar alloc? Porque eso se aloja en stack, no en heap.
        Single escalaFinal = zbuf / this.ProfundidadZ;
        this._posInterpolada = Vector2.Lerp(this.PosicionAnterior, this.PosicionActual, alfa);
        this.Destino.X = this._posInterpolada.X;
        this.Destino.Y = this._posInterpolada.Y;

        this._desplazado.X = this.Destino.X * escalaFinal + desp.X;
        this._desplazado.Y = this.Destino.Y * escalaFinal + desp.Y;
        // EZ
        this._desplazado.Width = this.Destino.Width * escalaFinal * this.Amplificacion.X;
        this._desplazado.Height = this.Destino.Height * escalaFinal * this.Amplificacion.Y;

        // LOGICA DE CULLING
        // Por qué no la mitad? Porque si el Sprite no tiene el centro en el medio, esto vuela a LCDSM
        areaVisible.Width += this.Destino.Width;
        areaVisible.Height += this.Destino.Height;
        // CheckCheck: ¿El rectángulo del sprite se toca con el de la capa?
        if (!Raylib.CheckCollisionRecs(this._desplazado, areaVisible))
            return 0;
        // EZ
        this._centrado.X = this.Centro.X * escalaFinal * this.Amplificacion.X;
        this._centrado.Y = this.Centro.Y * escalaFinal * this.Amplificacion.Y;

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

    public void Rotar(Single rad)
    {
        this.Rotacion += rad;
    }

    public void Estabilizar(Single rad)
    {
        this.Rotacion = rad;
    }

    public void AplicarZoom(Single zoom)
    {
        this.ProfundidadZ = Math.Clamp(this.ProfundidadZ + zoom, 0, (Single)Half.MaxValue);
    }

    public void SetZoom(Single valor)
    {
        this.ProfundidadZ = valor;
    }

    public void SetFlip(Boolean x, Boolean y)
    {
        if (x)
            this.Fuente.X = -this.Fuente.X;

        if (y)
            this.Fuente.Y = -this.Fuente.Y;
    }


}

public class MultiSprite2D : IRenderizable, IActualizable, ITransformable
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
    protected Boolean _activo = true;
    public Boolean Activo
    {
        get => this._activo;
        set => this._activo = value;
    }

    public Sprite2D Prototipo; // Usamos un sprite base para sacar los datos
    public Vector2[] Posiciones;
    public Vector2[] PosicionesAnteriores;

    public Single PrioridadZ
    {
        get => this.Prototipo.ProfundidadZ;
        set => this.Prototipo.ProfundidadZ = value;
    }
    public Single CapaPrioridad
    {
        get => this.Prototipo.CapaPrioridad;
        set => this.Prototipo.CapaPrioridad = value;
    }
    public Vector2 Posicion
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public Single ProfundidadZ
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public Single Rotacion
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public Vector2 Amplificacion
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
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

    public void Mover(Vector2 mov)
    {
        throw new NotImplementedException();
    }

    public void Posicionar(Vector2 pos)
    {
        throw new NotImplementedException();
    }

    public void Rotar(Single rad)
    {
        throw new NotImplementedException();
    }

    public void Estabilizar(Single rad)
    {
        throw new NotImplementedException();
    }

    public void AplicarZoom(Single delta)
    {
        throw new NotImplementedException();
    }

    public void SetZoom(Single valor)
    {
        throw new NotImplementedException();
    }

    public void SetFlip(Boolean x, Boolean y)
    {
        throw new NotImplementedException();
    }
}

public interface IRenderizable
{
    Boolean Activo { get; set; }
    Boolean Eliminado { get; set; }
    Single CapaPrioridad { get; set; } // Para el Sort dentro de la capa

    Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible);
}

public interface IActualizable
{
    // No hay gran ciencia. Cada cosa renderizable que se mueva debe tener un update.
    public abstract void Update();
}

public interface ITransformable
{
    Vector2 Posicion { get; set; }
    Single ProfundidadZ { get; set; } // Lo que llamabas Escala (distancia)
    Single Rotacion { get; set; }
    Vector2 Amplificacion { get; set; }

    // Métodos de acción
    void Mover(Vector2 mov);
    void Posicionar(Vector2 pos);
    void Rotar(Single rad);
    void Estabilizar(Single rad); // Set directo

    // Transformaciones visuales
    void AplicarZoom(Single delta);
    void SetZoom(Single valor);
    void SetFlip(bool x, bool y);
}