using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace YATest.Utilities
{
    class FischerTimeScheme : AbstractTimeScheme
    {
        long bonusSeconds;

        /// <summary>
        /// Builds and object of type FischerTimeScheme
        /// </summary>
        /// <param name="countDownPeriod">The count down period for both players in seconds</param>
        /// <param name="bonusSeconds">The bonus period in seconds the player gets after playing a turn</param>
        public FischerTimeScheme(long countDownPeriod, long bonusSeconds) : base(countDownPeriod) {
            this.bonusSeconds = bonusSeconds;
        }

        public override void SwitchTurn(bool isPlayer1Turn, bool isHistory)
        {
            if(isHistory == false) //It's not a history move, if it is, then we shouldn't add extra time
                if (isPlayer1Turn == true)
                    secondsElapsedPlayer2 += bonusSeconds;
                else
                    secondsElapsedPlayer1 += bonusSeconds;
            base.SwitchTurn(isPlayer1Turn, isHistory);
        }
    }
}
