using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class BishopMoves : AbstractMove
    {
        public BishopMoves(AbstractPiece piece) : base(piece) { }

        public override List<Position> getAllMoves()
        {
            moves.Clear();

            Position piecePos = piece.position;

            int xreflex = 1;
            int yreflex = 1;
            for (int xIncrement = piecePos.x + 1, yIncrement = piecePos.y + 1; ; xIncrement += xreflex, yIncrement += yreflex)
            {
                if (xIncrement < 0 || yIncrement < 0)
                    break;
                if (xIncrement > 7 || yIncrement > 7)
                {
                    if (xreflex == -1) //break this path anyway if the path was visited twice
                        break;
                    xIncrement = piecePos.x;
                    yIncrement = piecePos.y;
                    xreflex = -1;
                    yreflex = -1;
                    continue;
                }

                Position possibleMove = new Position(xIncrement, yIncrement, piecePos.z);
                if (Chessboard.getReference()[xIncrement, yIncrement, piecePos.z] == null)
                    //before adding, check if this move leads to check mate!
                    moves.Add(possibleMove);
                else
                {
                    if (piece.player !=
                        Chessboard.getReference()[xIncrement, yIncrement, piecePos.z].player) //the piece is enemy
                        moves.Add(possibleMove);
                    if (xreflex == -1) //break this path anyway if the path was visited twice
                        break;
                    else
                    {
                        xIncrement = piecePos.x;
                        yIncrement = piecePos.y;
                        xreflex = -1;
                        yreflex = -1;
                        continue;
                    }
                }
            }

            xreflex =-1;
            yreflex = 1;
            for (int xIncrement = piecePos.x - 1, yIncrement = piecePos.y + 1; ; xIncrement += xreflex, yIncrement += yreflex)
            {
                if (xIncrement < 0 || yIncrement < 0)
                {
                    if (xreflex == 1) //break this path anyway if the path was visited twice
                        break;
                    xIncrement = piecePos.x;
                    yIncrement = piecePos.y;
                    xreflex = 1;
                    yreflex = -1;
                    continue;

                }
                if (xIncrement > 7 || yIncrement > 7)
                {
                    if (xreflex == 1) //break this path anyway if the path was visited twice
                        break;
                    xIncrement = piecePos.x;
                    yIncrement = piecePos.y;
                    xreflex =  1;
                    yreflex = -1;
                    continue;
                    
                }

                Position possibleMove = new Position(xIncrement, yIncrement, piecePos.z);
                if (Chessboard.getReference()[xIncrement, yIncrement, piecePos.z] == null)
                    moves.Add(possibleMove);
                else
                {
                    if (piece.player !=
                        Chessboard.getReference()[xIncrement, yIncrement, piecePos.z].player) //the piece is enemy
                        moves.Add(possibleMove);
                    if (xreflex == 1) //break this path anyway if the path was visited twice
                        break;
                    else
                    {
                        xIncrement = piecePos.x;
                        yIncrement = piecePos.y;
                        xreflex =  1;
                        yreflex = -1;
                        continue;
                    }
                }
            }

            yreflex = 1;
            int zreflex = 1;
            for (int yIncrement = piecePos.y + 1, zIncrement = piecePos.z + 1; ; yIncrement += yreflex, zIncrement += zreflex)
            {
                if (yIncrement < 0 || zIncrement < 0)
                    break;
                if (yIncrement > 7 || zIncrement > 7)
                {
                    if (yreflex == -1)
                        break;
                    yIncrement = piecePos.y;
                    zIncrement = piecePos.z;
                    yreflex = -1;
                    zreflex = -1;
                    continue;
                }

                Position possibleMove = new Position(piecePos.x, yIncrement, zIncrement);
                if (Chessboard.getReference()[piecePos.x, yIncrement, zIncrement] == null)
                    moves.Add(possibleMove);
                else
                {
                    if (piece.player !=
                        Chessboard.getReference()[piecePos.x, yIncrement, zIncrement].player) //the piece is enemy
                        moves.Add(possibleMove);
                    if (yreflex == -1) //break this path anyway if the path was visited twice
                        break;
                    else
                    {
                        yIncrement = piecePos.y;
                        zIncrement = piecePos.z;
                        yreflex = -1;
                        zreflex = -1;
                        continue;
                    }
                }
            }

            yreflex = 1;
            zreflex =-1;
            for (int yIncrement = piecePos.y + 1, zIncrement = piecePos.z - 1; ; yIncrement += yreflex, zIncrement += zreflex)
            {
                if (yIncrement < 0 || zIncrement < 0)
                {
                    if (yreflex == -1)
                        break;
                    yIncrement = piecePos.y;
                    zIncrement = piecePos.z;
                    yreflex = -1;
                    zreflex = 1;
                    continue;
                }
                if (yIncrement > 7 || zIncrement > 7)
                {
                    if (yreflex == -1)
                        break;
                    yIncrement = piecePos.y;
                    zIncrement = piecePos.z;
                    yreflex = -1;
                    zreflex =  1;
                    continue;
                }

                Position possibleMove = new Position(piecePos.x, yIncrement, zIncrement);
                if (Chessboard.getReference()[piecePos.x, yIncrement, zIncrement] == null)
                    moves.Add(possibleMove);
                else
                {
                    if (piece.player !=
                        Chessboard.getReference()[piecePos.x, yIncrement, zIncrement].player) //the piece is enemy
                        moves.Add(possibleMove);
                    if (yreflex ==-1) //break this path anyway if the path was visited twice
                        break;
                    else
                    {
                        yIncrement = piecePos.y;
                        zIncrement = piecePos.z;
                        yreflex = -1;
                        zreflex =  1;
                        continue;
                    }
                }
            }

            //testing x-z allignment
            xreflex = 1;
            zreflex = 1;
            for (int xIncrement = piecePos.x + 1, zIncrement = piecePos.z + 1; ; xIncrement += xreflex, zIncrement += zreflex)
            {
                if (xIncrement < 0 || zIncrement < 0)
                    break;
                if (xIncrement > 7 || zIncrement > 7)
                {
                    if (xreflex == -1)
                        break;
                    xIncrement = piecePos.x;
                    zIncrement = piecePos.z;
                    xreflex = -1;
                    zreflex = -1;
                    continue;
                }

                Position possibleMove = new Position(xIncrement, piecePos.y, zIncrement);
                if (Chessboard.getReference()[xIncrement, piecePos.y, zIncrement] == null)
                    moves.Add(possibleMove);
                else
                {
                    if (piece.player !=
                        Chessboard.getReference()[xIncrement, piecePos.y, zIncrement].player) //the piece is enemy
                        moves.Add(possibleMove);
                    if (xreflex == -1) //break this path anyway if the path was visited twice
                        break;
                    else
                    {
                        xIncrement = piecePos.x;
                        zIncrement = piecePos.z;
                        xreflex = -1;
                        zreflex = -1;
                        continue;
                    }
                }
            }

            xreflex =-1;
            zreflex = 1;
            for (int xIncrement = piecePos.x - 1, zIncrement = piecePos.z + 1; ; xIncrement += xreflex, zIncrement += zreflex)
            {
                if (xIncrement < 0 || zIncrement < 0)
                {
                    if (xreflex == 1)
                        break;
                    xIncrement = piecePos.x;
                    zIncrement = piecePos.z;
                    xreflex = 1;
                    zreflex = -1;
                    continue;
                }
                if (xIncrement > 7 || zIncrement > 7)
                {
                    if (xreflex == 1)
                        break;
                    xIncrement = piecePos.x;
                    zIncrement = piecePos.z;
                    xreflex =  1;
                    zreflex = -1;
                    continue;
                }

                Position possibleMove = new Position(xIncrement, piecePos.y, zIncrement);
                if (Chessboard.getReference()[xIncrement, piecePos.y, zIncrement] == null)
                    moves.Add(possibleMove);
                else
                {
                    if (piece.player !=
                            Chessboard.getReference()[xIncrement, piecePos.y, zIncrement].player) //the piece is enemy
                            moves.Add(possibleMove);
                    if (xreflex == 1) //break this path anyway if the path was visited twice
                        break;
                    else
                    {
                        xIncrement = piecePos.x;
                        zIncrement = piecePos.z;
                        xreflex = 1;
                        zreflex = -1;
                        continue;
                    }
                }
            }
            return moves;
        }
    }
}
