using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auction_Boxing_2
{
    public class GameTimer
    {
        /// <summary>
        /// A timer that is Enabled after a certain amount of time. 
        /// </summary>
        /// <remarks>
        /// First parameter is the wait time and the second parameter is the active time.
        /// </remarks>
        System.Timers.Timer ActiveTimer;
        System.Timers.Timer WaitTimer;

        /// <summary>
        /// Returns true is WaitTime is over and ActiveTime is still running.
        /// </summary>
        public bool Active
        {
            get { return ActiveTimer.Enabled && !WaitTimer.Enabled; }
        }

        /// <summary>
        /// Returns true if ButtonTimer is Active
        /// </summary>
        public bool Enabled
        {
            get { return ActiveTimer.Enabled; }
        }

        protected int TimerInterval;

        protected int WaitTimerInterval;

        public GameTimer(int WaitTime, int ActiveTime)
        {
            this.WaitTimerInterval = WaitTime;
            this.TimerInterval = ActiveTime + WaitTime;

            this.ActiveTimer = new System.Timers.Timer(TimerInterval);
            this.ActiveTimer.Elapsed += new System.Timers.ElapsedEventHandler(ActiveTimer_Elapsed);

            this.WaitTimer = new System.Timers.Timer(WaitTimerInterval);
            this.WaitTimer.Elapsed += new System.Timers.ElapsedEventHandler(WaitTimer_Elapsed);

        }

        public GameTimer(float WaitTime, float ActiveTime)
        {
            this.WaitTimerInterval = (int)WaitTime;
            this.TimerInterval = (int)(ActiveTime + WaitTime);

            this.ActiveTimer = new System.Timers.Timer(TimerInterval);
            this.ActiveTimer.Elapsed += new System.Timers.ElapsedEventHandler(ActiveTimer_Elapsed);

            this.WaitTimer = new System.Timers.Timer(WaitTimerInterval);
            this.WaitTimer.Elapsed += new System.Timers.ElapsedEventHandler(WaitTimer_Elapsed);

        }

        public void Start()
        {
            ActiveTimer.Start();
            WaitTimer.Start();
        }
        void ActiveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            ActiveTimer.Dispose();
            ActiveTimer = new System.Timers.Timer(TimerInterval);
            ActiveTimer.Elapsed += new System.Timers.ElapsedEventHandler(ActiveTimer_Elapsed);
        }

        void WaitTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            WaitTimer.Dispose();
            WaitTimer = new System.Timers.Timer(WaitTimerInterval);
            WaitTimer.Elapsed += new System.Timers.ElapsedEventHandler(WaitTimer_Elapsed);

        }
    }
}
