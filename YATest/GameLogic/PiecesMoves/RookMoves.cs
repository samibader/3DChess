using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class RookMoves : AbstractMove
    {
        public RookMoves(AbstractPiece piece) : base(piece) { }

        public override List<Position> getAllMoves()
        {
            moves.Clear();

            Position piecePos = piece.position;

            int reflex = 1;
            for (int xIncrement = piecePos.x + 1; ; xIncrement += reflex){
                if (xIncrement < 0)
                    break;
                if (xIncrement > 7){
                    xIncrement = piecePos.x;
                    reflex = -1;
                    continue;
                }

                Position possibleMove = new Position(xIncrement, piecePos.y, piecePos.z);
                if (Chessboard.getReference()[xIncrement, piecePos.y, piecePos.z] == null)
                    moves.Add(possibleMove);
                else
                {
                    if (piece.player != 
                        Chessboard.getReference()[xIncrement, piecePos.y, piecePos.z].player) //the piece is enemy
                        moves.Add(possibleMove);
                    if (reflex == -1) //break this path anyway
                        break;
                    else
                    {
                        xIncrement = piecePos.x;
                        reflex = -1;
                        continue;
                    }
                }
            }
            reflex = 1;
            for (int yIncrement = piecePos.y + 1; ; yIncrement += reflex)
            {
                if (yIncrement < 0)
                    break;
                if (yIncrement > 7)
                {
                    yIncrement = piecePos.y;
                    reflex = -1;
                    continue;
                }

                Position possibleMove = new Position(piecePos.x, yIncrement, piecePos.z);
                if (Chessboard.getReference()[piecePos.x, yIncrement, piecePos.z] == null)
                    moves.Add(possibleMove);
                else
                {
                    if (piece.player !=
                        Chessboard.getReference()[piecePos.x, yIncrement, piecePos.z].player) //the piece is enemy
                        moves.Add(possibleMove);
                    if (reflex == -1) //break this path anyway
                        break;
                    else
                    {
                        yIncrement = piecePos.y;
                        reflex = -1;
                        continue;
                    }
                }
            }
            reflex = 1;
            for (int zIncrement = piecePos.z + 1; ; zIncrement += reflex)
            {
                if (zIncrement < 0)
                    break;
                if (zIncrement > 7)
                {
                    zIncrement = piecePos.z;
                    reflex = -1;
                    continue;
                }

                Position possibleMove = new Position(piecePos.x, piecePos.y, zIncrement);

                if (Chessboard.getReference()[piecePos.x, piecePos.y, zIncrement] == null)
                    moves.Add(possibleMove);
                else
                {
                    if (piece.player !=
                        Chessboard.getReference()[piecePos.x, piecePos.y, zIncrement].player) //the piece is enemy
                        moves.Add(possibleMove);
                    if (reflex == -1) //break this path anyway
                        break;
                    else
                    {
                        zIncrement = piecePos.z;
                        reflex = -1;
                        continue;
                    }
                }
            }
            return moves;
        }
    }
}
