using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeSolver
{
    class NumTrail : ICharTrail
    {
        /// <summary>
        /// Current value of the marker.
        /// </summary>
        int currentNum;
        
        /// <summary>
        /// Property used to store the display name of the character trail.
        /// </summary>
        public string TrailName { get; set; }

        /// <summary>
        /// Returns the display name of the character trail, or "NumTrail" if the display name is not defined.
        /// </summary>
        /// <returns>The display name of the character trail, or "NumTrail" if the display name is not defined</returns>
        public string Name()
        {
            if (TrailName != "" && TrailName != null)
            {
                return TrailName;
            }

            return "NumTrail";
        }
        /// <summary>
        /// Constructor for NumTrail, sets first value to 0.
        /// </summary>
        public NumTrail()
        {
            currentNum = 0;
        }

        /// <summary>
        /// Returns current character marker and increments it for the next call.
        /// </summary>
        /// <returns>Current character marker</returns>
        public char LeaveMarker()
        {
            int tempNum = currentNum;
            currentNum = (currentNum + 1) % 10;
            return Convert.ToString(tempNum)[0];
        }

        /// <summary>
        /// Returns the blocking character used to mark dead-ends.
        /// </summary>
        /// <returns>Blocking character.</returns>
        public char BlockChar()
        {
            return (char)1;
        }

        /// <summary>
        /// Retreats one character in the trail cycle.
        /// </summary>
        public void Decrement()
        {
            currentNum = (currentNum + 9) % 10;
        }

        /// <summary>
        /// Resets character to the first in the trail cycle.
        /// </summary>
        public void ResetMarker()
        {
            currentNum = 0;
        }
    }
}
