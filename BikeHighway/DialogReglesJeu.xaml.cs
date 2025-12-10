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
    /// Logique d'interaction pour DialogReglesJeu.xaml
    /// </summary>
    public partial class DialogReglesJeu : Window
    {
        public DialogReglesJeu()
        {
            InitializeComponent();
        }
        // -- FERME LA FENETRE --
        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
