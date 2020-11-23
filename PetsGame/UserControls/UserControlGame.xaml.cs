using PetsGame.Properties;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PetsGame.UserControls
{
    /// <summary>
    /// Interaction logic for UserControlGame.xaml
    /// </summary>
    public partial class UserControlGame : UserControl, IDisposable
    {
        public void Dispose()
        {
        }

        #region Variables
        int mDays = 20;
        int currentDay = 1;
        int happiness = 15;
        int bonusHappy = 0;
        int hungers = 0;
        int modifierHunger = 2;
        int money = 1;
        int freeHour = 1;

        int wins = 0;
        int happiniessForWalk = 13;
        int priceToy = 4;
        int priceFood = 2;
        int priceBone = 14;

        bool bigBone = false;
        bool dailyChoiceSpecialAvailable = false;
        bool dailyCommments;

        Queue queue = new Queue();
        Random random = new Random();

        string namePet = "";

        string[] dailyMessage =
       {
            "Your pet dreamt of you tonight.",
            "Play more and win to unlock other pets.", "Consider taking a break.",
            "There's a secret pet.", "You can always erase your statistics. (but what for?)",
            "Your pet have met another pet for the first time",
            "Your pet has woken up.",
            "Your pet wants to go for a walk.",
            "Your pet loves you.",
            "A big bone is very useful if you're tired of feeding your pet.",
            "Your pet tore your jacket.", "Never kick your pet.",
            "The name is very important (e.g. a parrot called Rex is aggressive)."
        };
        #endregion

        #region Loading
        public UserControlGame(string petName, int days, int happyBonus, int hunger, int hungerModifier, string petType, string description)
        {
            InitializeComponent();

            namePet = petName;
            mDays = days;
            bonusHappy = happyBonus;
            hungers = hunger;
            modifierHunger = hungerModifier;

            ImageSourceConverter imgs = new ImageSourceConverter();
            imgPet.SetValue(Image.SourceProperty, imgs.ConvertFromString(string.Format("pack://application:,,,/Images/{0}.png", petType)));

            txt_description.Text = description;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            wins = (int)Settings.Default["wins"];



            #region writing daily message

            dailyCommments = (bool)Settings.Default["dailyComments"];

            if (dailyCommments == true)
            {
                ArrayList arrayList = new ArrayList();
                arrayList.AddRange(dailyMessage);

                while (arrayList.Count > 0)
                {
                    int rand = random.Next(0, arrayList.Count);

                    string str = arrayList[rand].ToString();

                    arrayList.RemoveAt(rand);
                    queue.Enqueue(str);
                }
            }
            else
            {
                txt_deilyMessage.Visibility = Visibility.Hidden;
            }

            #endregion

            DailyBox.Header = string.Format($"Day: {currentDay}/{mDays}");
            txt_petsName.Text = namePet;
            txtb_currentHappiness.Text = string.Format($"❤ {happiness}");
            txtb_currentmMoney.Text = string.Format($"$ {money}");
            txtb_currentFreeHours.Text = string.Format($"Free Hours {freeHour}");
            txtb_currentHunger.Text = string.Format($"Hunger {hungers}");

            if (queue.Count > 0)
            {
                txt_deilyMessage.Text = queue.Dequeue().ToString();
            }

            dailyChoiceWalk.Content = string.Format($"Take {namePet} for a walk (+{happiniessForWalk} ❤, -1 free hour)");
            dailyChoiceFeed.Content = string.Format($"Feed your pet {namePet} with vegetables (+5 ❤, -1 hunger, -1$)");
            dailyChoiceSpecial.Content = string.Format($"Play with your pet {namePet} (+15 ❤)");
        }

        #endregion

        #region Upper Buttons
        #region Restart button
        private void btn_restart_Click(object sender, RoutedEventArgs e)
        {

            RestarGame();
        }

        private void RestarGame()
        {
            this.Content = new UserControlSelectPet();
        }
        #endregion

        #region Quit to main menu
        private void btn_mainMenu_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControlMainWindow();
        }
        #endregion

        #region Exit button
        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
        #endregion

        #region Play game
        private void Play()
        {
            dailyChoiceSpecial.Visibility = Visibility.Hidden;

            txtb_currentHappiness.Text = "❤  " + happiness;
            txtb_currentHunger.Text = "Hunger " + hungers;
            txtb_currentmMoney.Text = "$ " + money;
            txtb_currentFreeHours.Text = "Free hours " + freeHour;

            currentDay++;

            if (currentDay > mDays)
            {
                Finaly();
            }
            else
            {
                DailyBox.Header = string.Format($"Day {currentDay}/{mDays}");

                if (currentDay == mDays)
                {
                    txt_lastDay.Visibility = Visibility.Visible;
                }

                if (queue.Count > 0)
                {
                    txt_deilyMessage.Text = queue.Dequeue().ToString();
                }

                if (random.Next(0, 6) == 1)
                {
                    dailyChoiceSpecial.Visibility = Visibility.Visible;

                    dailyChoiceSpecialAvailable = true;
                }
                else
                {
                    if (dailyChoiceSpecial.IsChecked == true)
                    {
                        dailyChoiceWalk.IsChecked = true;
                    }
                }
            }
        }
        #endregion

        #region Finish game
        private void Finaly()
        {
            btn_nextDay.Visibility = Visibility.Hidden;
            txt_final.Visibility = Visibility.Visible;
            txt_result.Visibility = Visibility.Visible;

            txt_final.Text = "Final ❤ " + happiness;

            if (happiness >= 100)
            {
                wins++;
                Settings.Default["wins"] = wins;
                Settings.Default.Save();

                txt_result.Text = "You won!";
            }
            else
            {
                txt_result.Text = "You lost, your pet is sad.";
            }
        }
        #endregion

        #region Next day button
        private void btn_nextDay_Click(object sender, RoutedEventArgs e)
        {
            if (dailyChoiceWalk.IsChecked == true && freeHour >= 1)
            {
                happiness += happiniessForWalk;
                freeHour -= 1;
            }

            if (dailyChoiceFeed.IsChecked == true && money > 1)
            {
                happiness += 5;
                money -= 2;
                hungers -= 2;
            }

            if (dailyChoiceSpecial.IsChecked == true && dailyChoiceSpecialAvailable == true)
            {
                happiness += 15;
            }

            random = new Random();

            if (hungers > 0)
            {
                happiness -= (hungers * modifierHunger) + random.Next(0, 2);
            }

            if (bigBone == false)
            {
                hungers += 1;
            }

            if (hungers < 0)
            {
                hungers = 0;
            }

            happiness += bonusHappy;
            money += 1;
            freeHour += 1;
            Play();
        }
        #endregion;

        #region Buy button
        private void btn_buy_Click(object sender, RoutedEventArgs e)
        {
            if (rbtnBone.IsChecked == true)
            {

                if (bigBone == false && money >= priceBone)
                {
                    bigBone = true;
                    hungers = 0;
                    money -= priceBone;

                    txtb_currentHunger.Text = "Hunger " + hungers;
                    txtb_currentmMoney.Text = "$ " + money;
                }


            }
            else if (rbtnToy.IsChecked == true)
            {
                if (money >= priceToy)
                {
                    bonusHappy += 1;
                    money -= priceToy;

                    txtb_currentmMoney.Text = "$ " + money;

                }

            }
            else if (rbtnForage.IsChecked == true)
            {
                if (money >= 10)
                {
                    modifierHunger -= 1;
                    money -= 10;

                    txtb_currentmMoney.Text = "$ " + money;
                }

            }
            else if (rbtnFruits.IsChecked == true)
            {
                if (money >= priceFood)
                {
                    hungers -= 2;
                    money -= priceFood;

                    txtb_currentmMoney.Text = "$ " + money;
                    txtb_currentHunger.Text = "Hunger " + hungers;
                }
            }
        }
        #endregion
    }
}
