using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class PawnMoves : AbstractMove
    {
        public PawnMoves(AbstractPiece piece) : base(piece) { }

        new private void handleMove(Position newMovePos)
        {
            if (positionIsValid(newMovePos))
            {
                if (Chessboard.getReference()[newMovePos.x, newMovePos.y, newMovePos.z] == null) //check if position is empty
                    moves.Add(newMovePos);
            }
        }

        private void handleMoveOpponent(Position newMovePos)
        {
            if (positionIsValid(newMovePos))
                if (Chessboard.getReference()[newMovePos.x, newMovePos.y, newMovePos.z] != null)
                {
                    if (piece.player !=
                        Chessboard.getReference()[newMovePos.x, newMovePos.y, newMovePos.z].player) //the piece is enemy
                        moves.Add(newMovePos);
                }
        }

        private enum EnPassantedPosition { Right, Left, Up, Down };

        private void handleMoveEnPassant(Position newMovePos, EnPassantedPosition opponentPos)
        {
            int xIncrement = 1;
            //int yIncrement = 1;
            int zIncrement = 1;

            if (piece.player is Player2)
            {
                xIncrement = -1;
                //yIncrement = -1;
                zIncrement = -1;
            }

            if (positionIsValid(newMovePos))
            {
                /*important: handle changing coords according to the player*/
                AbstractPiece opponent = null;
                switch (opponentPos)
                {
                    case EnPassantedPosition.Left: opponent = Chessboard.getReference()[piece.position.x - xIncrement, piece.position.y, piece.position.z]; break;
                    case EnPassantedPosition.Right: opponent = Chessboard.getReference()[piece.position.x + xIncrement, piece.position.y, piece.position.z]; break;
                    case EnPassantedPosition.Up: opponent = Chessboard.getReference()[piece.position.x, piece.position.y + zIncrement, piece.position.z]; break;
                    case EnPassantedPosition.Down: opponent = Chessboard.getReference()[piece.position.x, piece.position.y - zIncrement, piece.position.z]; break;
                }

                if (opponent == null)
                    return;

                if ((opponent.player == piece.player) || (opponent.name != ChessNames.Pawn))
                    return;

                if (!((Pawn)opponent).hasMovedTwoBlocks)
                    return;

                if (!History.getReference().peakPhase().oldPiece.Equals(opponent))
                    return;

                //Stack operations to check if it's the last piece moved!! //ATTENTION!

                //It's all ok! add it to available moves
                //opponent.isCapturable = true; //IMPORTANT FOR A.I.
                opponent.isCapturable = true;
                moves.Add(newMovePos);
            }
        }

        public override List<Position> getAllMoves()
        {
            int xIncrement = 1;
            int yIncrement = 1;
            int zIncrement = 1;
            moves.Clear();

            Position piecePos = piece.position;

            if (piece.player is Player2)
            {
                xIncrement = -1;
                yIncrement = -1;
                zIncrement = -1;
            }

            Position possibleMove;

            //y increment
            possibleMove = new Position(piece.position.x, piece.position.y + yIncrement, piece.position.z);
            handleMove(possibleMove);

            //z increment
            possibleMove = new Position(piece.position.x, piece.position.y, piece.position.z + zIncrement);
            handleMove(possibleMove);

            //two steps forward on y
            if ((!piece.hasMoved) && (Chessboard.getReference()[piece.position.x, piece.position.y + yIncrement, piece.position.z] == null))
            {
                possibleMove = new Position(piece.position.x, piece.position.y + 2 * yIncrement, piece.position.z);
                handleMove(possibleMove);
            }

            //two steps forward on z
            if ((!piece.hasMoved) && (Chessboard.getReference()[piece.position.x, piece.position.y, piece.position.z + zIncrement] == null))
            {
                possibleMove = new Position(piece.position.x, piece.position.y, piece.position.z + 2 * zIncrement);
                handleMove(possibleMove);
            }

            //opponent moves
            possibleMove = new Position(piece.position.x + xIncrement, piece.position.y, piece.position.z + yIncrement);
            handleMoveOpponent(possibleMove);
            possibleMove = new Position(piece.position.x - xIncrement, piece.position.y, piece.position.z + yIncrement);
            handleMoveOpponent(possibleMove);
            possibleMove = new Position(piece.position.x, piece.position.y + yIncrement, piece.position.z + zIncrement);
            handleMoveOpponent(possibleMove);

            //En Passant moves حيوووووووووتي إت 
            possibleMove = new Position(piece.position.x + xIncrement, piece.position.y, piece.position.z + yIncrement);
            handleMoveEnPassant(possibleMove, EnPassantedPosition.Right);
            possibleMove = new Position(piece.position.x - xIncrement, piece.position.y, piece.position.z + yIncrement);
            handleMoveEnPassant(possibleMove, EnPassantedPosition.Left);
            possibleMove = new Position(piece.position.x, piece.position.y + yIncrement, piece.position.z + zIncrement);
            handleMoveEnPassant(possibleMove, EnPassantedPosition.Up);
            possibleMove = new Position(piece.position.x, piece.position.y + yIncrement, piece.position.z - zIncrement);
            handleMoveEnPassant(possibleMove, EnPassantedPosition.Down);


            return moves; // مع السلومة يا حمومة
        }
    }
}
