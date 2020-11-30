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

    #region Variables
        int wins;
        int day = 20;
        int happiness = 15;
        int hunger = 0;
        int hungerModifier = 2;
        int happyBonus = 0;
        string petName = "";
        #endregion

    #region Loading game
    public UserControlSelectPet()
    {
        InitializeComponent();

        rbtn_oneMonth.Checked += AgeChoice_CheckedChanged;
        rbtn_sixMonth.Checked += AgeChoice_CheckedChanged;
        rbtn_year.Checked += AgeChoice_CheckedChanged;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        wins = (int)Settings.Default["wins"];

        Wins.Text = "Wins " + wins + " ❤";

        if (wins >= 1)
        {
            rbtn_sixMonth.IsEnabled = true;

            if (wins >= 2)
            {
                rbtn_year.IsEnabled = true;

                if (wins >= 5)
                {
                    kitty.IsEnabled = true;
                    rbtn_kitty.IsEnabled = true;

                    if (wins >= 10)
                    {
                        parrot.IsEnabled = true;
                        rbtn_parrot.IsEnabled = true;

                        if (wins >= 15)
                        {
                            hamster.IsEnabled = true;
                            rbtn_hamster.IsEnabled = true;

                            if (wins >= 20)
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
        this.Content = new UserControlSelectPet();
    }
    #endregion

    #region Main Menu button
    private void btn_mainMenu_Cliked(object sender, RoutedEventArgs e)
    {
        Content = new UserControlMainWindow();
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
        wins++;

        Settings.Default["wins"] = wins;
        Settings.Default.Save();

        Wins.Text = "Wins " + wins + " ❤";

        RestarGame();
    }
    #endregion

    #region Reset Result button
    private void btn_ResetResult_Clicked(object sender, RoutedEventArgs e)
    {
        wins = 0;

        Wins.Text = "Wins 0 ❤";

        Settings.Default["wins"] = wins;
        Settings.Default.Save();
    }
    #endregion

    #region Start button
    private void btn_start_Cliked(object sender, RoutedEventArgs e)
    {
        if (textBox_name.Text.Length > 0)
        {
            petName = textBox_name.Text;

            string petType = " ";
            string description = "Pet effects: ";

            //Pet choice
            if (rbtn_puppy.IsChecked == true)
            {
                happiness += 5;

                petType = "puppy";
                description += "+5 ❤";
            }
            else if (rbtn_kitty.IsChecked == true)
            {
                day += 1;

                petType = "kitty";
                description += "+1 day";
            }
            else if (rbtn_parrot.IsChecked == true)
            {
                hungerModifier -= 1;

                petType = "parrot";
                description += "-1 hunger -50% weaker";
            }
            else if (rbtn_hamster.IsChecked == true)
            {
                happiness += 10;
                day -= 1;

                petType = "hamster";
                description += "+10 ❤, -1 day";
            }
            else if (rbtn_panda.IsChecked == true)
            {
                happyBonus += 1;

                petType = "panda";
                description += "+1 ❤ every day";
            }


            description += "\nAge: ";
            //Age choice
            if (rbtn_oneMonth.IsChecked == true)
            {
                day += 1;
                hunger += 1;
                description += "1 month";
            }
            else if (rbtn_sixMonth.IsChecked == true)
            {
                description += "6 months";
            }
            else if (rbtn_year.IsChecked == true)
            {
                day -= 1;
                happiness += 1;
                description += "12 months";
            }

            petName = textBox_name.Text;
            description += "\nAge " + txt_effects.Text;

            this.Content = new UserControlGame(petName, day, happyBonus, hunger, hungerModifier, petType, description);
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
            txt_effects.Text = "+1 Day, +1 huger";
        }
        else if (rbtn_sixMonth.IsChecked == true)
        {
            txt_effects.Text = "none";
        }
        else if (rbtn_year.IsChecked == true)
        {
            txt_effects.Text = "+1 ❤, -1 day";
        }
    }
    #endregion

    private void ChangedPet(object sender, RoutedEventArgs e)
    {

    }
}

