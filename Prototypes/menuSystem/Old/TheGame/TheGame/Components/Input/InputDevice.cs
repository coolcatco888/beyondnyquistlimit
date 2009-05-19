using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGame
{
    public abstract class InputDevice<T> : Component
    {
        protected InputDevice(GameScreen parent)
            : base(parent)
        {
        }

        public abstract T State { get; }
    }
}
