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
        public static string Moto = "moto";
        public static int pasFond = 8;
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
        private void AfficheDemarrage()
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
        }
        private void Jeu(object? sender, EventArgs e)
        {
            Deplace(FondBase, pasFond);
            Deplace(FondForet, pasFond);

            if (ZoneJeu.Content is UCJeu ucJeu)
            {
                ucJeu.DeplacerVoitures(pasFond);
            }

        }
        private void AfficherChoixMoto(object sender, RoutedEventArgs e)
        {
            UCChoixMoto uc = new UCChoixMoto();
            ZoneJeu.Content = uc;
            uc.butJouer.Click += AfficherJeu;
        }
        private void AfficherOptions(object sender, RoutedEventArgs e)
        {
            minuterie.Stop();
            DialogOptions options = new DialogOptions();
            options.Owner = this;
            options.ShowDialog();
        }
        public void AfficherJeu(object sender, RoutedEventArgs e)
        {
            UCJeu uc = new UCJeu();
            ZoneJeu.Content = uc;
            uc.butOptions.Click += AfficherOptions;
            minuterie.Start();
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
        private void InitMusique()
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