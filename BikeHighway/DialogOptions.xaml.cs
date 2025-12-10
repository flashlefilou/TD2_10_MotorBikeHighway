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
using System.Windows.Shapes;

namespace MotorBikeHighway
{
    /// <summary>
    /// Logique d'interaction pour DialogOptions.xaml
    /// </summary>
    public partial class DialogOptions : Window
    {
        public DialogOptions()
        {
            InitializeComponent();
        }

        // --- GESTION MUSIQUE ---

        private void ChkMusique_Changed(object sender, RoutedEventArgs e)
        {
            if (chkMusique.IsChecked == true)
            {
                // TODO: Relancer la musique de fond
            }
            else
            {
                // TODO: Arrêter la musique de fond
            }
        }

        private void SliderMusique_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // e.NewValue est entre 0 et 100
        }

        // --- GESTION SFX ---

        private void ChkSFX_Changed(object sender, RoutedEventArgs e)
        {
            if (chkSFX.IsChecked == true)
            {
                // TODO: Autoriser les SFX
            }
            else
            {
                // TODO: Interdire les SFX
            }
        }

        private void SliderSFX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // e.NewValue est entre 0 et 100
            // TODO: Appliquer aux sons ponctuels
        }

        // --- NAVIGATION ---

        private void BtnRegles_Click(object sender, RoutedEventArgs e)
        {
            DialogReglesJeu regles = new DialogReglesJeu();
            regles.Owner = this;
            regles.ShowDialog();
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
