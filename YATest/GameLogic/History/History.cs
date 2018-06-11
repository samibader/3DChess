using System;
using System.Collections.Generic;
using YATest.Utilities;
using YATest.GameEngine;

namespace YATest.GameLogic
{
    class History
    {
        private static History reference = null;
        private List<HistoryPhase> history = new List<HistoryPhase>();
        private int currentPhase = -1;

        private History() { }

        private static GameEngine.Chessboard vchessboard = null;

        public static GameEngine.Chessboard VChessboard
        {
            get { return History.vchessboard; }
            set { History.vchessboard = value; }
        }

        public static History getReference()
        {
            if (reference == null)
                reference = new History();
            return reference;
        }

        public static void resetReference()
        {
            if (reference != null)
                reference = null;
        }

        public void pushPhase(AbstractPiece newPiece, Position newPos, AbstractPiece oldPiece, Position oldPos)
        {
            history.Add(new HistoryPhase(newPiece, newPos, oldPiece, oldPos));
            //PanelInfo event
        }

        public void pushPhase(HistoryPhase hp)
        {
            int totalCount = history.Count;
            for (int recentHistory = currentPhase + 1; (recentHistory < totalCount) && (totalCount > 0); recentHistory++)
                history.RemoveAt(history.Count - 1);
            currentPhase++;
            history.Add(hp);
            PanelHistory.getReference().addMessage(GameManager.getReference(null).curPlayer().ToString() + " moved " + hp.oldPiece.ToString() + " from " + hp.oldPos.ToString() + " to " + hp.newPos.ToString(), GameManager.getReference(null).isPlayer1Turn());
            if (hp.newPiece != null)
                PanelHistory.getReference().addMessage(GameManager.getReference(null).curPlayer().ToString() + " captured a " + hp.newPiece.ToString(), GameManager.getReference(null).isPlayer1Turn());
        }

        public void undo()
        {
            if (currentPhase < 0)
                return;
            //reverse camera and turn
            GameManager.getReference(null).toggleTurn(true); 
            HistoryPhase currHP = history[currentPhase];
            if (currHP.newPiece == null) //we've moved to a null position
            {
                //check castling
                if (currHP.oldPiece.GetType() == typeof(King))
                {
                    if (Math.Abs(currHP.oldPos.x - currHP.newPos.x) > 1) //i.e. castling happened
                    {
                        //just restore the Rook as well
                        //we have to know where the rook is
                        if (currHP.newPos.x < currHP.oldPos.x) //rook is left
                        {

                            AbstractPiece curRook = vchessboard.ModelAt(currHP.newPos.x + 1, currHP.newPos.y, currHP.newPos.z).LogicalPieceRef;

                            Chessboard.getReference()[0, currHP.newPos.y, currHP.newPos.z] = curRook;
                            Console.WriteLine(curRook);
                        }
                        else
                        {
                            AbstractPiece curRook = vchessboard.ModelAt(currHP.newPos.x - 1, currHP.newPos.y, currHP.newPos.z).LogicalPieceRef;
                            Chessboard.getReference()[7, currHP.newPos.y, currHP.newPos.z] = curRook;
                        }
                    }
                }
                //check for unpassant move  
                if (currHP is PawnHistoryPhase)
                {
                    //upassant = changing direction on X axis
                    if (Math.Abs(currHP.oldPos.x - currHP.newPos.x) == 1)
                    {
                        //Just restore the captured pawn, the rest of the code
                        //restores this pawn
                        AbstractPiece capturedPawn = vchessboard.LogicalModelAt(currHP.newPos.x, currHP.newPos.y, currHP.oldPos.z);
                        Chessboard.getReference()[currHP.newPos.x, currHP.newPos.y, currHP.oldPos.z] = capturedPawn;
                        capturedPawn.IsCaptured = false;
                        ((Pawn)capturedPawn).hasMovedTwoBlocks = true;
                    }
                }
                Chessboard.getReference()[currHP.oldPos.x, currHP.oldPos.y, currHP.oldPos.z] = currHP.oldPiece;
                currHP.oldPiece.position = currHP.oldPos;
                if (currHP is PawnHistoryPhase)
                {
                    //restore previous state (just to check if it has moved two steps forward
                    ((Pawn)currHP.oldPiece).hasMovedTwoBlocks = ((PawnHistoryPhase)currHP).hasMovesTwoBlocks;
                }
                currHP.oldPiece.hasMoved = currHP.oldHasMoved;
                currentPhase--;
            }
            else //we've captured something
            {
                Chessboard.getReference()[currHP.oldPos.x, currHP.oldPos.y, currHP.oldPos.z] =
                    currHP.oldPiece;
                currHP.oldPiece.position = currHP.oldPos;
                currHP.oldPiece.isSelected = false;
                Chessboard.getReference()[currHP.newPos.x, currHP.newPos.y, currHP.newPos.z] =
                    currHP.newPiece;
                currHP.newPiece.IsCaptured = false; //for rendering
                currHP.newPiece.isSelected = false;
                currentPhase--;
            }
            PanelHistory.getReference().addMessage("Undo: " + GameManager.getReference(null).curPlayer().ToString() + " moved " + currHP.oldPiece.ToString() + " back from " + currHP.oldPos.ToString() + " to " + currHP.newPos.ToString(), GameManager.getReference(null).isPlayer1Turn());
        }

        public void redo()
        {
            if (currentPhase == history.Count - 1)
                return;
            //reverse camera and turn
            GameManager.getReference(null).toggleTurn(true); 
            currentPhase++;
            HistoryPhase currHP = history[currentPhase];
            if (currHP.newPiece == null) //we've moved to a null position
            {
                Chessboard.getReference()[currHP.newPos.x, currHP.newPos.y, currHP.newPos.z] = currHP.oldPiece;
                currHP.oldPiece.position = currHP.newPos;
            }
            else //we've captured something
            {
                //check castling and enpassant later
                Chessboard.getReference()[currHP.newPos.x, currHP.newPos.y, currHP.newPos.z] = currHP.oldPiece;
                currHP.oldPiece.position = currHP.newPos;
                currHP.oldPiece.isSelected = false;
                currHP.newPiece.IsCaptured = true; //for rendering
                currHP.newPiece.isSelected = false;
            }
            PanelHistory.getReference().addMessage("Redo: " + GameManager.getReference(null).curPlayer().ToString() + " moved " + currHP.oldPiece.ToString() + " again from " + currHP.oldPos.ToString() + " to " + currHP.newPos.ToString(), GameManager.getReference(null).isPlayer1Turn());
        }

        public HistoryPhase peakPhase()
        {
            if (currentPhase < 0)
                return null;
            return history[currentPhase];
        }
    }
}