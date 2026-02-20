using RayLibRPG.Clases.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayLibRPG.Clases.Escenario
{
    public abstract class EscenarioEngine
    {
        public virtual void Update()
        {

        }

        public virtual void Draw(Single alfa)
        {
            ScreenManager.DibujarTodo(alfa, EngineManager.FramesTranscurridos);
        }

        public virtual void Initialize()
        {

        }
    }
}
