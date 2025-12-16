using System.Diagnostics.Metrics;
using System.Media;
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
        public static int vies = 2;
        public static List<int> TableScore = new List<int>();
        public static MediaPlayer sonCrash;
        public static MediaPlayer sonMoto;
        public const int volumeSFX = 1;
        public static bool SFXEnabled = true;
        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            AfficheDemarrage();
            InitMusique();
            InitSon();
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
            minuterieOil.Interval = TimeSpan.FromSeconds(10);
            minuterieOil.Tick += DeclencherHuile;
        }
        private void DeclencherHuile(object? sender, EventArgs e)
        {
            // On vérifie s'il y a bien un jeu en cours
            if (ZoneJeu.Content is UCJeu monJeu)
            {
                // On appelle la méthode sur l'instance réelle du jeu
                monJeu.AfficheTacheOil();
            }
        }
        private void Jeu(object? sender, EventArgs e)
        {
            Deplace(FondBase, pasFond);
            Deplace(FondForet, pasFond);

            if (ZoneJeu.Content is UCJeu ucJeu)
            {
                Deplace(ucJeu.tacheHuile, pasFond);
                ucJeu.DeplacerVoitures(pasFond);
                ucJeu.lbScore.Content = score;
                ucJeu.lbVies.Content = vies;
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
            if (MainWindow.SFXEnabled)
            {
                MainWindow.sonMoto.Stop();
                MainWindow.sonMoto.Play();
            }
            MainWindow.musique.Volume = valeurSon;
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
        public void InitSon()
        {
            sonCrash = new MediaPlayer();
            sonCrash.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "sons/crash.mp3"));
            sonCrash.Volume = volumeSFX;
            sonMoto = new MediaPlayer();
            sonMoto.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "sons/motorcycle-engine-rev-2-337870.mp3"));
            sonMoto.Volume= volumeSFX;
        }
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