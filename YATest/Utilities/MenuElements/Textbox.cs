using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace YATest.Utilities.MenuElements
{
    class Textbox : HotZone
    {
        private Color textColor, selectedTextColor, hoveredTextColor;
        private Color backgroundColor, selectedBackgroundColor, hoveredBackgroundColor;
        private Color curTextColor, curBackgroundColor;
        private SpriteBatch curSpriteBatch;
        private SpriteFont font;
        protected KeyboardState curKeyState1, oldKeyState1;
        MouseState curMouseState, oldMouseState;
        protected string text;
        protected string oldText;
        protected string defaultText;
        protected bool isFocused = false;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public string OldText
        {
            set { oldText = value; }
            get { return oldText; }
        }

        private Vector2 pos;
        private Texture2D texture;

        public const int MAX_CHARS = 8;

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
        #endregion

        public Textbox(Game game, int x, int y,  int width, int height, string defaultText, SpriteFont font)
            : base(game, x, y, width, height)
        {
            curSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            this.oldText = this.text = defaultText;
            curKeyState1 = oldKeyState1 = Keyboard.GetState();
            oldMouseState = curMouseState = Mouse.GetState();
            this.font = font;
            this.pos = new Vector2(x, y);
            this.textColor = Color.DarkGray;
            this.selectedTextColor = Color.White;
            this.hoveredTextColor = Color.White;
            this.backgroundColor = new Color(Color.Black, 150);
            this.selectedBackgroundColor = new Color(Color.Black, 100);
            this.hoveredBackgroundColor = new Color(Color.Black, 130);
            texture = new Texture2D(Game.GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
            this.defaultText = defaultText;
            base.Click += new ClickHandler(Label_Click);
            base.Release += new ReleaseHandler(Label_Release);
            base.MouseIn += new MouseInHandler(Label_MouseIn);
            base.MouseOut += new MouseOutHandler(Label_MouseOut);
        }

        void Label_MouseOut()
        {
            if (isFocused == false)
            {
                curTextColor = textColor;
                curBackgroundColor = backgroundColor;
            }
        }

        void Label_MouseIn()
        {
            if (isFocused == false)
            {
                curTextColor = hoveredTextColor;
                curBackgroundColor = hoveredBackgroundColor;
            }
        }

        void Label_Click()
        {
            base.MouseIn -= new MouseInHandler(Label_MouseIn);
            curTextColor = selectedTextColor;
            curBackgroundColor = selectedBackgroundColor;
            isFocused = true;
        }

        void Label_Release()
        {
            base.MouseIn += new MouseInHandler(Label_MouseIn);
        }

        protected void LostFocus()
        {
            curTextColor = textColor;
            curBackgroundColor = backgroundColor;
            isFocused = false;
            if (text == "")
                text = defaultText;
        }

        public override void Draw(GameTime gameTime)
        {
            texture.SetData(new Color[] { curBackgroundColor });
            Game.GraphicsDevice.RenderState.DepthBufferEnable = false;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            curSpriteBatch.Begin(SpriteBlendMode.AlphaBlend); 
            curSpriteBatch.Draw(texture, hotZone, curBackgroundColor);
            curSpriteBatch.DrawString(font, text, pos, curTextColor);
            curSpriteBatch.End();
            Game.GraphicsDevice.RenderState.DepthBufferEnable = true;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            pos.X =X +  (Width / 2) - (font.MeasureString(Text).X / 2);
            pos.Y = Y + (Height / 2) - (font.MeasureString(Text).Y / 2);
            base.Update(gameTime);
        }

        public override void HandleKeyboardInput()
        {
            if (isFocused == true)
            {
                curKeyState1 = Keyboard.GetState();
                //Handle backspace
                if (curKeyState1.IsKeyDown(Keys.Back) && oldKeyState1.IsKeyUp(Keys.Back) && text.Length > 0)
                {
                    Text = Text.Substring(0, Text.Length - 1);
                }
                else
                    if (text.Length < MAX_CHARS)
                    {
                        //Handle character hits + digits
                        for (int i = 48; i <= 90; i++)
                            if (curKeyState1.IsKeyDown((Keys)(i)) && oldKeyState1.IsKeyUp((Keys)(i)))
                            {
                                Text += Convert.ToChar(i);
                            }
                        //Handle space character
                        if (curKeyState1.IsKeyDown(Keys.Space) && oldKeyState1.IsKeyUp(Keys.Space))
                        {
                            Text += ' ';
                        }
                        if (curKeyState1.IsKeyDown(Keys.Enter) && oldKeyState1.IsKeyDown(Keys.Enter))
                        {
                            if (text == "")
                                text = defaultText;
                            LostFocus();
                        }
                    }
                oldKeyState1 = curKeyState1;
            }
            base.HandleKeyboardInput();
        }

        public override void HandleMouseInput()
        {
            curMouseState = Mouse.GetState();
            if (curMouseState.LeftButton == ButtonState.Pressed)
                if (hotZone.Contains(curMouseState.X, curMouseState.Y) == false)
                    LostFocus();
            oldMouseState = curMouseState;
            base.HandleMouseInput();
        }
    }
}
