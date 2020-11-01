using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromSeconds(0.1);
            _timer.Tick += Timer_Tick;
        }


        private const int AnzahlZellenBreit = 100;
        private const int AnzahlZellenHoch = 100;
        private readonly Rectangle[,] _felder = new Rectangle[AnzahlZellenBreit, AnzahlZellenHoch];
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private void ButtonStartClick(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            for (int i = 0; i < AnzahlZellenHoch; i++)
            {
                for (int j = 0; j < AnzahlZellenBreit; j++)
                {
                    Rectangle r = DefeniereRechteck(j, i);
                    r.Fill = (random.Next(0, 13)%9 == 0) ? Brushes.Red : Brushes.Beige;
                    // r.Fill = Brushes.Beige;
                }
            }
            
        }

        private Rectangle DefeniereRechteck(int j, int i)
        {
            Rectangle r = new Rectangle
            {
                Width = AkturelleZeichenFlächeBreit() - 2.0, Height = AktuelleZeichenFlächeHoch() - 2.0
            };


            Zeichenfläche.Children.Add(r);
            Canvas.SetLeft(r, j * AkturelleZeichenFlächeBreit());
            Canvas.SetTop(r, i * AktuelleZeichenFlächeHoch());
            r.MouseDown += ROnMouseDown;
            _felder[i, j] = r;
            return r;
        }

        private void ROnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle) sender).Fill = (((Rectangle) sender).Fill == Brushes.Beige) ? Brushes.Red : Brushes.Beige;
        }

        private double AktuelleZeichenFlächeHoch()
        {
            return Zeichenfläche.ActualHeight / AnzahlZellenHoch;
        }

        private double AkturelleZeichenFlächeBreit()
        {
            return Zeichenfläche.ActualWidth / AnzahlZellenBreit;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int[,] anzahlNachbarn = new int[AnzahlZellenHoch, AnzahlZellenBreit];

            for (int i = 0; i < AnzahlZellenHoch; i++)
            {
                for (int j = 0; j < AnzahlZellenBreit; j++)
                {
                    anzahlNachbarn[i, j] = ZähleNachbarn(i, j);
                }
            }

            for (int i = 0; i < AnzahlZellenHoch; i++)
            {
                for (int j = 0; j < AnzahlZellenBreit; j++)
                {
                    if (anzahlNachbarn[i, j] < 2 || anzahlNachbarn[i, j] > 3)
                    {
                        _felder[i, j].Fill = Brushes.Beige;
                    }
                    else if (anzahlNachbarn[i, j] == 3)
                    {
                        _felder[i, j].Fill = Brushes.Red;
                    }
                }
            }
        }

        private int ZähleNachbarn(int zeile, int spalte)
        {
            int nachbarn = 0;

            if (_felder[ÜberprüfeI(zeile - 1), ÜberprüfeJ(spalte - 1)].Fill == Brushes.Red)
            {
                nachbarn++;
            }

            if (_felder[ÜberprüfeI(zeile - 1), ÜberprüfeJ(spalte)].Fill == Brushes.Red)
            {
                nachbarn++;
            }

            if (_felder[ÜberprüfeI(zeile - 1), ÜberprüfeJ(spalte + 1)].Fill == Brushes.Red)
            {
                nachbarn++;
            }

            if (_felder[ÜberprüfeI(zeile), ÜberprüfeJ(spalte - 1)].Fill == Brushes.Red)
            {
                nachbarn++;
            }

            if (_felder[ÜberprüfeI(zeile), ÜberprüfeJ(spalte + 1)].Fill == Brushes.Red)
            {
                nachbarn++;
            }

            if (_felder[ÜberprüfeI(zeile + 1), ÜberprüfeJ(spalte - 1)].Fill == Brushes.Red)
            {
                nachbarn++;
            }

            if (_felder[ÜberprüfeI(zeile + 1), ÜberprüfeJ(spalte)].Fill == Brushes.Red)
            {
                nachbarn++;
            }

            if (_felder[ÜberprüfeI(zeile + 1), ÜberprüfeJ(spalte + 1)].Fill == Brushes.Red)
            {
                nachbarn++;
            }

            return nachbarn;
        }

        private static int ÜberprüfeJ(int j)
        {
            if (j > AnzahlZellenBreit - 1)
            {
                return 0;
            }

            if (j < 0)
            {
                return AnzahlZellenBreit - 1;
            }

            return j;
        }

        private static int ÜberprüfeI(int i)
        {
            if (i < 0)
            {
                {
                    return AnzahlZellenHoch - 1;
                }
            }

            if (i > AnzahlZellenHoch - 1)
            {
                {
                    return 0;
                }
            }

            return i;
        }

        

        private void StartStopAnimationOnClick(object sender, RoutedEventArgs e)
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
                StartStop.Content = "Starte Animation";
            }
            else
            {
                _timer.Start();
                StartStop.Content = "Stope Animation";
            }
        }
    }
}