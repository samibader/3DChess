using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using YATest.GameEngine;
using YATest.Utilities.CameraUtil;

namespace YATest.Utilities
{
    /// <summary>
    /// Handles players turns, contains history logic
    /// </summary>
    class GameManager
    {
        private AbstractPlayer[] players;
        short playerIndex;

        private GameManager()
        {
            players = new AbstractPlayer[3];
            playerIndex = 2;
            players[1] = new Player1();
            players[2] = new Player2();
        }


        private static Game game;
        private static GameManager gameManager = null;
        public static GameManager getReference(Game _game)
        {
            if (gameManager == null)
            {
                game = _game;
                gameManager = new GameManager();
            }
            return
                gameManager;
        }

        public static void resetReference()
        {
            if (gameManager != null)
                gameManager = null;
        }
        public AbstractPlayer getOpponent()
        {
            if (playerIndex == 1)
                return players[2];
            else
                return players[1];
        }
        public AbstractPlayer curPlayer()
        {
            return players[playerIndex];
        }

        public bool isPlayer1Turn()
        {
            return (playerIndex == 1);
        }

        public void toggleTurn(bool isHistory)
        {
            System.Console.WriteLine("Turn has toggeled!");
            if (playerIndex == 1)
            {
                playerIndex = 2;
                ((GameCamera)game.Services.GetService(typeof(BasicCamera))).changeCamera();
            }
            else
            {
                playerIndex = 1;
                ((GameCamera)game.Services.GetService(typeof(BasicCamera))).changeCamera();
            }
            ((PanelTime)game.Services.GetService(typeof(PanelTime))).newTurn(isPlayer1Turn(), isHistory);
        }

        public string getCurPlayerName()
        {
            return players[playerIndex].ToString();
        }

        public void setPlayerOneName(string newName)
        {
            players[1].Name = newName;
        }

        public void setPlayerTwoName(string newName)
        {
            players[2].Name = newName;
        }

        public string getPlayerOneName()
        {
            return players[1].Name;
        }

        public string getPlayerTwoName()
        {
            return players[2].Name;
        }

        public AbstractPlayer getPlayerOne()
        {
            return players[1];
        }

        public AbstractPlayer getPlayerTwo()
        {
            return players[2];
        }

        public string getSecPlayerName()
        {
            if (playerIndex == 1)
                return players[2].ToString();
            else
                return players[1].ToString();
        }

        public void finishGame(AbstractPlayer loser)
        {
            //Get the winner name
            string winner;
            if (loser == players[1])
                winner = players[2].ToString();
            else
                winner = players[1].ToString();
            //Just enable the winning panel!
            ActionScene ags = (ActionScene)game.Services.GetService(typeof(ActionScene));
            foreach(DrawableGameComponent gc in ags.SubComponents)
                if (gc is PanelFinish)
                {
                    ((PanelFinish)gc).setWinnerName(winner);
                    gc.Enabled = true;
                    gc.Visible = true;
                    break;
                }

        }
    }
}
