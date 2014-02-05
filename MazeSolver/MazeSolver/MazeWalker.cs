using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MazeSolver
{
    /// <summary>
    /// Object used to compute the solution of a maze.
    /// </summary>
    class MazeWalker : IObserver<MazeInfo>
    {
        // Current position coordinates
        private int x_pos;
        private int y_pos;
        // Stack of prior coordinates
        private Stack<int> x_moves;
        private Stack<int> y_moves;
        // Dimensions of the Maze
        private int? mazeX;
        private int? mazeY;
        // Stores Maze characters
        private char[] maze;
        // Boolean variables to expedite solution computations
        private bool solutionFound;
        private bool solutionAttempted;
        // Flag to determine if a maze exists
        private bool mazeExists;
        // Interfaces for compositions
        private IDisposable targetMaze;
        private ICharTrail charTrail;

        /// <summary>
        /// MazeWalker constructor.
        /// </summary>
        /// <param name="trail">New instance of MazeWalker</param>
        public MazeWalker(ICharTrail trail = null)
        {
            x_pos = 1;
            y_pos = 1;
            x_moves = new Stack<int>();
            y_moves = new Stack<int>();
            solutionFound = false;
            solutionAttempted = false;
            mazeExists = false;

            if (trail == null || !(trail is ICharTrail))
            {
                charTrail = new AlphaTrail();
            }
            else
            {
                charTrail = trail;
            }
        }

        public string Name
        {
            get
            {
                return charTrail.Name();
            }
        }

        /// <summary>
        /// Stores Maze data in a MazeWalker array.
        /// </summary>
        /// <param name="inMaze">Maze from which to retrieve data</param>
        public void BuildMaze(Maze inMaze)
        {
            if (!mazeX.HasValue)
            {
                mazeX = inMaze.Width;
            }
            if (!mazeY.HasValue)
            {
                mazeY = inMaze.Height;
            }

            maze = new char[mazeX.Value * mazeY.Value];

            for (int y = 0; y < mazeY; y++)
            {
                for (int x = 0; x < mazeX; x++)
                {
                    maze[x + mazeX.Value * y] = inMaze.Cell(x, y);
                }
            }

            mazeExists = true;
        }
        /// <summary>
        /// Returns a string that represents the maze, with or without solution.
        /// </summary>
        /// <returns>A string that represents the maze, with or without solution.</returns>
        public string PrintMaze()
        {
            string displayMaze = "";
            if (mazeX.HasValue && mazeY.HasValue)
            {
                prepMaze();
                for (int i = 0; i < mazeY.Value; i++)
                {
                    for (int j = 0; j < mazeX.Value; j++)
                    {
                        displayMaze += maze[mazeX.Value * i + j];
                    }

                    displayMaze += "\n";
                }
            }

            return displayMaze;
        }
        /// <summary>
        /// Attempts to solve maze.
        /// </summary>
        /// <returns>true if solution was found, false if no solution was found.</returns>
        public bool SolveMaze()
        {
            if (!mazeExists)
            {
                return false;
            }

            if (solutionAttempted)
            {
                return solutionFound;
            }
            if (x_pos == (mazeX - 2) && y_pos == (mazeY - 2))
            {
                markTrail();
                solutionAttempted = true;
                solutionFound = true;
                return true;
            }
            else if (canMoveEast())
            {
                markTrail();
                moveEast();
                return SolveMaze();
            }
            else if (canMoveSouth())
            {
                markTrail();
                moveSouth();
                return SolveMaze();
            }
            else if (canMoveWest())
            {
                markTrail();
                moveWest();
                return SolveMaze();
            }
            else if (canMoveNorth())
            {
                markTrail();
                moveNorth();
                return SolveMaze();
            }
            else if (x_moves.Count > 0 && y_moves.Count > 0)
            {
                charTrail.Decrement();
                setPos(charTrail.BlockChar());
                x_pos = x_moves.Pop();
                y_pos = y_moves.Pop();
                return SolveMaze();
            }
            else
            {
                setPos(charTrail.BlockChar());
                solutionAttempted = true;
                solutionFound = false;
                return false;
            }
        }

        /// <summary>
        /// Clears maze for display; removes block characters.
        /// </summary>
        private void prepMaze()
        {
            for (int i = 0; i < (mazeX.Value * mazeY.Value - 1); i++)
            {
                if (maze[i] == charTrail.BlockChar())
                {
                    maze[i] = ' ';
                }
            }
        }

        /// <summary>
        /// Leaves a mark to trace the route to the end of the maze.
        /// </summary>
        private void markTrail()
        {
            setPos(charTrail.LeaveMarker());
            x_moves.Push(x_pos);
            y_moves.Push(y_pos);
        }

        // Methods to move about the maze; words such as "north" and "south"
        private void moveNorth()
        {
            y_pos -= 1;
        }

        private void moveSouth()
        {
            y_pos += 1;
        }

        private void moveEast()
        {
            x_pos += 1;
        }

        private void moveWest()
        {
            x_pos -= 1;
        }

        // Methods to see if cells are available
        private bool canMoveEast()
        {
            if (!mazeX.HasValue)
            {
                return false;
            }
            return emptySpace(x_pos + 1, y_pos);
        }

        private bool canMoveWest()
        {
            if (!mazeX.HasValue)
            {
                return false;
            }
            return emptySpace(x_pos - 1, y_pos);
        }

        private bool canMoveNorth()
        {
            if (!mazeY.HasValue)
            {
                return false;
            }
            return emptySpace(x_pos, y_pos - 1);
        }
        
        private bool canMoveSouth()
        {
            if (!mazeY.HasValue)
            {
                return false;
            }
            return emptySpace(x_pos, y_pos + 1);
        }
        
        /// <summary>
        /// Verifies that a cell is available for movement.
        /// </summary>
        /// <param name="x">X-index of cell (0 - mazeX, left to right)</param>
        /// <param name="y">Y-index of cell (0 - mazeY, top to bottom)</param>
        /// <returns>true if Maze(x,y) is empty, false if not</returns>
        private bool emptySpace(int x, int y)
        {
            return maze[x + y * mazeX.Value] == ' ';
        }

        /// <summary>
        /// Sets contents of the maze at position (x_pos,y_pos) to input.
        /// </summary>
        /// <param name="input"></param>
        private void setPos(char input)
        {
            maze[x_pos + y_pos * mazeX.Value] = input;
        }

        /// <summary>
        /// Subscribes the MazeWalker to the Maze.
        /// </summary>
        /// <param name="target">Maze to which to subscribe</param>
        public void TrackMaze(Maze target)
        {
            targetMaze = target.Subscribe(this as IObserver<MazeInfo>);
        }

        /// <summary>
        /// Calls the Maze's Dispose method to unsubscribe the MazeWalker.
        /// </summary>
        public void Unsubscribe()
        {
            targetMaze.Dispose();
        }

        /// <summary>
        /// Preps MazeWalker to attempt to find another solution.
        /// </summary>
        public void OnCompleted()
        {
            mazeExists = true;
            solutionAttempted = false;
            solutionFound = false;
            charTrail.ResetMarker();
            x_moves.Clear();
            y_moves.Clear();
            x_pos = 1;
            y_pos = 1;
        }

        /// <summary>
        /// Alerts user to error in data transmission between MazeWalker and Maze.
        /// </summary>
        /// <param name="error">Error that was thrown</param>
        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
            Console.ReadKey();
        }

        /// <summary>
        /// Populates private copy of the maze with new data.
        /// </summary>
        /// <param name="value">Character array to copy</param>
        public void OnNext(MazeInfo update)
        {
            maze = new char[update.Width * update.Height];
            mazeX = update.Width;
            mazeY = update.Height;

            for (int i = 0; i < (mazeX * mazeY); i++)
            {
                maze[i] = update.Cell1D(i);
            }
        }
    }
}
