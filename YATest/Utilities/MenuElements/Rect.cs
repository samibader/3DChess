using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.Utilities.MenuElements
{
    class Rect : HotZone
    {
        #region Properties
        private Color backgroundColor, selectedBackgroundColor, hoveredBackgroundColor;
        protected Color curBackgroundColor;
        protected SpriteBatch curSpriteBatch;
        private Vector2 pos;
        protected Texture2D texture;

        private bool mouseIsIn = false;
        private bool mouseWasIn = false;

        public const int ALPHA_MIN = 140;
        public const int ALPHA_MAX = 220;
        #endregion
        #region Properties
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = new Color(value, ALPHA_MIN); }
        }

        public Color SelectedBackgroundColor
        {
            get { return selectedBackgroundColor; }
            set { selectedBackgroundColor = new Color(value, ALPHA_MIN); }
        }

        public Color HoveredBackgroundColor
        {
            get { return hoveredBackgroundColor; }
            set { hoveredBackgroundColor = new Color(value, ALPHA_MIN); }
        }
        #endregion

        public Rect(Game game, int x, int y, int width, int height)
            : base(game, x, y, width, height)
        {
            curSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            this.pos = new Vector2(x, y);
            this.BackgroundColor = Color.Gray;
            curBackgroundColor = backgroundColor;
            this.SelectedBackgroundColor = Color.DarkGoldenrod;
            this.HoveredBackgroundColor = Color.DarkGray;
            texture = new Texture2D(Game.GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
            base.Click += new ClickHandler(Rect_Click);
            base.Release += new ReleaseHandler(Rect_Release);
            base.MouseIn += new MouseInHandler(Rect_MouseIn);
            base.MouseOut += new MouseOutHandler(Rect_MouseOut);
        }
        #region Events Extensions
        protected void Rect_MouseOut()
        {
            if (mouseIsIn == true)
            {
                mouseIsIn = false;
                mouseWasIn = true;
                curBackgroundColor = new Color(backgroundColor, curBackgroundColor.A);
            }
        }
        protected void Rect_MouseIn()
        {
            if (mouseIsIn == false)
            {
                mouseIsIn = true;
                curBackgroundColor = new Color(hoveredBackgroundColor, curBackgroundColor.A);
            }
        }
        protected void Rect_Click()
        {
            base.MouseIn -= new MouseInHandler(Rect_MouseIn);
            curBackgroundColor = new Color(selectedBackgroundColor, curBackgroundColor.A);
        }
        protected void Rect_Release()
        {
            base.MouseIn += new MouseInHandler(Rect_MouseIn);
        }
        #endregion
        #region DrawableGameComponent Members
        public override void Update(GameTime gameTime)
        {
            if (mouseIsIn == true)
            {
                if (curBackgroundColor.A < ALPHA_MAX)
                    curBackgroundColor.A += 10;
            }
            if (mouseWasIn == true)
            {
                curBackgroundColor.A -= 5;
                if (curBackgroundColor.A < ALPHA_MIN)
                    mouseWasIn = false;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            texture.SetData(new Color[] { curBackgroundColor });
            Game.GraphicsDevice.RenderState.DepthBufferEnable = false;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            curSpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            curSpriteBatch.Draw(texture, hotZone, curBackgroundColor);
            curSpriteBatch.End();
            Game.GraphicsDevice.RenderState.DepthBufferEnable = true;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            base.Draw(gameTime);
        }
        #endregion
    }
}
