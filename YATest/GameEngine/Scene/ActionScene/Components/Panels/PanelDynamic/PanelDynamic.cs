using YATest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YATest.GameEngine
{
    class PanelDynamic : CompoundGameComponent, IControllable //NEEDS WORKING ON PERFORMANCE ISSUES
    {
        private byte transparency;
        private SpriteBatch spriteBatch;
        private MouseState curMouseState, oldMouseState;

        private Texture2D sideBarTexture;
        private Texture2D panelTexture;

        protected bool isOpened;
        private Rectangle sideBarRect;
        private Rectangle panelRect;

        private Rectangle absoluteSideBarRect;
        private Rectangle absolutePanelRect;

        private Texture2D curTexture;

        bool sideHandleVisible;

        protected bool isMovingLeft;
        protected bool isMovingRight;
        private bool isHovered;
        protected bool isEnlarged;
        private Vector2 sideBarAnimPos;
        private Vector2 panelAnimPos;
        private float totalElapsed;
        private float frameTime;

        private int height;
        private string sideHandleFileName;
        private string completePaneFilename;

        public PanelDynamic(Game game, CompoundGameComponent parent, string sideHandleFilename, string completePaneFilename, int height)
            : base(game, parent)
        {
            this.height = height;
            this.sideHandleFileName = sideHandleFilename;
            this.completePaneFilename = completePaneFilename;
            LoadContent();
        }

        protected override void LoadContent()
        {
            transparency = 100;

            //Textures
            sideBarTexture = Game.Content.Load<Texture2D>(sideHandleFileName);
            panelTexture = Game.Content.Load<Texture2D>(completePaneFilename);
            curTexture = sideBarTexture; //default texture


            isOpened = false;
            isMovingLeft = false;
            isHovered = false;
            isEnlarged = false;

            //Rects
            sideBarRect = new Rectangle(0, 0, 20, 100);
            panelRect   = new Rectangle(0, 0, 180, 150);
            absoluteSideBarRect = new Rectangle(Game.GraphicsDevice.Viewport.Width - 20, height, 20, 100);
            absolutePanelRect = new Rectangle(Game.GraphicsDevice.Viewport.Width - 180, height, 180, 150);

            sideHandleVisible = true;

            curTexture = sideBarTexture;
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            //Animation stuff
            sideBarAnimPos = new Vector2(Game.GraphicsDevice.Viewport.Width - 20, height);
            panelAnimPos = new Vector2(Game.GraphicsDevice.Viewport.Width, height);

            totalElapsed = 0;
            frameTime = 1 / 60 /*FPS*/;

            curMouseState = oldMouseState = Mouse.GetState();
            
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            if (isMovingLeft == true)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                totalElapsed += elapsed;
                if (totalElapsed > frameTime)
                {
                    if (sideHandleVisible == true)
                        if (sideBarAnimPos.X < Game.GraphicsDevice.Viewport.Width)
                            sideBarAnimPos.X += 5;
                        else
                            sideHandleVisible = false;
                    if (sideHandleVisible == false)
                        if (panelAnimPos.X > Game.GraphicsDevice.Viewport.Width - 180)
                            panelAnimPos.X -= 5;
                        else
                        {
                            isMovingLeft = false;
                            isEnlarged = true;
                        }
                    totalElapsed -= frameTime;
                }
            }

            if (isMovingRight == true)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                totalElapsed += elapsed;
                if (totalElapsed > frameTime)
                {
                    if (sideHandleVisible == false)
                        if (panelAnimPos.X < Game.GraphicsDevice.Viewport.Width)
                            panelAnimPos.X += 5;
                        else
                            sideHandleVisible = true;

                    if (sideHandleVisible == true)
                        if (sideBarAnimPos.X > Game.GraphicsDevice.Viewport.Width - 20)
                            sideBarAnimPos.X -= 5;
                        else
                        {
                            isMovingRight = false;
                            isEnlarged = false;
                            isOpened = false;
                        }

                    totalElapsed -= frameTime;
                }
            }

            if (Blocked == false)
            {
                HandleKeyboardInput();
                HandleMouseInput();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.RenderState.DepthBufferEnable = false;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = true;

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            if (sideHandleVisible == true)
            {
                curTexture = sideBarTexture;
                spriteBatch.Draw(curTexture, sideBarAnimPos, sideBarRect, new Color(255, 255, 255, transparency), 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
            }
            else
            {
                curTexture = panelTexture;
                spriteBatch.Draw(curTexture, panelAnimPos, panelRect, new Color(255, 255, 255, transparency), 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
            }
            spriteBatch.End();

            Game.GraphicsDevice.RenderState.DepthBufferEnable = true;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            base.Draw(gameTime);
        }

        #region IControllable Members

        private bool controlsAreBlocked;

        public void HandleKeyboardInput() { }

        public void HandleMouseInput()
        {
            curMouseState = Mouse.GetState();

            if (isOpened == false && isEnlarged == false) //default display - Hovering
            {
                if (absoluteSideBarRect.Contains(curMouseState.X, curMouseState.Y) == true) //small menu (when hovered enlarges)
                {
                    transparency = 255;
                    isMovingLeft = true;
                }
            }

            if (isOpened == true) //default display
            {
                if (absolutePanelRect.Contains(curMouseState.X, curMouseState.Y) == true)
                {
                    if (curMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) //user clicked
                    {
                        transparency = 100;
                        isMovingRight = true;
                    }
                }
            }

            if (isOpened == false && isEnlarged == true) // Clicking Enlarged menu to make it fixed
            {
                if (absolutePanelRect.Contains(curMouseState.X, curMouseState.Y) == true)
                {
                    transparency = 255;
                    if (curMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) //user clicked
                    {
                        isOpened = true;
                    }
                }
                else
                {
                    transparency = 100;
                    isMovingRight = true;
                }
            }
            oldMouseState = curMouseState;
        }

        public bool Blocked
        {
            get
            {
                return controlsAreBlocked;
            }
            set
            {
                controlsAreBlocked = value;
            }
        }

        #endregion
    }
}
