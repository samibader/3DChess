using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.Utilities.MenuElements
{
    class SlidingRect : Rect
    {
        #region Properties
        private int xDestination;
        private int yDestination;
        private int xOrigin;
        private int yOrigin;

        private bool isSlidingInY = false;
        protected bool isSlidingOutY = false;
        private bool slidingIsUp = false;

        public delegate void FinishedSlidingInHandler();
        public event FinishedSlidingInHandler FinishedSlidingIn;
        protected virtual void OnFinishedSlidingIn()
        {
            if (FinishedSlidingIn != null)
                FinishedSlidingIn();
        }

        public delegate void FinishedSlidingOutHandler();
        public event FinishedSlidingOutHandler FinishedSlidingOut;
        protected virtual void OnFinishedSlidingOut()
        {
            if (FinishedSlidingOut != null)
                FinishedSlidingOut();
        }

        private Rectangle slidingRect;
        #endregion

        public SlidingRect(Game game, int xSource, int ySource, int width, int height, int xDestination, int yDestination)
            : base(game, xSource, ySource, width, height)
        {
            this.xDestination = xDestination;
            this.yDestination = yDestination;
            this.xOrigin = xSource;
            this.yOrigin = ySource;
            if (yDestination < ySource)
                slidingIsUp = true;
        }
        public virtual void doSlideInY()
        {
            Visible = true;
            isSlidingInY = true;
            isSlidingOutY = false;
            Blocked = true;
        }

        public void UpdateSlidingRect()
        {
            slidingRect.X = X;
            slidingRect.Y = Y;

            if (Y != yOrigin)
                slidingRect.Height = Math.Abs(Y - yOrigin);
            else
                slidingRect.Height = hotZone.Height;

            if (X != xOrigin)
                slidingRect.Width = Math.Abs(X - xOrigin);
            else
                slidingRect.Width = hotZone.Width;
            if (X == xOrigin && Y == yOrigin) //avoids flicker
            {
                slidingRect.Width = 0;
                slidingRect.Height = 0;
            }
        }

        public void Mute(Color muteColor)
        {
            this.BackgroundColor = muteColor;
            this.SelectedBackgroundColor = muteColor;
            this.HoveredBackgroundColor = muteColor;
            base.Click -= Rect_Click;
            base.Release -= Rect_Release;
            base.MouseIn -= Rect_MouseIn;
            base.MouseOut -= Rect_MouseOut;
        }

        public virtual void doSlideOutY()
        {
            isSlidingOutY = true;
            isSlidingInY = false;
            Blocked = true;
        }
        #region DrawableGameComponent Members
        public override void Update(GameTime gameTime)
        {
            //implmentation now only for Y
            if (isSlidingInY == true) // go to destination
            {
                
                if (slidingIsUp == true)
                {
                    if (Y > yDestination)
                    {
                        hotZone.Y -= 3;
                    }
                    else
                    {
                        isSlidingInY = false;
                        Blocked = false;
                        OnFinishedSlidingIn();
                    }
                }
                else
                {
                    if (Y < yDestination)
                    {
                        hotZone.Y += 3;
                    }
                    else
                    {
                        isSlidingInY = false;
                        Blocked = false;
                        OnFinishedSlidingIn();
                    }
                }
                UpdateSlidingRect();
            }
            if (isSlidingOutY == true)
            {
                if (slidingIsUp == true)
                {
                    if (Y < yOrigin)
                    {
                        hotZone.Y += 3;  
                    }
                    else
                    {
                        isSlidingOutY = false;
                        Blocked = true;
                        Visible = false;
                        OnFinishedSlidingOut();
                    }
                }
                else
                {
                    if (Y > yOrigin)
                    {
                        hotZone.Y -= 3;
                    }
                    else
                    {
                        isSlidingOutY = false;
                        Blocked = true;
                        Visible = false;
                        OnFinishedSlidingOut();
                    }
                }
                UpdateSlidingRect();
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            texture.SetData(new Color[] { curBackgroundColor });
            Game.GraphicsDevice.RenderState.DepthBufferEnable = false;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            curSpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            if (isSlidingInY || isSlidingOutY)
                curSpriteBatch.Draw(texture, slidingRect, curBackgroundColor);
            else
                curSpriteBatch.Draw(texture, hotZone, curBackgroundColor);
            curSpriteBatch.End();
            Game.GraphicsDevice.RenderState.DepthBufferEnable = true;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
        }
        #endregion
    }
}
