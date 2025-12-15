using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Formats.Asn1.AsnWriter;

namespace MotorBikeHighway
{
    public partial class UCJeu : UserControl
    {
        private const int LIMITE_GAUCHE = 70;
        private const int LIMITE_DROITE = 310;
        private const int VITESSE_LATERALE = 15;
        private const double WINDOW_HEIGHT = 700.0;
        public static Random random = new Random();

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

            MasquerToutesVoitures();
            InitialiserTroisVoitures();
        }

        private void MasquerToutesVoitures()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    images[i, j].Visibility = Visibility.Hidden;
        }

        private void InitialiserTroisVoitures()
        {
            voituresActives[0] = images[0, 0];
            voituresActives[1] = images[1, 0];
            voituresActives[2] = images[2, 0];

            int[] lanes = { 0, 1, 2 };
            for (int i = 0; i < lanes.Length; i++)
            {
                int idx = random.Next(i, lanes.Length);
                int tmp = lanes[i]; lanes[i] = lanes[idx]; lanes[idx] = tmp;
            }

            for (int k = 0; k < 3; k++)
            {
                Image v = voituresActives[k];
                int lane = lanes[k];

                // IMPORTANT : effacer Canvas.Top défini en XAML pour que Bottom soit pris en compte
                v.ClearValue(Canvas.TopProperty);

                Canvas.SetLeft(v, laneLefts[lane]);
                Canvas.SetBottom(v, WINDOW_HEIGHT + random.Next(100, 600));
                v.Visibility = Visibility.Visible;
            }
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

        public void DeplacerVoitures(int pas)
        {
            
            foreach (var v in voituresActives)
            {
                if (v == null) continue;

                // s'assurer que Top n'interfère pas
                v.ClearValue(Canvas.TopProperty);

                double bottom = Canvas.GetBottom(v);
                // si Bottom n'était pas défini (NaN), on le initialise
                if (double.IsNaN(bottom)) bottom = WINDOW_HEIGHT + random.Next(100, 600);
                bottom -= pas;
                Canvas.SetBottom(v, bottom);

                if (bottom <= -v.Height)
                {
                    Canvas.SetBottom(v, WINDOW_HEIGHT + random.Next(150, 800));
                    int lane = random.Next(0, laneLefts.Length);
                    Canvas.SetLeft(v, laneLefts[lane]);
                    MainWindow.score++;

                }

                
                
            }
            
        }
        
    }
}