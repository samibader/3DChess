using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities.MenuElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.GameEngine
{
    class YesNoPanel : SlidingRect
    {
        string msgStr;
        public Rect yesPanel, noPanel;
        SpriteFont fontRegular, fontBold;
        public Label msg;
        protected Label yes, no;
        CompoundGameComponent parent;


        public YesNoPanel(Game game, CompoundGameComponent parent, int xSource, int ySource, int width, int height, int xDestination, int yDestination, string message)
            : base(game, xSource, ySource, width, height, xDestination, yDestination)
        {
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
                Convert.ToInt32(yExpanded + (Height / 4) - (labelSize.Y / 2)),
                fontBold,
                msgStr);
            msg.Visible = false;
            msg.Blocked = true;

            yesPanel = new Rect(
                Game,
                X,
                yExpanded + (Height / 2),
                Width / 2,
                Height / 2);
            yesPanel.Visible = false;
            yesPanel.Blocked = true;
            yesPanel.HoveredBackgroundColor = Color.DarkRed;

            noPanel = new Rect(
                Game,
                X + (Width / 2),
                yExpanded + (Height / 2),
                Width / 2,
                Height / 2);
            noPanel.Visible = false;
            noPanel.Blocked = true;
            noPanel.HoveredBackgroundColor = Color.Green;

            parent.SubComponents.Add(msg);
            parent.SubComponents.Add(yesPanel);
            parent.SubComponents.Add(noPanel);

            labelSize = fontRegular.MeasureString("Yes");
            yes = new Label(Game,
                Convert.ToInt32(yesPanel.X + (yesPanel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(yesPanel.Y + (yesPanel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Yes");
            yes.Visible = false;
            yes.Blocked = true;
            parent.SubComponents.Add(yes);

            labelSize = fontRegular.MeasureString("No");
            no = new Label(Game,
                Convert.ToInt32(noPanel.X + (noPanel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(noPanel.Y + (noPanel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "No");
            no.Visible = false;
            no.Blocked = true;
            parent.SubComponents.Add(no);

            this.FinishedSlidingIn += new FinishedSlidingInHandler(YesNoPanel_FinishedSlidingIn);

            base.LoadContent();
        }

        void YesNoPanel_FinishedSlidingIn()
        {
            yesPanel.Visible = true;
            yesPanel.Blocked = false;
            noPanel.Visible = true;
            noPanel.Blocked = false;
            msg.Visible = true;
            msg.Blocked = false;
            yes.Visible = true;
            yes.Blocked = false;
            no.Visible = true;
            no.Blocked = false;
        }

        public override void doSlideOutY()
        {
            yesPanel.Visible = false;
            yesPanel.Blocked = true;
            noPanel.Visible = false;
            noPanel.Blocked = true;
            msg.Visible = false;
            msg.Blocked = true;
            yes.Visible = false;
            yes.Blocked = true;
            no.Visible = false;
            no.Blocked = true;
            base.doSlideOutY();
        }
    }
}
