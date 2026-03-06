using Raylib_cs;
using RayLibRPG.Clases;
using RayLibRPG.Clases.Letras;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayLibRPG.Logic.Personaje
{
    public class Personaje
    {
        public String Id { get; set; }
        public String Nombre { get; set; }
        public Color ColorPrimario { get; set; }

        // Registros comunes
        // PC = Puntos Corazón, o Puntos Cuarzo
        public Int32 PCMax { get; set; }

        // Por cada combate, se va perdiendo el "máximo" hasta descansar.
        public Int32 PCActual { get; set; }
        public Int32 PCMaxActual { get; set; }
        // TODO: Resto jaja



    }

    public class PersonajeHUD : IRenderizable, IActualizable, IDesplazable
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
        Vector2 IDesplazable.Posicion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Single Escala { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Int32 PCActual;
        public Int32 PCMaxActual;     // El escrito
        public Personaje PJ;

        // Posición
        public Vector2 Posicion;

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
            this.RichNombre.Update();
            this.RichPCActual.Update();
            this.RichPCMax.Update();
            this.Barra.Update();
        }

        public void Mover(Vector2 mov)
        {
            this.RichNombre.Mover(mov);
        }

        public void Posicionar(Vector2 pos)
        {
            throw new NotImplementedException();
        }

        public void Zoom(Single zoom)
        {
            throw new NotImplementedException();
        }

        public void Rotar(Single ang)
        {
            throw new NotImplementedException();
        }

        public void Estabilizar(Single ang)
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
