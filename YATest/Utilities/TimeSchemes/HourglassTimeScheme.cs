using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace YATest.Utilities
{
    class HourglassTimeScheme : AbstractTimeScheme
    {
        public HourglassTimeScheme(long countDownPeriod) : base(countDownPeriod) { }

        protected override void clock_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isPlayer1Turn)
                secondsElapsedPlayer2++;
            else
                secondsElapsedPlayer1++;
            base.clock_Elapsed(sender, e);
        }
    }
}