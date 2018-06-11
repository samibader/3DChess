using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities.MenuElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.GameEngine
{
    class ToggleMenuPanel : SlidingRect
    {
        string msgStr;
        public Rect savePanel, cancelPanel;
        SpriteFont fontRegular, fontBold;
        public Label msg;
        Label save, cancel;
        CompoundGameComponent parent;
        private string option1Str, option2Str;
        public Rect option1, option2;
        private Label option1Label, option2Label;

        public enum Options { option1, option2 };

        private Options selectedOption;

        public Options SelectedOption
        {
            get { return selectedOption; }
            set { selectedOption = value; }
        }

        public ToggleMenuPanel(
            Game game, CompoundGameComponent parent,
            int xSource, int ySource,
            int width, int height,
            int xDestination, int yDestination,
            string message,
            string option1,
            string option2
            )
            : base(game, xSource, ySource, width, height, xDestination, yDestination)
        {
            this.msgStr = message;
            this.parent = parent;
            this.option1Str = option1;
            this.option2Str = option2;
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
                Convert.ToInt32(yExpanded + (Height / 6) - (labelSize.Y / 3)),
                fontBold,
                msgStr);
            msg.Visible = false;
            msg.Blocked = true;
            parent.SubComponents.Add(msg);
        }

        private void loadOptions()
        {
            int yExpanded = Y - Height;
            option1 = new Rect(
                Game,
                X,
                yExpanded + (Height / 3),
                Width / 2,
                Height / 3);
            option1.Visible = false;
            option1.Blocked = true;
            option1.HoveredBackgroundColor = Color.Yellow;
            option1.SelectedBackgroundColor = Color.YellowGreen;
            option1.Click += new ClickHandler(option1_Click);

            option2 = new Rect(
                Game,
                X + (Width / 2),
                yExpanded + (Height / 3),
                Width / 2,
                Height / 3);
            option2.Visible = false;
            option2.Blocked = true;
            option2.HoveredBackgroundColor = Color.Yellow;
            option2.SelectedBackgroundColor = Color.YellowGreen;
            option2.Click += new ClickHandler(option2_Click);

            parent.SubComponents.Add(option1);
            parent.SubComponents.Add(option2);

            Vector2 labelSize = fontRegular.MeasureString(option1Str);
            option1Label = new Label(Game,
                Convert.ToInt32(option1.X + (option1.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(option1.Y + (option1.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                option1Str);
            option1Label.Visible = false;
            option1Label.Blocked = true;
            parent.SubComponents.Add(option1Label);

            labelSize = fontRegular.MeasureString(option2Str);
            option2Label = new Label(Game,
                Convert.ToInt32(option2.X + (option2.Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(option2.Y + (option2.Height / 2) - (labelSize.Y / 2)),
                fontRegular,
                option2Str);
            option2Label.Visible = false;
            option2Label.Blocked = true;
            parent.SubComponents.Add(option2Label);
        }

        void option2_Click()
        {
            option1.Blocked = false;
            option2.Blocked = true;
            this.selectedOption = Options.option2;
        }

        void option1_Click()
        {
            option2.Blocked = false;
            option1.Blocked = true;
            this.selectedOption = Options.option1;
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
        }

        protected override void LoadContent()
        {
            Mute(Color.White);

            loadFonts();

            loadTitle();

            loadOptions();

            loadButtons();

            selectedOption = Options.option2; //High details

            this.FinishedSlidingIn += new FinishedSlidingInHandler(YesNoPanel_FinishedSlidingIn);

            base.LoadContent();
        }

        void setItemsVisibility(bool visibility)
        {
            if (visibility == false)
            {
                option1.Visible = false;
                option1.Blocked = true;
                option2.Visible = false;
                option2.Blocked = true;
                option1Label.Visible = false;
                option1Label.Blocked = true;
                option2Label.Visible = false;
                option2Label.Blocked = true;
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
            }
            else
            {
                option1.Visible = true;
                option1.Blocked = false;
                option2.Visible = true;
                option2.Blocked = false;
                option1Label.Visible = true;
                option1Label.Blocked = false;
                option2Label.Visible = true;
                option2Label.Blocked = false;
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
        }
        
        void YesNoPanel_FinishedSlidingIn()
        {
            setItemsVisibility(true);
        }

        public override void doSlideOutY()
        {
            setItemsVisibility(false);
            base.doSlideOutY();
        }
    }
}
