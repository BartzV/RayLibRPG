using Raylib_cs;
using RayLibRPG.Clases;
using RayLibRPG.Clases.Letras;
using RayLibRPG.Clases.Trigo.Barras;
using RayLibRPG.Logic.Reglas.Tags;
using System.Numerics;

namespace RayLibRPG.Logic.Personaje
{
    public class PersonajeHUD : IEntidad, ITransformable
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
                Vector2 rel = value - this._posicion;
                this.RichNombre.Posicion += rel;
                this.RichPCActual.Posicion += rel;
                this.RichPCMax.Posicion += rel;
                this.Diagonal.Posicion += rel;
                this.BarraPC.Posicion += rel;
                this._posicion = value;
            }
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

        // Letras
        public RichText RichNombre;
        public RichText RichPCActual;
        public RichText RichPCMax;
        public Letra Diagonal;
        public BarraVCParalelo BarraPC;

        public PersonajeHUD(Personaje pj, Vector2 pos)
        {
            this.PJ = pj;
            this.PCMaxActual = pj.PCMaxActual;
            this.PCActual = pj.PCActual;
            this._posicion = pos;
            // Letras
            this.RichNombre = new($"{{c0}}{pj.Nombre}", 
                [Color.White], 
                [LetraPaletaHelper.Arcoiris()], 
                pos, 
                Vector2.One, 
                0);
            this.RichPCActual = new(GetPCActual(this.PCActual),
                [PersonajeHUDColores.GetLetraSaludColor(this.PJ.Raza, 0, 0)],
                null,
                pos + new Vector2(8 * 16, 0),
                Vector2.One,
                0);
            this.RichPCMax = new(GetPCMax(this.PCMaxActual),
                [Color.White],
                null,
                pos + new Vector2(8 * 23, 0),
                Vector2.One, 0);
            this.Diagonal = new('/',
                pos + new Vector2(8 * 22, 0),
                Vector2.One,
                Color.White);
            this.BarraPC = new(pos + new Vector2(8 * 15, 2),
                new(8 * 11, 3),
                this.PCActual,
                this.PCMaxActual,
                PersonajeHUDColores.GetBarraFrenteSaludColor(this.PJ.Raza),
                PersonajeHUDColores.GetBarraFondoSaludColor(this.PJ.Raza));

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
            c += this.Diagonal.Draw(alfa, desp, zbuf, areaVisible);
            c += this.BarraPC.Draw(alfa, desp, zbuf, areaVisible);
            return c;
        }

        public void Update()
        {
            if (this.PCActual != this.PJ.PCActual)
            {
                this.PCActual = this.PJ.PCActual;
                this.RichPCActual = new(GetPCActual(this.PCActual), [PersonajeHUDColores.GetLetraSaludColor(this.PJ.Raza, 0, 0)], null, this.Posicion + new Vector2(8 * 16, 0), Vector2.One, 0);
            }
            if (this.PCMaxActual != this.PJ.PCMaxActual)
            {
                this.PCMaxActual = this.PJ.PCMaxActual;
                this.RichPCMax = new(GetPCMax(this.PCMaxActual), [Color.White], null, this.Posicion + new Vector2(8 * 23, 0), Vector2.One, 0);
            }

            this.RichNombre.Update();
            this.RichPCActual.Update();
            this.RichPCMax.Update();
            this.Diagonal.Update();
            this.BarraPC.Update();

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
            // Nada
        }

        public void Estabilizar(Single rad)
        {
            // Nada
        }

        public void AplicarZoom(Single delta)
        {
            // Nada
        }

        public void SetZoom(Single valor)
        {
            // Nada
        }

        public void SetFlip(Boolean x, Boolean y)
        {
            // Nada
        }


    }

    public static class PersonajeHUDColores
    {
        public static Color GetLetraSaludColor(RazaTag tag, Single min, Single max)
        {

            switch (tag)
            {
                case RazaTag.Humano:
                    return new Color(255, 230, 230);
                case RazaTag.Desconocido:
                    break;
                case RazaTag.Bestia:
                    break;
                case RazaTag.NoMuerto:
                    break;
                case RazaTag.Espectro:
                    break;
                default:
                    break;
            }
            return Color.White;
        }

        public static Color[] GetBarraFrenteSaludColor(RazaTag tag)
        {
            switch (tag)
            {
                case RazaTag.Humano:
                    return [
                        new Color(255, 230, 230),
                        new Color(255, 179, 179),
                        new Color(255, 204, 204),
                        new Color(255, 128, 128),
                        ];
                case RazaTag.Desconocido:
                    break;
                case RazaTag.Bestia:
                    break;
                case RazaTag.NoMuerto:
                    break;
                case RazaTag.Espectro:
                    break;
                default:
                    break;
            }
            return [Color.White, Color.White, Color.White, Color.White];
        }
        public static Color[] GetBarraFondoSaludColor(RazaTag tag)
        {
            switch (tag)
            {
                case RazaTag.Humano:
                    return [
                        new Color(64, 0, 0),
                        new Color(48, 0, 0),
                        new Color(48, 0, 0),
                        new Color(32, 0, 0),
                        ];
                case RazaTag.Desconocido:
                    break;
                case RazaTag.Bestia:
                    break;
                case RazaTag.NoMuerto:
                    break;
                case RazaTag.Espectro:
                    break;
                default:
                    break;
            }
            return [Color.Black, Color.Black, Color.Black, Color.Black];
        }
    }

    //public class PersonajeSprite : IDesplazable, IRenderizable, IActualizable
    //{
    //    public Sprite2D[] Sprites { get; set; }

    //    public PersonajeSprite()
    //    {

    //    }

    //}
}
