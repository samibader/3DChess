using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YATest.Utilities.MenuElements
{
    class HotZone : DrawableGameComponent, IControllable
    {
        protected Rectangle hotZone;
        private MouseState curMouseState, oldMouseState;
        private bool isBlocked;
        public int X
        {
            get { return hotZone.X; }
        }

        public int Y
        {
            get { return hotZone.Y; }
            set { hotZone.Y = value; }
        }
        public int Width
        {
            get { return hotZone.Width; }
        }
        public int Height
        {
            set { hotZone.Height = value; }
            get { return hotZone.Height; }
        }
        public HotZone(Game game, int x, int y, int width, int height)
            : base(game)
        {
            hotZone = new Rectangle(x, y, width, height);
        }

        public HotZone(Game game, Rectangle rect)
            : base(game)
        {
            hotZone = rect;
        }

        #region Delegates and Events
        public delegate void ClickHandler();
        public delegate void MouseInHandler();
        public delegate void MouseOutHandler();
        public delegate void ReleaseHandler();

        public event ClickHandler Click;
        public event MouseInHandler MouseIn;
        public event MouseOutHandler MouseOut;
        public event ReleaseHandler Release;

        protected virtual void OnClick()
        {
            if (Click != null)
                Click();
        }

        protected virtual void OnMouseIn()
        {
            if (MouseIn != null)
                MouseIn();
        }

        protected virtual void OnMouseOut()
        {
            if (MouseOut != null)
                MouseOut();
        }

        protected virtual void OnRelease()
        {
            if (Release != null)
                Release();
        }
        #endregion

        #region DrawableGameComponent Members
        public override void Update(GameTime gameTime)
        {
            if (Blocked == false)
            {
                HandleKeyboardInput();
                HandleMouseInput();
            }
            base.Update(gameTime);
        }
        #endregion

        #region IControllable Members

        public virtual void HandleKeyboardInput()
        {
            //Nothing
        }

        protected bool clicked = false;
        //Handle mouse hovering and clicking
        public virtual void HandleMouseInput()
        {
            curMouseState = Mouse.GetState();

            if (hotZone.Contains(curMouseState.X, curMouseState.Y) == true)
            {
                OnMouseIn();

                if (curMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                {
                    OnClick();
                    clicked = true;
                }
                if (curMouseState.LeftButton == ButtonState.Released && clicked == true)
                {
                    OnRelease();
                    clicked = false;
                }
            }
            else
                OnMouseOut();

            oldMouseState = curMouseState;
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
