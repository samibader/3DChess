using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

//J


namespace YATest.GameLogic
{

    /// <summary>
    /// The Abstract class that all the Moves (MoveClasses) extend .
    /// </summary>
    abstract public class AbstractMove
    {
        protected AbstractPiece piece = null;
        protected List<Position> moves = null;
        /// <summary>
        /// The constructor takes one parameter which is the moving AbstractPiece 
        /// </summary>
        /// <param name="piece"></param>
        public AbstractMove(AbstractPiece piece)
        {
            this.piece = piece;
            moves = new List<Position>();
        }
        /// <summary>
        /// Returns true if the passed position is in the ChessBoard , else returns false .
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected bool positionIsValid(Position pos)
        {
            if (pos.x > 7 || pos.x < 0 || pos.y > 7 || pos.y < 0 || pos.z > 7 || pos.z < 0)
                return false;
            return true;
        }

        /// <summary>
        /// adds the passed position to the List of moves only if the position is valid and has no piece of the same player .
        /// </summary>
        /// <param name="newMovePos"></param>
        protected void handleMove(Position newMovePos)
        {
            if (positionIsValid(newMovePos))
            {
                if (Chessboard.getReference()[newMovePos.x, newMovePos.y, newMovePos.z] == null) //check if position is empty
                    moves.Add(newMovePos);
                else
                {
                    if (piece.player !=
                        Chessboard.getReference()[newMovePos.x, newMovePos.y, newMovePos.z].player) //the piece is enemy
                        moves.Add(newMovePos);
                }
            }
        }

        abstract public List<Position> getAllMoves();
    }
}
