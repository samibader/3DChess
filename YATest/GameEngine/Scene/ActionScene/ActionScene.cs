using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using YATest.GameLogic;
using YATest.Utilities;
using System.Data.SqlTypes;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace YATest.GameEngine
{
    class ActionScene : AbstractGameScene, IControllable
    {
        private bool isBlocked;
        private KeyboardState curKeyState, oldKeyState;

        private Chessboard chessboard;
        private PanelTime panelTime;
        private PanelPause panelPause;
        private PanelFinish panelFinish;
        private Panel2D panel2d;
        private PanelExit panelExit;
        private AbstractTimeScheme abstractTimeScheme;
        private ChessboardFactory chessboardFactory;
        private bool highSetting=true;
        private GraphicsDeviceManager graphics;


        public ActionScene(Game game, System.Type type, AbstractTimeScheme abstractTimeScheme, string p1Name, string p2Name)
            : base(game)
        {

            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));
            if (highSetting)
            {
                graphics.PreferMultiSampling = true;
            }
             //   graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
           // graphics.IsFullScreen = true;
           graphics.ApplyChanges();


            Utilities.GameManager.resetReference();
            Utilities.GameManager.getReference(Game).setPlayerOneName(p1Name);
            Utilities.GameManager.getReference(Game).setPlayerTwoName(p2Name);
            if (type == typeof(BasicChessboardFactory))
                chessboardFactory = new BasicChessboardFactory(Game, "GameLogic/gameLayout.xml");

            isBlocked = false;

            this.abstractTimeScheme = abstractTimeScheme;

            //Reset the GameManager

            //Reset the History
            History.resetReference();
            curKeyState = oldKeyState = Keyboard.GetState();
            setChessboard();
            
        }

        //void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        //{
        //    //DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        //    //e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
        //    //int screenHeight = displayMode.Height;
        //    //int screenWidth = displayMode.Width;
        //    //e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = screenWidth;
        //    //e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = screenHeight;

        //    PresentationParameters pp =
        //        e.GraphicsDeviceInformation.PresentationParameters;
        //    int quality = 0;
        //    GraphicsAdapter adapter = e.GraphicsDeviceInformation.Adapter;
        //    SurfaceFormat format = adapter.CurrentDisplayMode.Format;
        //    // Check for 4xAA
        //    if (adapter.CheckDeviceMultiSampleType(DeviceType.Hardware, format,
        //        false, MultiSampleType.FourSamples, out quality))
        //    {
        //        // even if a greater quality is returned, we only want quality 0
        //        pp.MultiSampleQuality = 0;
        //        pp.MultiSampleType =
        //            MultiSampleType.FourSamples;
        //    }
        //    // Check for 2xAA
        //    else if (adapter.CheckDeviceMultiSampleType(DeviceType.Hardware,
        //        format, false, MultiSampleType.TwoSamples, out quality))
        //    {
        //        // even if a greater quality is returned, we only want quality 0
        //        pp.MultiSampleQuality = 0;
        //        pp.MultiSampleType =
        //            MultiSampleType.TwoSamples;
        //    }
        //    return;


        //}



        private void setChessboard()
        {
            //ChessboardFactory chessboardFactory = new BasicChessboardFactory(Game, "GameLogic/gameLayout.xml", 0.06f, 0.7f, 0.7f);
            chessboard = chessboardFactory.getChessboard(this);
            History.VChessboard = chessboard;

            panelTime = new PanelTime(Game, this, abstractTimeScheme);
            if (Game.Services.GetService(typeof(PanelTime)) != null)
                Game.Services.RemoveService(typeof(PanelTime));
            Game.Services.AddService(typeof(PanelTime), panelTime);

           
            PanelHistory.resetReference();
            PanelHistory.createReference(Game, this); //this is sufficient for adding the only PanelHistory object to SubComponents

            panel2d = new Panel2D(Game, this);

            panelPause = new PanelPause(Game, this);
            panelPause.Visible = false;
            panelPause.Enabled = false;

            panelFinish = new PanelFinish(Game, this);
            panelFinish.Visible = false;
            panelFinish.Enabled = false;

            panelExit = new PanelExit(Game, this);
            panelExit.Visible = false;
            panelExit.Enabled = false;
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

        protected override void UnloadContent()
        {
            //if (chessboard != null)
            //{
            //    chessboard = null;
            //    chessboard.Dispose();
            //}

            //if (panel2d != null)
            //{
            //    panel2d = null;
            //    panel2d.Dispose();
            //}

            //if (panelPause != null)
            //{
            //    panelPause = null;
            //    panelPause.Dispose();
            //}

            //if (panelFinish != null)
            //{
            //    panelFinish = null;
            //    panelFinish.Dispose();
            //}

            //if (panelExit != null)
            //{
            //    panelExit = null;
            //    panelExit.Dispose();
            //}

            base.UnloadContent();
        }


        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing == true)
        //    {

        //    }
        //    base.Dispose(disposing);
        //}

        #region IControllable Members

        public void HandleKeyboardInput()
        {
            curKeyState = Keyboard.GetState();


            if (curKeyState.IsKeyDown(Keys.P) && oldKeyState.IsKeyUp(Keys.P))
            {
                if (panelExit.Visible == false)
                {
                    panelPause.Visible = true;
                    panelPause.Enabled = true;
                }
            }

            if (curKeyState.IsKeyDown(Keys.Escape) && oldKeyState.IsKeyUp(Keys.Escape))
            {
                if (panelPause.Visible == true)
                {
                    panelPause.Visible = false;
                    panelPause.Enabled = false;
                }
                panelExit.Visible = !panelExit.Visible;
                panelExit.Enabled = !panelExit.Enabled;
            }

            oldKeyState = curKeyState;
        }

        public void HandleMouseInput()
        {
           
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
