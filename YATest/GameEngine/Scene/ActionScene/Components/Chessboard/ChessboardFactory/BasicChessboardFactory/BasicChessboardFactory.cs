using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace YATest.GameEngine
{
    class BasicChessboardFactory : ChessboardFactory
    {
        public BasicChessboardFactory(Game game, string gameFile)
            : base(game, gameFile)
        {
            checkerFactory = new RectangleCheckerFactory(game);
            chessModelFactory = new BasicChessModelFactory(game);
            chessboardTextureFactory = new MainBorderTextureFactory(game);
            init();
        }
    }
}
