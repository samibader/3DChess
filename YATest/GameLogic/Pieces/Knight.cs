using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class Knight : AbstractPiece
    {
        public Knight(int x, int y, int z, AbstractPlayer player) :
            base(x, y, z, player)
        {
            name = ChessNames.Knight;
        }

        public Knight(Position pos, AbstractPlayer player)
            : base(pos.x, pos.y, pos.z, player)
        {
            name = ChessNames.Knight;
        }
        
        public bool isThreatend = false;
        public override List<Position> getAvailableMoves()
        {
            AbstractMove am = new KnightMoves(this);
            return am.getAllMoves();
        }

        //public override void moveTo(Position newPosition)
        //{
        //    return;
        //}
    }
}
