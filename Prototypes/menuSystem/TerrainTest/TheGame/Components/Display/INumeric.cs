using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGame.Components.Display
{
    /// <summary>
    /// All components who's purpose is to store one numerical component such as
    /// a character's level must implement this interface
    /// </summary>
    interface INumeric
    {
        /// <summary>
        /// Value of this component
        /// </summary>
        int Value
        {
            set;
            get;
        }
    }
}
