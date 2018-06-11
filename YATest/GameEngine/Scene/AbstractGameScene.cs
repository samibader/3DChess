using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace YATest.GameEngine
{
    /// <summary>
    /// AbstractGameScene provides the basic functionality for creating a composite scene.
    /// A Scene is a collection of objects that are drawn together.
    /// </summary>
    class AbstractGameScene : CompoundGameComponent
    {
        public AbstractGameScene(Game game)
            : base(game, null)
        {
            Visible = false;
            Enabled = false;
        }

        public void showScene()
        {
            Visible = true;
            Enabled = true;
        }

        public void hideScene()
        {
            Visible = false;
            Enabled = false;
        }
    }
}
