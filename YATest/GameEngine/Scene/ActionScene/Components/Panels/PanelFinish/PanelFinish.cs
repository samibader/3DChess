using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using YATest.Utilities;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using YATest.Utilities.CameraUtil;

namespace YATest.GameEngine
{
    /// <summary>
    /// Simply a panel that shows time of players
    /// </summary>
    class PanelFinish : CompoundGameComponent, IControllable, IControlBlocker
    {
        private Texture2D background;
        private SpriteFont fontBig;
        private SpriteFont fontSmall;
        private SpriteBatch curSpriteBatch;
        private string strWinner;
        private string strTip;
        private int strTransparency;
        private bool isBlocked;
        private KeyboardState curKeyState, oldKeyState;
        private int numBlocksY;
        private int numBlocksX;
        private Vector2 strWinnerPos;
        private Vector2 strTipPos;

        private float totalElapsed;
        private float frameTime;

        public PanelFinish(Game game, CompoundGameComponent parent)
            : base(game, parent)
        {
            LoadContent();
        }

        protected override void LoadContent()
        {
            strTransparency = 100;
            background = Game.Content.Load<Texture2D>("Panels\\Finish");
            fontBig = Game.Content.Load<SpriteFont>("Fonts\\InfoFontLarge");
            fontSmall = Game.Content.Load<SpriteFont>("Fonts\\InfoFontSmall");
            
            strTip = "Match ended, press (Esc) to return to main menu";
            isBlocked = false;
            curSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            curKeyState = oldKeyState = Keyboard.GetState();
            totalElapsed = 0.0f;
            frameTime = 1 / 60;

            base.LoadContent();
        }

        public void setWinnerName(string name)
        {
            strWinner = name + " won!";
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled == false) //reset everything
            {
                //reset everything
                foreach (GameComponent gc in parent.SubComponents)
                {
                    if (gc is IControlBlocker)
                        continue;
                    gc.Enabled = true;
                }

                //reset camera
                ((IControllable)Game.Services.GetService(typeof(BasicCamera))).Blocked = false;
            }
            else //do your thing!
            {
                //get coordinates for drawing text and sprites
                numBlocksY = (Game.GraphicsDevice.Viewport.Height / background.Height) + 1;
                numBlocksX = (Game.GraphicsDevice.Viewport.Width / background.Width) + 1;

                strWinnerPos.X = (Game.GraphicsDevice.Viewport.Width / 2) - (fontBig.MeasureString(strWinner).X / 2);
                strWinnerPos.Y = (Game.GraphicsDevice.Viewport.Height / 2) - (fontBig.MeasureString(strWinner).Y / 2);

                strTipPos.X = ((Game.GraphicsDevice.Viewport.Width / 2) - fontSmall.MeasureString(strTip).X / 2);
                strTipPos.Y = (Game.GraphicsDevice.Viewport.Height / 2) - (fontBig.MeasureString(strWinner).Y / 2) + (fontBig.MeasureString(strWinner).Y);

                //block everything
                foreach (GameComponent gc in parent.SubComponents)
                {
                    if (gc is PanelFinish)
                        continue;
                    else
                        gc.Enabled = false;
                    if (gc is Chessboard)
                        ((Chessboard)gc).resetHoveredStuff();
                }

                //block camera
                //((IControllable)Game.Services.GetService(typeof(BasicCamera))).Blocked = true;
            }

            base.OnEnabledChanged(sender, args);
        }

        private int transparencyShift = 1;
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            totalElapsed += elapsed;
            if (totalElapsed > frameTime)
            {
                if (strTransparency >= 255)
                    transparencyShift = -5;
                if (strTransparency <= 50)
                    transparencyShift = +5;
                strTransparency += transparencyShift;
                totalElapsed -= frameTime;
            }

            if (isBlocked == false)
            {
                HandleKeyboardInput();
                HandleMouseInput();
            }
            base.Update(gameTime);
        }

        #region IControllable Members

        public void HandleKeyboardInput()
        {
            curKeyState = Keyboard.GetState();
            if (curKeyState.IsKeyDown(Keys.Escape) && oldKeyState.IsKeyUp(Keys.Escape))
            {
                this.Visible = false;
                this.Enabled = false;
            }
            oldKeyState = curKeyState;
        }

        public override void Draw(GameTime gameTime)
        {
            //Draw a screen that is semi - transparent and black
            Game.GraphicsDevice.RenderState.DepthBufferEnable = false;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            curSpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            for (int x = 0; x < numBlocksX; x++)
                for (int y = 0; y < numBlocksY; y++)
                    curSpriteBatch.Draw(background, new Vector2(x * background.Width, y * background.Height), new Color(255, 255, 255, 180));
            curSpriteBatch.DrawString(fontBig, strWinner, strWinnerPos, new Color(0, 0, 0, (byte)strTransparency));
            curSpriteBatch.DrawString(fontSmall, strTip, strTipPos, new Color(0, 0, 0, (byte)strTransparency));
            curSpriteBatch.End();
            Game.GraphicsDevice.RenderState.DepthBufferEnable = true;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            base.Draw(gameTime);
        }

        public void HandleMouseInput()
        {
            //Nothing to handle in a paused menu!
        }

        public bool Blocked
        {
            get
            {
                return isBlocked;
            }
            set
            {
                isBlocked = value;
            }
        }

        #endregion
    }
}
