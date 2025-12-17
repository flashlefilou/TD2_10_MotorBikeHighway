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
        //-- CONSTANTES -- 
        public const int SCORE_BASE = 0;
        public const int VIES_BASE = 1;
        public const int FENETRE_LARGEUR = 455;
        public const int FENETRE_HAUTEUR = 700;
        public const int HAUT_TOTAL_FONDS = 3500;
        public const int volumeSFX = 1;

        // -- STATIQUES MINUTERIES --
        public static DispatcherTimer minuterie;
        public static DispatcherTimer minuterieOil;
        public static DispatcherTimer minuterieBonus;
        public static DispatcherTimer minuterieVitesse;

        // -- VARIABLES STATIQUES --
        public static string Moto = "moto";
        public static int pasFond = 8;
        public static int score = 0;
        public static int vies = 1;
        public static List<int> TableScore = new List<int>();
        public static MediaPlayer sonCrash;
        public static MediaPlayer sonMoto;
        public static MediaPlayer sonColisionHuile;
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

            if (positionActuelle <= -FENETRE_HAUTEUR) // Si l'image est complètement sortie de l'écran en bas
                positionActuelle += HAUT_TOTAL_FONDS; ; // remettre en haut

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
            // configure l'intervalle du Timer : 62 images par s
            minuterie.Interval = TimeSpan.FromMilliseconds(16);
            // associe l’appel de la méthode Jeu à la fin de la minuterie
            minuterie.Tick += Jeu;


            minuterieOil = new DispatcherTimer();
            // configure l'intervalle du Timer huile : 1 flaque toute les 10 secondes
            minuterieOil.Interval = TimeSpan.FromSeconds(10);
            minuterieOil.Tick += DeclencherHuile;

            minuterieBonus = new DispatcherTimer();
            minuterieBonus.Interval = TimeSpan.FromSeconds(15);
            minuterieBonus.Tick += DeclencherBonus;

            minuterieVitesse = new DispatcherTimer();
            // configure l'intervalle du Timer huile : vitesse augmente toute les 8 secondes
            minuterieVitesse.Interval = TimeSpan.FromSeconds(8);
            minuterieVitesse.Tick += AugmenterVitesse;
        }
        private void DeclencherHuile(object? sender, EventArgs e)
        {
            // Lien entre UCJeu (AfficheTacheOil) et MainWindow 
            if (ZoneJeu.Content is UCJeu monJeu)
            {
                monJeu.AfficheTacheOil();
            }
        }
        private void DeclencherBonus(object? sender, EventArgs e)
        {
            // Lien entre UCJeu (AfficheTacheOil) et MainWindow 
            if (ZoneJeu.Content is UCJeu monJeu)
            {
                monJeu.AfficheBonus();
            }
        }

        private void AugmenterVitesse(object? sender, EventArgs e)
        {
            if (pasFond < 25)
            {
                pasFond += 1;
                Console.WriteLine("Vitesse augmentée : " + pasFond);
            }
        }
        private void Jeu(object? sender, EventArgs e)
        {
            Deplace(FondBase, pasFond);
            Deplace(FondForet, pasFond);
            Deplace(FondBase_Neige, pasFond);
            Deplace(FondNeige, pasFond);
            Deplace(FondNeige_Base, pasFond);

            if (ZoneJeu.Content is UCJeu ucJeu)
            {
                Image oil = ucJeu.tacheHuile;
                Image bonus = ucJeu.bonus;
                if (oil.Visibility == Visibility.Visible)
                {
                    double positionActuelle = Canvas.GetBottom(oil);
                    positionActuelle -= pasFond;

                    // Si l'huile sort de l'ecran
                    if (positionActuelle <= -oil.Height)
                    {
                        // On la cache et on ARRETE de la boucler.
                        // Elle ne réapparaîtra que quand "minuterieOil" déclenchera "AfficheTacheOil"
                        oil.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        // Sinon, on la déplace
                        Canvas.SetBottom(oil, positionActuelle);
                    }
                }
                ucJeu.VerifierCollisionHuile();
                if (bonus.Visibility == Visibility.Visible)
                {
                    double positionActuelle = Canvas.GetBottom(bonus);
                    positionActuelle -= pasFond;

                    // Si le bonus sort de l'ecran
                    if (positionActuelle <= -bonus.Height)
                    {
                        // On la cache et on ARRETE de la boucler.
                        // Elle ne réapparaîtra que quand "minuterieOil" déclenchera "AfficheTacheOil"
                        bonus.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        // Sinon, on la déplace
                        Canvas.SetBottom(bonus, positionActuelle);
                    }
                }
                ucJeu.VerifierCollisionBonus();

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
            minuterie.Stop(); // pause du jeu
            minuterieVitesse.Stop(); // pause de l'accélération

            DialogOptions options = new DialogOptions();
            options.Owner = this;
            options.ShowDialog();
        }
        public void AfficherJeu(object sender, RoutedEventArgs e)
        {
            score = SCORE_BASE;

            pasFond = 8; // réinitialise la vitesse du fond

            UCJeu uc = new UCJeu();
            ZoneJeu.Content = uc;
            uc.butOptions.Click += AfficherOptions;

            minuterie.Start(); // démarrer la minuterie pour Jeu
            minuterieOil.Start(); // démarrer la minuterie pour Huile
            minuterieVitesse.Start(); // démarrer la minuterie pour Vitesse
            minuterieBonus.Start();

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
            sonColisionHuile = new MediaPlayer();
            sonColisionHuile.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "sons/VEHSkid_Crissement de pneus 2 (ID 2369)_LS.wav"));
            sonColisionHuile.Volume = volumeSFX;
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

                musique.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "sons/retro-retro-synthwave-gaming-music-270173.mp3"));
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