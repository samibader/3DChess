using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.GameLogic;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class PawnHistoryPhase : HistoryPhase
    {
        public bool hasMovesTwoBlocks;

        public PawnHistoryPhase(Pawn oldPiece, Position oldPos, AbstractPiece newPiece, Position newPos)
            : base((AbstractPiece)oldPiece, oldPos, newPiece, newPos)
        {
            hasMovesTwoBlocks = oldPiece.hasMovedTwoBlocks;
        }
    }
}
