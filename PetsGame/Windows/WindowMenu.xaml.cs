using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for WindowMenu.xaml
    /// </summary>
    public partial class WindowMenu : Window
    {
        public string command;

        #region Private members

        private readonly SinglePet mSinglePet;

        private readonly int mCoins;

        private readonly int mFreeHours;

        private readonly int mCurrentDay;

        private readonly bool mDailySpeciale;
        #endregion

        public WindowMenu(SinglePet singlePet, int coins, int freeHours, int currentDay, bool dailySpeciale)
        {
            InitializeComponent();

            mSinglePet = singlePet;

            mCoins = coins;
            mFreeHours = freeHours;
            mCurrentDay = currentDay;

            mDailySpeciale = dailySpeciale;
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_MainMenu_Click(object sender, RoutedEventArgs e)
        {
            command = "Main menu";
            this.Close();
        }

        private void btn_Restart_Click(object sender, RoutedEventArgs e)
        {
            command = "Restart";
            this.Close();
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            // Create new file
            using (StreamWriter sw = File.CreateText($"save.sav"))
            {
                sw.WriteLine(mSinglePet.petName);
                sw.WriteLine(mSinglePet.totalDays);
                sw.WriteLine(mSinglePet.mHappiness);
                sw.WriteLine(mSinglePet.happyBonus);
                sw.WriteLine(mSinglePet.hunger);
                sw.WriteLine(mSinglePet.hungerModifier);
                sw.WriteLine(mSinglePet.petType);
                sw.WriteLine(mSinglePet.descriptionPet);
                sw.WriteLine(mSinglePet.age);
                sw.WriteLine(mSinglePet.descriptionAge);

                sw.WriteLine(mCoins);
                sw.WriteLine(mFreeHours);
                sw.WriteLine(mCurrentDay);

                sw.WriteLine(mDailySpeciale);
            }

            Close();
        }

        private void Button_Continue(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            checkBox_Music.IsChecked = (bool)Properties.Settings.Default["music"];
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default["music"] = checkBox_Music.IsChecked;
            Properties.Settings.Default.Save();
        }
    }
}
