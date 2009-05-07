using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TheGame.Components.Input
{
    /// <summary>
    /// Describes a sequences of buttons which must be pressed to active the move.
    /// A real game might add a virtual PerformMove() method to this class.
    /// </summary>
    class Move
    {
        private string name;

        // The sequence of button presses required to activate this move.
        private Buttons[] sequence;

        // Set this to true if the input used to activate this move may
        // be reused as a component of longer moves.
        private bool isSubMove;

        public string Name
        {
            set { name = value; }
            get { return name; }
        }

        public Buttons[] Sequence
        {
            set { sequence = value; }
            get { return sequence; }
        }

        public bool IsSubMove
        {
            set { isSubMove = value; }
            get { return isSubMove; }
        }


        public Move(string name, bool isSubMove, params Buttons[] sequence)
        {
            this.name = name;
            this.isSubMove = isSubMove;
            this.sequence = sequence;
        }

        public Move(string name, params Buttons[] sequence)
            : this(name, true, sequence)
        {
        }
    }
}
