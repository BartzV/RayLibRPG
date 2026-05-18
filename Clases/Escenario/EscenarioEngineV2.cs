using System;
using System.Collections.Generic;
using System.Text;

namespace RayLibRPG.Clases.Escenario;

internal class EscenarioEngineV2 : IDisposable
{
    protected Boolean _eliminado = false;

    public void Dispose()
    {
        if (this._eliminado) return;
        //for (Int32 i = 0; i < this.Capas.Count; i++)
        //{
        //    this.Capas[i].Dispose();
        //}
        this._eliminado = true;
        // Esto es para que el GC no llame al finalizador, ya que ya liberamos los recursos manualmente.
        GC.SuppressFinalize(this);
    }
}
