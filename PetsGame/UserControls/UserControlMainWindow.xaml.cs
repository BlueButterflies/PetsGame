using PetsGame.UserControls;
using PetsGame.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace PetsGame
{
    /// <summary>
    /// Interaction logic for UserControlMainWindow.xaml
    /// </summary>
    public partial class UserControlMainWindow : UserControl, IDisposable
    {
        public void Dispose()
        {
        }

        public UserControlMainWindow()
        {
            InitializeComponent();

            Version versions = Assembly.GetExecutingAssembly().GetName().Version;
            text_version.Text = string.Format($"Version {versions.Major}.{versions.Minor}.{versions.Build}");

            text_version.Visibility = Visibility.Collapsed;
        }

        private void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new UserControlSelectPet();
        }

        private void btn_Options_Click(object sender, RoutedEventArgs e)
        {
            WindowOptions options = new WindowOptions();
            options.ShowDialog();
        }

        private void btn_Creators_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_Version_Click(object sender, RoutedEventArgs e)
        {
            if (text_version.Visibility == Visibility.Visible)
            {
                text_version.Visibility = Visibility.Collapsed;
            }
            else
            {
                text_version.Visibility = Visibility.Visible;
            }
        }

        private void btn_Feedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // here i wish set the parameters of email in this way 
                // 1. mailto = url;
                // 2. subject = txtSubject.Text;
                // 3. body = txtBody.Text;
                Process.Start("mailto:dsvk23020818@outlook.it?subject=GamePets&body=test");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
