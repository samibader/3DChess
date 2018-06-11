using System.Collections.Generic;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class Pawn : AbstractPiece
    {
        public Pawn(int x, int y, int z, AbstractPlayer player) :
            base(x, y, z, player)
        {
            name = ChessNames.Pawn;
        }

        public Pawn(Position pos, AbstractPlayer player)
            : base(pos.x, pos.y, pos.z, player)
        {
            name = ChessNames.Pawn;
        }

        public bool hasMovedTwoBlocks = false;

        public override List<Position> getAvailableMoves()
        {
            AbstractMove am = new PawnMoves(this);
            return am.getAllMoves();
        }
        //public override void moveTo(Position newPosition)
        //{
        //    return;
        //}
    }
}
