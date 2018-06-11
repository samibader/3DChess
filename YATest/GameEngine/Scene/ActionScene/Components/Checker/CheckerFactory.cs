using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YATest.GameEngine
{
    abstract class CheckerFactory
    {
        public abstract Checker CreateDarkChecker(); //factory method h3h3h3
        public abstract Checker CreateLightChecker(); //factory method h3h3h3
    }
}
