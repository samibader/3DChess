using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class QueenMoves : AbstractMove
    {
        AbstractMove rookMoves = null;
        AbstractMove bishopMoves = null;
        
        public QueenMoves(AbstractPiece piece) : base(piece) { 
            rookMoves = new RookMoves(piece);
            bishopMoves = new BishopMoves(piece);
        }

        public override List<Position> getAllMoves()
        {
            moves.Clear();

            moves.AddRange(rookMoves.getAllMoves());
            moves.AddRange(bishopMoves.getAllMoves());

            return moves;
        }
    }
}
