using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using YATest.Utilities.CameraUtil;
using YATest.Utilities;

namespace YATest.GameEngine
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AbstractGameScene msc;
        AbstractGameScene bsc;
        GameCamera cam;
        TexturesLibrary texturesLibrary;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Services.AddService(typeof(GraphicsDeviceManager), graphics);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //graphics.PreparingDeviceSettings += new System.EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            //graphics.PreferredBackBufferWidth = 1920; /*1920*/
            //graphics.PreferredBackBufferHeight = 1080; /*1080*/
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            //graphics.IsFullScreen = true;
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            //try 1920X1080 first
            foreach (Microsoft.Xna.Framework.Graphics.DisplayMode displayMode
    in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (displayMode.Width == 1920 && displayMode.Height == 1080)
                {
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = displayMode.Height;
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = displayMode.Width;
                    e.GraphicsDeviceInformation.PresentationParameters.FullScreenRefreshRateInHz = displayMode.RefreshRate;
                    e.GraphicsDeviceInformation.PresentationParameters.IsFullScreen = true;
                    return;
                }
            }

            //try 1024X768 second
            foreach (Microsoft.Xna.Framework.Graphics.DisplayMode displayMode
in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (displayMode.Width == 1024 && displayMode.Height == 768)
                {
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = displayMode.Height;
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = displayMode.Width;
                    e.GraphicsDeviceInformation.PresentationParameters.FullScreenRefreshRateInHz = displayMode.RefreshRate;
                    e.GraphicsDeviceInformation.PresentationParameters.IsFullScreen = true;
                    return;
                }
            }
        }

        protected override void LoadContent()
        {

        }
        protected override void Initialize()
        {
            cam = new GameCamera(this, graphics.GraphicsDevice.Viewport);
            Services.AddService(typeof(BasicCamera), cam);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            texturesLibrary = new TexturesLibrary(Content);
            Services.AddService(typeof(TexturesLibrary), texturesLibrary);
            msc = new MainMenuScene(this);
            Components.Add(bsc);
            Components.Add(msc);
            //to control the order of operations
            Components.Add(cam);
            msc.showScene();
            base.Initialize();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);
            base.Draw(gameTime);
        }
    }
}
