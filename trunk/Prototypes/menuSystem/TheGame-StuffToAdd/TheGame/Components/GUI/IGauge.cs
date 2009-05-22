using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGame.Components.Display
{
    /// <summary>
    /// All gauge displays in say a HUD must implement this interface.
    /// This gauge ranges from 0 to MaxValue.
    /// </summary>
    interface IGauge
    {
        /// <summary>
        /// Maximum value of the gauge
        /// </summary>
        int MaxValue
        {
            set;
            get;
        }

        /// <summary>
        /// Current Gauge Value
        /// </summary>
        int CurrentValue
        {
            set;
            get;
        }

        /// <summary>
        /// Adds or subtracts a specified value from the gauge, 
        /// if amount is greater than gauge limit, then CurrentValue is the gauge limit,
        /// if amount is less than 0, then CurrentValue is zero.
        /// </summary>
        /// <param name="value"></param>
        void IncreaseDecreaseValue(int value);
    }
}
