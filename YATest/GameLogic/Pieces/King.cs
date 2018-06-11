using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class King : AbstractPiece
    {
        public King(int x, int y, int z, AbstractPlayer player) :
            base(x, y, z, player)
        {
            name = ChessNames.King;
        }

        public King(Position pos, AbstractPlayer player)
            : base(pos.x, pos.y, pos.z, player)
        {
            name = ChessNames.King;
        }

        public bool isThreatend = false;
        public override List<Position> getAvailableMoves()
        {
            AbstractMove am = new KingMoves(this);
            return am.getAllMoves();
        }

        protected override void PieceCaptured()
        {
            GameManager.getReference(null).finishGame(GameManager.getReference(null).getOpponent());
        }

        //public override void moveTo(Position newPosition)
        //{
        //    return;
        //}
    }
}
