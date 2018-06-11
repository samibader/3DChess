using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities.MenuElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.GameEngine
{
    class TimerPanelTwoSubPanels : TimerPanelOneSubPanel
    {
        private string subExplanation2str, units2str;
        public Label subExplanation2, units2;
        public TextBoxNumeric numberBox2;
        private int maxInt2;
        public TimerPanelTwoSubPanels(
            Game game, 
            CompoundGameComponent parent, 
            int xSource, int ySource, 
            int width, int height, 
            int xDestination, 
            int yDestination, 
            string message, 
            string explanation, 
            string subExplanation1, 
            string units1, 
            string subExplanation2, 
            string units2, 
            int maxInt1,
            int maxInt2
            ) : 
            base(game, parent, xSource,ySource, width,  height, xDestination, yDestination, message, explanation, subExplanation1, units1, maxInt1)
    {
        this.subExplanation2str = subExplanation2;
        this.units2str = units2;
        this.maxInt2 = maxInt2;
        LoadContent1();
    }

        void LoadContent1()
        {
            int yExpanded = Y - Height;
            Vector2 labelSize;
            labelSize = fontRegular.MeasureString(subExplanation2str);
            subExplanation2 = new Label(
                Game,
                units.X + units.Width + 15,
                yExpanded + 2 * (Height / 4),
                fontSubExplanation,
                subExplanation2str);
            subExplanation2.Visible = false;
            subExplanation2.Blocked = true;
            parent.SubComponents.Add(subExplanation2);

            numberBox2 = new TextBoxNumeric(
                Game,
                subExplanation2.X + subExplanation2.Width + 5,
                yExpanded + (2 * (Height / 4)) - Math.Abs(((subExplanation2.Height - (Height / 4)) / 2)),
                40,
                Height / 4,
                maxInt2,
                "5",
                fontTyping);
            numberBox2.Visible = false;
            numberBox2.Blocked = true;
            parent.SubComponents.Add(numberBox2);

            labelSize = fontRegular.MeasureString(units2str);
            units2 = new Label(
                Game,
                numberBox2.X + numberBox2.Width + 5,
                yExpanded + 2 * (Height / 4),
                fontSubExplanation,
                units2str);
            units2.Visible = false;
            units2.Blocked = true;
            parent.SubComponents.Add(units2);

            this.FinishedSlidingIn += new FinishedSlidingInHandler(YesNoPanel_FinishedSlidingIn2);
        }

        public override void doSlideOutY()
        {
            base.doSlideOutY();
            subExplanation2.Visible = false;
            subExplanation2.Blocked= true;
            units2.Visible = false;
            units2.Blocked= true;
            numberBox2.Visible = false;
            numberBox2.Blocked= true;
        }

        void YesNoPanel_FinishedSlidingIn2()
        {
            subExplanation2.Visible = true;
            subExplanation2.Blocked = false;
            units2.Visible = true;
            units2.Blocked = false;
            numberBox2.Visible = true;
            numberBox2.Blocked = false;
        }


    }
}
