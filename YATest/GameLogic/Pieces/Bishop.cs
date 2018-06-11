using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class Bishop : AbstractPiece
    {
        public Bishop(int x, int y, int z, AbstractPlayer player) :
            base(x, y, z, player)
        {
            name = ChessNames.Bishop;
        }

        public Bishop(Position pos, AbstractPlayer player)
            : base(pos.x, pos.y, pos.z, player)
        {
            name = ChessNames.Bishop;
        }

        public bool hasMovedTwoBlocks = false;

        public override List<Position> getAvailableMoves()
        {
            AbstractMove am = new BishopMoves(this);
            return am.getAllMoves();
        }
        //public override void moveTo(Position newPosition)
        //{
        //    return;
        //}
    }
}
