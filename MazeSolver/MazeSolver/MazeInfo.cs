using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeSolver
{
    /// <summary>
    /// Structure for transferring data about the maze to the MazeWalker
    /// </summary>
    class MazeInfo
    {
        char[] mazeArray;
        int xDim;
        int yDim;

        public MazeInfo()
        {
        }

        /// <summary>
        /// Method for loading data from a Maze to MazeInfo for transfer to a MazeWalker.
        /// </summary>
        /// <param name="inMaze">Maze with data for transfer</param>
        public void LoadMaze(Maze inMaze)
        {
            xDim = 1 + 2 * inMaze.Width;
            yDim = 1 + 2 * inMaze.Height;
            mazeArray = new char[xDim * yDim];
            for (int i = 0; i < yDim; i++)
            {
                for (int j = 0; j < xDim; j++)
                {
                    mazeArray[i * xDim + j] = inMaze.Cell(i, j);
                }
            }
        }

        /// <summary>
        /// Width of the sending Maze
        /// </summary>
        public int Width
        {
            get { return xDim; }
        }

        /// <summary>
        /// Height of the sending Maze
        /// </summary>
        public int Height
        {
            get { return yDim; }
        }

        /// <summary>
        /// Character at a particular index of the array
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public char Cell1D(int index)
        {
            return mazeArray[index];
        }
    }
}
