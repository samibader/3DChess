using System.Collections.Generic;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class Rook : AbstractPiece
    {
        public Rook(int x, int y, int z, AbstractPlayer player) :
            base(x, y, z, player)
        {
            name = ChessNames.Rook;
        }

        public Rook(Position pos, AbstractPlayer player)
            : base(pos.x, pos.y, pos.z, player)
        {
            name = ChessNames.Rook;
        }
        
        public override List<Position> getAvailableMoves()
        {
            AbstractMove am = new RookMoves(this);
            return am.getAllMoves();
        }
    }
}
