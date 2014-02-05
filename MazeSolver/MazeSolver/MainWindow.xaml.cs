using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MazeSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Maze _appMaze;
        List<MazeWalker> _walkerList;
        int _width = 27;
        int _height = 14;

        public MainWindow()
        {
            _walkerList = new List<MazeWalker>();
            _appMaze = Maze.GetInstance();
            InitializeComponent();

            // Deserialization of objects from a file may ultimately go here. For now, this just serves to demonstrate the use of the combo box.
            NumTrail numericTrail = new NumTrail();
            ASnakeTrail atSignTrail = new ASnakeTrail();
            AlphaTrail alphabeticTrail = new AlphaTrail();
            _walkerList.Add(new MazeWalker(numericTrail));
            _walkerList.Add(new MazeWalker(alphabeticTrail));
            _walkerList.Add(new MazeWalker(atSignTrail));
            cboMazeWalkerList.Items.Add("Numeric Trail");
            cboMazeWalkerList.Items.Add("Alphabetic Trail");
            cboMazeWalkerList.Items.Add("@ Symbol Trail");
            foreach (MazeWalker walker in _walkerList)
            {
                walker.TrackMaze(_appMaze);
            }
            Maze.ResetInstance(_width, _height);
            MazeDisplay.Text = _appMaze.PrintedMaze();
            
        }
        /// <summary>
        /// Generates a new maze; solves the maze if a character trail has been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGenerateMaze_Click(object sender, RoutedEventArgs e)
        {
            Maze.ResetInstance(_width, _height);
            lblSolutionFound.Content = "";
            if (cboMazeWalkerList.SelectedIndex >= 0)
            {
                // If a character trail has been selected, attempt to solve the maze and display the solution.
                cboMazeWalkerList_SelectionChanged(this, null);
            }
            else
            {
                // If a character trail hasn't been selected, display the unsolved maze.
                MazeDisplay.Text = _appMaze.PrintedMaze();
            }
        }

        private void cboMazeWalkerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MazeWalker selectedWalker = _walkerList[cboMazeWalkerList.SelectedIndex];
            // Attempt to solve the maze; inform the user whether or not the attempt was successful.
            if(selectedWalker.SolveMaze())
            {
                lblSolutionFound.Content = "Yes";
                lblSolutionFound.Foreground = Brushes.Green;
            }
            else
            {
                lblSolutionFound.Content = "No";
                lblSolutionFound.Foreground = Brushes.Red;
            }

            MazeDisplay.Text = selectedWalker.PrintMaze();
        }
    }
}
