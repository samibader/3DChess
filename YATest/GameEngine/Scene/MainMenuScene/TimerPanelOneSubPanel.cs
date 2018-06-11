using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities.MenuElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.GameEngine
{
    class TimerPanelOneSubPanel : SlidingRect
    {
        protected string msgStr, explanationStr, subExplanationStr, unitsStr;
        public Rect savePanel, cancelPanel;
        protected SpriteFont fontRegular, fontBold, fontExplanation, fontSubExplanation, fontTyping;
        public Label msg, explanation, subExplanation, units;
        public TextBoxNumeric numberBox;
        protected Label save, cancel;
        protected CompoundGameComponent parent;
        int maxInt;

        public TimerPanelOneSubPanel(Game game, 
            CompoundGameComponent parent, 
            int xSource, int ySource, 
            int width, int height, 
            int xDestination, 
            int yDestination, 
            string message, 
            string explanation, 
            string subExplanation, 
            string units, 
            int maxInt 
            )
            : base(game, xSource, ySource, width, height, xDestination, yDestination)
        {
            this.msgStr = message;             
            this.explanationStr = explanation; 
            this.subExplanationStr = subExplanation;
            this.unitsStr = units; 
            this.maxInt = maxInt;
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
            fontExplanation = Game.Content.Load<SpriteFont>("Fonts\\Typing");
            fontSubExplanation = Game.Content.Load<SpriteFont>("Fonts\\TypingSmall");
            fontTyping = Game.Content.Load<SpriteFont>("Fonts\\TypingSmall");
            int yExpanded = Y - Height;

            Vector2 labelSize = fontBold.MeasureString(msgStr);
            msg = new Label(Game,
                Convert.ToInt32(X + (Width / 2) - (labelSize.X / 2)),
                Convert.ToInt32(yExpanded + (Height / 8) - (labelSize.Y / 2)),
                fontBold,
                msgStr);
            msg.Visible = false;
            msg.Blocked = true;
            parent.SubComponents.Add(msg);

            labelSize = fontRegular.MeasureString(explanationStr);
            explanation = new Label(
                Game,
                X+4,
                yExpanded + (Height / 4),
                fontExplanation,
                explanationStr);
            explanation.Visible = false;
            explanation.Blocked = true;
            parent.SubComponents.Add(explanation);

            labelSize = fontRegular.MeasureString(subExplanationStr);
            subExplanation = new Label(
                Game,
                X + 30,
                yExpanded + 2 * (Height / 4),
                fontSubExplanation,
                subExplanationStr);
            subExplanation.Visible = false;
            subExplanation.Blocked = true;
            parent.SubComponents.Add(subExplanation);

            numberBox = new TextBoxNumeric(
                Game,
                subExplanation.X + subExplanation.Width + 5,
                yExpanded + (2 * (Height / 4)) - Math.Abs(((subExplanation.Height-(Height/4)) / 2)),
                40,
                Height / 4,
                maxInt,
                "10",
                fontTyping);
            numberBox.Visible = false;
            numberBox.Blocked = true;
            parent.SubComponents.Add(numberBox);

            labelSize = fontRegular.MeasureString(unitsStr);
            units = new Label(
                Game,
                numberBox.X + numberBox.Width + 5,
                yExpanded + 2 * (Height / 4),
                fontSubExplanation,
                unitsStr);
            units.Visible = false;
            units.Blocked = true;
            parent.SubComponents.Add(units);

            savePanel = new Rect(
                Game,
                X,
                yExpanded + 3 * (Height / 4),
                Width / 2,
                Height / 4);
            savePanel.Visible = false;
            savePanel.Blocked = true;
            savePanel.HoveredBackgroundColor = Color.Green;
            parent.SubComponents.Add(savePanel);

            cancelPanel = new Rect(
                Game,
                X + (Width / 2),
                yExpanded + 3 * (Height / 4),
                Width / 2,
                Height / 4);
            cancelPanel.Visible = false;
            cancelPanel.Blocked = true;
            cancelPanel.HoveredBackgroundColor = Color.DarkRed;
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
            explanation.Visible = true;
            explanation.Blocked = false;
            subExplanation.Visible = true;
            subExplanation.Blocked = false;
            numberBox.Visible = true;
            numberBox.Blocked = false;
            units.Visible = true;
            units.Blocked = false;
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
            explanation.Visible = false;
            explanation.Blocked = true;
            subExplanation.Visible = false;
            subExplanation.Blocked = true;
            numberBox.Visible = false;
            numberBox.Blocked = true;
            units.Visible = false;
            units.Blocked = true;
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
