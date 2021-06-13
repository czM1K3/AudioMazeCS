using LiveCharts;
using LiveCharts.Wpf;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
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

namespace FFTransformace
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<short> ListSamp = new List<short>();

        public SeriesCollection SeriesFFT { get; set; }
        ChartValues<float> OutputFFT = new ChartValues<float>();

        private readonly Game _game;

        public int Current { get; private set; }

        public MainWindow()
        {
            Closed += WindowsClosing;
            InitializeComponent();
            var waveIn = new WaveIn {DeviceNumber = 0, WaveFormat = new WaveFormat(8000, 1)};
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();

            SeriesFFT = new SeriesCollection();
            var series = new LineSeries
            {
                Values = OutputFFT,
                StrokeThickness = 1,
                PointGeometry = null
            };

            SeriesFFT.Add(series);
            DataContext = this;

            _game = new Game(this);
            _game.Show();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            ListSamp.Clear();
            OutputFFT.Clear();
            for (var index = 0; index < e.BytesRecorded; index += 2)
            {
                var sample = (short) ((e.Buffer[index + 1] << 8) | e.Buffer[index]);
                //Console.WriteLine(sample);
                ListSamp.Add(sample);
            }

            var n = ListSamp.Count;
            var max = 0f;
            const int minFrekvence = 0;
            const int maxFrekvence = 2000;

            for (var f = minFrekvence; f <= maxFrekvence; f = f + 5)
            {
                var ko = (0.5 + (n * f)) / 8000;
                var w = (2.0 * Math.PI * ko) / n;
                var cosine = Math.Cos(w);
                var sine = Math.Sin(w);
                var coeff = 2 * cosine;
                double q1 = 0;
                double q2 = 0;
                double q0;
                foreach (short pol in ListSamp)
                {
                    q0 = coeff * q1 - q2 + pol;
                    q2 = q1;
                    q1 = q0;
                }

                var real = (q1 - q2 * cosine) / (n / 2);
                var im = (q2 * sine) / (n / 2);
                var outputData = (float) Math.Sqrt(real * real + im * im);

                if (outputData > max)
                {
                    max = outputData;
                    if (max > 100)
                    {
                        LbMaxFrekvence.Content = f;
                        Current = f;
                    }
                    else
                    {
                        LbMaxFrekvence.Content = 0;
                        Current = 0;
                    }
                }

                OutputFFT.Add(outputData);
            }
        }

        private void WindowsClosing(object sender, EventArgs e)
        {
            _game.Close();
        }
    }
}