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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public UCJeu()
        {
            InitializeComponent();
            MettreAJourMotoJeu();
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
    }
}
