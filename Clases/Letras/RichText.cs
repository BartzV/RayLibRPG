using Raylib_cs;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace RayLibRPG.Clases.Letras;

/// <summary>
/// Los comandos para el Richtext son:
/// <list type="bullet">
/// <item>{c#} para dibujar caracteres de color simple.</item>
/// <item>{p#} para dibujar caracteres cuyo colores cambia.</item>
/// <item>{s} para salto de línea.</item>
/// <item>{e#} para los efectos.</item>
/// <item>{t##} para espacios a tabular (para no dibujar espacios vacíos)</item>
/// </list>
/// </summary>
public class RichText : IEntidad, ITransformable
{
    // Estado
    public Boolean _eliminado = false;
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

    // Colores
    public Color[] Colores;             // Para los Letra.
    public Color[][] Paletas;           // Para los LetraPaleta.
    // Posiciones y Transformaciones
    protected Vector2 _posicion;        // Posición del primer caracter.
    protected Single _prioridad;
    protected Single _rotacion;
    protected Vector2 _amplificacion;
    // Tratar de rehacer esto
    protected Vector2 Espaciado;
    // Reservado para efectos Fade In.
    public Single ProfundidadZ
    {
        get => this._prioridad;
        set
        {
            this._prioridad = value;
            for (Int32 i = 0; i < this.Letras.Count; i++)
            {
                this.Letras[i].ProfundidadZ = value;
            }
        }
    }
    public Vector2 Amplificacion
    {
        get => this._amplificacion;
        set => this._amplificacion = Vector2.Abs(value);
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

    public Single Rotacion
    {
        get => this._rotacion;
        set => this._rotacion = value;
    }

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
            for (Int32 i = 0; i < this.Letras.Count; i++)
            {
                this.Letras[i].Posicion += desp;
            }
        }
    }


    public RichText(String palabra, Color[]? colores, Color[][]? paletas, Vector2 posicion, Vector2 amplitud, Int32 velTexto = 1, Vector2? espaciado = null)
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
        this.VelTexto = velTexto;
        this.Espaciado = espaciado ?? Letra.TAM_LETRA;
        // Estados
        Int32 colorActual = 0;
        Int32 paletaActual = 0;
        EfectoLetra efectoActual = EfectoLetra.Ninguno;
        Vector2 posActual = posicion;
        Func<Char, Color, Color[], EfectoLetra, Vector2, Vector2, Int32, Letra> tipoActual = GenerarLetra;
        Int64 cronometroInterno = 0; // Este es nuestro "puntero" de tiempo

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
                    // Color Simple
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
                    // Color de Paleta
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
                    // Salto de línea
                    case 's':
                        // Nos saltamos el '}'
                        i++;
                        posActual.X = posicion.X;                          // CR
                        posActual.Y += this.Espaciado.Y * amplitud.Y;      // LF
                        continue;
                    // Efectos: 0 = Normal, 1 = Tembleque, 2 = Oleaje
                    case 'e':
                        i++;
                        // Failsafe otra vez...
                        if (i >= palabra.Length)
                            return;
                        efectoActual = (EfectoLetra)Convert.ToInt32(palabra[i]) - '0';
                        // Nos saltamos el '}'
                        i++;
                        continue;
                    // Tab espacios, para no meter espacios a lo loco
                    case 't':
                        i += 2;
                        // Failsafe otra vez...
                        if (i >= palabra.Length)
                            return;
                        Int32 saltos = Math.Max(Convert.ToInt32(palabra[(i - 1)..(i + 1)]), 0);
                        posActual.X += this.Espaciado.X * amplitud.X * saltos;
                        // Nos saltamos el '}'
                        i++;
                        continue;
                    // Íconos (y funciona!)
                    case 'i':
                        i += 2;
                        // Failsafe otra vez...
                        if (i >= palabra.Length)
                            return;
                        Int16 simbolo = Math.Max(Convert.ToInt16(palabra[(i - 1)..(i + 1)], 16), (Int16)0);
                        c = (Char)(0xFF00 | simbolo);
                        // Nos saltamos el '}'
                        i++;
                        break;
                    case 'w': // {w05} -> Espera 50 frames (o lo que quieras)
                        i += 2;
                        Int32 espera = (Convert.ToInt32(palabra[(i - 1)..(i + 1)])) * 10; // Multiplicador de pausa
                        cronometroInterno += espera;
                        i++;
                        continue;
                    default:
                        continue;

                }
            }
            // Creamos y agregamos la letra
            Letra l = tipoActual(c, this.Colores[colorActual], this.Paletas[paletaActual], efectoActual, posActual, amplitud, i);
            l.MomentoAparicion = cronometroInterno;
            this.Letras.Add(l);
            // Seguimos...
            posActual.X += this.Espaciado.X * amplitud.X;
            cronometroInterno += this.VelTexto;
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
    public void Terminar() => this.TiempoActivo = Int32.MaxValue;

    public Int32 Draw(Single alfa, Vector2 desp, Single zbuf, Rectangle areaVisible)
    {
        Int32 counter = 0;
        for (Int32 i = 0; i < this.Letras.Count; i++)
        {
            // Ya no comparamos con el índice "i", sino con su propio tiempo
            if (this.Letras[i].MomentoAparicion <= this.TiempoActivo)
            {
                counter += this.Letras[i].Draw(alfa, desp, zbuf, areaVisible);
            }
        }
        return counter;
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

    // No le des bola, no voy a implementar esta poronga.
    public void Rotar(Single ang)
    {
        throw new NotImplementedException();
    }

    public void Estabilizar(Single ang)
    {
        throw new NotImplementedException();
    }

    public void AplicarZoom(Single zoom)
    {
        this.ProfundidadZ = Math.Max(0, this.ProfundidadZ);
    }

    public void SetZoom(Single zoom)
    {
        this.ProfundidadZ = zoom;
    }

    public void SetFlip(Boolean x, Boolean y)
    {
        throw new NotImplementedException();
    }
}
