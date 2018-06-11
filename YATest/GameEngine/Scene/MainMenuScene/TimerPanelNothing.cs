using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities.MenuElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.GameEngine
{
    class TimerPanelNothing : YesNoPanel
    {
        public TimerPanelNothing(Game game,
            CompoundGameComponent parent,
            int xSource, int ySource,
            int width, int height,
            int xDestination, int yDestination,
            string message
            )
            : base(
            game, parent, 
            xSource, ySource, 
            width, height, 
            xDestination, yDestination, 
            message)
        {
            LoadContent();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.yes.Text = "Save";
            this.no.Text = "Cancel";
        }
    }
}
