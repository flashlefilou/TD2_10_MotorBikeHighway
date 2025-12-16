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
        //public MainWindow main;
        public DialogOptions()
        {
            InitializeComponent();
        }

        // --- GESTION MUSIQUE ---

        private void ChkMusique_Changed(object sender, RoutedEventArgs e)
        {
            if (MainWindow.musique == null) return; 

            if (chkMusique.IsChecked == true)
                MainWindow.musique.Play();
            else
                MainWindow.musique.Stop();
        }

        private void SliderMusique_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Vérifie que main et musique sont initialisés
            if (MainWindow.musique == null) return;

            // e.NewValue va de 0 à 100, MediaPlayer.Volume attend 0.0 à 1.0
            MainWindow.musique.Volume = e.NewValue / 100.0;
        }
        

        // --- GESTION SFX ---

        private void ChkSFX_Changed(object sender, RoutedEventArgs e)
        {
            MainWindow.SFXEnabled = chkSFX.IsChecked == true;

            if (!MainWindow.SFXEnabled)
            {
                MainWindow.sonMoto?.Stop();
                MainWindow.sonCrash?.Stop();
            }


        }

        private void SliderSFX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainWindow.sonCrash == null || MainWindow.sonMoto == null) return;

            double volume = e.NewValue / 100.0;

            MainWindow.sonCrash.Volume = volume;
            MainWindow.sonMoto.Volume = volume;
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
            MainWindow.minuterie.Start();
            MainWindow.minuterieVitesse.Start();
        }
    }
}
