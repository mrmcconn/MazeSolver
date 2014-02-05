using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    class Maze : IObservable<MazeInfo>
    {
        private static char _verWall = '|';
        private static char _horizWall = '-';
        private static char _corner = '+';
        private static Maze _singleInstance = new Maze();
        private char[,] _mazeArray;
        private MazeCell[,] _mazeCells;
        private int _xMaze;
        private int _yMaze;
        private List<IObserver<MazeInfo>> _walkers;
        private Random _numGenerator;

        /// <summary>
        /// Gets the only existing instance of Maze, or resets it with new parameters.
        /// </summary>
        /// <param name="width">Width of the maze.</param>
        /// <param name="height">Height of the maze.</param>
        /// <returns>The instance of Maze.</returns>
        public static Maze GetInstance(int width = 10, int height = 10)
        {
            if(_singleInstance == null)
            {
                _singleInstance = new Maze(width, height);
            }
            else
            {
                ResetInstance(width, height);
            }

            _singleInstance.updateWalkers();
            return _singleInstance;
        }

        /// <summary>
        /// Randomizes the Maze with the given parameters.
        /// </summary>
        /// <param name="width">Width of the maze.</param>
        /// <param name="height">Height of the maze.</param>
        public static void ResetInstance(int width = 10, int height = 10)
        {
            if (_singleInstance == null)
            {
                _singleInstance = new Maze(width, height);
            }
            else
            {
                _singleInstance.create(width, height);
            }

            _singleInstance.updateWalkers();
        }

        /// <summary>
        /// Creates a new instance of the maze; uses an observable singleton pattern.
        /// </summary>
        /// <param name="width">Width of the maze.</param>
        /// <param name="height">Height of the maze.</param>
        private Maze(int width = 10, int height = 10)
        {
            _walkers = new List<IObserver<MazeInfo>>();
            _numGenerator = new Random();
            create(width, height);
        }

        /// <summary>
        /// Returns the width of the maze.
        /// </summary>
        public int Width
        {
            get
            {
                return _xMaze;
            }
        }

        /// <summary>
        /// Returns the height of the maze.
        /// </summary>
        public int Height
        {
            get
            {
                return _yMaze;
            }
        }

        /// <summary>
        /// Retrieves a character in the ASCII display of the maze.
        /// </summary>
        /// <param name="row">Row from which to retrieve the character.</param>
        /// <param name="column">Column from which to retrieve the character.</param>
        /// <returns>Character at the specified row and column.</returns>
        public char Cell(int row, int column)
        {
            return _mazeArray[row, column];
        }

        /// <summary>
        /// Instantiates Maze.
        /// </summary>
        /// <param name="width">Width of the maze.</param>
        /// <param name="height">Height of the maze.</param>
        private void create(int width, int height)
        {
            int charWidth = (width * 2) + 1;
            int charHeight = (height * 2) + 1;
            _mazeArray = new char[charHeight, charWidth];
            _mazeCells = new MazeCell[height, width];
            _xMaze = width;
            _yMaze = height;
            for(int i = 0; i < _yMaze; i++)
            {
                for(int j = 0; j < _xMaze; j++)
                {
                    _mazeCells[i, j] = new MazeCell();
                }
            }
            for(int i = 0; i < Math.Max(_yMaze, _xMaze); i++)
            {
                if(i < _xMaze)
                {
                    _mazeCells[0, i].SetNorthBorder();
                    _mazeCells[_yMaze - 1, i].SetSouthBorder();
                }
                if(i < _yMaze)
                {
                    _mazeCells[i, 0].SetWestBorder();
                    _mazeCells[i, _xMaze - 1].SetEastBorder();
                }
            }

            buildMaze();
            renderMaze();
        }

        /// <summary>
        /// Randomizes the maze as an absraction of cells.
        /// </summary>
        private void buildMaze()
        {
            int startRow = _numGenerator.Next(_yMaze);
            int startCol = _numGenerator.Next(_xMaze);
            traverseGrid(startRow, startCol);
            for(int i = 0; i < _yMaze; i++)
            {
                for(int j = 0; j < _xMaze; j++)
                {
                    if(_numGenerator.Next(20) == 0)
                    {
                        _mazeCells[i, j].FlipWall(_numGenerator.Next(4));
                    }
                }
            }
        }

        /// <summary>
        /// Removes corners from the ASCII representation that don't have walls adjacent to them.
        /// </summary>
        public void ClearExtraCorners()
        {
            int charWidth = 1 + _xMaze * 2;
            int charHeight = 1 + _yMaze * 2;

            for(int i = 1; i < charHeight - 1; i++)
            {
                for(int j = 1; j < charWidth - 1; j++)
                {
                    if(_mazeArray[i, j] == _corner)
                    {
                        if (_mazeArray[i - 1, j] != ' ')
                            continue;
                        if (_mazeArray[i + 1, j] != ' ')
                            continue;
                        if (_mazeArray[i, j - 1] != ' ')
                            continue;
                        if (_mazeArray[i, j + 1] != ' ')
                            continue;
                        _mazeArray[i, j] = ' ';
                    }
                }
            }
        }

        /// <summary>
        /// Returns a string that represents the maze as ASCII characters.
        /// </summary>
        /// <returns>A string that represents the maze as ASCII characters.</returns>
        public string PrintedMaze()
        {
            string outString = "";
            int charHeight = 2 * _yMaze + 1;
            int charWidth = 2 * _xMaze + 1;
            for(int i = 0; i < charHeight; i++)
            {
                for(int j = 0; j < charWidth; j++)
                {
                    outString += _mazeArray[i, j];
                }
                outString += '\n';
            }

            return outString;
        }

        /// <summary>
        /// Iterates randomly through Maze to generate a perfect maze, starting at the selected cell.
        /// </summary>
        /// <param name="row">Row of the selected cell.</param>
        /// <param name="column">Column of the selected cell.</param>
        private void traverseGrid(int row, int column)
        {
            List<int> tryOrder = new List<int>();
            List<int> sortedOrder = new List<int>();
            int index;

            for(int i = 0; i < 4; i++)
            {
                sortedOrder.Add(i);
            }
            for(int i = 4; i > 0; i--)
            {
                index = _numGenerator.Next(i);
                tryOrder.Add(sortedOrder[index]);
                sortedOrder.Remove(sortedOrder[index]);
            }
            
            MazeCell currentCell = _mazeCells[row, column];
            currentCell.VisitCell();
            for (int i = 0; i < 4; i++)
            {
                if (tryOrder[i] == 0 && !currentCell.BorderToSouth() && !_mazeCells[row + 1, column].WasVisited)
                {
                    currentCell.BreakSouthWall();
                    _mazeCells[row + 1, column].BreakNorthWall();
                    traverseGrid(row + 1, column);
                }
                else if (tryOrder[i] == 1 && !currentCell.BorderToEast() && !_mazeCells[row, column + 1].WasVisited)
                {
                    currentCell.BreakEastWall();
                    _mazeCells[row, column + 1].BreakWestWall();
                    traverseGrid(row, column + 1);
                }
                else if (tryOrder[i] == 2 && !currentCell.BorderToWest() && !_mazeCells[row, column - 1].WasVisited)
                {
                    currentCell.BreakWestWall();
                    _mazeCells[row, column - 1].BreakEastWall();
                    traverseGrid(row, column - 1);
                }
                else if (tryOrder[i] == 3 && !currentCell.BorderToNorth() && !_mazeCells[row - 1, column].WasVisited)
                {
                    currentCell.BreakNorthWall();
                    _mazeCells[row - 1, column].BreakSouthWall();
                    traverseGrid(row - 1, column);
                }
            }
        }

        /// <summary>
        /// Generates an array of ASCII characters to represent the maze.
        /// </summary>
        private void renderMaze()
        {
            int sideParity;
            int charRow;
            int charColumn;
            int charWidth = 1 + _xMaze * 2;
            int charHeight = 1 + _yMaze * 2;

            for(int row = 0; row < _yMaze; row++)
            {
                for(int column = 0; column < _xMaze; column++)
                {
                    charRow = 1 + row * 2;
                    charColumn = 1 + column * 2;
                    _mazeArray[charRow + 1, charColumn + 1] = _corner;
                    _mazeArray[charRow - 1, charColumn + 1] = _corner;
                    _mazeArray[charRow + 1, charColumn - 1] = _corner;
                    _mazeArray[charRow - 1, charColumn - 1] = _corner;
                    _mazeArray[charRow, charColumn] = ' ';
                    if (!_mazeCells[row, column].EastWallDown())
                    {
                        _mazeArray[charRow, charColumn + 1] = _verWall;
                    }
                    else
                    {
                        _mazeArray[charRow, charColumn + 1] = ' ';
                    }

                    if (!_mazeCells[row, column].WestWallDown())
                    {
                        _mazeArray[charRow, charColumn - 1] = _verWall;
                    }
                    else
                    {
                        _mazeArray[charRow, charColumn - 1] = ' ';
                    }

                    if (!_mazeCells[row, column].NorthWallDown())
                    {
                        _mazeArray[charRow - 1, charColumn] = _horizWall;
                    }
                    else
                    {
                        _mazeArray[charRow - 1, charColumn] = ' ';
                    }

                    if (!_mazeCells[row, column].SouthWallDown())
                    {
                        _mazeArray[charRow + 1, charColumn] = _horizWall;
                    }
                    else
                    {
                        _mazeArray[charRow + 1, charColumn] = ' ';
                    }
                }

                ClearExtraCorners();
            }
            for (int i = 2; i < charHeight - 1; i += 2)
            {
                for(int j = 2; j < charWidth - 1; j += 2)
                {
                    sideParity = 0;
                    for(int k = -1; k < 2; k += 2)
                    {
                        if(_mazeArray[i + k, j] == _verWall)
                        {
                            sideParity++;
                        }
                        if(_mazeArray[i, j + k] == _horizWall)
                        {
                            sideParity--;
                        }
                    }
                    if(sideParity == 2)
                    {
                        _mazeArray[i, j] = _verWall;
                    }
                    else if(sideParity == -2)
                    {
                        _mazeArray[i, j] = _horizWall;
                    }
                }
            }
            for(int i = 1; i < charHeight - 1; i++)
            {
                if(_mazeArray[i,1] != _horizWall)
                {
                    _mazeArray[i, 0] = _verWall;
                }
                else
                {
                    _mazeArray[i, 0] = _corner;
                }
                if (_mazeArray[i, charWidth - 2] != _horizWall)
                {
                    _mazeArray[i, charWidth - 1] = _verWall;
                }
                else
                {
                    _mazeArray[i, charWidth - 1] = _corner;
                }
            }
            for (int i = 1; i < charWidth - 1; i++)
            {
                if (_mazeArray[1, i] != _verWall)
                {
                    _mazeArray[0, i] = _horizWall;
                }
                else
                {
                    _mazeArray[0, i] = _corner;
                }
                if (_mazeArray[charHeight - 2, i] != _verWall)
                {
                    _mazeArray[charHeight - 1, i] = _horizWall;
                }
                else
                {
                    _mazeArray[charHeight - 1, i] = _corner;
                }
            }
        }

        /// <summary>
        /// Unsubscriber class used to manage observers.
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<MazeInfo>> walkerList;
            private IObserver<MazeInfo> walker;

            public Unsubscriber(List<IObserver<MazeInfo>> list, IObserver<MazeInfo> w)
            {
                walkerList = list;
                walker = w;
            }

            public void Dispose()
            {
                if (walker != null && walkerList.Contains(walker))
                {
                    walkerList.Remove(walker);
                }
            }
        }

        /// <summary>
        /// Subscribe method used to connect Maze to observers.
        /// </summary>
        /// <param name="observer">Observing object.</param>
        /// <returns>Unsubscriber used to unsubscribe the observer.</returns>
        public IDisposable Subscribe(IObserver<MazeInfo> observer)
        {
            _walkers.Add(observer);
            return new Unsubscriber(_walkers, observer);
        }

        /// <summary>
        /// Supply observers (typically MazeWalkers) with data when the instance changes.
        /// </summary>
        private void updateWalkers()
        {
            MazeInfo transferData = new MazeInfo();
            transferData.LoadMaze(this);
            foreach (IObserver<MazeInfo> walker in _walkers)
            {
                walker.OnNext(transferData);
                walker.OnCompleted();
            }
        }
    }
}
