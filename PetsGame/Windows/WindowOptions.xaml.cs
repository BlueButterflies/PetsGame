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
    /// Interaction logic for WindowOptions.xaml
    /// </summary>
    public partial class WindowOptions : Window
    {
        public WindowOptions()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dailyCommentsCheked.IsChecked = (bool)Settings.Default["dailyComments"];
            musicCheked.IsChecked = (bool)Settings.Default["music"];
            petName.Text = (string)Settings.Default["default_name"];
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Settings.Default["dailyComments"] = dailyCommentsCheked.IsChecked;
            Settings.Default["music"] = musicCheked.IsChecked;
            Settings.Default["default_name"] = petName.Text;
            Settings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
