using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using YATest.Utilities;
using Microsoft.Xna.Framework.Input;
using YATest.Utilities.CameraUtil;
using YATest.Utilities.MenuElements;

namespace YATest.GameEngine
{
    /// <summary>
    /// Simply a panel that shows time of players
    /// </summary>
    class PanelExit : CompoundGameComponent, IControllable, IControlBlocker
    {
        private Texture2D background;
        
        private SpriteBatch curSpriteBatch;

        private bool isBlocked;
        private KeyboardState curKeyState, oldKeyState;
        private int numBlocksY;
        private int numBlocksX;

        private int menuWidth, menuHeight, menuX, menuY;
        private int buttonWidth, buttonX;

        public PanelExit(Game game, CompoundGameComponent parent)
            : base(game, parent)
        {
            LoadContent();
        }

        /*
         * My New Items (Using menus I designed
         */


        private void loadButtons()
        {
            float marginH = (Game.GraphicsDevice.Viewport.Width / 100.0f) * 12;
            float marginV = (Game.GraphicsDevice.Viewport.Height / 100.0f) * 20;
            btnMainMenu = new Rect(Game,
                Convert.ToInt32(marginH),
                Game.GraphicsDevice.Viewport.Height - Convert.ToInt32(marginV) - Convert.ToInt32(marginV / 4),
                (Game.GraphicsDevice.Viewport.Width - (2 * Convert.ToInt32(marginH))) * 25 / 100,
                Convert.ToInt32(marginV / 4));
            btnResume = new Rect(Game, btnMainMenu.X + btnMainMenu.Width, btnMainMenu.Y, btnMainMenu.Width * 2, btnMainMenu.Height);
            btnExitAll = new Rect(Game, btnResume.X + btnResume.Width, btnResume.Y, btnMainMenu.Width, btnMainMenu.Height);

            btnMainMenu.SelectedBackgroundColor = btnMainMenu.HoveredBackgroundColor;
            btnResume.SelectedBackgroundColor = btnResume.HoveredBackgroundColor;
            btnExitAll.SelectedBackgroundColor = btnExitAll.HoveredBackgroundColor;

            SubComponents.Add(btnMainMenu);
            SubComponents.Add(btnResume);
            SubComponents.Add(btnExitAll);
        }

        private void loadLabels()
        {
            Vector2 labelSize = fontBold.MeasureString("Main Menu");
            lblMainMenu = new Label(Game,
                Convert.ToInt32(btnMainMenu.X + (btnMainMenu.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(btnMainMenu.Y + (btnMainMenu.Height / 2) - (labelSize.Y / 2)),
                fontBold,
                "Main Menu");
            labelSize = fontBold.MeasureString("Resume");
            lblResume = new Label(Game,
                Convert.ToInt32(btnResume.X + (btnResume.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(btnResume.Y + (btnResume.Height / 2) - (labelSize.Y / 2)),
                fontBold,
                "Resume");
            labelSize = fontBold.MeasureString("Exit");
            lblExitAll = new Label(Game,
                Convert.ToInt32(btnExitAll.X + (btnExitAll.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(btnExitAll.Y + (btnExitAll.Height / 2) - (labelSize.Y / 2)),
                fontBold,
                "Exit");
            SubComponents.Add(lblMainMenu);
            SubComponents.Add(lblResume);
            SubComponents.Add(lblExitAll);
        }

        private void loadHandlers()
        {
            btnMainMenu.Click += mainMenu_onClick;
            btnExitAll.Click += exitAll_onClick;
            btnResume.Click += resume_OnClick;
        }

        Rect btnResume;
        Rect btnMainMenu;
        Rect btnExitAll;
        Label lblResume, lblMainMenu, lblExitAll;
        SpriteFont fontBold;
        protected override void LoadContent()
        {
            background = Game.Content.Load<Texture2D>("Panels\\Finish");
            isBlocked = false;
            curSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            fontBold = Game.Content.Load<SpriteFont>("Fonts\\InfoFontLarge");
            fontBold.Spacing = 4;

            loadButtons();
            loadLabels();
            loadHandlers();

            curKeyState = oldKeyState = Keyboard.GetState();
            base.LoadContent();
        }

        void mainMenu_onClick()
        {
            ((ActionScene)parent).hideScene();
            for (int i = 0; i < Game.Components.Count; i++)
                if (Game.Components[i] is ActionScene)
                {
                    Game.Services.RemoveService(typeof(ActionScene));
                    ActionScene gc = (ActionScene)Game.Components[i];
                    Game.Components.RemoveAt(i);
                    gc.Dispose();
                    gc = null;
                    break;
                }
            for (int i = 0; i < Game.Components.Count; i++)
                if (Game.Components[i] is MainMenuScene)
                    ((MainMenuScene)Game.Components[i]).showScene();        
        }

        void exitAll_onClick()
        {
            Game.Exit();
        }

        void resume_OnClick()
        {
            this.Enabled = false;
            this.Visible = false;
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
                //block everything
                foreach (GameComponent gc in parent.SubComponents)
                {
                    if (gc is PanelExit)
                        continue;
                    else
                        gc.Enabled = false;
                    if (gc is Chessboard)
                        ((Chessboard)gc).resetHoveredStuff();
                }

                //block camera
                ((IControllable)Game.Services.GetService(typeof(BasicCamera))).Blocked = true;
            }

            base.OnEnabledChanged(sender, args);
        }

        public override void Update(GameTime gameTime)
        {
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
            if (curKeyState.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter))
                resume_OnClick();
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
                    curSpriteBatch.Draw(background, new Vector2(x * background.Width, y * background.Height), new Color(255, 255, 255, 150));
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
