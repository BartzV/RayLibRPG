using Raylib_cs;
using RayLibRPG.Clases.Config;
using System.Numerics;
using System.Timers;

namespace RayLibRPG.Clases.Letras;

public class Letra : IRenderizable, IActualizable, IDesplazable
{
    // Constantes
    public static readonly Vector2 TAM_LETRA = new Vector2(8, 8);
    // Gráficos
    public Sprite2D Sprite; // IMPORTANTE: El Sprite es tonto, la clase contiene, no hereda.
    // Para qué lo guardo? Para que pregunten los boludos.
    public Char Caracter;
    protected Color _tinte;
    // IDesplazable
    public Single ZBuffer
    {
        get => this.Sprite.ZBuffer;
        set => this.Sprite.ZBuffer = value;
    }
    // Para efectos. Quien lo use para lógica lo mato.
    public EfectoLetra Efecto = EfectoLetra.Ninguno;
    public Int32 AlfaUpdate;
    public Int32 DelayUpdate;
    protected Int32 _actualAlfaUpdate = 0;
    protected Int32 _tiempoVida = 0;
    public Vector2 Amplitud;

    public Color Tinte
    {
        get { return _tinte; }
        set
        {
            _tinte = value;
            Sprite.Tinte = value;
        }
    }
    private Single _rotacion;
    public Single Rotacion
    {
        get => this._rotacion;
        set
        {
            this._rotacion = value;
            this.Sprite.Rotacion = value;
        }
    }

    // Lógica
    private Vector2 _posicion;
    public Vector2 Posicion
    {
        get { return this._posicion; }
        set
        {
            this._posicion = value;
            this.Sprite.Posicion = value;
        }
    }

    public Letra(Char caracter, Vector2 posicion, Vector2 amplitudes, Color? tinte = null, Int32 alfa = 0)
    {
        // Me fijo si está inicializado. No lo está? Lo inicializa la clase.
        // En teoría, esto debe de estar vivo 24/7, LetraManager no se puede apagar...
        LetraManager.Inicializar();
        // Failsafe. Arreglar más tarde (cuando tenga ganas)
        Rectangle rect = LetraManager.GetRectangle(caracter) ?? LetraManager.GetEspacio();
        this.Amplitud = amplitudes;
        this.Sprite = new Sprite2D(LetraManager.Textura, rect, new Rectangle(posicion, amplitudes * 8));

        this.Caracter = caracter;
        this.Posicion = posicion;

        this.Tinte = tinte ?? Color.White;
        this._tiempoVida = alfa * 8;
    }

    public void CambiarLetra(Char caracter)
    {
        Rectangle rect = LetraManager.GetRectangle(caracter) ?? LetraManager.GetEspacio();
        this.Sprite.Fuente = rect;
    }

    public virtual void Draw(float alfa)
    {
        this.Sprite.Draw(alfa);
    }

    public virtual void Draw(float alfa, Vector2 desp, float zbuf)
    {
        this.Sprite.Draw(alfa, desp, zbuf);
    }

    public virtual void Update()
    {
        this.Sprite.Update();
        this._tiempoVida++;

        if (Efecto == EfectoLetra.Temblor)
        {
            // Un temblor rancio pero efectivo usando el tiempo
            float offsetx = Raylib.GetRandomValue(-1, 1);
            float offsety = Raylib.GetRandomValue(-1, 1);
            this.Sprite.PosicionActual = this.Posicion + new Vector2(offsetx, offsety);
        }
        else if (Efecto == EfectoLetra.Ola)
        {
            Double gamma = (this._tiempoVida % 60) / 60.0 * Math.PI;
            Single desplazamientoY = (Single)Math.Sin(gamma) * 4f - 2;
            this.Sprite.PosicionActual = this.Posicion + new Vector2(0, desplazamientoY);
        }
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
