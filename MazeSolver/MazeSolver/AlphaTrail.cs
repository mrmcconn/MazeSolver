using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeSolver
{
    /// <summary>
    /// Character trail of lower-case alphabetic characters
    /// </summary>
    class AlphaTrail : ICharTrail
    {
        private char currentChar;

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
            if (TrailName != "")
            {
                return TrailName;
            }

            return "AlphaTrail";
        }

        /// <summary>
        /// Constructor for AlphaTrail, sets first character to 'a'.
        /// </summary>
        public AlphaTrail()
        {
            currentChar = 'a';
        }

        /// <summary>
        /// Returns current character marker and increments it for the next call.
        /// </summary>
        /// <returns>Current character marker</returns>
        public char LeaveMarker()
        {
            char tempChar = currentChar;
            if (currentChar == 'z')
            {
                currentChar = 'a';
            }
            else
            {
                currentChar++;
            }

            return tempChar;
        }
        /// <summary>
        /// Returns the blocking character used to mark dead-ends.
        /// </summary>
        /// <returns>Blocking character (+)</returns>
        public char BlockChar()
        {
            return (char)1;
        }

        /// <summary>
        /// Retreats one character in the trail cycle.
        /// </summary>
        public void Decrement()
        {
            if (currentChar == 'a')
            {
                currentChar = 'z';
            }
            else
            {
                currentChar--;
            }
        }

        /// <summary>
        /// Resets character to the first in the trail cycle.
        /// </summary>
        public void ResetMarker()
        {
            currentChar = 'a';
        }
    }
}
