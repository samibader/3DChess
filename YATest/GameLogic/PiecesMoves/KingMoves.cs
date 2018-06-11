using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class KingMoves : AbstractMove
    {
        public KingMoves(AbstractPiece piece)
            : base(piece)
        {
           //Empty Constructor
        }

        private enum CastleAllignment { Left, Right} ;

        private bool castlingIsOk(CastleAllignment castleAllign)
        {
            int rookX = ((castleAllign == CastleAllignment.Left) ? 0 : 7);

            AbstractPiece temp = Chessboard.getReference()[rookX, piece.position.y, piece.position.z];
            if (
                (temp != null) && (temp is Rook) &&
                (!temp.hasMoved) && (!piece.hasMoved) &&
                (!((King)piece).isThreatend)
                )
            {
                //check to see if the path is clear
                int kingX = piece.position.x;

                //check if there are any piece along the way
                //compare kingX with rookX to choose what pieces to check
                int xIncrement = (kingX > rookX ? -1 : 1);

                List<Position> freePieces = new List<Position>() ;
                freePieces.Add(new Position(kingX + xIncrement, piece.position.y, piece.position.z));
                freePieces.Add(new Position(kingX + (2 * xIncrement), piece.position.y, piece.position.z));

                if (xIncrement == -1)
                {
                    if (Chessboard.getReference()[kingX + (3 * xIncrement), piece.position.y, piece.position.z] != null)
                        return false;
                    freePieces.Add(new Position(kingX + (3 * xIncrement), piece.position.y, piece.position.z));
                }

                
                //get enemy pieces
                List<AbstractPiece> enemyPieces =
                       Chessboard.getReference().getNotCapturedEnemies
                      (Chessboard.getReference()[piece.position.x, piece.position.y, piece.position.z].player);


                foreach (Position freePiece in freePieces)
                {
                    if(Chessboard.getReference()[freePiece.x, freePiece.y, freePiece.z] != null)
                        return false;
                    
                    foreach(AbstractPiece enemyPiece in enemyPieces)
                    {
                        List<Position> enemyAvailableMoves = enemyPiece.getAvailableMoves();
                        foreach (Position enemyAvailableMove in enemyAvailableMoves)
                        {
                            if (enemyAvailableMove.x == freePiece.x && enemyAvailableMove.y == freePiece.y && enemyAvailableMove.z == freePiece.z)
                                return false;
                        }
                    }
                }
                ;
                return true;
            }
            return false;
        }

        private List<Position> getCastlings()
        {
            List<Position> castlings = new List<Position>();
            if (castlingIsOk(CastleAllignment.Left))
                castlings.Add(new Position(2, piece.position.y, piece.position.z));
            if (castlingIsOk(CastleAllignment.Right))
                castlings.Add(new Position(6, piece.position.y, piece.position.z));
            return castlings;
        }


        public override List<Position> getAllMoves()
        {
            moves.Clear();

            Position piecePos = piece.position;
            Position possibleMove;

            //x increment
            possibleMove = new Position(piece.position.x + 1, piece.position.y, piece.position.z);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x - 1, piece.position.y, piece.position.z);
            handleMove(possibleMove);

            //y increment
            possibleMove = new Position(piece.position.x, piece.position.y + 1, piece.position.z);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x, piece.position.y - 1, piece.position.z);
            handleMove(possibleMove);

            //z increment
            possibleMove = new Position(piece.position.x, piece.position.y, piece.position.z + 1);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x, piece.position.y, piece.position.z - 1);
            handleMove(possibleMove);

            //y-z increment
            possibleMove = new Position(piece.position.x, piece.position.y + 1, piece.position.z + 1);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x, piece.position.y + 1, piece.position.z - 1);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x, piece.position.y - 1, piece.position.z + 1);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x, piece.position.y - 1, piece.position.z - 1);
            handleMove(possibleMove);

            //x-z increment
            possibleMove = new Position(piece.position.x + 1, piece.position.y, piece.position.z + 1);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x + 1, piece.position.y, piece.position.z - 1);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x - 1, piece.position.y, piece.position.z + 1);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x - 1, piece.position.y, piece.position.z - 1);
            handleMove(possibleMove);

            //x-z increment
            possibleMove = new Position(piece.position.x + 1, piece.position.y + 1, piece.position.z);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x + 1, piece.position.y - 1, piece.position.z);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x - 1, piece.position.y + 1, piece.position.z);
            handleMove(possibleMove);
            possibleMove = new Position(piece.position.x - 1, piece.position.y - 1, piece.position.z);
            handleMove(possibleMove);

            if(GameManager.getReference(null).isPlayer1Turn() == true  && piece.player is Player1)
                moves.AddRange(getCastlings());
            if(GameManager.getReference(null).isPlayer1Turn() == false && piece.player is Player2)
                moves.AddRange(getCastlings());

            return moves;
        }
    }
}
