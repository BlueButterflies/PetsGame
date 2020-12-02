using PetsGame.Properties;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        int priceFruits = 2;
        int priceVegetables = 10;
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
        public UserControlGame(string petName, int days, int happyBonus, int hunger, int hungerModifier, string petType, string description, string age, string description_age)
        {
            InitializeComponent();

            namePet = petName;
            mDays = days;
            bonusHappy = happyBonus;
            hungers = hunger;
            modifierHunger = hungerModifier;

            ImageSourceConverter imgs = new ImageSourceConverter();
            imgPet.SetValue(Image.SourceProperty, imgs.ConvertFromString(string.Format("pack://application:,,,/Images/{0}.png", petType)));

            txt_effectsPet.Text = description;
            txt_age.Text = age;
            txt_typePet.Text = petType;

            rbtnBone.Checked += PriceEffects_CheckedChanged;
            rbtnToy.Checked += PriceEffects_CheckedChanged;
            rbtnFruits.Checked += PriceEffects_CheckedChanged;
            rbtnForage.Checked += PriceEffects_CheckedChanged;

            DataContext = this;

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

                    string str = "Daily message:\n";
                    str +=  arrayList[rand].ToString();

                    arrayList.RemoveAt(rand);
                    queue.Enqueue(str);
                }
            }
            else
            {
                txt_deilyMessage.Visibility = Visibility.Hidden;
            }

            #endregion

            DailyBox.Text = string.Format($"Day: {currentDay}/{mDays}");
            txt_petsName.Text = namePet;
            txtb_currentHappiness.Text = happiness.ToString();
            txtb_currentMoney.Text = money.ToString();
            txtb_currentFreeHours.Text = freeHour.ToString();
            txtb_currentHunger.Text = hungers.ToString();

            if (queue.Count > 0)
            {
                txt_deilyMessage.Text = queue.Dequeue().ToString();
            }

            dailyChoiceWalk.Content = string.Format($"Take {namePet} for a walk (+{happiniessForWalk} ❤, -1 free hour)");
            dailyChoiceFeed.Content = string.Format($"Feed your pet {namePet} with vegetables (+5 ❤, -1 hunger, -1$)");
            dailyChoiceSpecial.Content = string.Format($"Play with your pet {namePet} (+15 ❤)");
        }

        #endregion

        #region Menu button
        private void ShowMenu(object sender, RoutedEventArgs e)
        {
           (Parent as Window).Content = new UserControlMainWindow();
        }
        #endregion

        #region Play game
        private void Play()
        {
            dailyChoiceSpecial.Visibility = Visibility.Hidden;

            txtb_currentHappiness.Text = happiness.ToString();
            txtb_currentHunger.Text = hungers.ToString();
            txtb_currentMoney.Text =  money.ToString();
            txtb_currentFreeHours.Text = freeHour.ToString(); ;

            currentDay++;

            if (currentDay > mDays)
            {
                Finaly();
            }
            else
            {
                DailyBox.Text = string.Format($"Day {currentDay}/{mDays}");

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

                    txtb_currentHunger.Text = hungers.ToString();
                    txtb_currentMoney.Text = money.ToString();
                }


            }
            else if (rbtnToy.IsChecked == true)
            {
                if (money >= priceToy)
                {
                    bonusHappy += 1;
                    money -= priceToy;

                    txtb_currentMoney.Text = money.ToString();

                }

            }
            else if (rbtnForage.IsChecked == true)
            {
                if (money >= priceVegetables)
                {
                    modifierHunger -= 1;
                    money -= priceVegetables;

                    txtb_currentMoney.Text =  money.ToString();
                }

            }
            else if (rbtnFruits.IsChecked == true)
            {
                if (money >= priceFruits)
                {
                    hungers -= 2;
                    money -= priceFruits;

                    txtb_currentMoney.Text = money.ToString();
                    txtb_currentHunger.Text = hungers.ToString();
                }
            }
        }
        #endregion

        private void PriceEffects_CheckedChanged(object sender, RoutedEventArgs e)
        {

            if (rbtnBone.IsChecked == true)
            {
                txtbPriceEffects.Text = "Price: 14$\nEffects: no more hunger";
            }
            else if (rbtnForage.IsChecked == true)
            {
                txtbPriceEffects.Text = "Price:Price: 10$\nEffects: hunger is weaker";
            }
            else if (rbtnToy.IsChecked == true)
            {
                txtbPriceEffects.Text = "price 4$\nEffects: +1 ❤ evrery day";

            }
            else if (rbtnFruits.IsChecked == true)
            {
                txtbPriceEffects.Text = "Price: 2$\nEffects: -2 hunger";

            }
        }

    }
}
