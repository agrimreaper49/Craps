using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace YouShallNotPass
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int mFirstDiceNum = 0;
        private int mSecondDiceNum = 0;
        private int mTotalCount = 0;
        private int mSimCount = 0;
        private int mStake = 0;
        private int mBetAmount = 0;
        private int mWinCount = 0;
        private int mLoseCount = 0;
        private int mGoal = 0;
        private int mNumPlayCount = 0;
        private bool mGameWon = false;
        public event PropertyChangedEventHandler PropertyChanged;


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int NumPlayCount
        {
            get
            {
                return mNumPlayCount;
            }
            set
            {
                mNumPlayCount = value;
                OnPropertyChanged("NumPlayCount");
            }
        }
        public int ControlWinCount
        {
            get
            {
                return mWinCount;
            }
            set
            {
                mWinCount = value;
                OnPropertyChanged("ControlWinCount");
            }
        }
        public int ControlLoseCount
        {
            get
            {
                return mLoseCount;
            }
            set
            {
                mLoseCount = value;
                OnPropertyChanged("ControlLoseCount");
            }
        }
        private int updateDice()
        {
            Random r = new Random();
            //Generate a random number between 1 and 6
            mFirstDiceNum = r.Next(1, 7);
            mSecondDiceNum = r.Next(1, 7);
            return mFirstDiceNum + mSecondDiceNum;
        }
        private int mRunCount = 0;
        public void ButtonOnClick(Object sender, RoutedEventArgs e)
        {
            mSimCount = (int)SimCount.ControlValue;
            mStake = (int)Stakes.ControlValue;
            mBetAmount = (int)Bets.ControlValue;
            mGoal = (int)Profits.ControlValue;
            progress.Maximum = mSimCount;

            DispatcherTimer dispatcherTimer;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(sim);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            dispatcherTimer.Start();
        }

        public void sim(object sender, EventArgs e)
        {
            if (mRunCount >= mSimCount)
            {
                DispatcherTimer dt = sender as DispatcherTimer;
                dt.Stop();
            } 
            else
            {
                bool iswin = wonOrLost(mStake, mBetAmount, mGoal);
                if (iswin)
                {
                    ControlWinCount++;
                    mWinCount = ControlWinCount;
                }
                else
                {
                    ControlLoseCount++;
                    mLoseCount = ControlLoseCount;
                }
                mRunCount++;
                NumPlayCount++;
            }
           
        }

        public bool wonOrLost(int stake, int bet, int goal)
        {
            while (stake > 0 && stake < goal)
            {
                bool prevRolledCount = false;
                int rollCount = updateDice();
                int secondRollCount = 0;
                if (rollCount == 7 || rollCount == 11)
                {
                    bet *= 2;
                    stake += bet;
                }
                else if (rollCount == 2 || rollCount == 3 || rollCount == 12)
                {
                    stake -= bet;
                }
                else
                {
                    secondRollCount = updateDice();
                    if (secondRollCount == rollCount && !prevRolledCount)
                    {
                        prevRolledCount = true;
                        bet *= 2;
                        stake += bet;
                    }
                    else if (secondRollCount == 7 && !prevRolledCount)
                    {
                        stake -= bet;
                    }
                }
                //mStake = stake;
                //mBetAmount = bet;
            }
            if (stake <= 0)
            {
                return false;
            }
            return true;
        }
         
    }
}
