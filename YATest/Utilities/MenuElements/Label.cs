using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.Utilities.MenuElements
{
    class Label : HotZone
    {
        private Color textColor, selectedTextColor, hoveredTextColor;
        private Color backgroundColor, selectedBackgroundColor, hoveredBackgroundColor;
        private Color curTextColor, curBackgroundColor;
        private SpriteBatch curSpriteBatch;
        private SpriteFont font;
        private string text;
        private Vector2 pos;
        private Texture2D texture;

        #region Properties

        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public Color SelectedTextColor
        {
            get { return selectedTextColor; }
            set { selectedTextColor = value; }
        }

        public Color HoveredTextColor
        {
            get { return hoveredTextColor; }
            set { hoveredTextColor = value; }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public Color SelectedBackgroundColor
        {
            get { return selectedBackgroundColor; }
            set { selectedBackgroundColor = value; }
        }

        public Color HoveredBackgroundColor
        {
            get { return hoveredBackgroundColor; }
            set { hoveredBackgroundColor = value; }
        }
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        #endregion

        public Label(Game game, int x, int y, SpriteFont font, string text)
            : base(game, x, y, 0, 0)
        {
            curSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            this.font = font;
            this.text = text;
            this.pos = new Vector2(x, y);
            this.textColor = Color.White;
            this.selectedTextColor = Color.White;
            this.hoveredTextColor = Color.White;
            this.backgroundColor = new Color(Color.White, 0);
            this.selectedBackgroundColor = new Color(Color.White, 0);
            this.hoveredBackgroundColor = new Color(Color.White, 0);
            texture = new Texture2D(Game.GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
            base.hotZone.Width = Convert.ToInt32(font.MeasureString(text).X);
            base.hotZone.Height = Convert.ToInt32(font.MeasureString(text).Y);
            base.Click += new ClickHandler(Label_Click);
            base.Release += new ReleaseHandler(Label_Release);
            base.MouseIn += new MouseInHandler(Label_MouseIn);
            base.MouseOut += new MouseOutHandler(Label_MouseOut);
        }

        void Label_MouseOut()
        {
            curTextColor = textColor;
            curBackgroundColor = backgroundColor;
        }

        void Label_MouseIn()
        {
            curTextColor = hoveredTextColor;
            curBackgroundColor = hoveredBackgroundColor;
        }

        void Label_Click()
        {
            base.MouseIn -= new MouseInHandler(Label_MouseIn);
            curTextColor = selectedTextColor;
            curBackgroundColor = selectedBackgroundColor;
        }

        void Label_Release()
        {
            base.MouseIn += new MouseInHandler(Label_MouseIn);
        }

        public override void Draw(GameTime gameTime)
        {
            texture.SetData(new Color[] { curBackgroundColor });
            Game.GraphicsDevice.RenderState.DepthBufferEnable = false;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            curSpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            curSpriteBatch.DrawString(font, text, pos, curTextColor);
            curSpriteBatch.Draw(texture, hotZone, curBackgroundColor);
            curSpriteBatch.End();
            Game.GraphicsDevice.RenderState.DepthBufferEnable = true;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            base.Draw(gameTime);
        }
    }
}
