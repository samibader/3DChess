using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

//J


namespace YATest.GameLogic
{
    class Chessboard
    {
        
        /// <summary>
        /// 
        /// </summary>
        private Chessboard()
        {
            var f = 10;
            // parse XML for initial layout and store it in initialPieces matrix
            //to be changes
            //parse the XML file here
        }

        private void readLayoutFromXML(string xmlFile)
        {
        }

        private static Chessboard reference = null;

        /// <summary>
        /// Retuens a refrence to the chessBoard .
        /// </summary>
        /// <returns></returns>
        public static Chessboard getReference()
        {
            if (reference == null)
                reference = new Chessboard();
            return reference;
        }

        public static void resetReference()
        {
            if (reference != null)
            {
                reference = null;
            }
        }

        public const int thirdDimension = 8;

        private AbstractPiece[, ,] matrix = new AbstractPiece[8, 8, thirdDimension];

        /// <summary>
        /// Returns a list of the opponent's AbstractPieces that aren't captured .
        /// </summary>
        /// <param name="oppositeSide"></param>
        /// <returns></returns>
        public List<AbstractPiece> getNotCapturedEnemies(AbstractPlayer oppositeSide)
        {
            List<AbstractPiece> enemyPieces = new List<AbstractPiece>();

            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    for (int z = 0; z < thirdDimension; z++)
                        if (matrix[x, y, z] != null)
                            if ((matrix[x, y, z].player != oppositeSide) && (!matrix[x, y, z].IsCaptured))
                                enemyPieces.Add(matrix[x, y, z]);
            return enemyPieces;
        }

        /* Chessboard[0,0,0] = King */
        //this is actually the moveTo facility
        /// <summary>
        /// A helper method 2 the moveTo() method,this where the actual moving happens .
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public AbstractPiece this[int x, int y, int z]
        {
            get { return matrix[x, y, z]; }
            set
            {
                if (value == null)
                {
                    matrix[x, y, z] = null;
                    return;
                }

                if (((AbstractPiece)value).name == ChessNames.King)
                {
                    int kingX = ((AbstractPiece)value).position.x;
                    int kingY = ((AbstractPiece)value).position.y;
                    int kingZ = ((AbstractPiece)value).position.z;

                    if ((kingX - x == -2) && (kingX == 3 || kingX == 4))
                    {
                        AbstractPiece rook = matrix[7, kingY, kingZ];
                        matrix[7, kingY, kingZ] = null;
                        rook.position.x = kingX + 1;
                        matrix[kingX + 1, kingY, kingZ] = rook;
                    }
                    if (kingX - x == 2 && (kingX == 3 || kingX == 4))
                    {
                        AbstractPiece rook = matrix[0, kingY, kingZ];
                        matrix[0, kingY, kingZ] = null;
                        rook.position.x = kingX - 1;
                        matrix[kingX - 1, kingY, kingZ] = rook;
                    }
                }

                if (((AbstractPiece)value).name == ChessNames.Pawn)
                {
                    int oldPosX = ((AbstractPiece)value).position.x;
                    int oldPosY = ((AbstractPiece)value).position.y;
                    int oldPosZ = ((AbstractPiece)value).position.z;

                    //handling first move (two blocks)
                    if ((Math.Abs(oldPosY - y) == 2) ||
                        (Math.Abs(oldPosZ - z) == 2))
                    {
                        //Do internal swap
                        AbstractPiece temp1 = (AbstractPiece)value;
                        //original position
                        matrix[temp1.position.x, temp1.position.y, temp1.position.z] = null;
                        //new position
                        ((Pawn)temp1).hasMovedTwoBlocks = true;
                        temp1.position.x = x;
                        temp1.position.y = y;
                        temp1.position.z = z;
                        matrix[x, y, z] = value;
                        return;
                    }
                    else
                        ((Pawn)((AbstractPiece)value)).hasMovedTwoBlocks = false;
                    //handle unpassant. here only we will handle capturing the piece

                    //I'm not sure, but it looks something like this not the following:
                    //if((Math.Abs(x-oldPosX) != 0) || (Math.Abs(z-oldPosZ) != 0)) //it has captured something
                    //this is correct probably
                    if (Math.Abs(x - oldPosX) != 0)
                        if (Chessboard.getReference()[x, y, z] == null)
                        {//check if it has captured by unpassant move
                            if ((value.player is Player2) && (z - oldPosZ < 0))
                            {
                                Chessboard.getReference()[x, y, z+1].IsCaptured = true;
                                Chessboard.getReference()[x, y, z+1] = null;
                            }
                            else
                            {
                                if ((value.player is Player1) && (z - oldPosZ > 0))
                                {
                                    Chessboard.getReference()[x, y, z - 1].IsCaptured = true;
                                    Chessboard.getReference()[x, y, z - 1] = null;
                                }
                            }
                        }
                }

                AbstractPiece temp = (AbstractPiece)value;
                //original position
                matrix[temp.position.x, temp.position.y, temp.position.z] = null;
                //new position

                temp.position.x = x;
                temp.position.y = y;
                temp.position.z = z;
                matrix[x, y, z] = value;
            }
        }

        //just for printing on the console
        public override string ToString()
        {
            string temp = "";
            for (int k = 0; k < thirdDimension; k++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int i = 0; i < 8; i++)
                        if (matrix[i, j, k] != null)
                            temp += matrix[i, j, k].ToString();
                        else
                            temp += "X";
                    temp += "\n";
                }
                temp += "\n";
            }
            return temp;
        }

        /// <summary>
        /// Returns a list of Positions of the opponent's pieces that can capture the passed Position .
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public List<Position> getThreatningPiece(Position pos, AbstractPlayer player)
        {

            //get all enemy active pieces
            List<AbstractPiece> enemyPieces = Chessboard.getReference().getNotCapturedEnemies(player);

            //Now for each enemy piece, check if it's threatning us, if yes, add it to the list
            List<Position> threatningEnemiesPos = new List<Position>();
            foreach (AbstractPiece enemyPiece in enemyPieces)
            {
                if (enemyPiece is Pawn)
                {
                            if (GameManager.getReference(null).curPlayer() is Player2) //pawns up and increasing z
                            {
                                if ((enemyPiece.position.z + 1 == pos.z) && (Math.Abs(enemyPiece.position.x - pos.x) == 1) && (pos.y == enemyPiece.position.y)) 
                                    threatningEnemiesPos.Add(enemyPiece.position);
                                if ((pos.x == enemyPiece.position.x) && (pos.y - enemyPiece.position.y == 1) && (pos.z - enemyPiece.position.z == 1))
                                    threatningEnemiesPos.Add(enemyPiece.position);
                            }
                            else//pawns down and decreasing z
                            {
                                if ((enemyPiece.position.z - pos.z == 1) && (enemyPiece.position.x + 1 == pos.x || enemyPiece.position.x - 1 == pos.x) && (pos.y == enemyPiece.position.y))
                                    threatningEnemiesPos.Add(enemyPiece.position);
                                if ((pos.x == enemyPiece.position.x) && (enemyPiece.position.y - pos.y == 1) && (enemyPiece.position.z - pos.z == 1))
                                    threatningEnemiesPos.Add(enemyPiece.position);
                            }
                }
                else
                {
                    List<Position> enemyPositions = enemyPiece.getAvailableMoves();
                    foreach (Position availablePosition in enemyPositions)
                        if (availablePosition == pos)
                        {
                            threatningEnemiesPos.Add(enemyPiece.position);
                            break;
                        }
                }
            }
            return threatningEnemiesPos;
        }

    }
}
