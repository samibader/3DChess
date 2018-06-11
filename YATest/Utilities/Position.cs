namespace YATest.Utilities
{
    public struct Position : System.ICloneable 
    {
        public int x;
        public int y;
        public int z;

        public Position(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static bool operator == (Position p1,Position p2)
        {
            if (((object)p1 == null) && ((object)p2 == null))
                return true;
            if (((object)p1 == null) || ((object)p2 == null))
                return false;
            return (p1.x == p2.x && p1.y == p2.y && p1.z == p2.z);
        }

        public static bool operator !=(Position p1, Position p2)
        {
            if (((object)p1 == null) && ((object)p2 == null))
                return false;

            if (((object)p1 == null) || ((object)p2 == null))
                return true;
            return (p1.x != p2.x || p1.y != p2.y || p1.z != p2.z);
        }

        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
        }

        public static Position operator -(Position p1, Position p2)
        {
            return new Position(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
        }

        public override string ToString()
        {
            char c = System.Convert.ToChar(97 + x);
            return c + y.ToString() + (8-z).ToString();
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        private static Position invalidPosition = new Position(-1, -1, -1);
        public  static Position InvalidPosition { get { return Position.invalidPosition; }}

        #region ICloneable Members

        public object Clone()
        {
            return new Position(this.x, this.y, this.z);
        }

        #endregion
    }
}
