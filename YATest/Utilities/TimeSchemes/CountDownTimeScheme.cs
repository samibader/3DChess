using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace YATest.Utilities
{
    /// <summary>
    /// Simply counts the time down without any extra minutes of delays. Sometimes called "Sudden Death".
    /// To use this functionality, it's preffered to use long period (such as 90 minutes).
    /// </summary>
    class CountDownTimeScheme : AbstractTimeScheme
    {
        public CountDownTimeScheme(long countDownPeriod) : base(countDownPeriod) { }
    }
}
