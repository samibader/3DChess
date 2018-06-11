using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities.MenuElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.GameEngine
{
    class PlayerNamePanel : SlidingRect
    {
        string msgStr;
        public Rect savePanel, cancelPanel;
        public Textbox textbox;

        SpriteFont fontRegular, fontBold, fontTyping;
        public Label msg;
        Label save, cancel;
        CompoundGameComponent parent;
        private string defaultTextboxText;

        public PlayerNamePanel(
            Game game, 
            CompoundGameComponent parent, int xSource, int ySource, int width, int height, int xDestination, int yDestination, string message, string defaultTextboxText)
            : base(game, xSource, ySource, width, height, xDestination, yDestination)
        {
            this.msgStr = message;
            this.parent = parent;
            this.defaultTextboxText = defaultTextboxText;
            LoadContent();
        }

        protected override void LoadContent()
        {
            Mute(Color.White);

            fontBold = Game.Content.Load<SpriteFont>("Fonts\\InfoFontLarge");
            fontBold.Spacing = 4;
            fontRegular = Game.Content.Load<SpriteFont>("Fonts\\InfoFont");
            fontRegular.Spacing = 2;
            fontTyping = Game.Content.Load<SpriteFont>("Fonts\\Typing");
            int yExpanded = Y - Height;

            Vector2 labelSize = fontBold.MeasureString(msgStr);
            msg = new Label(Game,
                Convert.ToInt32(X + (Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(yExpanded + (Height / 6) - (labelSize.Y / 2)),
                fontBold,
                msgStr);
            msg.Visible = false;
            msg.Blocked = true;

            textbox = new Textbox(
                Game,
                X + Width / 4,
                yExpanded + (Height / 3),
                Width / 2,
                Height / 3,
                defaultTextboxText,
                fontTyping);
            textbox.Visible = false;
            textbox.Blocked = true;

            savePanel = new Rect(
                Game,
                X,
                yExpanded + 2* (Height / 3),
                Width / 2,
                Height / 3);
            savePanel.Visible = false;
            savePanel.Blocked = true;
            savePanel.HoveredBackgroundColor = Color.Green;

            cancelPanel = new Rect(
                Game,
                X + (Width / 2),
                yExpanded + 2 * (Height / 3),
                Width / 2,
                Height / 3);
            cancelPanel.Visible = false;
            cancelPanel.Blocked = true;
            cancelPanel.HoveredBackgroundColor = Color.DarkRed;

            parent.SubComponents.Add(msg);
            parent.SubComponents.Add(textbox);
            parent.SubComponents.Add(savePanel);
            parent.SubComponents.Add(cancelPanel);

            labelSize = fontRegular.MeasureString("Save");
            save = new Label(Game,
                Convert.ToInt32(savePanel.X + (savePanel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(savePanel.Y + (savePanel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Save");
            save.Visible = false;
            save.Blocked = true;
            parent.SubComponents.Add(save);

            labelSize = fontRegular.MeasureString("Cancel");
            cancel = new Label(Game,
                Convert.ToInt32(cancelPanel.X + (cancelPanel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(cancelPanel.Y + (cancelPanel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Cancel");
            cancel.Visible = false;
            cancel.Blocked = true;
            parent.SubComponents.Add(cancel);

            this.FinishedSlidingIn += new FinishedSlidingInHandler(YesNoPanel_FinishedSlidingIn);

            

            base.LoadContent();
        }

        void YesNoPanel_FinishedSlidingIn()
        {
            textbox.Visible = true;
            textbox.Blocked = false;
            savePanel.Visible = true;
            savePanel.Blocked = false;
            cancelPanel.Visible = true;
            cancelPanel.Blocked = false;
            msg.Visible = true;
            msg.Blocked = false;
            save.Visible = true;
            save.Blocked = false;
            cancel.Visible = true;
            cancel.Blocked = false;
        }

        public override void doSlideOutY()
        {
            textbox.Visible = false;
            textbox.Blocked = true;
            savePanel.Visible = false;
            savePanel.Blocked = true;
            cancelPanel.Visible = false;
            cancelPanel.Blocked = true;
            msg.Visible = false;
            msg.Blocked = true;
            save.Visible = false;
            save.Blocked = true;
            cancel.Visible = false;
            cancel.Blocked = true;
            base.doSlideOutY();
        }
    }
}
