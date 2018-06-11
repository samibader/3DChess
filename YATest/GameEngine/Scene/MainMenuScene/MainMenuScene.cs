using Microsoft.Xna.Framework;
using YATest.Utilities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using YATest.Utilities.MenuElements;
namespace YATest.GameEngine
{
    class MainMenuScene : AbstractGameScene, IControllable
    {
        public delegate void CancelDelegate();

        public CancelDelegate level0CancelEvent;
        public CancelDelegate level1CancelEvent;
        public CancelDelegate level2CancelEvent;
        public CancelDelegate level3CancelEvent;

        #region Menu Members
        private Texture2D background;
        private SpriteBatch curSpriteBatch;
        private bool isBlocked;
        private KeyboardState curKeyState, oldKeyState;
        Rect rLeft, rMiddle, rRight;
        Label newGameLabel, settingsGameLabel, exitGameLabel;
        SlidingRect rsLeft1, rsLeft2;
        YesNoPanel exitPanel;
        SettingsMenuRoot settingsPanelRoot;
        SpriteFont fontBold, fontRegular;
        PlayerNamePanel p1Panel, p2Panel;
        bool p1PanelCalled, p2PanelCalled, timersPanelCalled, detailsPanelCalled;
        TimersPanel timersPanel;
        TimerPanelOneSubPanel timerCountDownPanel, timerHourGlassPanel;
        TimerPanelTwoSubPanels timerFischerPanel;
        TimerPanelNothing timerNoTimer;
        ToggleMenuPanel detailsPanel;
        #endregion
        #region Game Members
        //TimeScheme that is created using our configurations
        private AbstractTimeScheme timeScheme = new FischerTimeScheme(180, 5);
        public AbstractTimeScheme TimeScheme
        {
            get { return timeScheme; }
            set { timeScheme = value; }
        }

        //Player1 Name
        private string player1Name = "Player1";
        public string Player1Name
        {
            get { return player1Name; }
            set { player1Name = value; }
        }

        //Player2 Name
        private string player2Name = "Player2";
        public string Player2Name
        {
            get { return player2Name; }
            set { player2Name = value; }
        }
        #endregion
        public MainMenuScene(Game game)
            : base(game)
        {
        }
        #region DrawableGameComponent Overriden
        protected override void LoadContent()
        {
            background = Game.Content.Load<Texture2D>("MainMenu\\MainMenuPic");
            curSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            fontBold = Game.Content.Load<SpriteFont>("Fonts\\InfoFontLarge");
            fontBold.Spacing = 7;
            fontRegular = Game.Content.Load<SpriteFont>("Fonts\\InfoFont");
            fontRegular.Spacing = 6;
            isBlocked = false;
            float marginH = (Game.GraphicsDevice.Viewport.Width / 100.0f) * 12;
            float marginV = (Game.GraphicsDevice.Viewport.Height / 100.0f) * 20;
            rLeft = new Rect(Game,
                Convert.ToInt32(marginH),
                Game.GraphicsDevice.Viewport.Height - Convert.ToInt32(marginV) - Convert.ToInt32(marginV / 4),
                (Game.GraphicsDevice.Viewport.Width - (2 * Convert.ToInt32(marginH))) * 25 / 100,
                Convert.ToInt32(marginV / 4));
            rMiddle = new Rect(Game, rLeft.X + rLeft.Width, rLeft.Y, rLeft.Width * 2, rLeft.Height);
            rRight = new Rect(Game, rMiddle.X + rMiddle.Width, rMiddle.Y, rLeft.Width, rLeft.Height);
            SubComponents.Add(rLeft);
            SubComponents.Add(rMiddle);
            SubComponents.Add(rRight);
            rLeft.Click += new HotZone.ClickHandler(rLeft_Click);
            rsLeft1 = new SlidingRect(Game, rLeft.X, rLeft.Y, (Game.GraphicsDevice.Viewport.Width - (2 * Convert.ToInt32(marginH))) * 33 / 100,
                rLeft.Height, rLeft.X, rLeft.Y - rLeft.Height);
            rsLeft2 = new SlidingRect(Game, rsLeft1.X + rsLeft1.Width, rsLeft1.Y, (Game.GraphicsDevice.Viewport.Width - (2 * Convert.ToInt32(marginH))) * 33 / 100,
            rLeft.Height, rLeft.X, rLeft.Y - rLeft.Height);
            rsLeft1.Visible = false;
            rsLeft1.Blocked = true;
            rsLeft2.Visible = false;
            rsLeft2.Blocked = true;
            SubComponents.Add(rsLeft1);
            SubComponents.Add(rsLeft2);

            Vector2 labelSize = fontBold.MeasureString("New Game");
            newGameLabel = new Label(Game,
                Convert.ToInt32(rMiddle.X + (rMiddle.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(rMiddle.Y + (rMiddle.Height / 2) - (labelSize.Y / 2)),
                fontBold,
                "New Game");
            labelSize = fontRegular.MeasureString("Settings");
            settingsGameLabel = new Label(Game,
                Convert.ToInt32(rLeft.X + (rLeft.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(rLeft.Y + (rLeft.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Settings");
            labelSize = fontRegular.MeasureString("Exit");
            exitGameLabel = new Label(Game,
                Convert.ToInt32(rRight.X + (rRight.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(rRight.Y + (rRight.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Exit");
            SubComponents.Add(newGameLabel);
            SubComponents.Add(settingsGameLabel);
            SubComponents.Add(exitGameLabel);

            exitPanel = new YesNoPanel(
                Game,
                this,
                rLeft.X,
                rLeft.Y,
                (Game.GraphicsDevice.Viewport.Width - (2 * Convert.ToInt32(marginH))),
                rLeft.Height * 2,
                rLeft.X, rLeft.Y - rLeft.Height * 2,
                "Are you sure you want to Exit?");
            exitPanel.Visible = false;
            exitPanel.yesPanel.Click += new HotZone.ClickHandler(yesPanel_Click);
            exitPanel.noPanel.Click += new HotZone.ClickHandler(noPanel_Click);
            SubComponents.Add(exitPanel);
            rRight.Click += new HotZone.ClickHandler(rRight_Click);
            rLeft.Click += new HotZone.ClickHandler(rLeft_Click);
            settingsPanelRoot = new SettingsMenuRoot(
                Game, this,
                rLeft.X, rLeft.Y,
                (Game.GraphicsDevice.Viewport.Width - (2 * Convert.ToInt32(marginH))),
                rLeft.Height * 3,
                rLeft.X, rLeft.Y - rLeft.Height * 3,
                "Settings");
            settingsPanelRoot.Visible = false;
            settingsPanelRoot.savePanel.Click += new HotZone.ClickHandler(savePanel_Click);
            settingsPanelRoot.cancelPanel.Click += new HotZone.ClickHandler(cancelPanel_Click);
            settingsPanelRoot.p1Panel.Click += new HotZone.ClickHandler(p1Panel_Click);
            settingsPanelRoot.p2Panel.Click += new HotZone.ClickHandler(p2Panel_Click);
            settingsPanelRoot.FinishedMinimizing += new SlidingRectShrinkableTitle.FinishedMinimizingHandler(settingsPanelRoot_FinishedMinimizing);
            settingsPanelRoot.FinishedMaximizing += new SlidingRectShrinkableTitle.FinishedMaximizingHandler(settingsPanelRoot_FinishedMaximizing);
            SubComponents.Add(settingsPanelRoot);

            timersPanel = new TimersPanel(
                Game,
                this,
                settingsPanelRoot.X,
                settingsPanelRoot.Y + (settingsPanelRoot.Height / 3) - settingsPanelRoot.Height,
                settingsPanelRoot.Width,
                settingsPanelRoot.Height,
                settingsPanelRoot.X,
                settingsPanelRoot.Y + (settingsPanelRoot.Height / 3) - (2 * settingsPanelRoot.Height),
                "Timing Schemes");
            timersPanel.Visible = false;
            SubComponents.Add(timersPanel);
            timersPanel.FinishedMinimizing += new SlidingRectShrinkableTitle.FinishedMinimizingHandler(timersPanel_FinishedMinimizing);
            timersPanel.FinishedMaximizing += new SlidingRectShrinkableTitle.FinishedMaximizingHandler(timersPanel_FinishedMaximizing);

            detailsPanel = new ToggleMenuPanel(
                Game,
                this,
                settingsPanelRoot.X,
                settingsPanelRoot.Y + (settingsPanelRoot.Height / 3) - settingsPanelRoot.Height,
                settingsPanelRoot.Width,
                settingsPanelRoot.Height,
                settingsPanelRoot.X,
                settingsPanelRoot.Y + (settingsPanelRoot.Height / 3) - (2 * settingsPanelRoot.Height),
                "Visual Details",
                "Low Details",
                "Heigh Details");
            detailsPanel.Visible = false;
            SubComponents.Add(detailsPanel);
            detailsPanel.FinishedSlidingOut += new SlidingRect.FinishedSlidingOutHandler(detailsPanel_FinishedSlidingOut);
            settingsPanelRoot.detailsPanel.Click += new HotZone.ClickHandler(detailsPanel_Click);
            detailsPanel.cancelPanel.Click += new HotZone.ClickHandler(cancelDetailsPanel_Click);
            detailsPanel.savePanel.Click += new HotZone.ClickHandler(saveDetailsPanel_Click);

            settingsPanelRoot.timersPanel.Click += new HotZone.ClickHandler(timersPanel_Click);
            p1Panel = new PlayerNamePanel(Game,
                this,
                settingsPanelRoot.X,
                settingsPanelRoot.Y + (settingsPanelRoot.Height / 3) - settingsPanelRoot.Height,
                settingsPanelRoot.Width,
                settingsPanelRoot.Height,
                settingsPanelRoot.X,
                settingsPanelRoot.Y + (settingsPanelRoot.Height / 3) - (2 * settingsPanelRoot.Height),
                "Player 1 Name", "Player1");
            p1Panel.Visible = false;
            p1Panel.Blocked = true;
            p1Panel.savePanel.Click += new HotZone.ClickHandler(p1savePanel_Click);
            p1Panel.cancelPanel.Click += new HotZone.ClickHandler(p1cancelPanel_Click);
            p1Panel.FinishedSlidingOut += new SlidingRect.FinishedSlidingOutHandler(p1Panel_FinishedSlidingOut);
            SubComponents.Add(p1Panel);

            p2Panel = new PlayerNamePanel(Game,
                this,
                settingsPanelRoot.X,
                settingsPanelRoot.Y + (settingsPanelRoot.Height / 3) - settingsPanelRoot.Height,
                settingsPanelRoot.Width,
                settingsPanelRoot.Height,
                settingsPanelRoot.X,
                settingsPanelRoot.Y + (settingsPanelRoot.Height / 3) - (2 * settingsPanelRoot.Height),
                "Player 2 Name", "Player2");
            p2Panel.Visible = false;
            p2Panel.Blocked = true;
            p2Panel.savePanel.Click += new HotZone.ClickHandler(p2savePanel_Click);
            p2Panel.cancelPanel.Click += new HotZone.ClickHandler(p2cancelPanel_Click);
            p2Panel.FinishedSlidingOut += new SlidingRect.FinishedSlidingOutHandler(p2Panel_FinishedSlidingOut);
            SubComponents.Add(p2Panel);
            #region timers panels
            timerCountDownPanel = new TimerPanelOneSubPanel(
                Game,
                this,
                timersPanel.X,
                timersPanel.Y - timersPanel.Height + rLeft.Height,
                timersPanel.Width,
                rLeft.Height * 4,
                timersPanel.X,
                timersPanel.Y - timersPanel.Height - (4 * rLeft.Height) + rLeft.Height,
                "Count Down Scheme",
                "Each side has certian amount of time that is counted down",
                "Total time for each player",
                "Min.",
                180
                );
            timerCountDownPanel.Visible = false;
            timersPanel.timer2Panel.Click += new HotZone.ClickHandler(timer2Panel_Click);
            timerCountDownPanel.cancelPanel.Click += new HotZone.ClickHandler(cancelCountDownPanel_Click);
            timerCountDownPanel.savePanel.Click += new HotZone.ClickHandler(saveCountDownPanel_Click);
            timerCountDownPanel.FinishedSlidingOut += new SlidingRect.FinishedSlidingOutHandler(timerCountDownPanel_FinishedSlidingOut);
            SubComponents.Add(timerCountDownPanel);

            timerHourGlassPanel = new TimerPanelOneSubPanel(
                Game,
                this,
                timersPanel.X,
                timersPanel.Y - timersPanel.Height + rLeft.Height,
                timersPanel.Width,
                rLeft.Height * 4,
                timersPanel.X,
                timersPanel.Y - timersPanel.Height - (4 * rLeft.Height) + rLeft.Height,
                "Hourglass Scheme",
                "When one player is losing time the other side gains time",
                "Total time for each player",
                "Min.",
                180
                );
            timerHourGlassPanel.Visible = false;
            timersPanel.timer3Panel.Click += new HotZone.ClickHandler(timer3Panel_Click);
            timerHourGlassPanel.cancelPanel.Click += new HotZone.ClickHandler(cancelHourglassPanel_Click);
            timerHourGlassPanel.savePanel.Click += new HotZone.ClickHandler(saveHourglassPanel_Click);
            timerHourGlassPanel.FinishedSlidingOut += new SlidingRect.FinishedSlidingOutHandler(timerHourglassPanel_FinishedSlidingOut);
            SubComponents.Add(timerHourGlassPanel);

            timerFischerPanel = new TimerPanelTwoSubPanels(
                Game,
                this,
                timersPanel.X,
                timersPanel.Y - timersPanel.Height + rLeft.Height,
                timersPanel.Width,
                rLeft.Height * 4,
                timersPanel.X,
                timersPanel.Y - timersPanel.Height - (4 * rLeft.Height) + rLeft.Height,
                "Fischer Scheme",
                "Each side gets extra seconds bonus after every move",
                "Total time for each player",
                "Min.",
                "Amount of added time",
                "Sec.",
                180,
                30
                );
            timerFischerPanel.Visible = false;
            timersPanel.timer1Panel.Click += new HotZone.ClickHandler(timer1Panel_Click);
            timerFischerPanel.cancelPanel.Click += new HotZone.ClickHandler(cancelFischerPanel_Click);
            timerFischerPanel.savePanel.Click += new HotZone.ClickHandler(saveFischerPanel_Click);
            timerFischerPanel.FinishedSlidingOut += new SlidingRect.FinishedSlidingOutHandler(timerFischerPanel_FinishedSlidingOut);
            SubComponents.Add(timerFischerPanel);

            timerNoTimer = new TimerPanelNothing(
                Game,
                this,
                timersPanel.X,
                timersPanel.Y - timersPanel.Height + rLeft.Height,
                timersPanel.Width,
                rLeft.Height * 2,
                timersPanel.X,
                timersPanel.Y - timersPanel.Height - (2 * rLeft.Height) + rLeft.Height,
                "No time during play?"
                );
            timerNoTimer.Visible = false;
            timersPanel.timer4Panel.Click += new HotZone.ClickHandler(timer4Panel_Click);
            timerNoTimer.noPanel.Click += new HotZone.ClickHandler(cancelNoTimer_Click);
            timerNoTimer.yesPanel.Click += new HotZone.ClickHandler(saveNoTimer_Click);
            timerNoTimer.FinishedSlidingOut += new SlidingRect.FinishedSlidingOutHandler(timerNoTimerPanel_FinishedSlidingOut);
            SubComponents.Add(timerNoTimer);

            timersPanel.cancelPanel.Click += new HotZone.ClickHandler(cancelTimersPanel_Click);
            timersPanel.savePanel.Click += new HotZone.ClickHandler(saveTimersPanel_Click);
            timersPanel.FinishedSlidingOut += new SlidingRect.FinishedSlidingOutHandler(timersPanel_FinishedSlidingOut);
            #endregion

            rMiddle.Click += new HotZone.ClickHandler(rMiddle_Click);

            curKeyState = oldKeyState = Keyboard.GetState();
            base.LoadContent();
        }
        
        void timersPanel_FinishedMaximizing()
        {
            timersPanelControls(false, true);
        }
        public override void Draw(GameTime gameTime)
        {
            //Draw a screen that is semi - transparent and black
            Game.GraphicsDevice.RenderState.DepthBufferEnable = false;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            curSpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            //Draw Background!
            curSpriteBatch.Draw(background, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), new Color(255, 255, 255, 255));

            curSpriteBatch.End();
            Game.GraphicsDevice.RenderState.DepthBufferEnable = true;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            base.Draw(gameTime);
        }
        #endregion
        #region Details Panel
        void detailsPanel_Click()
        {
            settingsPanelRoot.minimizeTitle();
            p1PanelCalled = false;
            p2PanelCalled = false;
            timersPanelCalled = false;
            detailsPanelCalled = true;
            settingsPanelRoot.BlockControls();
            level1CancelEvent += cancelDetailsPanel_Click;
        }
        void cancelDetailsPanel_Click()
        {
            detailsPanel.doSlideOutY();
            level1CancelEvent -= cancelDetailsPanel_Click;
        }
        void saveDetailsPanel_Click()
        {
            p1PanelCalled = false;
            p2PanelCalled = false;
            timersPanelCalled = false;
            detailsPanelCalled = true;
            detailsPanel.doSlideOutY();
        }
        void detailsPanel_FinishedSlidingOut()
        {
            settingsPanelRoot.maximizeTitle();
        }
        #endregion
        #region Timers Panels
        bool[] timerCalled = { false, false, false, false };

        void makeTimerCalledTrue(int whatTimer)
        {
            for (int i = 0; i < 4; i++)
                timerCalled[i] = false;
            timerCalled[whatTimer] = true;
        }

        void timer1Panel_Click()
        {
            timerFischerPanel.numberBox.Text = timerFischerPanel.numberBox.OldText;
            timerFischerPanel.numberBox2.Text = timerFischerPanel.numberBox2.OldText;
            makeTimerCalledTrue(0);
            timersPanelControls(true, true);
            timersPanel.savePanel.Blocked = true;
            timersPanel.minimizeTitle();
            level3CancelEvent += cancelFischerPanel_Click;
        }

        void saveFischerPanel_Click()
        {
            timerFischerPanel.numberBox.OldText = timerFischerPanel.numberBox.Text;
            timerFischerPanel.numberBox2.OldText = timerFischerPanel.numberBox2.Text;
            timersPanelCalled = false;
            timerFischerPanel.doSlideOutY();
            if (timeScheme != null)
                timeScheme = null;
            timersPanel.setSubSavedTimeScheme(timeScheme = new FischerTimeScheme(Convert.ToInt32(timerFischerPanel.numberBox.OldText) * 60,
                Convert.ToInt32(timerFischerPanel.numberBox.Text)));
            //timeScheme = new FischerTimeScheme(Convert.ToInt32(timerFischerPanel.numberBox.OldText) * 60,
            //    Convert.ToInt32(timerFischerPanel.numberBox.Text));
            //Console.WriteLine("Hello, Fischer: " + 
            //   Convert.ToInt32(timerFischerPanel.numberBox.OldText) * 60 +
            //   Convert.ToInt32(timerFischerPanel.numberBox.Text));
        }

        void cancelFischerPanel_Click()
        {
            timersPanelCalled = false;
            timerFischerPanel.doSlideOutY();
            level3CancelEvent -= cancelFischerPanel_Click;
        }

        void timerFischerPanel_FinishedSlidingOut()
        {
            timersPanel.maximizeTitle();
        }

        void timer2Panel_Click()
        {
            timerCountDownPanel.numberBox.Text = timerCountDownPanel.numberBox.OldText;
            makeTimerCalledTrue(1);
            timersPanelControls(true, true);
            timersPanel.savePanel.Blocked = true;
            timersPanel.minimizeTitle();
            level3CancelEvent += cancelCountDownPanel_Click;
        }

        void saveCountDownPanel_Click()
        {
            timerCountDownPanel.numberBox.OldText = timerCountDownPanel.numberBox.Text;
            timersPanelCalled = false;
            timerCountDownPanel.doSlideOutY();
            if (timeScheme != null)
                timeScheme = null;
            timersPanel.setSubSavedTimeScheme(new CountDownTimeScheme(Convert.ToInt32(timerCountDownPanel.numberBox.Text) * 60));
            //timeScheme = new CountDownTimeScheme(Convert.ToInt32(timerCountDownPanel.numberBox.Text) * 60);
        }

        void cancelCountDownPanel_Click()
        {
            timersPanelCalled = false;
            timerCountDownPanel.doSlideOutY();
            level3CancelEvent -= cancelCountDownPanel_Click;
        }

        void timerCountDownPanel_FinishedSlidingOut()
        {
            timersPanel.maximizeTitle();
        }

        void timer3Panel_Click()
        {
            timerHourGlassPanel.numberBox.Text = timerHourGlassPanel.numberBox.OldText;
            makeTimerCalledTrue(2);
            timersPanelControls(true, true);
            timersPanel.savePanel.Blocked = true;
            timersPanel.minimizeTitle();
            level3CancelEvent += cancelHourglassPanel_Click;
        }

        void saveHourglassPanel_Click()
        {
            timerHourGlassPanel.numberBox.OldText = timerHourGlassPanel.numberBox.Text;
            timersPanelCalled = false;
            timerHourGlassPanel.doSlideOutY();
            if (timeScheme != null)
                timeScheme = null;
            //timeScheme = new HourglassTimeScheme(Convert.ToInt32(timerCountDownPanel.numberBox.Text) * 60);
            timersPanel.setSubSavedTimeScheme(new HourglassTimeScheme(Convert.ToInt32(timerCountDownPanel.numberBox.Text) * 60));
        }

        void cancelHourglassPanel_Click()
        {
            timersPanelCalled = false;
            timerHourGlassPanel.doSlideOutY();
            level3CancelEvent -= cancelHourglassPanel_Click;
        }

        void timerHourglassPanel_FinishedSlidingOut()
        {
            timersPanel.maximizeTitle();
        }

        void timer4Panel_Click()
        {
            makeTimerCalledTrue(3);
            timersPanelControls(true, true);
            timersPanel.savePanel.Blocked = true;
            timersPanel.minimizeTitle();
            level3CancelEvent += cancelNoTimer_Click;
        }

        void saveNoTimer_Click()
        {
            timersPanelCalled = false;
            timerNoTimer.doSlideOutY();
            if (timeScheme != null)
                timeScheme = null;
            //timeScheme = new NoTimeScheme();
            timersPanel.setSubSavedTimeScheme(new NoTimeScheme());
        }

        void cancelNoTimer_Click()
        {
            timersPanelCalled = false;
            timerNoTimer.doSlideOutY();
            level3CancelEvent -= cancelNoTimer_Click;
        }

        void timerNoTimerPanel_FinishedSlidingOut()
        {
            timersPanel.maximizeTitle();
        }

        void timersPanel_Click()
        {
            settingsPanelRoot.minimizeTitle();
            p1PanelCalled = false;
            p2PanelCalled = false;
            timersPanelCalled = true;
            detailsPanelCalled = false;
            settingsPanelRoot.BlockControls();
            level2CancelEvent += cancelTimersPanel_Click;
        }

        void saveTimersPanel_Click()
        {
            p1PanelCalled = false;
            p2PanelCalled = false;
            timersPanelCalled = true;
            detailsPanelCalled = false;
            timersPanel.doSlideOutY();
            timersPanel.applySave();
        }

        void cancelTimersPanel_Click()
        {
            timersPanel.doSlideOutY();
            level2CancelEvent -= cancelTimersPanel_Click;
        }

        void timersPanel_FinishedMinimizing()
        {
            if (timerCalled[0] == true)
                timerFischerPanel.doSlideInY();
            if (timerCalled[1] == true)
                timerCountDownPanel.doSlideInY();
            if (timerCalled[2] == true)
                timerHourGlassPanel.doSlideInY();
            if (timerCalled[3] == true)
                timerNoTimer.doSlideInY();

        }

        void timersPanel_FinishedSlidingOut()
        {
            settingsPanelRoot.maximizeTitle();
        }

        void timersPanelControls(bool isBlocked, bool isVisible)
        {
            timersPanel.timer1Panel.Blocked = isBlocked;
            timersPanel.timer2Panel.Blocked = isBlocked;
            timersPanel.timer3Panel.Blocked = isBlocked;
            timersPanel.timer4Panel.Blocked = isBlocked;
            timersPanel.cancelPanel.Blocked = isBlocked;
            timersPanel.savePanel.Blocked = isBlocked;
            timersPanel.timer1Panel.Visible = isVisible;
            timersPanel.timer2Panel.Visible = isVisible;
            timersPanel.timer3Panel.Visible = isVisible;
            timersPanel.timer4Panel.Visible = isVisible;
            timersPanel.cancelPanel.Visible = isVisible;
            timersPanel.savePanel.Visible = isVisible;
        }
        #endregion
        #region Players Panels
        void p2savePanel_Click()
        {
            p2Panel.textbox.OldText = p2Panel.textbox.Text;
            p1PanelCalled = false;
            p2PanelCalled = true;
            timersPanelCalled = false;
            detailsPanelCalled = false;
            p2Panel.doSlideOutY();
        }
        void p2Panel_FinishedSlidingOut()
        {
            settingsPanelRoot.maximizeTitle();
        }
        void p2cancelPanel_Click()
        {
            p1PanelCalled = false;
            p2PanelCalled = true;
            timersPanelCalled = false;
            detailsPanelCalled = false;
            p2Panel.doSlideOutY();
            level2CancelEvent -= p2cancelPanel_Click;
        }
        void p1savePanel_Click()
        {
            p1Panel.textbox.OldText = p1Panel.textbox.Text;
            p1PanelCalled = true;
            p2PanelCalled = false;
            timersPanelCalled = false;
            detailsPanelCalled = false;
            p1Panel.doSlideOutY();
        }
        void p1Panel_FinishedSlidingOut()
        {
            settingsPanelRoot.maximizeTitle();
        }
        void p1cancelPanel_Click()
        {
            p1PanelCalled = true;
            p2PanelCalled = false;
            timersPanelCalled = false;
            detailsPanelCalled = false;
            p1Panel.doSlideOutY();
            level2CancelEvent -= p1cancelPanel_Click;
        }
        void p1Panel_Click()
        {
            p1Panel.textbox.Text = p1Panel.textbox.OldText;
            settingsPanelRoot.minimizeTitle();
            p1PanelCalled = true;
            p2PanelCalled = false;
            timersPanelCalled = false;
            detailsPanelCalled = false;
            settingsPanelRoot.BlockControls();
            level2CancelEvent += p1cancelPanel_Click;
        }
        void p2Panel_Click()
        {
            p2Panel.textbox.Text = p2Panel.textbox.OldText;
            settingsPanelRoot.minimizeTitle();
            p1PanelCalled = false;
            p2PanelCalled = true;
            timersPanelCalled = false;
            detailsPanelCalled = false;
            settingsPanelRoot.BlockControls();
            level2CancelEvent += p2cancelPanel_Click;
        }
        #endregion
        #region Settings Menu Controls
        void settingsPanelRoot_FinishedMinimizing()
        {
            if (p1PanelCalled == true)
            {
                p1Panel.doSlideInY();
            }
            if (p2PanelCalled == true)
            {
                p2Panel.doSlideInY();
            }
            if (timersPanelCalled == true)
            {
                timersPanel.doSlideInY();
            }
            if (detailsPanelCalled == true)
            {
                detailsPanel.doSlideInY();
            }

        }
        void cancelPanel_Click()
        {
            settingsPanelRoot.doSlideOutY();
            rRight.Blocked = false;
            rMiddle.Blocked = false;
            rLeft.Blocked = false;
            level0CancelEvent -= cancelPanel_Click;
        }
        void savePanel_Click()
        {
            player1Name = p1Panel.textbox.Text;
            player2Name = p2Panel.textbox.Text;

            /*Console.WriteLine("Selected Parameters:");
            Console.WriteLine(p1Panel.textbox.Text);
            Console.WriteLine(p2Panel.textbox.Text);
            Console.WriteLine(detailsPanel.SelectedOption.ToString());
            if (timeScheme != null)
                Console.WriteLine(timeScheme.ToString());*/

            settingsPanelRoot.doSlideOutY();
            rRight.Blocked = false;
            rMiddle.Blocked = false;
            rLeft.Blocked = false;
            
        }
        void settingsPanelRoot_FinishedMaximizing()
        {
            settingsPanelRoot.UnblockControls();
        }
        #endregion
        #region Exit Menu Controls
        void noPanel_Click()
        {
            exitPanel.doSlideOutY();
            rRight.Blocked = false;
            rMiddle.Blocked = false;
            rLeft.Blocked = false;
            level0CancelEvent -= noPanel_Click;
        }
        void yesPanel_Click()
        {
            Game.Exit();
        }
        #endregion
        #region Main Menu Controls
        void rLeft_Click()
        {
            rRight.Blocked = true;
            rMiddle.Blocked = true;
            rLeft.Blocked = true;
            settingsPanelRoot.doSlideInY();
            level0CancelEvent += cancelPanel_Click;
        }
        void rRight_Click()
        {
            rRight.Blocked = true;
            rMiddle.Blocked = true;
            rLeft.Blocked = true;
            exitPanel.doSlideInY();
            level0CancelEvent += noPanel_Click;
        }
        void rMiddle_Click()
        {
            this.hideScene();
            Game.GraphicsDevice.Clear(Color.White);
            for (int i = 0; i < Game.Components.Count; i++)
                if (Game.Components[i] is ActionScene)
                {
                    Game.Services.RemoveService(typeof(ActionScene));
                    Game.Components.RemoveAt(i);
                }
            //The parameters should be read from Settings menu!
            ActionScene actionScene = new ActionScene(
                Game, 
                typeof(BasicChessboardFactory), 
                timersPanel.TimeScheme, 
                player1Name, 
                player2Name);
            Game.Components.Add(actionScene);
            Game.Services.AddService(typeof(ActionScene), actionScene);
            actionScene.showScene();
        }
        #endregion
        #region IControllable Members


        public void HandleKeyboardInput()
        {
            curKeyState = Keyboard.GetState();

            if (curKeyState.IsKeyDown(Keys.Escape) && oldKeyState.IsKeyUp(Keys.Escape))
                if (level3CancelEvent != null)
                    level3CancelEvent();
                else
                    if (level2CancelEvent != null)
                        level2CancelEvent();
                    else
                        if (level1CancelEvent != null)
                            level1CancelEvent();
                        else
                            if (level0CancelEvent != null)
                                level0CancelEvent();
                            else
                                rRight_Click();
            if (curKeyState.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter))
            {
                if (exitPanel.Visible == true)
                    yesPanel_Click();
                else
                    if (settingsPanelRoot.Visible == false)
                        rMiddle_Click();
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

        public override void Update(GameTime gameTime)
        {
            if (isBlocked == false)
            {
                HandleKeyboardInput();
                HandleMouseInput();
            }
            base.Update(gameTime);
        }
        #endregion
    }

}