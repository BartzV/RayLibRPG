namespace RayLibRPG.Clases.Inputs;

/// <summary>
/// Clase base para los lectores.
/// </summary>
public abstract class LectorInput
{
    protected Int32 DELAY_INICIAL = 1;
    protected Int32 DELAY_REPETICION = 1;
    public abstract Boolean Procesar();
}
