using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.GameLogic;
using Microsoft.Xna.Framework;

namespace YATest.GameEngine
{
    abstract class ChessModelFactory
    {
        protected Game game;
        protected ModelProvider modelProvider;

        protected ChessModelFactory(Game game)
        {
            this.game = game;
            modelProvider = null;
        }

        public abstract ChessModel CreateChessModel(AbstractPiece logicalPiece); //factory method h3h3h3
    }
}
