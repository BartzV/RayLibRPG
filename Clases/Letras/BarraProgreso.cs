using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases.Letras;

//public class BarraProgreso : IActualizable, IRenderizable, IDesplazable
//{
//    // Estado
//    public Boolean _eliminado = false;
//    public Boolean Eliminado
//    {
//        get => this._eliminado;
//        set
//        {
//            this._eliminado = value;
//            this._fondo.Eliminado = value;
//            this._frente.Eliminado = value;
//            this._marcoI.Eliminado = value;
//            this._marcoM.Eliminado = value;
//            this._marcoD.Eliminado = value;
//            this.Activo = !value;
//        }
//    }
//    public Boolean Activo = true;

//    private Sprite2D _fondo;
//    private Sprite2D _frente;
//    private Sprite2D _marcoI;   // Marco Izquierdo
//    private Sprite2D _marcoM;   // Marco Central
//    private Sprite2D _marcoD;   // Marco Derecho
//    private Single _porcentaje;
//    private Single _escala;
//    public Single Escala
//    {
//        get => this._escala;
//        set
//        {
//            this._escala = value;
//            this._fondo.Escala = value;
//            this._frente.Escala = value;
//            this._marcoI.Escala = value;
//            this._marcoM.Escala = value;
//            this._marcoD.Escala = value;
//        }
//    }
//    private Single _prioridad;
//    public Single Prioridad
//    {
//        get => this._prioridad;
//        set
//        {
//            this._prioridad = value;
//            this._fondo.Prioridad = value;
//            this._frente.Prioridad = value;
//            this._marcoI.Prioridad = value;
//            this._marcoM.Prioridad = value;
//            this._marcoD.Prioridad = value;
//        }
//    }

//    public Single Porcentaje // 0.0f a 1.0f
//    {
//        get => this._porcentaje;
//        set => this._porcentaje = Math.Clamp(value, 0f, 1f);
//    }
//    public Vector2 Posicion
//    {
//        get => this._fondo.Posicion;
//        set
//        {
//            this._fondo.Posicion = value;
//            this._frente.Posicion = value;
//        }
//    }


//    public BarraProgreso(Vector2 pos, Single tam, Color frente, Color fondo, Color marco)
//    {
//        Texture2D atlas = LetraManager.Textura;
//        Rectangle fuenteBlanca = LetraManager.GetRectangle('\uFBF3') ?? throw new InvalidOperationException();
//        Rectangle fuenteMI = LetraManager.GetRectangle('\uFBF0') ?? throw new InvalidOperationException();
//        Rectangle fuenteMM = LetraManager.GetRectangle('\uFBF1') ?? throw new InvalidOperationException();
//        Rectangle fuenteMD = LetraManager.GetRectangle('\uFBF2') ?? throw new InvalidOperationException();
//        if (tam < 8)
//        {
//            tam = 8;
//        }

//        this._marcoI = new Sprite2D(atlas, fuenteMI, new Rectangle(pos.X, pos.Y, Letra.TAM_LETRA));
//        this._marcoM = new Sprite2D(atlas, fuenteMM, new Rectangle(pos.X + Letra.TAM_LETRA.X * 0.5F, pos.Y, tam - Letra.TAM_LETRA.X, Letra.TAM_LETRA.Y));
//        this._marcoM.Centro.X = 0;
//        this._marcoD = new Sprite2D(atlas, fuenteMD, new Rectangle(pos.X + tam, pos.Y, Letra.TAM_LETRA));
//        this._marcoI.Tinte = this._marcoM.Tinte = this._marcoD.Tinte = marco;

//        // El fondo suele ser negro o gris oscuro
//        this._fondo = new Sprite2D(atlas, fuenteBlanca, new Rectangle(pos.X, pos.Y, tam, Letra.TAM_LETRA.Y));
//        this._fondo.Centro.X = 0;
//        this._fondo.Tinte = fondo;

//        // El frente es el que se estira
//        this._frente = new Sprite2D(atlas, fuenteBlanca, new Rectangle(pos.X, pos.Y, tam, Letra.TAM_LETRA.Y));
//        this._frente.Centro.X = 0;
//        this._frente.Tinte = frente;

//        this.Porcentaje = 1.0f;
//    }

//    public void Update()
//    {
//        _fondo.Update();
//        _frente.Update();
//        _marcoI.Update();
//        _marcoM.Update();
//        _marcoD.Update();
//    }

//    public Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible)
//    {
//        Int32 c = 0;
//        c += _fondo.Draw(alfa, desp, zbuf, areaVisible);

//        _frente.Destino.Width = _fondo.Destino.Width * Porcentaje;
//        c += _frente.Draw(alfa, desp, zbuf, areaVisible);

//        c += this._marcoI.Draw(alfa, desp, zbuf, areaVisible);
//        c += this._marcoM.Draw(alfa, desp, zbuf, areaVisible);
//        c += this._marcoD.Draw(alfa, desp, zbuf, areaVisible);

//        return c;
//    }


//    public void Mover(Vector2 mov)
//    {
//        this.Posicion += mov;
//    }

//    public void Posicionar(Vector2 pos)
//    {
//        this.Posicion = pos;
//    }

//    public void Zoom(Single zoom)
//    {
//        this.Escala += zoom;
//    }

//    public void Rotar(Single ang)
//    {
//        // No quiero implementar esto
//        return;
//    }

//    public void Estabilizar(Single ang)
//    {
//        // No quiero implementar esto
//        return;
//    }
//}

