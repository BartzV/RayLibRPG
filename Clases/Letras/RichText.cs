using Raylib_cs;
using System.Numerics;

namespace RayLibRPG.Clases.Letras;

/// <summary>
/// Los comandos para el Richtext son:
/// <list type="bullet">
/// <item>{c#} para dibujar caracteres de color simple.</item>
/// <item>{p#} para dibujar caracteres cuyo colores cambia.</item>
/// <item>{s} para salto de línea.</item>
/// <item>{e#} para los efectos.</item>
/// </list>
/// </summary>
public class RichText : IActualizable, IRenderizable, IDesplazable
{
    // Colores
    public Color[] Colores;         // Para los Letra.
    public Color[][] Paletas;       // Para los LetraPaleta.
    // Posiciones y Transformaciones
    protected Vector2 _posicion;        // Posición del primer caracter.
    public Vector2 Amplitudes;      // Para TODAS las letras. No nos venimos con chiquitas.
    public Vector2 Rotaciones;      // Para TODAS las letras. No rota los textos, sólo las letras. No implementado.
    public Vector2 Espaciado;      // Para TODAS las letras. No rota los textos, sólo las letras. No implementado.
    // Tiempo de Vida y Tiempos
    public Int64 TiempoActivo;
    public Int32 VelTexto = 8;
    // Statics para optimizaciones
    private static Color[] COLOR_NULL = new Color[1] { Color.White };
    private static Color[][] PALETTE_NULL = new Color[1][] { [Color.White] };
    // Contenedores
    public List<Letra> Letras;

    public Vector2 Posicion 
    { 
        get => this._posicion; 
        set 
        {
            Vector2 desp = value - this._posicion;

            this._posicion = value;
            for(Int32 i = 0; i < this.Letras.Count; i++)
            {
                this.Letras[i].Posicion += desp;
            }
        } 
    }
    public float ZBuffer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


    public RichText(String palabra, Color[]? colores, Color[][]? paletas, Vector2 posicion, Vector2 amplitud, Vector2? espaciado = null)
    {
        this.Letras = new List<Letra>();
        this.Posicion = posicion;
        if (colores is null || colores.Length == 0)
        {
            this.Colores = COLOR_NULL;
        }
        else
        {
            this.Colores = colores;
        }
        if (paletas is null || paletas.Length == 0)
        {
            this.Paletas = PALETTE_NULL;
        }
        else
        {
            this.Paletas = paletas;
        }
        this.Espaciado = espaciado ?? Letra.TAM_LETRA;
        // Sin uso de momento.
        this.Amplitudes = amplitud;
        // Estados
        Int32 colorActual = 0;
        Int32 paletaActual = 0;
        EfectoLetra efectoActual = EfectoLetra.Ninguno;
        Vector2 posActual = posicion;
        Func<Char, Color, Color[], EfectoLetra, Vector2, Vector2, Int32, Letra> tipoActual = GenerarLetra;

        for (Int32 i = 0; i < palabra.Length; i++)
        {
            Char c = palabra[i];
            // Es un comando!!! Corran!
            if (c == '{')
            {
                i++;
                // Failsafe
                if (i >= palabra.Length)
                    return;
                c = palabra[i];
                switch (c)
                {
                    case 'c':
                        i++;
                        // Failsafe otra vez...
                        if (i >= palabra.Length)
                            return;
                        // Si no hay número, me retorna el 0 como failsafe.
                        colorActual = Math.Clamp(Convert.ToInt32(palabra[i]) - '0', 0, this.Colores.Length - 1);
                        tipoActual = GenerarLetra;
                        // Nos saltamos el '}'
                        i++;
                        continue;
                    case 'p':
                        i++;
                        // Failsafe otra vez...
                        if (i >= palabra.Length)
                            return;
                        paletaActual = Math.Clamp(Convert.ToInt32(palabra[i]) - '0', 0, this.Paletas.Length - 1);
                        tipoActual = GenerarLetraPaleta;
                        // Nos saltamos el '}'
                        i++;
                        continue;
                    case 's':
                        // Nos saltamos el '}'
                        i++;
                        posActual.X = posicion.X;                          // CR
                        posActual.Y += this.Espaciado.Y * amplitud.Y;      // LF
                        continue;
                    case 'e':
                        i++;
                        // Failsafe otra vez...
                        if (i >= palabra.Length)
                            return;
                        efectoActual = (EfectoLetra)Convert.ToInt32(palabra[i]) - '0';
                        // Nos saltamos el '}'
                        i++;
                        continue;
                    default:
                        continue;

                }
            }

            Letra l = tipoActual(c, this.Colores[colorActual], this.Paletas[paletaActual], efectoActual, posActual, amplitud, i);
            this.Letras.Add(l);
            posActual.X += this.Espaciado.X * amplitud.X;
        }

    }


    public static Letra GenerarLetra(Char letra, Color color, Color[] paleta, EfectoLetra efecto, Vector2 posicion, Vector2 amplitud, Int32 alfa)
    {
        Letra res = new Letra(letra, posicion, amplitud, color, alfa);
        res.Efecto = efecto;
        return res;
    }
    public static Letra GenerarLetraPaleta(Char letra, Color color, Color[] paleta, EfectoLetra efecto, Vector2 posicion, Vector2 amplitud, Int32 alfa)
    {
        LetraPaleta res = new LetraPaleta(letra, posicion, amplitud, paleta, 7, alfa);
        res.Efecto = efecto;
        return res;
    }

    public void Reiniciar() => this.TiempoActivo = 0;

    public void Draw(float alfa)
    {
        this.Letras.ForEach(x => x.Draw(alfa));
    }

    public void Draw(float alfa, Vector2 desp, float zbuf)
    {
        for (int i = 0; i < this.Letras.Count; i++)
        {
            if ((i * this.VelTexto) <= this.TiempoActivo)
                this.Letras[i].Draw(alfa, desp, zbuf);
        }

    }

    public void Update()
    {
        this.Letras.ForEach(x => x.Update());
        this.TiempoActivo++;
    }

    public void Mover(Vector2 mov)
    {
        this.Posicion += mov;
    }

    public void Posicionar(Vector2 pos)
    {
        this.Posicion = pos;
    }

    public void Zoom(float zoom)
    {
        throw new NotImplementedException();
    }
}
