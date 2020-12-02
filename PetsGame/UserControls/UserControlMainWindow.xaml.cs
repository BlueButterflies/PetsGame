using PetsGame.Properties;
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
using System.Windows.Media.Effects;
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

        private MediaPlayer player = new MediaPlayer();

        public UserControlMainWindow()
        {
            InitializeComponent();

            Version versions = Assembly.GetExecutingAssembly().GetName().Version;
            text_version.Text = string.Format($"Version {versions.Major}.{versions.Minor}.{versions.Build}");

            text_version.Visibility = Visibility.Visible;

            player.Open(new Uri(string.Format($"{AppDomain.CurrentDomain.BaseDirectory}//backroundMusic.mp3")));
            player.MediaEnded += new EventHandler(MediaEndeds);

            if ((bool)Settings.Default["music"] == true)
            {
                player.Play();
            }
        }

        private void MediaEndeds(object sender, EventArgs e)
        {
            if ((bool)Properties.Settings.Default["music"] == true)
            {
                player.Position = TimeSpan.Zero;
                player.Play();
            }
        }

        private void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            (Parent as Window).Content = new UserControlSelectPet();
        }

        private void btn_Options_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.4;
            this.Effect = new BlurEffect();

            WindowOptions options = new WindowOptions()
            {
                Owner = Parent as Window,
                ShowInTaskbar = false
            };

            options.ShowDialog();

            this.Opacity = 1;
            this.Effect = null;

            if ((bool)Settings.Default["music"] == false)
            {
                player.Stop();
            }
            else
            {
                player.Position = TimeSpan.Zero;
                player.Play();
            }
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
