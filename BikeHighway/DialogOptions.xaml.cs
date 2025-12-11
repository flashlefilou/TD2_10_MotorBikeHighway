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
        public MainWindow main;
        public DialogOptions()
        {
            InitializeComponent();
        }

        // --- GESTION MUSIQUE ---

        private void ChkMusique_Changed(object sender, RoutedEventArgs e)
        {
            if (main?.musique == null) return; // <-- évite le NullReferenceException

            if (chkMusique.IsChecked == true)
                main.musique.Play();
            else
                main.musique.Stop();
        }

        private void SliderMusique_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Vérifie que main et musique sont initialisés
            if (main?.musique == null) return;

            // e.NewValue va de 0 à 100, MediaPlayer.Volume attend 0.0 à 1.0
            main.musique.Volume = e.NewValue / 100.0;
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
            // TODO: Appliquer aux SFX
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
