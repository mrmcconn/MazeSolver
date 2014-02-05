using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeSolver
{
    /// <summary>
    /// Interface for character trails
    /// </summary>
    interface ICharTrail
    {
        /// <summary>
        /// Returns the display name of the character trail.
        /// </summary>
        /// <returns>Display name of the character trail</returns>
        string Name();

        /// <summary>
        /// Used to mark last occupied space of a MazeWalker, increments markers in the character trail cycle.
        /// </summary>
        /// <returns>Trail marking character</returns>
        char LeaveMarker();

        /// <summary>
        /// Returns character used to mark dead ends, used to clear the maze for display.
        /// </summary>
        /// <returns>Character used to mark dead ends</returns>
        char BlockChar();

        /// <summary>
        /// Reverses by one character in the character trail cycle.
        /// </summary>
        void Decrement();

        /// <summary>
        /// Resets character to the first in the trail cycle.
        /// </summary>
        void ResetMarker();
    }
}
