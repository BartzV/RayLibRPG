using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayLibRPG.Clases.Trigo.Barras;

public class BarraVCParalelo : IEntidad, ITransformable
{
    private Boolean _activo = true;
    public bool Activo
    {
        get => this._activo;
        set => this._activo = value;
    }
    private Boolean _eliminado = false;
    public bool Eliminado
    {
        get => this._eliminado;
        set
        {
            this._eliminado = value;
            if (value is false)
            {
                this._activo = false;
            }
        }
    }
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
    private Vector2 _posicion;
    public Vector2 Posicion
    {
        get => this._posicion;
        set
        {
            if (this._posicion == value)
                return;
            Vector2 dif = value - this._posicion;
            this.PolFrente.Posicion += dif;
            this.PolFondo.Posicion += dif;
            this._posicion = value;
        }
    }
    private Single _profundidad;
    public Single ProfundidadZ
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    private Single _rotacion;
    public Single Rotacion
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    private Vector2 _amplificacion;
    public Vector2 Amplificacion
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public event Action? OnCambioPrioridad;

    // --------------------------------------------------
    // Datos de la Barra
    // --------------------------------------------------
    protected Single Ancho { get; set; }
    protected Single Largo { get; set; }
    private Single _valorActual;
    protected Single ValorActual
    {
        get => this._valorActual;
        set
        {
            if (this._valorActual != value)
            {
                this._valorActual = value;
                this._debeActualizar = true;
            }
        }
    }
    private Single _valorMax;
    protected Single ValorMax
    {
        get => this._valorMax;
        set
        {
            if (this._valorMax != value)
            {
                this._valorMax = value;
                this._debeActualizar = true;
            }
        }
    }

    // Datos de la Barra
    protected Poligono2DVertexColor PolFrente;
    protected Poligono2DVertexColor PolFondo;

    // Optimización de Cálculos
    private Boolean _debeActualizar = true;
    // --------------------------------------------------
    public BarraVCParalelo(Vector2 pos, Vector2 dim, Single valAct, Single valMax, Color[] frente, Color[] fondo)
    {
        Single pActual = valAct / valMax * (dim.X - dim.Y);
        pActual = Math.Clamp(pActual, 0, valMax);

        (Vector2 v, Color c)[] frenteAux = new (Vector2, Color)[4];
        (Vector2 v, Color c)[] fondoAux = new (Vector2, Color)[4];
        // Como va a ser un Paralelogramo, el "centro" va a ser C.
        /*         ___________
         *        /          /
         *     C /          /
         *      /__________/
        */
        // Primer Vértice:
        frenteAux[0].v = new Vector2(dim.Y, -dim.Y / 2);
        frenteAux[0].c = frente[0];
        fondoAux[0].v = new Vector2(dim.Y, -dim.Y / 2);
        fondoAux[0].c = fondo[0];
        // Segundo Vértice:
        frenteAux[1].v = new Vector2(0, dim.Y / 2);
        frenteAux[1].c = frente[1];
        fondoAux[1].v = new Vector2(0, dim.Y / 2);
        fondoAux[1].c = fondo[1];
        // Tercer Vértice (variable)
        frenteAux[2].v = new Vector2(dim.Y + pActual, -dim.Y / 2);
        frenteAux[2].c = frente[2];
        fondoAux[2].v = new Vector2(dim.X, -dim.Y / 2);
        fondoAux[2].c = fondo[2];
        // Cuarto Vértice (variable)
        frenteAux[3].v = new Vector2(pActual, dim.Y / 2);
        frenteAux[3].c = frente[3];
        fondoAux[3].v = new Vector2(dim.X - dim.Y, dim.Y / 2);
        fondoAux[3].c = fondo[3];

        this.PolFondo = new(fondoAux, pos);
        this.PolFondo.CapaPrioridad = 1001;
        this.PolFrente = new(frenteAux, pos);
        this.PolFrente.CapaPrioridad = 1000;

        this._posicion = pos;
    }

    protected void Actualizar()
    {

    }

    public Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible)
    {
        Int32 counter = 0;
        counter += this.PolFondo.Draw(alfa, desp, zbuf, areaVisible);
        counter += this.PolFrente.Draw(alfa, desp, zbuf, areaVisible);
        return counter;
    }

    public void Update()
    {
        this.PolFondo.Update();
        this.PolFrente.Update();
    }

    public void Mover(Vector2 mov)
    {
        
    }

    public void Posicionar(Vector2 pos)
    {
        
    }

    public void Rotar(float rad)
    {
        
    }

    public void Estabilizar(float rad)
    {
        
    }

    public void AplicarZoom(float delta)
    {
        
    }

    public void SetZoom(float valor)
    {
        
    }

    public void SetFlip(bool x, bool y)
    {
        
    }
}
