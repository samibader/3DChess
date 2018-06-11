using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using YATest.Utilities.CameraUtil;

namespace YATest.GameEngine
{
    class RectangleCheckerFactory : CheckerFactory
    {
        Game game;
        GameCamera cam;

        public RectangleCheckerFactory(Game game)
        {
            float aspectRatio = game.GraphicsDevice.Viewport.AspectRatio;
            cam = (GameCamera)game.Services.GetService(typeof(BasicCamera));
            this.game = game;
        }
        public override Checker CreateDarkChecker()
        {
            return new RectangleChecker(game, Tint.Dark);
        }
        public override Checker CreateLightChecker()
        {
            return new RectangleChecker(game, Tint.Light);
        }
    }
}
