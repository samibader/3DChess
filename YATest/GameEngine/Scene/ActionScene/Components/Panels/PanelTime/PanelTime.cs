using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using YATest.Utilities;

namespace YATest.GameEngine
{
    /// <summary>
    /// Simply a panel that shows time of players
    /// </summary>
    class PanelTime : PanelDynamic
    {
        private AbstractTimeScheme time;

        private SpriteFont smallFont;
        private SpriteFont largeFont;
        private SpriteBatch curSpriteBatch;
        string curPlayer;
        string secPlayer;

        private int width;

        private float totalElapsed;
        private float frameTime;
        private bool startBlinking;
        private bool blink;
        private bool wasOpened;
        private bool wasCritical;

        public PanelTime(Game game, CompoundGameComponent parent, AbstractTimeScheme timeScheme) : base(game, parent, "Panels\\SideHandle", "Panels\\CompletePane", 10)
        {
            this.time = timeScheme;
            time.Start();
            LoadContent();
        }

        protected override void LoadContent()
        {
            smallFont = Game.Content.Load<SpriteFont>("Fonts/InfoFont");
            largeFont = Game.Content.Load<SpriteFont>("Fonts/InfoFontLarge");
            curSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            totalElapsed = 0;
            frameTime = 1;
            blink = false;
            startBlinking = false;
            wasOpened = false;
            wasCritical = false;
            base.LoadContent();
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled == false)
                time.Pause();
            else
                time.Continue();
        }

        public override void Update(GameTime gameTime)
        {
            
            curPlayer = "  " + Utilities.GameManager.getReference(Game).getCurPlayerName() + "\n   ";
            secPlayer = Utilities.GameManager.getReference(Game).getSecPlayerName() + " ";

            width = Game.GraphicsDevice.Viewport.Width;

            if (Utilities.GameManager.getReference(Game).isPlayer1Turn())
            {
                curPlayer += time.TimeStringPlayer1;
                secPlayer += time.TimeStringPlayer2;
            }
            else
            {
                curPlayer += time.TimeStringPlayer2;
                secPlayer += time.TimeStringPlayer1;
            }


            //if time less than 10 seconds, appear and notify by blinking
            if (time.curTimeCritical())
            {
                base.Blocked = true;
                startBlinking = true;
                wasCritical = true;
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                totalElapsed += elapsed;
                if (isEnlarged == false)
                {
                    wasOpened = false;
                    isMovingLeft = true;
                }
                else
                    wasOpened = true;
                if (totalElapsed > frameTime)
                {
                    //blinking code here
                    blink = !blink;
                    totalElapsed -= frameTime;
                }
            }
            else
            {
                if (wasCritical == true) //restore previous state
                {
                    base.Blocked = false; //restore controls (makes the panel restores its state
                    totalElapsed = 0; //calibrate blinking again
                    if (wasOpened == false) //if it was NOT opened, hide it
                        isOpened = false;
                    wasCritical = false; //avoid waterfall effect
                    startBlinking = false; //calibrate blinking
                }
            }

            curPlayer += "\n";
            secPlayer += "\n";

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            Game.GraphicsDevice.RenderState.DepthBufferEnable = false;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = true;

            curSpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            if ((isOpened == true || isEnlarged == true) && isMovingRight == false)
            {
                curSpriteBatch.DrawString(smallFont, "current turn:", new Vector2(width - 140, 30), Color.White);
                if(startBlinking == true)
                    if(blink == true)
                        curSpriteBatch.DrawString(largeFont, curPlayer, new Vector2(width - 140, 60), Color.Red);
                    else
                        curSpriteBatch.DrawString(largeFont, curPlayer, new Vector2(width - 140, 60), Color.White);
                else
                    curSpriteBatch.DrawString(largeFont, curPlayer, new Vector2(width - 140, 60), Color.White);
                curSpriteBatch.DrawString(smallFont, secPlayer, new Vector2(width - 140, 120), Color.White);
            }
            curSpriteBatch.End();

            Game.GraphicsDevice.RenderState.DepthBufferEnable = true;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            
        }

        public void newTurn(bool isPlayer1Turn, bool isHistory)
        {
            time.SwitchTurn(isPlayer1Turn, isHistory);
        }
    }
}
