using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities.MenuElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using YATest.Utilities;

namespace YATest.GameEngine
{
    class TimersPanel : SlidingRectShrinkableTitle
    {
        private string msgStr;
        public Rect timer1Panel, timer2Panel, timer3Panel, timer4Panel;
        private SpriteFont fontRegular, fontBold;
        public Label msg;
        private Label timer1, timer2, timer3, timer4;
        private CompoundGameComponent parent;
        private Label saveLabel, cancelLabel;
        public Rect savePanel, cancelPanel;

        private AbstractTimeScheme subSavedTimeScheme, timeScheme;

        public AbstractTimeScheme TimeScheme
        {
            get { return timeScheme; }
        }

        /// <summary>
        /// When "save" button pressed in a sub-menu, modify the subSaved. i.e. Create a semi-saved state.
        /// </summary>
        /// <param name="timeScheme">The new time scheme saved by the user</param>
        public void setSubSavedTimeScheme(AbstractTimeScheme timeScheme)
        {
            subSavedTimeScheme = timeScheme;
        }

        /// <summary>
        /// Saves the final changes the user has made. Modifies <paramref name="timeScheme"/>.
        /// </summary>
        public void applySave()
        {
            timeScheme = subSavedTimeScheme;
        }

        public TimersPanel(Game game, CompoundGameComponent parent, int xSource, int ySource, int width, int height, int xDestination, int yDestination, string message)
            : base(game, xSource, ySource, width, height, xDestination, yDestination, null, 40)
        {
            timeScheme = new FischerTimeScheme(180, 5);
            this.msgStr = message;
            this.parent = parent;
            LoadContent();
        }

        protected override void LoadContent()
        {
            Mute(Color.White);

            fontBold = Game.Content.Load<SpriteFont>("Fonts\\InfoFontLarge");
            fontBold.Spacing = 4;
            fontRegular = Game.Content.Load<SpriteFont>("Fonts\\InfoFont");
            fontRegular.Spacing = 2;
            int yExpanded = Y - Height;



            Vector2 labelSize = fontBold.MeasureString(msgStr);
            msg = new Label(Game,
                Convert.ToInt32(X + (Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(yExpanded + (Height / 6) - (labelSize.Y / 2)),
                fontBold,
                msgStr);
            msg.Visible = false;
            msg.Blocked = true;
            parent.SubComponents.Add(msg);

            this.TitleRef = msg;
            this.TitleSize = Height / 3;

            timer1Panel = new Rect(
                Game,
                X,
                yExpanded + (Height / 3),
                Width / 4,
                Height / 3);
            timer1Panel.Visible = false;
            timer1Panel.Blocked = true;
            parent.SubComponents.Add(timer1Panel);
            timer1Panel.HoveredBackgroundColor = Color.Yellow;

            timer2Panel = new Rect(
                Game,
                X + (Width / 4),
                yExpanded + (Height / 3),
                Width / 4,
                Height / 3);
            timer2Panel.Visible = false;
            timer2Panel.Blocked = true;
            parent.SubComponents.Add(timer2Panel);
            timer2Panel.HoveredBackgroundColor = Color.Yellow;

            timer3Panel = new Rect(
                Game,
                X + 2 * (Width / 4),
                yExpanded + (Height / 3),
                Width / 4,
                Height / 3);
            timer3Panel.Visible = false;
            timer3Panel.Blocked = true;
            parent.SubComponents.Add(timer3Panel);
            timer3Panel.HoveredBackgroundColor = Color.Yellow;

            timer4Panel = new Rect(
                Game,
                X + 3 * (Width / 4),
                yExpanded + (Height / 3),
                Width / 4,
                Height / 3);
            timer4Panel.Visible = false;
            timer4Panel.Blocked = true;
            parent.SubComponents.Add(timer4Panel);
            timer4Panel.HoveredBackgroundColor = Color.Yellow;

            labelSize = fontRegular.MeasureString("Fisher Time");
            timer1 = new Label(Game,
                Convert.ToInt32(timer1Panel.X + (timer1Panel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(timer1Panel.Y + (timer1Panel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Fisher Time");
            timer1.Visible = false;
            timer1.Blocked = true;
            parent.SubComponents.Add(timer1);

            labelSize = fontRegular.MeasureString("Countdown");
            timer2 = new Label(Game,
                Convert.ToInt32(timer2Panel.X + (timer2Panel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(timer2Panel.Y + (timer2Panel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Countdown");
            timer2.Visible = false;
            timer2.Blocked = true;
            parent.SubComponents.Add(timer2);

            labelSize = fontRegular.MeasureString("Hourglass");
            timer3 = new Label(Game,
                Convert.ToInt32(timer3Panel.X + (timer3Panel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(timer3Panel.Y + (timer3Panel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Hourglass");
            timer3.Visible = false;
            timer3.Blocked = true;
            parent.SubComponents.Add(timer3);

            labelSize = fontRegular.MeasureString("No Timer");
            timer4 = new Label(Game,
                Convert.ToInt32(timer4Panel.X + (timer4Panel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(timer4Panel.Y + (timer4Panel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "No Timer");
            timer4.Visible = false;
            timer4.Blocked = true;
            parent.SubComponents.Add(timer4);

            savePanel = new Rect(
                Game,
                X,
                yExpanded + 2 * (Height / 3),
                Width / 2,
                Height / 3
                );
            savePanel.Visible = false;
            savePanel.Blocked = true;
            savePanel.HoveredBackgroundColor = Color.Green;

            cancelPanel = new Rect(
                Game,
                X + savePanel.Width,
                yExpanded + 2 * (Height / 3),
                Width / 2,
                Height / 3
                );
            cancelPanel.Visible = false;
            cancelPanel.Blocked = true;
            cancelPanel.HoveredBackgroundColor = Color.DarkRed;

            parent.SubComponents.Add(savePanel);
            parent.SubComponents.Add(cancelPanel);

            labelSize = fontRegular.MeasureString("Save");
            saveLabel = new Label(Game,
                Convert.ToInt32(savePanel.X + (savePanel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(savePanel.Y + (savePanel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Save");
            saveLabel.Visible = false;
            saveLabel.Blocked = true;
            parent.SubComponents.Add(saveLabel);

            labelSize = fontRegular.MeasureString("Cancel");
            cancelLabel = new Label(Game,
                Convert.ToInt32(cancelPanel.X + (cancelPanel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(cancelPanel.Y + (cancelPanel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Cancel");
            cancelLabel.Visible = false;
            cancelLabel.Blocked = true;
            parent.SubComponents.Add(cancelLabel);


            this.FinishedSlidingIn += new FinishedSlidingInHandler(TimerPanel_FinishedSlidingIn);

            base.LoadContent();
        }

        void TimerPanel_FinishedSlidingIn()
        {
            timer1Panel.Visible = true;
            timer1Panel.Blocked = false;
            timer2Panel.Visible = true;
            timer2Panel.Blocked = false;
            timer3Panel.Visible = true;
            timer3Panel.Blocked = false;
            timer4Panel.Visible = true;
            timer4Panel.Blocked = false;
            savePanel.Visible = true;
            savePanel.Blocked = false;
            cancelPanel.Visible = true;
            cancelPanel.Blocked = false;
            msg.Visible = true;
            msg.Blocked = false;
            timer1.Visible = true;
            timer1.Blocked = false;
            timer2.Visible = true;
            timer2.Blocked = false;
            timer3.Visible = true;
            timer3.Blocked = false;
            timer4.Visible = true;
            timer4.Blocked = false;
            saveLabel.Visible = true;
            saveLabel.Blocked = false;
            cancelLabel.Visible = true;
            cancelLabel.Blocked = false;
        }

        public override void doSlideOutY()
        {
            timer1Panel.Visible = false;
            timer1Panel.Blocked = true;
            timer2Panel.Visible = false;
            timer2Panel.Blocked = true;
            timer3Panel.Visible = false;
            timer3Panel.Blocked = true;
            timer4Panel.Visible = false;
            timer4Panel.Blocked = true;
            savePanel.Visible = false;
            savePanel.Blocked = true;
            cancelPanel.Visible = false;
            cancelPanel.Blocked = true;
            msg.Visible = false;
            msg.Blocked = true;
            timer1.Visible = false;
            timer1.Blocked = true;
            timer2.Visible = false;
            timer2.Blocked = true;
            timer3.Visible = false;
            timer3.Blocked = true;
            timer4.Visible = false;
            timer4.Blocked = true;
            saveLabel.Visible = false;
            saveLabel.Blocked = true;
            cancelLabel.Visible = false;
            cancelLabel.Blocked = true;
            base.doSlideOutY();
        }
    }
}
