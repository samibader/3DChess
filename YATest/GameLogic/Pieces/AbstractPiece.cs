using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

//J


namespace YATest.GameLogic
{
    /// <summary>
    /// The Abstarct class which all the Pieces (PieceClasses) extend .
    /// </summary>
    abstract public class AbstractPiece
    {
        /// <summary>
        /// The constructor initializes the position of the piece and its player .
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="player"></param>
        public AbstractPiece(int x, int y, int z, AbstractPlayer player)
        {
            this.player = player;
            position.x = x;
            position.y = y;
            position.z = z;
        }

        public ChessNames name;
        public Position position;
        public AbstractPlayer player;
        public bool isCapturable;
        private bool isCaptured;

        public bool IsCaptured
        {
            get { return isCaptured; }
            set { isCaptured = value;
            if (isCaptured == true)
                PieceCaptured();
            }
        }

        public bool isSelected;
        AbstractMove am;
        public abstract List<Position> getAvailableMoves();

        /// <summary>
        /// Moves the current AbstractPiece to the new passed Position .
        /// </summary>
        /// <param name="newPosition"></param>
        public void moveTo(Position newPosition)
        {
            HistoryPhase hp;
            if (this is Pawn)
            {
                hp = new PawnHistoryPhase(
                        (Pawn)this,
                        this.position,
                        Chessboard.getReference()[newPosition.x, newPosition.y, newPosition.z],
                        newPosition
                    );
            }
            else
            {
                hp = new HistoryPhase(
                        this,
                        this.position,
                        Chessboard.getReference()[newPosition.x, newPosition.y, newPosition.z],
                        newPosition
                        );
            }

            History.getReference().pushPhase(hp);

            this.hasMoved = true;
            Chessboard.getReference()[newPosition.x, newPosition.y, newPosition.z] =
                this;
        }
        public bool hasMoved = false;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string temp = "";
            switch (name)
            {
                case ChessNames.Bishop: temp = "Bishop"; break;
                case ChessNames.King: temp = "King"; break;
                case ChessNames.Knight: temp = "Knight"; break;
                case ChessNames.Pawn: temp = "Pawn"; break;
                case ChessNames.Queen: temp = "Queen"; break;
                case ChessNames.Rook: temp = "Rook"; break;
            }
            return temp;
        }
        
        //
        /// <summary>
        /// Returns a list of the positions of the threatning enemies
        /// </summary>
        /// <returns></returns>
        public List<Position> getPositionOfThreatningEnemies()
        {
            return Chessboard.getReference().getThreatningPiece(this.position, this.player);
        }

        public static bool operator ==(AbstractPiece ap1, AbstractPiece ap2)
        {
            if (((object)ap1 == null) && ((object)ap2 == null))
                return true;
            if (((object)ap1 == null) || ((object)ap2 == null))
                return false;
            return ((ap1.name == ap2.name) && (ap1.player == ap2.player));
        }

        public static bool operator !=(AbstractPiece ap1, AbstractPiece ap2)
        {
            if (((object)ap1 == null) && ((object)ap2 == null))
                return false;

            if (((object)ap1 == null) || ((object)ap2 == null))
                return true;
            return ((ap1.name != ap2.name) || (ap1.player != ap2.player));
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected virtual void PieceCaptured()
        {

        }
    }
}
