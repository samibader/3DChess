using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;

namespace YATest.GameLogic
{
    class HistoryPhase
    {

        public AbstractPiece oldPiece { set; get; }
        public Position oldPos { set; get; }
        public bool oldHasMoved { set; get; }

        public AbstractPiece newPiece { set; get; }
        public Position newPos { set; get; }
        public bool newHasMoved { set; get; }


        public HistoryPhase(AbstractPiece oldPiece, Position oldPos, AbstractPiece newPiece, Position newPos)
        {
            this.newPiece = newPiece;
            this.newPos = newPos;
            newHasMoved = (newPiece == null) ? false : newPiece.hasMoved;


            oldHasMoved = oldPiece.hasMoved;
            this.oldPiece = oldPiece;
            this.oldPos = oldPos;

        }
    }
}
