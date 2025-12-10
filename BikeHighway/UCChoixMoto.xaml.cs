using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour UCChoixMoto.xaml
    /// </summary>
    public partial class UCChoixMoto : UserControl
    {
        public static uint conteur = 0;
        public static BitmapImage[] moto = new BitmapImage[3];
        public static string[] img_Moto = { "moto", "moto_blue", "moto_pink" };

        public UCChoixMoto()
        {
            InitializeComponent();
            MettreAJourBoutons();  
        }

        private void butdroit_Click(object sender, RoutedEventArgs e)
        {
            conteur++;
            MettreAJourBoutons();
            MettreAJourImage();
        }

        private void butgauche_Click(object sender, RoutedEventArgs e)
        {
            conteur--;
            MettreAJourBoutons();
            MettreAJourImage();
        }

        
        private void MettreAJourBoutons()
        {
            butgauche.IsEnabled = (conteur > 0); // Désactive le bouton gauche si on est sur la première moto
            butdroit.IsEnabled = (conteur < img_Moto.Length - 1); // Désactive le bouton droit si on est sur la dernière moto
        }

        
        private void MettreAJourImage()
        {
            Uri img = new Uri($"pack://application:,,,/img/{img_Moto[conteur]}.png");
            moto[conteur] = new BitmapImage(img);
            imageMoto.Source = moto[conteur];
        }

    }
}
