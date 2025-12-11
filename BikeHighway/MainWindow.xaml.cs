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
        public MainWindow()
        {
            InitializeComponent();
            AfficheDemarrage();
            InitMusique();
            
        }
        private void AfficheDemarrage()
        {
            UCAccueil uc = new UCAccueil();
            ZoneJeu.Content = uc;
            uc.butChoixMoto.Click += AfficherChoixMoto;
            uc.butOptions.Click += AfficherOptions;
        }

        // -- INITIALISATION DE LA MINUTERIE --
        private void InitializeTimer()
        {
            minuterie = new DispatcherTimer();
            // configure l'intervalle du Timer :62 images par s
            minuterie.Interval = TimeSpan.FromMilliseconds(16);
            // associe l’appel de la méthode Jeu à la fin de la minuterie
            minuterie.Tick += Jeu;
            // lancement du timer
            minuterie.Start();
        }
        private void Jeu(object? sender, EventArgs e)
        {
            InitializeTimer();
        }
        private void AfficherChoixMoto(object sender, RoutedEventArgs e)
        {
            UCChoixMoto uc = new UCChoixMoto();
            ZoneJeu.Content = uc;
        }
        private void AfficherOptions(object sender, RoutedEventArgs e)
        {
            DialogOptions options = new DialogOptions();
            options.Owner = this;
            options.main = this;
            options.ShowDialog();
        }
        private void AfficherJeu(object sender, RoutedEventArgs e)
        {
            //UCJeu uc = new UCJeu();
            //ZoneJeu.Content = uc;
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