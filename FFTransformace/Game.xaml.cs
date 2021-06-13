using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FFTransformace
{
    public partial class Game : Window
    {
        private const int Size = 15;
        private Rectangle[,] _rectangles;
        private readonly MainWindow _mainWindow;
        private readonly int[,] _maze;
        private MyPoint _currentPos;

        public Game(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            Closed += WindowClosing;
            _rectangles = new Rectangle[Size, Size];

            var generator = new Generator(Size);
            _maze = generator.Generate();
            _currentPos = new MyPoint(1, 1);

            for (var i = 0; i < Size; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
            }
            
            for (var x = 0; x < Size; x++)
            for (var y = 0; y < Size; y++)
            {
                _rectangles[x, y] = new Rectangle()
                {
                    Fill = _maze[x,y] == 1 ? Brushes.Black :(x == _currentPos.X && y == _currentPos.Y) ? Brushes.Yellow : Brushes.DodgerBlue,
                    Margin = new Thickness(1,1,1,1)
                };
                _rectangles[x,y].SetValue(Grid.RowProperty, x);
                _rectangles[x,y].SetValue(Grid.ColumnProperty, y);
                grid.Children.Add(_rectangles[x, y]);
            }

            var timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 100)};
            timer.Tick += TimerOnTick;
            timer.Start();
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            var currentFrequency = _mainWindow.Current;
            _rectangles[_currentPos.X, _currentPos.Y].Fill = Brushes.DodgerBlue;
            if (Enumerable.Range(1000, 1250).Contains(currentFrequency)) // ➡
            {
                
            }
            else if (Enumerable.Range(1250, 1500).Contains(currentFrequency)) // ⬇
            {
                
            }
            else if (Enumerable.Range(1500, 1750).Contains(currentFrequency)) // ⬅
            {
                
            }
            else if (Enumerable.Range(1750, 2000).Contains(currentFrequency)) // ⬆
            {
                
            }

            _rectangles[_currentPos.X, _currentPos.Y].Fill = Brushes.Yellow;
        }

        private void WindowClosing(object sender, EventArgs e)
        {
            _mainWindow.Close();
        }
    }

    struct MyPoint
    {
        public MyPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }
}