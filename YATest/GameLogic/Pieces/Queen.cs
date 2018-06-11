using System.Collections.Generic;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class Queen : AbstractPiece
    {
        public Queen(int x, int y, int z, AbstractPlayer player) :
            base(x, y, z, player)
        {
            name = ChessNames.Queen;
        }

        public Queen(Position pos, AbstractPlayer player)
            : base(pos.x, pos.y, pos.z, player)
        {
            name = ChessNames.Queen;
        }

        public bool hasMovedTwoBlocks = false;

        public override List<Position> getAvailableMoves()
        {
            AbstractMove am1 = new RookMoves(this);
            AbstractMove am2 = new BishopMoves(this);
            List<Position> list = new List<Position>();

            list.AddRange(am1.getAllMoves());
            list.AddRange(am2.getAllMoves());
            return list;
        }
    }
}
