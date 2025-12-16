using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Formats.Asn1.AsnWriter;

namespace MotorBikeHighway
{
    public partial class UCJeu : UserControl
    {
        public Image tacheHuile { get { return oil; } }
        private const int LIMITE_GAUCHE = 70;
        private const int LIMITE_DROITE = 310;
        private const int VITESSE_LATERALE = 15;
        private const double WINDOW_HEIGHT = 700.0;
        private const int NOUVELLE_POSITION_HUILE = 800;
        private const int TOURS_ANIMATION_HUILE = 720;
        public static Random random = new Random();

        private Rectangle debugRectMoto;
        private Rectangle debugRectVehicule;

        Image[,] images;
        private double[] laneLefts = new double[3];
        private double dernierX = -999;
        private bool controleBloque = false;

        public UCJeu()
        {
            InitializeComponent();
            MettreAJourMotoJeu();

            images = new Image[3, 3]
            {
                { gVoiture1, gVoiture2, gVoiture3 },
                { cVoiture1, cVoiture2, cVoiture3 },
                { dVoiture1, dVoiture2, dVoiture3 }
            };

            // récupère X des trois colonnes depuis le XAML
            laneLefts[0] = Canvas.GetLeft(images[0, 0]);
            laneLefts[1] = Canvas.GetLeft(images[1, 0]);
            laneLefts[2] = Canvas.GetLeft(images[2, 0]);
            InitialiserTroisVoitures();
            debugRectMoto = CreerRectangleDebug(Colors.LimeGreen);
            debugRectVehicule = CreerRectangleDebug(Colors.Red);


            // point de pivot de la moto au centre de l'image
            imgMoto.RenderTransformOrigin = new Point(0.5, 0.5);
            // preparation de la rotation
            RotateTransform rt = new RotateTransform();
            imgMoto.RenderTransform = rt;
        }
        private Rectangle CreerRectangleDebug(Color couleur)
        {
            Rectangle r = new Rectangle();
            r.Stroke = new SolidColorBrush(couleur);
            r.StrokeThickness = 2;
            r.Fill = Brushes.Transparent;


            Panel.SetZIndex(r, 99);
            canvasJeu.Children.Add(r);
            return r;
        }
        public void AfficheTacheOil()
        {
            double nouveauX;

            // position trop proche de la précédente => nouvelle position
            do
            {
                nouveauX = random.Next(95, 315);
            }
            while (Math.Abs(nouveauX - dernierX) < 50);

            // sauvegarde de cette position
            dernierX = nouveauX;

            Canvas.SetLeft(oil, nouveauX);

            Canvas.SetBottom(oil, NOUVELLE_POSITION_HUILE);

            oil.Visibility = Visibility.Visible;
        }
        public void VerifierCollisionHuile()
        {
            // 1. Si on glisse déjà ou si l'huile est cachée, on ne vérifie pas
            if (controleBloque || oil.Visibility != Visibility.Visible) return;

            // 2. Vérification simple de collision (Rect)
            Rect rectMoto = new Rect(Canvas.GetLeft(imgMoto), Canvas.GetBottom(imgMoto), imgMoto.Width, imgMoto.Height);
            Rect rectOil = new Rect(Canvas.GetLeft(oil), Canvas.GetBottom(oil), oil.Width, oil.Height);

            // On réduit un peu la zone de collision de l'huile pour être gentil (Hitbox permissive)
            rectOil.Inflate(-10, -10);

            if (rectMoto.IntersectsWith(rectOil))
            {
                DeclencherGlissade();
            }
        }
        private void DeclencherGlissade()
        {
            // bloque les commandes
            controleBloque = true;

            oil.Visibility = Visibility.Hidden;

            // lance l'animation 
            RotateTransform rt = new RotateTransform();
            imgMoto.RenderTransform = rt;

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = TOURS_ANIMATION_HUILE; // Fait 2 tours rapides
            animation.Duration = TimeSpan.FromSeconds(1.5); // Dure 1.5 secondes

            // D. QUAND L'ANIMATION EST FINIE
            animation.Completed += (s, e) =>
            {
                controleBloque = false; // commandes débloquées
                rt.Angle = 0; // moto droite
            };

            rt.BeginAnimation(RotateTransform.AngleProperty, animation);
        }
        private void AfficherRejouer()
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;

            MainWindow.minuterie.Stop();
            MainWindow.minuterieOil.Stop();
            MainWindow.minuterieVitesse.Stop();

            DialogRejouer rejouer = new DialogRejouer();
            rejouer.lbScoreActuel.Content = MainWindow.score;
            MainWindow.TableScore.Add(MainWindow.score);
            Console.WriteLine(MainWindow.TableScore[MainWindow.TableScore.Count - 1]);
            rejouer.lbMeilleurScoreAffichage.Content = MainWindow.TableScore.Max();
            rejouer.Owner = main;

            rejouer.butRejouer.Click += (s, e) => ActionButRejouer(rejouer, s, e);
            rejouer.butAccueil.Click += (s, e) => main.AfficheDemarrage();
            rejouer.butAccueil.Click += (s, e) => MainWindow.musique.Stop();
            rejouer.butAccueil.Click += (s, e) => MainWindow.minuterieVitesse.Stop();
            rejouer.butAccueil.Click += (s, e) => main.InitMusique();
            rejouer.butAccueil.Click += (s, e) => rejouer.Close();
            
            rejouer.ShowDialog();
        }

        private void ActionButRejouer(DialogRejouer rejouer, object sender, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            rejouer.Close();          // ferme le dialogue
            main.AfficherJeu(sender, e); // relance le jeu
        }
        private void MasquerToutesVoitures()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    images[i, j].Visibility = Visibility.Hidden;
        }
        public void MettreAJourMotoJeu()
        {
            Uri img = new Uri($"pack://application:,,,/img/{MainWindow.Moto}.png");
            imgMoto.Source = new BitmapImage(img);
        }

        public void DeplaceMotoGauche()
        {
            if (controleBloque) return;

            double positionActuelle = Canvas.GetLeft(imgMoto);
            if (positionActuelle > LIMITE_GAUCHE)
                Canvas.SetLeft(imgMoto, positionActuelle - VITESSE_LATERALE);
        }

        public void DeplaceMotoDroite()
        {
            if (controleBloque) return;

            double positionActuelle = Canvas.GetLeft(imgMoto);
            if (positionActuelle < LIMITE_DROITE)
                Canvas.SetLeft(imgMoto, positionActuelle + VITESSE_LATERALE);
        }
        private void InitialiserTroisVoitures()
        {
            MasquerToutesVoitures();

            for (int col = 0; col < 3; col++)
            {
                int ligneAleatoire = random.Next(0, 3);
                Image voitureChoisie = images[col, ligneAleatoire];

                voitureChoisie.Visibility = Visibility.Visible;

                voitureChoisie.ClearValue(Canvas.TopProperty);

                double depart = 200 + (col * 400);
                Canvas.SetBottom(voitureChoisie, depart);
            }
        }

        public void DeplacerVoitures(int pas)
        {
            // parcourt les 3 colonnes
            for (int col = 0; col < 3; col++)
            {
                // cherche voiture visible dns la colonne
                Image voitureActive = null;
                for (int row = 0; row < 3; row++)
                {
                    if (images[col, row].Visibility == Visibility.Visible)
                    {
                        voitureActive = images[col, row];
                        break;
                    }
                }

                // Si on a trouvé une voiture active
                if (voitureActive != null)
                {
                    voitureActive.ClearValue(Canvas.TopProperty);
                    
                    double bottomActive = Canvas.GetBottom(voitureActive) - pas;
                    // déplacement
                    Canvas.SetBottom(voitureActive, bottomActive);
                    // sortie de l'ecran ?
                    if (bottomActive <= -voitureActive.Height)
                    {
                        // cacher l'ancienne voiture
                        voitureActive.Visibility = Visibility.Hidden;

                        // nouvelle voiture
                        int indexAleatoire = random.Next(0, 3);
                        Image nouvelleVoiture = images[col, indexAleatoire];

                        nouvelleVoiture.ClearValue(Canvas.TopProperty);

                        double decalage = indexAleatoire * 250;

                        Canvas.SetBottom(nouvelleVoiture, 1400 + decalage);

                        // Rendre visible la nouvelle voiture
                        nouvelleVoiture.Visibility = Visibility.Visible;
                        MainWindow.score++;
                    }
                }
                if (IsCollision(imgMoto, voitureActive))
                {
                    if (MainWindow.vies <= 1)
                    {
                        MainWindow.vies = MainWindow.VIES_BASE;
                        AfficherRejouer();
                    }
                    else
                    {
                        MainWindow.vies--;
                        // Réinitialiser la position de la moto au centre
                        Canvas.SetLeft(imgMoto, 190); 
                        InitialiserTroisVoitures();
                    }
                    
                }
            }
        }
        private bool IsCollision(Image moto, Image vehicule)
        {
            double motox = Canvas.GetLeft(moto);
            double motoy = Canvas.GetBottom(moto);

            double vehx = Canvas.GetLeft(vehicule);
            double vehy = Canvas.GetBottom(vehicule);
            Rect rectangleVehicule = new Rect(vehx + 35, vehy +15, (int)vehicule.Width - 70, (int)vehicule.Height - 25);
            Rect rectangleMoto = new Rect(motox + 20, motoy + 10, (int)moto.Width - 40, (int)moto.Height - 25);


            // ================
            // HITBOX VISIBLE 
            // ================

            debugRectMoto.Width = rectangleMoto.Width;
            debugRectMoto.Height = rectangleMoto.Height;
            Canvas.SetLeft(debugRectMoto, rectangleMoto.X);  

            debugRectMoto.ClearValue(Canvas.TopProperty);
            Canvas.SetBottom(debugRectMoto, rectangleMoto.Y); 

            debugRectVehicule.Width = rectangleVehicule.Width;
            debugRectVehicule.Height = rectangleVehicule.Height;
            Canvas.SetLeft(debugRectVehicule, rectangleVehicule.X); 
            debugRectVehicule.ClearValue(Canvas.TopProperty);
            Canvas.SetBottom(debugRectVehicule, rectangleVehicule.Y); 

            return rectangleMoto.IntersectsWith(rectangleVehicule);
        }
        
    }
}