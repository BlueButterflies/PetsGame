using PetsGame.Properties;
using PetsGame.UserControls;
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
    /// Interaction logic for SelectLanguage.xaml
    /// </summary>
    public partial class SelectLanguage : Window
    {
        public SelectLanguage()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            if (check_english.IsChecked == true)
            {

            }
            else if (check_italian.IsChecked == true)
            {
                
            }
            else if (check_bulgarian.IsChecked == true)
            {

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
