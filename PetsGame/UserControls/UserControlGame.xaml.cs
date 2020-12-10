using PetsGame.Properties;
using PetsGame.Windows;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
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
        int mCurrentDay = 1;
        int mMoney = 1;
        int mFreeHour = 1;

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

        string[] dailyMessage =
       {
            Properties.Resources.commentOne,
            Properties.Resources.commentTwo,
            Properties.Resources.commentThree,
            Properties.Resources.commentFive,
            Properties.Resources.commentSix,
            Properties.Resources.commentSeven,
            Properties.Resources.commentEight,
            Properties.Resources.commentNine,
            Properties.Resources.commentTen,
            Properties.Resources.commentEleven,
            Properties.Resources.commentTwelve,
            Properties.Resources.commentThirteen
        };

        private SinglePet mSinglePet;
        #endregion

        #region Loading
        public UserControlGame(SinglePet singlePet, int coins = 1, int freeHours = 1, int currentDay = 1, bool dailySpeciale = false)
        {
            InitializeComponent();

            mSinglePet = singlePet;

            ImageSourceConverter imgs = new ImageSourceConverter();
            imgPet.SetValue(Image.SourceProperty, imgs.ConvertFromString(string.Format("pack://application:,,,/Images/{0}.png", mSinglePet.petType)));

            txt_typePet.Text = mSinglePet.petType;
            txt_effectsPet.Text = mSinglePet.descriptionPet;

            txt_age.Text = mSinglePet.age;
            txt_ageEffects.Text = mSinglePet.descriptionAge;



            rbtnBone.Checked += PriceEffects_CheckedChanged;
            rbtnToy.Checked += PriceEffects_CheckedChanged;
            rbtnFruits.Checked += PriceEffects_CheckedChanged;
            rbtnForage.Checked += PriceEffects_CheckedChanged;

            DataContext = this;

            mMoney = coins;
            mFreeHour = freeHours;
            mCurrentDay = currentDay;
            dailyChoiceSpecialAvailable = dailySpeciale;

            if (dailyChoiceSpecialAvailable == true)
            {
                dailyChoiceSpecial.Visibility = Visibility.Visible;
            }

            pbar_days.Value = ((double)mCurrentDay / mSinglePet.totalDays) * 100;
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
                    str += arrayList[rand].ToString();

                    arrayList.RemoveAt(rand);
                    queue.Enqueue(str);
                }
            }
            else
            {
                txt_deilyMessage.Visibility = Visibility.Hidden;
            }

            #endregion

            tblcDay.Text = string.Format($"{mCurrentDay}/{mSinglePet.totalDays}");

            txt_petsName.Text = mSinglePet.petName;
            txtb_currentHappiness.Text = mSinglePet.mHappiness.ToString();
            txtb_currentMoney.Text = mMoney.ToString();
            txtb_currentFreeHours.Text = mFreeHour.ToString();
            txtb_currentHunger.Text = mSinglePet.hunger.ToString();

            if (queue.Count > 0)
            {
                txt_deilyMessage.Text = queue.Dequeue().ToString();
            }

            dailyChoiceWalk.Content = string.Format($"Take {mSinglePet.petName} for a walk (+{happiniessForWalk} ❤, -1 free hour)");
            dailyChoiceFeed.Content = string.Format($"Feed your pet {mSinglePet.petName} with vegetables (+5 ❤, -1 hunger, -1$)");
            dailyChoiceSpecial.Content = string.Format($"Play with your pet {mSinglePet.petName} (+15 ❤, -2 free hours)");
        }

        #endregion

        #region Menu button
        void RestartGame()
        {
            (Parent as Window).Content = new UserControlSelectPet();
        }

        private void ShowMenu(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.4;
            this.Effect = new BlurEffect();

            WindowMenu w = new WindowMenu(mSinglePet, mMoney, mFreeHour, mCurrentDay, dailyChoiceSpecialAvailable)
            {
                Owner = this.Parent as Window,
                ShowInTaskbar = false
            };

            if (w.ShowDialog() == false)
            {
                string command = w.command;

                this.Opacity = 1;
                this.Effect = null;
                (Parent as Window).ShowInTaskbar = true;


                if (command == "Restart")
                {
                    RestartGame();
                }
                else if (command == "Main menu")
                {
                    (Parent as Window).Content = new UserControlMainWindow();
                }
                else if (command == "Exit")
                {

                }
            }
        }
        #endregion

        #region Play game
        private void Play()
        {
            dailyChoiceSpecial.Visibility = Visibility.Hidden;

            txtb_currentHappiness.Text = mSinglePet.mHappiness.ToString();
            txtb_currentHunger.Text = mSinglePet.hunger.ToString();
            txtb_currentMoney.Text = mMoney.ToString();
            txtb_currentFreeHours.Text = mFreeHour.ToString(); ;

            pbar_days.Value = ((double)mCurrentDay / mSinglePet.totalDays) * 100;

            mCurrentDay++;

            if (mCurrentDay > mSinglePet.totalDays)
            {
                Finaly();
            }
            else
            {
                tblcDay.Text = string.Format($"{mCurrentDay}/{mSinglePet.totalDays}");

                if (mCurrentDay == mSinglePet.totalDays)
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

                    if (mFreeHour < 2)
                    {
                        dailyChoiceSpecial.IsEnabled = false;
                    }
                    else
                    {
                        dailyChoiceSpecial.IsEnabled = true;
                    }
                }
                else
                {
                    if (dailyChoiceSpecial.IsChecked == true)
                    {
                        dailyChoiceWalk.IsChecked = true;
                    }
                }
            }

            CheckMoneyForBuy();
        }

        private void CheckMoneyForBuy()
        {
            // btn_buy.IsEnabled = false;

            if (mMoney >= priceFruits)
            {
                btn_buy.IsEnabled = true;
            }
        }
        #endregion

        #region Finish game
        private void Finaly()
        {
            btn_nextDay.Visibility = Visibility.Hidden;

            this.Opacity = 0.4;
            this.Effect = new BlurEffect();

            WindowFinalResultGame finalResultGame = new WindowFinalResultGame(mSinglePet.mHappiness)
            {
                Owner = this.Parent as Window,
                ShowInTaskbar = false
            };

            finalResultGame.ShowDialog();

            this.Opacity = 1;
            this.Effect = null;

            (Parent as Window).ShowInTaskbar = true;
            RestartGame();
        }
        #endregion

        #region Next day button
        private void btn_nextDay_Click(object sender, RoutedEventArgs e)
        {
            if (dailyChoiceWalk.IsChecked == true && mFreeHour >= 1)
            {
                mSinglePet.mHappiness += happiniessForWalk;
                mFreeHour -= 2;
            }
            else if (dailyChoiceFeed.IsChecked == true && mMoney > 1)
            {
                mSinglePet.mHappiness += 5;
                mMoney -= 2;
                mSinglePet.hunger -= 2;
            }
            else if (dailyChoiceSpecial.IsChecked == true && dailyChoiceSpecialAvailable == true && mFreeHour >= 2)
            {
                mSinglePet.mHappiness += 15;
                mFreeHour -= 3;
            }

            random = new Random();

            if (mSinglePet.hunger > 0)
            {
                mSinglePet.mHappiness -= (mSinglePet.hunger * mSinglePet.hungerModifier) + random.Next(0, 2);
            }

            if (bigBone == false)
            {
                mSinglePet.hunger += 1;
            }

            if (mSinglePet.hunger < 0)
            {
                mSinglePet.hunger = 0;
            }

            mSinglePet.mHappiness += mSinglePet.happyBonus;
            mMoney += 1;
            mFreeHour += 1;
            Play();
        }
        #endregion;

        #region Buy button
        private void btn_buy_Click(object sender, RoutedEventArgs e)
        {
            if (rbtnBone.IsChecked == true)
            {
                if (bigBone == false && mMoney >= priceBone)
                {
                    bigBone = true;
                    mSinglePet.hunger = 0;
                    mMoney -= priceBone;

                    txtb_currentHunger.Text = mSinglePet.hunger.ToString();
                    txtb_currentMoney.Text = mMoney.ToString();
                }


            }
            else if (rbtnToy.IsChecked == true)
            {
                if (mMoney >= priceToy)
                {
                    mSinglePet.happyBonus += 1;
                    mMoney -= priceToy;

                    txtb_currentMoney.Text = mMoney.ToString();

                }

            }
            else if (rbtnForage.IsChecked == true)
            {
                if (mMoney >= priceVegetables)
                {
                    mSinglePet.hungerModifier -= 1;
                    mMoney -= priceVegetables;

                    txtb_currentMoney.Text = mMoney.ToString();
                }

            }
            else if (rbtnFruits.IsChecked == true)
            {
                if (mMoney >= priceFruits)
                {
                    mSinglePet.hunger -= 2;
                    mMoney -= priceFruits;

                    txtb_currentMoney.Text = mMoney.ToString();
                    txtb_currentHunger.Text = mSinglePet.hunger.ToString();
                }
            }
        }
        #endregion

        #region When checked your choice, view effects and price of food or toy
        private void PriceEffects_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (rbtnFruits.IsChecked == true)
            {
                txtbPriceEffects.Text = "Price: 2$\nEffects: -2 hunger";

            }
            else if (rbtnForage.IsChecked == true)
            {
                txtbPriceEffects.Text = "Price:Price: 10$\nEffects: hunger is weaker";
            }
            else if (rbtnToy.IsChecked == true)
            {
                txtbPriceEffects.Text = "Price 4$\nEffects: +1 ❤ evrery day";

            }
            else if (rbtnBone.IsChecked == true)
            {
                txtbPriceEffects.Text = "Price: 14$\nEffects: no more hunger";
            }
        }
        #endregion

    }
}
