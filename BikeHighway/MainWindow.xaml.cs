using System.Diagnostics.Metrics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MotorBikeHighway
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DispatcherTimer minuterie;
        public static DispatcherTimer minuterieOil;
        public static string Moto = "moto";
        public static int pasFond = 8;
        public static int score = 0;
        public static List<int> TableScore = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
            AfficheDemarrage();
            InitMusique();
            InitializeTimer();
            this.KeyDown += MainWindow_KeyDown;
        }
        public void Deplace(Image image, int pas)
        {
            double positionActuelle = Canvas.GetBottom(image);

            positionActuelle -= pas;
            Canvas.SetBottom(image, Canvas.GetBottom(image) - pas);

            if (positionActuelle <= -700) // Si l'image est complètement sortie de l'écran en bas
                positionActuelle += 1400; ; // remettre en haut

            Canvas.SetBottom(image, positionActuelle);
        }
        public void AfficheDemarrage()
        {
            UCAccueil uc = new UCAccueil();
            ZoneJeu.Content = uc;
            uc.butChoixMoto.Click += AfficherChoixMoto;
            uc.butOptions.Click += AfficherOptions;
            uc.butJouer.Click += AfficherJeu;
        }


        // -- INITIALISATION DE LA MINUTERIE --
        public void InitializeTimer()
        {
            minuterie = new DispatcherTimer();
            // configure l'intervalle du Timer :62 images par s
            minuterie.Interval = TimeSpan.FromMilliseconds(16);
            // associe l’appel de la méthode Jeu à la fin de la minuterie
            minuterie.Tick += Jeu;
            minuterieOil = new DispatcherTimer();
            minuterieOil.Interval = TimeSpan.FromSeconds(2);
            minuterieOil.Tick += AfficheTacheOil;

        }
        public Image oil;
        private async void AfficheTacheOil(object? sender, EventArgs e)
        {
            oil = new Image();
            Random rand = new Random();
            Uri img = new Uri($"pack://application:,,,/img/oil_top_down.png");
            oil.Source = new BitmapImage(img);
            oil.Width = 50;
            oil.Height = 50;
            Canvas.SetLeft(oil, rand.Next(700));
            Canvas.SetTop(oil, rand.Next(450));
            canvasJeu.Children.Add(oil);
        }

        private void Jeu(object? sender, EventArgs e)
        {
            Deplace(FondBase, pasFond);
            Deplace(FondForet, pasFond);
            

            if (ZoneJeu.Content is UCJeu ucJeu)
            {
                ucJeu.DeplacerVoitures(pasFond);
                ucJeu.lbScore.Content = score;
            }

        }
        private void AfficherChoixMoto(object sender, RoutedEventArgs e)
        {
            UCChoixMoto uc = new UCChoixMoto();
            ZoneJeu.Content = uc;
            uc.butJouer.Click += AfficherJeu;
        }
        public  void AfficherOptions(object sender, RoutedEventArgs e)
        {
            minuterie.Stop();
            DialogOptions options = new DialogOptions();
            options.Owner = this;
            options.ShowDialog();
        }
        public void AfficherJeu(object sender, RoutedEventArgs e)
        {
            score = 0;
            UCJeu uc = new UCJeu();
            ZoneJeu.Content = uc;
            uc.butOptions.Click += AfficherOptions;
            minuterie.Start();
            minuterieOil.Start();
            uc.lbScore.Content = score;
            this.Focusable = true;
            this.Focus();
        }


        public void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (ZoneJeu.Content is not UCJeu ucJeu)
                return;
            else if (e.Key == Key.Left)
            {
                ucJeu.DeplaceMotoGauche();
            }
            else if (e.Key == Key.Right)
            {
                ucJeu.DeplaceMotoDroite();
            }

        }
        public static MediaPlayer musique;

        public static double valeurSon = 50;
        public void InitMusique()
        {

            musique = new MediaPlayer();
            musique.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "sons/son acceuil.mp3"));
            musique.MediaEnded += RelanceMusique;
            musique.Volume = (double)valeurSon / 100;
            musique.Play();
        }
        public static bool aDemarreJeu = false;
        public static int conteurMusique = 0;
        private void RelanceMusique(object? sender, EventArgs e)
        {
            if (!aDemarreJeu)
            {
                musique.Position = TimeSpan.Zero;
                musique.Play();
                return;
            }

            if (conteurMusique == 0)
            {
                conteurMusique++;

                musique.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory +
                                     "sons/retro-retro-synthwave-gaming-music-270173.mp3"));
                musique.Play();
                return;
            }

            // Musique 2 boucle
            if (conteurMusique >= 1)
            {
                musique.Position = TimeSpan.Zero;
                musique.Play();
            }
        }
    }
}