using Raylib_cs;
using RayLibRPG.Clases;
using RayLibRPG.Clases.Letras;
using System.Numerics;

namespace RayLibRPG.Logic.Personaje
{
    public class PersonajeHUD : IRenderizable, IActualizable, ITransformable
    {
        private Boolean _eliminado = false;
        public Boolean Eliminado
        {
            get => _eliminado;
            set
            {
                _eliminado = value;

            }
        }
        private Single _prioridad;
        public Single Prioridad
        {
            get => _prioridad;
            set => _prioridad = value;

        }

        protected Boolean _activo = true;
        public Boolean Activo
        {
            get => this._activo;
            set => this._activo = value;
        }
        protected Single _capaProfundidad;
        public Single CapaPrioridad 
        { 
            get => this._capaProfundidad;
            set 
            {
                this._capaProfundidad = value;
                throw new NotImplementedException();
            }
        }

        public Int32 PCActual;
        public Int32 PCMaxActual;     // El escrito
        public Personaje PJ;

        // Posición
        protected Vector2 _posicion;
        public Vector2 Posicion
        {
            get => this._posicion;
            set
            {
                this._posicion = value;
                this.RichNombre.Posicion = value;
                this.RichPCActual.Posicion = value;
                this.RichPCMax.Posicion = value;
                this.Barra.Posicion = value;
            }
        }

        public Single ProfundidadZ { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Single Rotacion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector2 Amplificacion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        // Letras
        public RichText RichNombre;
        public RichText RichPCActual;
        public RichText RichPCMax;
        public Letra Barra;

        public PersonajeHUD(Personaje pj, Vector2 pos)
        {
            this.PJ = pj;
            this.PCMaxActual = pj.PCMaxActual;
            this.PCActual = pj.PCActual;
            this.Posicion = pos;
            // Letras
            this.RichNombre = new($"{{c0}}{pj.Nombre}", [this.PJ.ColorPrimario], [LetraPaletaHelper.Arcoiris()], pos, Vector2.One, 0);
            this.RichPCActual = new(GetPCActual(this.PCActual), [PersonajeHUD.Salud], null, pos + new Vector2(8 * 16, 0), Vector2.One, 0);
            this.RichPCMax = new(GetPCMax(this.PCMaxActual), [Color.White], null, pos + new Vector2(8 * 23, 0), Vector2.One, 0);
            this.Barra = new('/', pos + new Vector2(8 * 22, 0), Vector2.One, Color.White);
        }
        // A lo sumo son 6 caracteres
        public static String GetPCActual(Int32 pc) => $"{{c0}}{pc / 10,4}.{pc % 10}";
        public static String GetPCMax(Int32 pc) => $"{{c0}}{pc / 10,4}";

        public Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible)
        {
            Int32 c = 0;
            c += this.RichNombre.Draw(alfa, desp, zbuf, areaVisible);
            c += this.RichPCActual.Draw(alfa, desp, zbuf, areaVisible);
            c += this.RichPCMax.Draw(alfa, desp, zbuf, areaVisible);
            c += this.Barra.Draw(alfa, desp, zbuf, areaVisible);
            return c;
        }

        public void Update()
        {
            if (this.PCActual != this.PJ.PCActual)
            {
                this.PCActual = this.PJ.PCActual;
                this.RichPCActual = new(GetPCActual(this.PCActual), [PersonajeHUD.Salud], null, this.Posicion + new Vector2(8 * 16, 0), Vector2.One, 0);
            }
            if (this.PCMaxActual != this.PJ.PCMaxActual)
            {
                this.PCMaxActual = this.PJ.PCMaxActual;
                this.RichPCMax = new(GetPCMax(this.PCMaxActual), [Color.White], null, this.Posicion + new Vector2(8 * 23, 0), Vector2.One, 0);
            }

            this.RichNombre.Update();
            this.RichPCActual.Update();
            this.RichPCMax.Update();
            this.Barra.Update();

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

        public static Color Salud = new Color(255, 230, 230);
    }

    //public class PersonajeSprite : IDesplazable, IRenderizable, IActualizable
    //{
    //    public Sprite2D[] Sprites { get; set; }

    //    public PersonajeSprite()
    //    {

    //    }

    //}
}
