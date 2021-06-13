using System;
using System.ComponentModel;
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

        public Game(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            Closed += WindowClosing;
            _rectangles = new Rectangle[Size, Size];

            Generator generator = new Generator(Size);
            _maze = generator.Generate();

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
                    Fill = _maze[x,y] == 1 ? Brushes.Black : Brushes.DodgerBlue,
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
            Title = _mainWindow.Current.ToString();
        }

        private void WindowClosing(object sender, EventArgs e)
        {
            _mainWindow.Close();
        }
    }
}