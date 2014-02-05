using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeSolver
{
    class ASnakeTrail : ICharTrail
    {
        /// <summary>
        /// Property to store the name of the instance of ASnakeTrail
        /// </summary>
        public string TrailName { get; set; }

        public string Name()
        {
            if (TrailName != "")
            {
                return TrailName;
            }

            return "ASnakeTrail";
        }

        public char LeaveMarker()
        {
            return '@';
        }

        public char BlockChar()
        {
            return (char)1;
        }

        public void Decrement()
        {
            // Not used for a non-sequential trail
        }

        public void ResetMarker()
        {
            // Not used for a non-sequential trail
        }
    }
}
