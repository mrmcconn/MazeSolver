using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    /// <summary>
    /// Abstraction of space that can be occupied in a maze.
    /// </summary>
    class MazeCell
    {
        bool[] _walls;
        bool[] _barrier;
        bool _visited;
        /// <summary>
        /// Creates new instance of MazeCell.
        /// </summary>
        public MazeCell()
        {
            _visited = false;
            _walls = new bool[4];
            _barrier = new bool[4];

            for(int i = 0; i < 4; i++)
            {
                _walls[i] = false;
                _barrier[i] = false;
            }
        }

        /// <summary>
        /// Flag to determine if the cell has been occupied.
        /// </summary>
        public bool WasVisited
        {
            get
            {
                return _visited;
            }
        }
        
        /// <summary>
        /// Sets the WasVisited flag to true.
        /// </summary>
        public void VisitCell()
        {
            _visited = true;
        }

        /// <summary>
        /// Permits movement to the adjacent cell to the north.
        /// </summary>
        public void BreakNorthWall()
        {
            breakWall(0);
        }

        /// <summary>
        /// Permits movement to the adjacent cell to the east.
        /// </summary>
        public void BreakEastWall()
        {
            breakWall(1);
        }

        /// <summary>
        /// Permits movement to the adjacent cell to the south.
        /// </summary>
        public void BreakSouthWall()
        {
            breakWall(2);
        }

        /// <summary>
        /// Permits movement to the adjacent cell to the west.
        /// </summary>
        public void BreakWestWall()
        {
            breakWall(3);
        }

        /// <summary>
        /// Returns true if the north wall is down.
        /// </summary>
        /// <returns>True if north wall is down, otherwise false.</returns>
        public bool NorthWallDown()
        {
            return wallDown(0);
        }

        /// <summary>
        /// Returns true if the east wall is down.
        /// </summary>
        /// <returns>True if east wall is down, otherwise false.</returns>
        public bool EastWallDown()
        {
            return wallDown(1);
        }

        /// <summary>
        /// Returns true if the south wall is down.
        /// </summary>
        /// <returns>True if south wall is down, otherwise false.</returns>
        public bool SouthWallDown()
        {
            return wallDown(2);
        }

        /// <summary>
        /// Returns true if the west wall is down.
        /// </summary>
        /// <returns>True if west wall is down, otherwise false.</returns>
        public bool WestWallDown()
        {
            return wallDown(3);
        }

        /// <summary>
        /// Specifies that the border to the north is an exterior wall of the maze.
        /// </summary>
        public void SetNorthBorder()
        {
            setBorder(0);
        }

        /// <summary>
        /// Specifies that the border to the east is an exterior wall of the maze.
        /// </summary>
        public void SetEastBorder()
        {
            setBorder(1);
        }

        /// <summary>
        /// Specifies that the border to the south is an exterior wall of the maze.
        /// </summary>
        public void SetSouthBorder()
        {
            setBorder(2);
        }

        /// <summary>
        /// Specifies that the border to the west is an exterior wall of the maze.
        /// </summary>
        public void SetWestBorder()
        {
            setBorder(3);
        }

        /// <summary>
        /// Returns true if the north wall is a border.
        /// </summary>
        /// <returns>True if north wall is a border, otherwise false.</returns>
        public bool BorderToNorth()
        {
            return wallIsBorder(0);
        }

        /// <summary>
        /// Returns true if the east wall is a border.
        /// </summary>
        /// <returns>True if east wall is a border, otherwise false.</returns>
        public bool BorderToEast()
        {
            return wallIsBorder(1);
        }

        /// <summary>
        /// Returns true if the south wall is a border.
        /// </summary>
        /// <returns>True if south wall is a border, otherwise false.</returns>
        public bool BorderToSouth()
        {
            return wallIsBorder(2);
        }

        /// <summary>
        /// Returns true if the west wall is a border.
        /// </summary>
        /// <returns>True if west wall is a border, otherwise false.</returns>
        public bool BorderToWest()
        {
            return wallIsBorder(3);
        }

        /// <summary>
        /// Reverses the state of a wall if it's not a border; 0-3 map to NESW, respectively.
        /// </summary>
        /// <param name="index">Integer between 0 and 3, which maps to north, east, south, or west, respectively.</param>
        public void FlipWall(int index)
        {
            if(!_barrier[index])
            {
                _walls[index] = !_walls[index];
            }
        }

        /// <summary>
        /// Breaks a wall if it's not a border.
        /// </summary>
        /// <param name="index">Integer between 0 and 3, which maps to north, east, south, or west, respectively.</param>
        private void breakWall(int index)
        {
            if (!_barrier[index])
            {
                _walls[index] = true;
            }
        }

        /// <summary>
        /// Returns true if the cell wall is down, false if not.
        /// </summary>
        /// <param name="index">Integer between 0 and 3, which maps to north, east, south, or west, respectively.</param>
        /// <returns>True if wall is down, false, otherwise.</returns>
        private bool wallDown(int index)
        {
            return _walls[index];
        }

        /// <summary>
        /// Specifies that the given wall is an exterior border.
        /// </summary>
        /// <param name="index">Integer between 0 and 3, which maps to north, east, south, or west, respectively.</param>
        private void setBorder(int index)
        {
            _barrier[index] = true;
        }

        /// <summary>
        /// Returns true if the cell wall is a border, false if not.
        /// </summary>
        /// <param name="index">Integer between 0 and 3, which maps to north, east, south, or west, respectively.</param>
        /// <returns>True if wall is a border, false if not.</returns>
        private bool wallIsBorder(int index)
        {
            return _barrier[index];
        }
    }
}
