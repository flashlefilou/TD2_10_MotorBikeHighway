using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MotorBikeHighway
{
    /// <summary>
    /// Logique d'interaction pour UCJeu.xaml
    /// </summary>
    public partial class UCJeu : UserControl
    {
        private const int LIMITE_GAUCHE = 70;
        private const int LIMITE_DROITE = 310;
        private const int VITESSE_LATERALE = 15;
        public static int[,,] tabPosition = new int[3, 3, 2];
        Image[,] images;
        public static Random random = new Random();

        public UCJeu()
        {
            InitializeComponent();
            MettreAJourMotoJeu();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);  
            timer.Tick += AfficheVoitureAleatoire;
            timer.Start();
            images = new Image[3, 3]
        {
            { gVoiture1, gVoiture2, gVoiture3 },
            { cVoiture1, cVoiture2, cVoiture3 },
            { dVoiture1, dVoiture2, dVoiture3 }
        };
            for (int i = 0; i < tabPosition.GetLength(0); i++) 
                for (int j = 0; j < tabPosition.GetLength(1); j++)
                {
                    tabPosition[i,j, 0] = (int)Canvas.GetLeft(images[i,j]);
                    tabPosition[i,j, 1] = (int)Canvas.GetTop(images[i,j]);
                    Console.WriteLine(tabPosition[i, j, 0]);
                    Console.WriteLine(tabPosition[i, j, 1]);
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
            {
                positionActuelle -= VITESSE_LATERALE;
                Canvas.SetLeft(imgMoto, positionActuelle);
            }
        }
        public void DeplaceMotoDroite()
        {
            double positionActuelle = Canvas.GetLeft(imgMoto);
            if (positionActuelle < LIMITE_DROITE)
            {
                positionActuelle += (VITESSE_LATERALE);
                Canvas.SetLeft(imgMoto, positionActuelle);
                Console.WriteLine("Test");
            }
            else
            {
            }
        }
        public void AfficheVoitureAleatoire(object sender, EventArgs e)
        {
            
            for (int k = 0; k < 3; k++)
            {
                int i = random.Next(tabPosition.GetLength(0) );
                int j = random.Next(tabPosition.GetLength(1) );
                if (images[i, j].Visibility == Visibility.Visible)
                    images[i, j].Visibility = Visibility.Hidden;
                else 
                    images[i, j].Visibility = Visibility.Visible;
                Console.WriteLine(i+" "+ j);
            }
            
        }
    }
}
