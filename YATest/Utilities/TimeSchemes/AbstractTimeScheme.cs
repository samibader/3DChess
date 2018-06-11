using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace YATest.Utilities
{
    abstract class AbstractTimeScheme
    {
        protected Timer clock;
        protected long secondsElapsedPlayer1;
        protected long secondsElapsedPlayer2;
        protected long countDownPeriod;
        protected bool isPlayer1Turn;
        protected bool isHistory;

        protected String timeStringPlayer1;

        /// <summary>
        /// Gets/Sets the string that represent the time left for player 1.
        /// </summary>
        public String TimeStringPlayer1
        {
            get { return timeStringPlayer1; }
            set { timeStringPlayer1 = value; }
        }

        protected String timeStringPlayer2;

        /// <summary>
        /// Gets/Sets the string that represent the time left for player 2.
        /// </summary>
        public String TimeStringPlayer2
        {
            get { return timeStringPlayer2; }
            set { timeStringPlayer2 = value; }
        }

        /// <summary>
        /// CountDownTimer constructor, constructs a new object with count down period set.
        /// </summary>
        /// <param name="countDownPeriod">The count down period for both players in seconds</param>
        public AbstractTimeScheme(long countDownPeriod)
        {
            timeStringPlayer1 = "00:00";
            timeStringPlayer2 = "00:00";

            clock = new Timer(1000);
            clock.Elapsed += new ElapsedEventHandler(clock_Elapsed);

            this.countDownPeriod = countDownPeriod;
            secondsElapsedPlayer1 = countDownPeriod;
            secondsElapsedPlayer2 = countDownPeriod;
        }

        /// <summary>
        /// Clock handler, override this method to control timing logic
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Parameters</param>
        protected virtual void clock_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Set time
            if (isPlayer1Turn == true)
                secondsElapsedPlayer1--;
            else
                secondsElapsedPlayer2--;

            //Finish the game when time hits the limits
            if ((secondsElapsedPlayer1 == 0) || (secondsElapsedPlayer2 == 0))
            {
                GameManager.getReference(null).finishGame(GameManager.getReference(null).curPlayer());
                clock.Stop();
                return;
            }

            //Set time string for player1 and player2
            long hours = secondsElapsedPlayer1 / 60 / 60;
            long minutes = (secondsElapsedPlayer1 - hours * 60) / 60;
            long seconds = secondsElapsedPlayer1 % 60;
            if (hours == 0)
                TimeStringPlayer1 = ((minutes.ToString().Length == 1) ? ("0" + minutes.ToString()) : minutes.ToString()) + ":"
                + ((seconds.ToString().Length == 1) ? ("0" + seconds.ToString()) : seconds.ToString());
            else
                TimeStringPlayer1 = ((hours.ToString().Length == 1) ? ("0" + hours.ToString()) : hours.ToString()) + ":"
                + ((minutes.ToString().Length == 1) ? ("0" + minutes.ToString()) : minutes.ToString()) + ":"
                + ((seconds.ToString().Length == 1) ? ("0" + seconds.ToString()) : seconds.ToString());

            hours = secondsElapsedPlayer2 / 60 / 60;
            minutes = (secondsElapsedPlayer2 - hours * 60) / 60;
            seconds = secondsElapsedPlayer2 % 60;
            if(hours == 0)
                TimeStringPlayer2 = ((minutes.ToString().Length == 1) ? ("0" + minutes.ToString()) : minutes.ToString()) + ":"
                 + ((seconds.ToString().Length == 1) ? ("0" + seconds.ToString()) : seconds.ToString());
            else
                TimeStringPlayer2 = ((hours.ToString().Length == 1) ? ("0" + hours.ToString()) : hours.ToString()) + ":"
                + ((minutes.ToString().Length == 1) ? ("0" + minutes.ToString()) : minutes.ToString()) + ":"
                + ((seconds.ToString().Length == 1) ? ("0" + seconds.ToString()) : seconds.ToString());
        }


        /// <summary>
        /// Starts the timer from zero
        /// </summary>
        public void Start()
        {
            secondsElapsedPlayer1 = countDownPeriod;
            secondsElapsedPlayer2 = countDownPeriod;
            clock.Start();
        }

        /// <summary>
        /// Pauses the timer, can be resumed using Continue method
        /// </summary>
        public void Pause()
        {
            clock.Stop();
        }

        /// <summary>
        /// Resumes the timer, can be reset using Continue method
        /// </summary>
        public void Continue()
        {
            clock.Start();
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            clock.Stop();
        }

        public virtual bool curTimeCritical()
        {
            if (isPlayer1Turn == true)
                return secondsElapsedPlayer1 < 10;
            else
                return secondsElapsedPlayer2 < 10;
        }


        /// <summary>
        /// When called, stops counting down for previous players and starts counting for
        /// the other one.
        /// </summary>
        /// <param name="isPlayer1Turn">Indicates whether the current turn is Player1's turn</param>
        public virtual void SwitchTurn(bool isPlayer1Turn, bool isHistory)
        {
            this.isHistory = isHistory;
            this.isPlayer1Turn = isPlayer1Turn;
        }
    }
}
