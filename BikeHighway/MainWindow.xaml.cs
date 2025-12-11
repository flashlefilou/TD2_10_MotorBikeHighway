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
        private DispatcherTimer minuterie;
        public static string Moto = "moto";
        public MainWindow()
        {
            InitializeComponent();
            AfficheDemarrage();
            InitMusique();
            InitializeTimer();
            this.KeyDown += MainWindow_KeyDown;
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
        private void InitializeTimer()
        {
            minuterie = new DispatcherTimer();
            // configure l'intervalle du Timer :62 images par s
            minuterie.Interval = TimeSpan.FromMilliseconds(16);
            // associe l’appel de la méthode Jeu à la fin de la minuterie
            minuterie.Tick += Jeu;
        }
        private void Jeu(object? sender, EventArgs e)
        {
            InitializeTimer();
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
            options.main = this;
            options.ShowDialog();
        }
        internal void AfficherJeu(object sender, RoutedEventArgs e)
        {
            UCJeu uc = new UCJeu();
            ZoneJeu.Content = uc;
            uc.butOptions.Click += AfficherOptions;
            minuterie.Start();
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Si le jeu n'est pas lancé, on ne fait rien

        }
        public  MediaPlayer musique;

        public double valeurSon = 50;
        private void InitMusique()
        {

            musique = new MediaPlayer();
            musique.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "sons/son acceuil.mp3"));
            musique.MediaEnded += RelanceMusique;
            musique.Volume = (double)valeurSon / 100;
            musique.Play();
        }

        private void RelanceMusique(object? sender, EventArgs e)
        {
            musique.Position = TimeSpan.Zero;
            musique.Play();
        }
       
    }
}