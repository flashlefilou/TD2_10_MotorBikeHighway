using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;
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
        public static Random random = new Random();

        private Rectangle debugRectMoto;
        private Rectangle debugRectVehicule;

        Image[,] images;
        private Image[] voituresActives = new Image[3];
        private double[] laneLefts = new double[3];

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
            Random rand = new Random();
            Canvas.SetLeft(oil, rand.Next(95, 315));
            Canvas.SetBottom(oil, rand.Next(800, 1000));

            oil.Visibility = Visibility.Visible;
        }
        private void AfficherRejouer()
        {
            MainWindow.minuterie.Stop();
            DialogRejouer rejouer = new DialogRejouer();
            rejouer.ShowDialog();
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
            double positionActuelle = Canvas.GetLeft(imgMoto);
            if (positionActuelle > LIMITE_GAUCHE)
                Canvas.SetLeft(imgMoto, positionActuelle - VITESSE_LATERALE);
        }

        public void DeplaceMotoDroite()
        {
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
                        Image nouvelleVoiture = images[col, random.Next(0, 3)];

                        nouvelleVoiture.ClearValue(Canvas.TopProperty);
                        Canvas.SetBottom(nouvelleVoiture, bottomActive + 1400);

                        // Rendre visible la nouvelle voiture
                        nouvelleVoiture.Visibility = Visibility.Visible;
                        MainWindow.score++;
                    }
                }
                if (IsCollision(imgMoto, voitureActive))
                {
                    if (MainWindow.vies <= 1)
                    {
                        MainWindow.vies = 0;
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