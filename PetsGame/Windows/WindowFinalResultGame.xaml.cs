using PetsGame.Properties;
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

namespace PetsGame.Windows
{
    /// <summary>
    /// Interaction logic for WindowFinalResultGame.xaml
    /// </summary>
    public partial class WindowFinalResultGame : Window
    {
        public WindowFinalResultGame(int mHappiness)
        {
            InitializeComponent();

            txtbFinalHappiness.Text = mHappiness.ToString();

            if (mHappiness >= 100)
            {
                Settings.Default["wins"] = (int)Settings.Default["wins"] + 1;
                Settings.Default.Save();

                txtbResult.Text = "You won!";
                txtbComment.Text = "Excellent!";
            }
            else
            {
                header.Background = Brushes.FloralWhite;
                txtbResult.Text = "You lost.";
                txtbComment.Text = "Your pet is sad.";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
