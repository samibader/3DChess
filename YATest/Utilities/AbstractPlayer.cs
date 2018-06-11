using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//J


namespace YATest.Utilities
{
    abstract public class AbstractPlayer
    {
        protected string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Returns true if both AbstractPlayer objects point to the same player 
        /// </summary>
        /// <param name="ap1"></param>
        /// <param name="ap2"></param>
        /// <returns></returns>
        public static bool operator==(AbstractPlayer ap1, AbstractPlayer ap2)
        {
            if (((object)ap1 == null) && ((object)ap2 == null))
                return true;
            if (((object)ap1 == null) || ((object)ap2 == null))
                return false;
            return (ap1.name == ap2.name);
        }
        /// <summary>
        /// Returns true if both AbstractPlayer objects don't point to the same player
        /// </summary>
        /// <param name="ap1"></param>
        /// <param name="ap2"></param>
        /// <returns></returns>
        public static bool operator !=(AbstractPlayer ap1, AbstractPlayer ap2)
        {
            if (((object)ap1 == null) && ((object)ap2 == null))
                return false;

            if (((object)ap1 == null) || ((object)ap2 == null))
                return true;
            return (ap1.name != ap2.name);
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return name;
        }
    }

    class Player1 : AbstractPlayer
    {
        public Player1()
        {
        }
    }

    class Player2 : AbstractPlayer
    {
        public Player2()
        {
        }
    }
}