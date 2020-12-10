using PetsGame.Properties;
using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PetsGame.UserControls
{
    /// <summary>
    /// Interaction logic for UserControlSelectPet.xaml
    /// </summary>
    public partial class UserControlSelectPet : UserControl
    {
        #region Variables
        private int mWins;

        private readonly SinglePet mSinglePet = new SinglePet();
        #endregion

        #region Propperties              
        public Animals SelectedPet { get; set; }
        #endregion

        #region Loading game
        public UserControlSelectPet()
        {

            SelectedPet = new Animals();

            InitializeComponent();

            rbtn_oneMonth.Checked += AgeChoice_CheckedChanged;
            rbtn_sixMonth.Checked += AgeChoice_CheckedChanged;
            rbtn_year.Checked += AgeChoice_CheckedChanged;

            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mWins = (int)Settings.Default["wins"];

            Wins.Text = "Wins " + mWins + " ❤";

            if (mWins >= 1)
            {
                rbtn_sixMonth.IsEnabled = true;

                if (mWins >= 2)
                {
                    rbtn_year.IsEnabled = true;

                    if (mWins >= 5)
                    {
                        kitty.IsEnabled = true;
                        rbtn_kitty.IsEnabled = true;

                        if (mWins >= 10)
                        {
                            parrot.IsEnabled = true;
                            rbtn_parrot.IsEnabled = true;

                            if (mWins >= 15)
                            {
                                hamster.IsEnabled = true;
                                rbtn_hamster.IsEnabled = true;

                                if (mWins >= 20)
                                {
                                    panda.IsEnabled = true;
                                    rbtn_panda.IsEnabled = true;
                                }
                            }
                        }
                    }
                }
            }
            textBox_name.Text = (string)Settings.Default["default_name"];
        }
        #endregion

        #region Restart button
        private void btn_Restar_Clicked(object sender, RoutedEventArgs e)
        {
            RestarGame();
        }

        private void RestarGame()
        {
            (Parent as Window).Content = new UserControlSelectPet();
        }
        #endregion

        #region Main Menu button
        private void btn_mainMenu_Cliked(object sender, RoutedEventArgs e)
        {
            (Parent as Window).Content = new UserControlMainWindow();
        }
        #endregion

        #region Exit button
        private void btn_exit_Cliked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Add Wins button
        private void btn_AddWin_Clicked(object sender, RoutedEventArgs e)
        {
            mWins++;

            Settings.Default["wins"] = mWins;
            Settings.Default.Save();

            Wins.Text = "Wins " + mWins + " ❤";

            RestarGame();
        }
        #endregion

        #region Reset Result button
        private void btn_ResetResult_Clicked(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to erase your results and achievements?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                mWins = 0;

                Wins.Text = "Wins 0 ❤";

                Settings.Default["wins"] = mWins;
                Settings.Default.Save();
            }
        }
        #endregion

        #region Start button
        private void btn_start_Cliked(object sender, RoutedEventArgs e)
        {
            if (textBox_name.Text.Length > 0)
            {
                mSinglePet.petName = textBox_name.Text;

                string petType = " ";
                string description = " ";

                //Pet choice
                if (rbtn_puppy.IsChecked == true)
                {
                    mSinglePet.mHappiness += 5;

                    petType = "Puppy";
                    description += "+5 ❤";
                }
                else if (rbtn_kitty.IsChecked == true)
                {
                    mSinglePet.totalDays += 1;

                    petType = "Kitty";
                    description += "+1 day";
                }
                else if (rbtn_parrot.IsChecked == true)
                {
                    mSinglePet.hungerModifier -= 1;

                    petType = "Parrot";
                    description += "-1 hunger -50% weaker";
                }
                else if (rbtn_hamster.IsChecked == true)
                {
                    mSinglePet.mHappiness += 10;
                    mSinglePet.totalDays -= 1;

                    petType = "Hamster";
                    description += "+10 ❤, -1 day";
                }
                else if (rbtn_panda.IsChecked == true)
                {
                    mSinglePet.happyBonus += 1;

                    petType = "Panda";
                    description += "+1 ❤ every day";
                }


                string age = " ";
                //Age choice
                if (rbtn_oneMonth.IsChecked == true)
                {
                    mSinglePet.totalDays += 1;
                    mSinglePet.hunger += 1;
                    age += "1 month";
                }
                else if (rbtn_sixMonth.IsChecked == true)
                {
                    age += "6 months";
                }
                else if (rbtn_year.IsChecked == true)
                {
                    mSinglePet.totalDays -= 1;
                    mSinglePet.mHappiness += 1;
                    age += "12 months";
                }
                string description_age = txt_effects.Text.Replace("\n", ", ");

                mSinglePet.petType = petType;
                mSinglePet.descriptionPet = description;
                mSinglePet.age = age;
                mSinglePet.descriptionAge = description_age;

                (Parent as Window).Content = new UserControlGame(mSinglePet);
            }
            else
            {
                txt_errorPetName.Visibility = Visibility.Visible;
            }

        }
        #endregion

        #region Checked Age Choice
        private void AgeChoice_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (rbtn_oneMonth.IsChecked == true)
            {
                txt_effects.Text = "+1 Day\n+1 huger";
            }
            else if (rbtn_sixMonth.IsChecked == true)
            {
                txt_effects.Text = "none";
            }
            else if (rbtn_year.IsChecked == true)
            {
                txt_effects.Text = "+1 ❤\n-1 day";
            }
        }
        #endregion

        private void ChangedPet(object sender, RoutedEventArgs e)
        {
            if (rbtn_puppy.IsChecked == true)
            {
                SelectedPet.Type = PetType.Puppy;
            }
            else if (rbtn_kitty.IsChecked == true)
            {
                SelectedPet.Type = PetType.Kitty;
            }
            else if (rbtn_parrot.IsChecked == true)
            {
                SelectedPet.Type = PetType.Parrot;
            }
            else if (rbtn_hamster.IsChecked == true)
            {
                SelectedPet.Type = PetType.Hamster;
            }
            else if (rbtn_panda.IsChecked == true)
            {
                SelectedPet.Type = PetType.Panda;
            }
        }
    }
}

