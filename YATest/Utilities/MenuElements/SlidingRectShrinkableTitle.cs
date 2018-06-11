using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace YATest.Utilities.MenuElements
{
    class SlidingRectShrinkableTitle : SlidingRect
    {
        private int titleSize;
        private Label titleRef;
        private bool isMinimizing = false;
        private int originalHeight;

        public delegate void FinishedMinimizingHandler();
        public event FinishedMinimizingHandler FinishedMinimizing;
        protected virtual void OnFinishedMinimizing()
        {
            if (FinishedMinimizing != null)
                FinishedMinimizing();
        }

        public delegate void FinishedMaximizingHandler();
        public event FinishedMaximizingHandler FinishedMaximizing;
        protected virtual void OnFinishedMaximizing()
        {
            if (FinishedMaximizing != null)
                FinishedMaximizing();
        }

        public Label TitleRef
        {
            get { return titleRef; }
            set { titleRef = value; }
        }

        public int TitleSize
        {
            get { return titleSize; }
            set { titleSize = value; }
        }

        public SlidingRectShrinkableTitle(Game game, int xSource, int ySource, int width, int height, int xDestination, int yDestination, Label titleRef, int titleSize)
            : base(game, xSource, ySource, width, height, xDestination, yDestination)
        {
            this.titleSize = titleSize;
            this.titleRef = titleRef;
            this.originalHeight = height;
        }

        private bool stateChanged = false;
        public void minimizeTitle()
        {
            titleRef.Visible = false;
            isMinimizing = true;
            stateChanged = true;
        }

        public void maximizeTitle()
        {
            isMinimizing = false;
            stateChanged = true;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (isMinimizing == true)
            {
                if (this.Height + titleSize > originalHeight)
                {
                    this.Height -= 2;
                    this.Y += 2;
                }
                else
                    if (stateChanged == true)
                    {
                        stateChanged = false;
                        OnFinishedMinimizing();
                    }
            }
            if (isMinimizing == false)
            {
                if (this.Height < originalHeight)
                {
                    this.Height += 2;
                    this.Y -= 2;
                }
                else
                    if (stateChanged == true)
                    {
                        if (Blocked == false && isSlidingOutY == false)
                        {
                            titleRef.Visible = true;
                            OnFinishedMaximizing();
                        }
                        stateChanged = false;
                        
                    }
            }
            base.Update(gameTime);
        }
    }
}
