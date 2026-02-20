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
public class RichText : IActualizable, IRenderizable
{
    // Colores
    public Color[] Colores;         // Para los Letra.
    public Color[][] Paletas;       // Para los LetraPaleta.
    // Posiciones y Transformaciones
    public Vector2 Posicion;        // Posición del primer caracter.
    public Vector2 Amplitudes;      // Para TODAS las letras. No nos venimos con chiquitas.
    public Vector2 Rotaciones;      // Para TODAS las letras. No rota los textos, sólo las letras. No implementado.
    public Vector2 Espaciado;      // Para TODAS las letras. No rota los textos, sólo las letras. No implementado.
    // Contenedores
    public List<Letra> Letras;

    public RichText(String palabra, Color[] colores, Color[][] paletas, Vector2 posicion, Vector2 amplitud, Vector2? espaciado = null)
    {
        this.Letras = new List<Letra>();
        this.Posicion = posicion;
        this.Colores = colores;
        this.Paletas = paletas;
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
                        colorActual = Math.Clamp(Convert.ToInt32(palabra[i]) - '0', 0, colores.Length);
                        tipoActual = GenerarLetra;
                        // Nos saltamos el '}'
                        i++;
                        continue;
                    case 'p':
                        i++;
                        // Failsafe otra vez...
                        if (i >= palabra.Length)
                            return;
                        paletaActual = Math.Clamp(Convert.ToInt32(palabra[i]) - '0', 0, paletas.Length);
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

            Letra l = tipoActual(c, colores[colorActual], paletas[paletaActual], efectoActual, posActual, amplitud, i);
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

    public void Draw(float alfa)
    {
        this.Letras.ForEach(x => x.Draw(alfa));
    }

    public void Draw(float alfa, Vector2 desp, float zbuf)
    {
        this.Letras.ForEach(x => x.Draw(alfa, desp, zbuf));
    }

    public void Update()
    {
        this.Letras.ForEach(x => x.Update());
    }
}
