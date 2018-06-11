using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities.MenuElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.GameEngine
{
    class SettingsMenuRoot : SlidingRectShrinkableTitle
    {
        public Rect p1Panel, p2Panel, timersPanel, detailsPanel;
        public Rect savePanel, cancelPanel;
        SpriteFont fontRegular, fontBold;
        public Label msg;
        Label player1, player2, timers, details;
        Label saveLabel, cancelLabel;
        CompoundGameComponent parent;

        string msgStr;

        public SettingsMenuRoot(Game game, CompoundGameComponent parent, int xSource, int ySource, int width, int height, int xDestination, int yDestination, string message)
            : base(game, xSource, ySource, width, height, xDestination, yDestination, null, 40)
        {
            this.parent = parent;
            this.msgStr = message;
            LoadContent();
        }

        private void loadFonts()
        {
            fontBold = Game.Content.Load<SpriteFont>("Fonts\\InfoFontLarge");
            fontBold.Spacing = 4;
            fontRegular = Game.Content.Load<SpriteFont>("Fonts\\InfoFont");
            fontRegular.Spacing = 2;
        }

        private void loadTitle()
        {
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
        }

        private void loadSettingsItems()
        {
            int yExpanded = Y - Height;

            Vector2 labelSize = fontBold.MeasureString(msgStr);

            timersPanel = new Rect(
                Game,
                X,
                yExpanded + (Height / 3),
                Width / 4,
                Height / 3);
            timersPanel.Visible = false;
            timersPanel.Blocked = true;
            timersPanel.HoveredBackgroundColor = Color.Yellow;

            p1Panel = new Rect(
                Game,
                X + timersPanel.Width,
                yExpanded + (Height / 3),
                Width / 4,
                Height / 3);
            p1Panel.Visible = false;
            p1Panel.Blocked = true;
            p1Panel.HoveredBackgroundColor = Color.Yellow;

            p2Panel = new Rect(
                Game,
                p1Panel.X + p1Panel.Width,
                yExpanded + (Height / 3),
                Width / 4,
                Height / 3);
            p2Panel.Visible = false;
            p2Panel.Blocked = true;
            p2Panel.HoveredBackgroundColor = Color.Yellow;

            detailsPanel = new Rect(
                Game,
                p2Panel.X + p2Panel.Width,
                yExpanded + (Height / 3),
                Width / 4,
                Height / 3);
            detailsPanel.Visible = false;
            detailsPanel.Blocked = true;
            detailsPanel.HoveredBackgroundColor = Color.Yellow;

            parent.SubComponents.Add(timersPanel);
            parent.SubComponents.Add(p1Panel);
            parent.SubComponents.Add(p2Panel);
            parent.SubComponents.Add(detailsPanel);

            labelSize = fontRegular.MeasureString("Timer");
            timers = new Label(Game,
                Convert.ToInt32(timersPanel.X + (timersPanel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(timersPanel.Y + (timersPanel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Timer");
            timers.Visible = false;
            timers.Blocked = true;
            parent.SubComponents.Add(timers);

            labelSize = fontRegular.MeasureString("Player 1 Name");
            player1 = new Label(Game,
                Convert.ToInt32(p1Panel.X + (p1Panel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(p1Panel.Y + (p1Panel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Player 1 Name");
            player1.Visible = false;
            player1.Blocked = true;
            parent.SubComponents.Add(player1);

            labelSize = fontRegular.MeasureString("Player 2 Name");
            player2 = new Label(Game,
                Convert.ToInt32(p2Panel.X + (p2Panel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(p2Panel.Y + (p2Panel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Player 2 Name");
            player2.Visible = false;
            player2.Blocked = true;
            parent.SubComponents.Add(player2);

            labelSize = fontRegular.MeasureString("Details");
            details = new Label(Game,
                Convert.ToInt32(detailsPanel.X + (detailsPanel.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(detailsPanel.Y + (detailsPanel.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                "Details");
            details.Visible = false;
            details.Blocked = true;
            parent.SubComponents.Add(details);
        }

        private void loadButtons()
        {
            int yExpanded = Y - Height;

            Vector2 labelSize = fontBold.MeasureString(msgStr);

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
        }

        protected override void LoadContent()
        {
            Mute(Color.White);

            loadFonts();

            loadTitle();

            loadSettingsItems();

            loadButtons();

            this.FinishedSlidingIn += new FinishedSlidingInHandler(SettingsPanel_FinishedSlidingIn);

            base.LoadContent();
        }

        void SettingsPanel_FinishedSlidingIn()
        {
            setItemsVisibility(true);
        }

        public void setItemsVisibility(bool visibility)
        {
            if (visibility == true)
            {
                msg.Visible = true;
                msg.Blocked = false;
                timersPanel.Visible = true;
                timersPanel.Blocked = false;
                p1Panel.Visible = true;
                p1Panel.Blocked = false;
                p2Panel.Visible = true;
                p2Panel.Blocked = false;
                timers.Visible = true;
                timers.Blocked = false;
                player1.Visible = true;
                player1.Blocked = false;
                player2.Visible = true;
                player2.Blocked = false;
                details.Visible = true;
                details.Blocked = false;
                detailsPanel.Visible = true;
                detailsPanel.Blocked = false;
                savePanel.Visible = true;
                savePanel.Blocked = false;
                cancelPanel.Visible = true;
                cancelPanel.Blocked = false;
                saveLabel.Visible = true;
                saveLabel.Blocked = false;
                cancelLabel.Visible = true;
                cancelLabel.Blocked = false;
            }
            else
            {
                msg.Visible = false;
                msg.Blocked = true;
                timersPanel.Visible = false;
                timersPanel.Blocked = true;
                p1Panel.Visible = false;
                p1Panel.Blocked = true;
                p2Panel.Visible = false;
                p2Panel.Blocked = true;
                timers.Visible = false;
                timers.Blocked = true;
                player1.Visible = false;
                player1.Blocked = true;
                player2.Visible = false;
                player2.Blocked = true;
                details.Visible = false;
                details.Blocked = true;
                detailsPanel.Visible = false;
                detailsPanel.Blocked = true;
                savePanel.Visible = false;
                savePanel.Blocked = true;
                cancelPanel.Visible = false;
                cancelPanel.Blocked = true;
                saveLabel.Visible = false;
                saveLabel.Blocked = true;
                cancelLabel.Visible = false;
                cancelLabel.Blocked = true;
            }
        }

        public override void doSlideOutY()
        {
            setItemsVisibility(false);
            base.doSlideOutY();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void UnblockControls()
        {
            p1Panel.Blocked = false;
            p2Panel.Blocked = false;
            timersPanel.Blocked = false;
            detailsPanel.Blocked = false;
            cancelPanel.Blocked = false;
            savePanel.Blocked = false;
        }

        public void BlockControls()
        {
            p1Panel.Blocked = true;
            p2Panel.Blocked = true;
            timersPanel.Blocked = true;
            detailsPanel.Blocked = true;
            cancelPanel.Blocked = true;
            savePanel.Blocked = true;
        }
    }
}

