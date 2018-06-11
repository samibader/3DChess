using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using YATest.GameLogic;

namespace YATest.GameEngine
{
    class BasicChessModelFactory : ChessModelFactory
    {
        public BasicChessModelFactory(Game game) : base(game)
        {
            modelProvider = new BasicModelProvider(game);
        }

        public override ChessModel CreateChessModel(AbstractPiece logicalPiece)
        {
            return new BasicChessModel(game, logicalPiece, modelProvider);
        }
    }
}
