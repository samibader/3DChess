using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class ChessboardBuilder
    {
        private enum Axis { XAxis, YAxis, ZAxis };

        private static Position[] positions = new Position[6];

        public static void readXML(string gameFile)
        {

            XElement xGame = XElement.Load(gameFile);

            var xPlayers = (from p in xGame.Descendants("player") select p).ToList();

            foreach (var xPlayer in xPlayers) //we have two player only!
            {
                var xPieces = (from p in xPlayer.Descendants("piece") select p).ToList();
                foreach (var xPiece in xPieces)
                {
                    int x = System.Convert.ToInt16(xPiece.Elements("x").First().Value);
                    int y = System.Convert.ToInt16(xPiece.Elements("y").First().Value);
                    int z = System.Convert.ToInt16(xPiece.Elements("z").First().Value);
                    Position p = new Position(x, y, z);
                    switch (xPiece.Attribute("name").Value)
                    {
                        case "Pawn": positions[(xPlayer.Attribute("name").Value == "player1" ? 0 : 3)] = p; break;
                        case "Rook": positions[(xPlayer.Attribute("name").Value == "player1" ? 1 : 4)] = p; break;
                        case "Knight": positions[(xPlayer.Attribute("name").Value == "player1" ? 2 : 5)] = p; break;
                    }
                }
            }
        }

        public static void fillChessboard()
        {
            Axis axisToDelpoy;
            Position pawn1   = positions[0];
            Position rook1   = positions[1];
            Position knight1 = positions[2];
            Position pawn2   = positions[3];
            Position rook2   = positions[4];
            Position knight2 = positions[5];
            //First check the different axis between the first rook and bishop 
            if ((rook1.x ^ knight1.x) != 0)
                axisToDelpoy = Axis.XAxis;
            else if ((rook1.y ^ knight1.y) != 0)
                axisToDelpoy = Axis.YAxis;
            else
                axisToDelpoy = Axis.ZAxis;

            int[] increment = { 0 /*x*/, 0 /*y*/, 0 /*z*/};
            switch (axisToDelpoy)
            {
                case Axis.XAxis: { if (rook1.x == 7) increment[0] = -1; else increment[0] = 1; } break;
                case Axis.YAxis: { if (rook1.y == 7) increment[1] = -1; else increment[1] = 1; } break;
                case Axis.ZAxis: { if (rook1.z == 7) increment[2] = -1; else increment[2] = 1; } break;
            }

            //start with rook positions
            Chessboard.getReference()[rook1.x, rook1.y, rook1.z] = new Rook(rook1, GameManager.getReference(null).getPlayerOne());
            int temp = rook1.y + (increment[1] * 7);
            Chessboard.getReference()[rook1.x + (increment[0] * 7), temp, rook1.z + (increment[2] * 7)] = new Rook(rook1.x + (increment[0] * 7), temp, rook1.z + (increment[2] * 7), GameManager.getReference(null).getPlayerOne());

            Chessboard.getReference()[rook1.x + increment[0], rook1.y + increment[1], rook1.z + increment[2]] = new Knight(rook1.x + increment[0], rook1.y + increment[1], rook1.z + increment[2], GameManager.getReference(null).getPlayerOne());
            Chessboard.getReference()[
                rook1.x + (increment[0] * 7) - increment[0],
                rook1.y + (increment[1] * 7) - increment[1],
                rook1.z + (increment[2] * 7) - increment[2]]
              = new Knight(
                   rook1.x + (increment[0] * 7) - increment[0],
                   rook1.y + (increment[1] * 7) - increment[1],
                   rook1.z + (increment[2] * 7) - increment[2],
                   GameManager.getReference(null).getPlayerOne());

            Chessboard.getReference()[
                rook1.x + (2 * increment[0]),
                rook1.y + (2 * increment[1]),
                rook1.z + (2 * increment[2])]
                = new Bishop(
                  rook1.x + (2 * increment[0]),
                  rook1.y + (2 * increment[1]),
                  rook1.z + (2 * increment[2]),
                  GameManager.getReference(null).getPlayerOne()
                );

            Chessboard.getReference()[
                rook1.x + (increment[0] * 7) - (2 * increment[0]),
                rook1.y + (increment[1] * 7) - (2 * increment[1]),
                rook1.z + (increment[2] * 7) - (2 * increment[2])]
                = new Bishop(
                rook1.x + (increment[0] * 7) - (2 * increment[0]),
                rook1.y + (increment[1] * 7) - (2 * increment[1]),
                rook1.z + (increment[2] * 7) - (2 * increment[2]),
                  GameManager.getReference(null).getPlayerOne()
                );

            //Hint: Swapping the Queen and the King for the next team must occur
            Chessboard.getReference()
                [rook1.x + (3 * increment[0]), rook1.y + (3 * increment[1]), rook1.z + (3 * increment[2])]
            = new Queen(
                rook1.x + (3 * increment[0]),
                rook1.y + (3 * increment[1]),
                rook1.z + (3 * increment[2]),
                GameManager.getReference(null).getPlayerOne()
                );

            Chessboard.getReference()
                [rook1.x + (4 * increment[0]), rook1.y + (4 * increment[1]), rook1.z + (4 * increment[2])] =
                new King(
                    rook1.x + (4 * increment[0]),
                    rook1.y + (4 * increment[1]),
                    rook1.z + (4 * increment[2]),
                    GameManager.getReference(null).getPlayerOne());

            //Now we'll align the soldiers.
            increment[0] = 0; increment[1] = 0; increment[2] = 0;
            switch (axisToDelpoy)
            {
                case Axis.XAxis: { if (pawn1.x == 7) increment[0] = -1; else increment[0] = 1; } break;
                case Axis.YAxis: { if (pawn1.y == 7) increment[1] = -1; else increment[1] = 1; } break;
                case Axis.ZAxis: { if (pawn1.z == 7) increment[2] = -1; else increment[2] = 1; } break;
            }

            for (int soldierCnt = 0; soldierCnt < 8; soldierCnt++)
                Chessboard.getReference()
                    [pawn1.x + (soldierCnt * increment[0]),
                     pawn1.y + (soldierCnt * increment[1]),
                     pawn1.z + (soldierCnt * increment[2])]
                 = new Pawn(
                     pawn1.x + (soldierCnt * increment[0]),
                     pawn1.y + (soldierCnt * increment[1]),
                     pawn1.z + (soldierCnt * increment[2]),
                     GameManager.getReference(null).getPlayerOne());

            //Now, do the same thing for the second team (paste from the above code, DA!)

            //First check the different axis between the second rook and bishop 
            if ((rook2.x ^ knight2.x) != 0)
                axisToDelpoy = Axis.XAxis;
            else if ((rook2.y ^ knight2.y) != 0)
                axisToDelpoy = Axis.YAxis;
            else
                axisToDelpoy = Axis.ZAxis;

            increment[0] = 0; increment[1] = 0; increment[2] = 0;
            switch (axisToDelpoy)
            {
                case Axis.XAxis: { if (rook2.x == 7) increment[0] = -1; else increment[0] = 1; } break;
                case Axis.YAxis: { if (rook2.y == 7) increment[1] = -1; else increment[1] = 1; } break;
                case Axis.ZAxis: { if (rook2.z == 7) increment[2] = -1; else increment[2] = 1; } break;
            }

            //start with rook positions
            Chessboard.getReference()[rook2.x, rook2.y, rook2.z] = new Rook(rook2, GameManager.getReference(null).getPlayerTwo());

            Chessboard.getReference()[
                rook2.x + (increment[0] * 7),
                rook2.y + (increment[1] * 7),
                rook2.z + (increment[2] * 7)]
                = new Rook(
                   rook2.x + (increment[0] * 7),
                   rook2.y + (increment[1] * 7),
                   rook2.z + (increment[2] * 7),
                   GameManager.getReference(null).getPlayerTwo());

            Chessboard.getReference()[
                rook2.x + increment[0],
                rook2.y + increment[1],
                rook2.z + increment[2]]
                = new Knight(
                    rook2.x + increment[0],
                    rook2.y + increment[1],
                    rook2.z + increment[2],
                    GameManager.getReference(null).getPlayerTwo());

            Chessboard.getReference()[
                    rook2.x + (increment[0] * 7) - increment[0],
                    rook2.y + (increment[1] * 7) - increment[1],
                    rook2.z + (increment[2] * 7) - increment[2]]
                    = new Knight(
                        rook2.x + (increment[0] * 7) - increment[0],
                        rook2.y + (increment[1] * 7) - increment[1],
                        rook2.z + (increment[2] * 7) - increment[2],
                        GameManager.getReference(null).getPlayerTwo());

            Chessboard.getReference()[
                rook2.x + (2 * increment[0]),
                rook2.y + (2 * increment[1]),
                rook2.z + (2 * increment[2])]
                = new Bishop(
                rook2.x + (2 * increment[0]),
                rook2.y + (2 * increment[1]),
                rook2.z + (2 * increment[2]),
                GameManager.getReference(null).getPlayerTwo());

            Chessboard.getReference()[
                rook2.x + (increment[0] * 7) - (2 * increment[0]),
                rook2.y + (increment[1] * 7) - (2 * increment[1]),
                rook2.z + (increment[2] * 7) - (2 * increment[2])] =
                new Bishop(
                    rook2.x + (increment[0] * 7) - (2 * increment[0]),
                    rook2.y + (increment[1] * 7) - (2 * increment[1]),
                    rook2.z + (increment[2] * 7) - (2 * increment[2]),
                    GameManager.getReference(null).getPlayerTwo());

            //Hint: Swapping the Queen and the King for the next team must occur
            Chessboard.getReference()[rook2.x + (3 * increment[0]), rook2.y + (3 * increment[1]), rook2.z + (3 * increment[2])]
                = new Queen(
                    rook2.x + (3 * increment[0]),
                    rook2.y + (3 * increment[1]),
                    rook2.z + (3 * increment[2]),
                    GameManager.getReference(null).getPlayerTwo()
                    );

            Chessboard.getReference()[rook2.x + (4 * increment[0]), rook2.y + (4 * increment[1]), rook2.z + (4 * increment[2])]
                = new King(
                    rook2.x + (4 * increment[0]),
                    rook2.y + (4 * increment[1]),
                    rook2.z + (4 * increment[2]),
                    GameManager.getReference(null).getPlayerTwo()
                    );

            //Now we'll align the soldiers.

            increment[0] = 0; increment[1] = 0; increment[2] = 0;

            switch (axisToDelpoy)
            {
                case Axis.XAxis: { if (pawn2.x == 7) increment[0] = -1; else increment[0] = 1; } break;
                case Axis.YAxis: { if (pawn2.y == 7) increment[1] = -1; else increment[1] = 1; } break;
                case Axis.ZAxis: { if (pawn2.z == 7) increment[2] = -1; else increment[2] = 1; } break;
            }

            for (int soldierCnt = 0; soldierCnt < 8; soldierCnt++)
                Chessboard.getReference()[pawn2.x + (soldierCnt * increment[0]), pawn2.y + (soldierCnt * increment[1]), pawn2.z + (soldierCnt * increment[2])]
                    = new Pawn(
                        pawn2.x + (soldierCnt * increment[0]),
                        pawn2.y + (soldierCnt * increment[1]),
                        pawn2.z + (soldierCnt * increment[2]),
                        GameManager.getReference(null).getPlayerTwo()
                        );
        }
    }
}