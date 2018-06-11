using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace YATest.Utilities
{
    class NoTimeScheme : AbstractTimeScheme
    {
        /// <summary>
        /// Builds and object of type NoTimeScheme
        /// </summary>
        public NoTimeScheme()
            : base(5 /*arbitrary number!*/)
        {
        }

        public override void SwitchTurn(bool isPlayer1Turn, bool isHistory)
        {
            //do nothing, because there is no timing-logic here
        }

        public override bool curTimeCritical()
        {
            return false; //always! Don't show the critical time panel
        }

        protected override void clock_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isPlayer1Turn)
            {
                TimeStringPlayer1 = "";
                TimeStringPlayer2 = "waiting";
            }
            else
            {
                TimeStringPlayer2 = "";
                TimeStringPlayer1 = "waiting";
            }

        }
    }
}
