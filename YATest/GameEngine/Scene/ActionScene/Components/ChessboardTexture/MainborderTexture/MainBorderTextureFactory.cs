using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.GameEngine
{
    class MainBorderTextureFactory : AbstractBorderTextureFactory
    {
        Game game;

        public MainBorderTextureFactory(Game game)
        {
            this.game = game;
            //MainChessboardTexture.Effect = new BasicEffect(game.GraphicsDevice, null);
            //MainChessboardTexture.Effect.EnableDefaultLighting();
            //MainChessboardTexture.Effect.TextureEnabled = true;
        }

        public override AbstractBorderTexture CreateBorderTexture(float thickness, float width, float bwidth, float height)
        {
            return new MainBorderTexture(game, thickness, width, bwidth, height);
        }
    }
}
