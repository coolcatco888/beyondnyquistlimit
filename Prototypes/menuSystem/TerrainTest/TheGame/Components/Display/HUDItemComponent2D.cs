using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGame.Components.Display
{
    abstract class HUDItemComponent2D : DisplayComponent2D
    {
        protected IGauge healthBar;

        protected IGauge manaBar;

        protected INumeric level;

        public IGauge Healthbar
        {
            get { return healthBar; }
        }

        public IGauge ManaBar
        {
            get { return manaBar; }
        }

        public INumeric Level
        {
            get { return level; }
        }

        protected HUDItemComponent2D(GameScreen parent)
            : base(parent)
        {

        }

    }
}
