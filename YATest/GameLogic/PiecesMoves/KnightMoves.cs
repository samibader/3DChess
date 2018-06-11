using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class KnightMoves : AbstractMove
    {
        public KnightMoves(AbstractPiece piece) : base(piece) { }
        public override List<Position> getAllMoves()
        {
            moves.Clear();

            Position p = piece.position;
            Position possibleMove;

            //On +Xs
            possibleMove = new Position(p.x + 2, p.y + 1, p.z);
            handleMove(possibleMove);

            possibleMove = new Position(p.x + 2, p.y - 1, p.z);
            handleMove(possibleMove);

            possibleMove = new Position(p.x + 2, p.y, p.z + 1);
            handleMove(possibleMove);

            possibleMove = new Position(p.x + 2, p.y, p.z - 1);
            handleMove(possibleMove);

            //On -Xs
            possibleMove = new Position(p.x - 2, p.y + 1, p.z);
            handleMove(possibleMove);

            possibleMove = new Position(p.x - 2, p.y - 1, p.z);
            handleMove(possibleMove);

            possibleMove = new Position(p.x - 2, p.y, p.z + 1);
            handleMove(possibleMove);

            possibleMove = new Position(p.x - 2, p.y, p.z - 1);
            handleMove(possibleMove);

            //On +Ys

            possibleMove = new Position(p.x + 1, p.y + 2, p.z);
            handleMove(possibleMove);

            possibleMove = new Position(p.x - 1, p.y + 2, p.z);
            handleMove(possibleMove);

            possibleMove = new Position(p.x, p.y + 2, p.z + 1);
            handleMove(possibleMove);

            possibleMove = new Position(p.x, p.y + 2, p.z - 1);
            handleMove(possibleMove);


            //On -Ys
            possibleMove = new Position(p.x + 1, p.y - 2, p.z);
            handleMove(possibleMove);

            possibleMove = new Position(p.x - 1, p.y - 2, p.z);
            handleMove(possibleMove);

            possibleMove = new Position(p.x, p.y - 2, p.z + 1);
            handleMove(possibleMove);

            possibleMove = new Position(p.x, p.y - 2, p.z - 1);
            handleMove(possibleMove);

            //On +Zs


            possibleMove = new Position(p.x + 1, p.y, p.z + 2);
            handleMove(possibleMove);

            possibleMove = new Position(p.x - 1, p.y, p.z + 2);
            handleMove(possibleMove);

            possibleMove = new Position(p.x, p.y + 1, p.z + 2);
            handleMove(possibleMove);

            possibleMove = new Position(p.x, p.y - 1, p.z + 2);
            handleMove(possibleMove);

            //On -Zs

            possibleMove = new Position(p.x + 1, p.y, p.z - 2);
            handleMove(possibleMove);

            possibleMove = new Position(p.x - 1, p.y, p.z - 2);
            handleMove(possibleMove);

            possibleMove = new Position(p.x, p.y + 1, p.z - 2);
            handleMove(possibleMove);

            possibleMove = new Position(p.x, p.y - 1, p.z - 2);
            handleMove(possibleMove);


            return moves;

        }
    }
}