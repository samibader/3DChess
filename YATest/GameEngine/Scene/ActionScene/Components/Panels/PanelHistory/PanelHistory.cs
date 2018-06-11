using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using YATest.Utilities;

namespace YATest.GameEngine
{
    /// <summary>
    /// Simply a panel that shows history information of the game
    /// </summary>
    class PanelHistory : PanelDynamic
    {
        struct Message
        {
            public Message(string content, bool sentByP1, int linesNum)
            {
                this.content = content;
                this.sentByP1 = sentByP1;
                this.linesNum = linesNum;
            }
            public string content;
            public bool sentByP1;
            public int linesNum;
        }

        private SpriteFont font;
        private Queue<Message> lastEvents;
        private SpriteBatch curSpriteBatch;

        private int width;
        private int linesNum; //should be less than 8

        private int messagesNum = 4;
        public int MessagesNum
        {
            get { return messagesNum; }
            set { messagesNum = value; }
        }

        private int height = 170;

        private static PanelHistory panelInfo = null;
        
        //<Singleton Pattern>
        protected PanelHistory(Game game, CompoundGameComponent parent)
            : base(game, parent, "Panels\\SideHandle2", "Panels\\CompletePane2", 170 /*height*/)
        {
            lastEvents = new Queue<Message>();
            font = Game.Content.Load<SpriteFont>("Fonts/InfoFontSmall");
            curSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            linesNum = 0;
        }
        
        public static PanelHistory createReference(Game game, CompoundGameComponent parent)
        {
            if (panelInfo == null)
                panelInfo = new PanelHistory(game, parent);
            return panelInfo;
        }

        public static void resetReference()
        {
            if (panelInfo != null)
            {
                panelInfo.Dispose();
                panelInfo = null;
            }
        }

        public static PanelHistory getReference()
        {
            return panelInfo;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public void addMessage(string message, bool sentByP1)
        {
            width = Game.GraphicsDevice.Viewport.Width;
            //Format the message and get the number of its lines (0 indicates 1 line)
            int msgLinesNum = Utilities.StringFormatter.SetTextWidth(ref message, font, 140.0f);

            int numOfLines = 0;
            //calculate the number of lines
            do
            {
                numOfLines = 0;
                foreach (Message m in lastEvents)
                    numOfLines += m.linesNum;
                if (numOfLines + msgLinesNum > 8)
                    lastEvents.Dequeue();
            } 
            while (numOfLines + msgLinesNum > 8);

            lastEvents.Enqueue(new Message(message, sentByP1, msgLinesNum));
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
            linesNum = 0;
            Game.GraphicsDevice.RenderState.DepthBufferEnable = false;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            if ((isOpened == true || isEnlarged == true) && isMovingRight == false)
            {
                curSpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                foreach (Message m in lastEvents)
                {
                    if (m.sentByP1 == true)
                        curSpriteBatch.DrawString(font, m.content, new Vector2(width - 155, height + 5 + linesNum * 18), Color.White);
                    else
                        curSpriteBatch.DrawString(font, m.content, new Vector2(width - 155, height + 5 + linesNum * 18), Color.Silver);
                    linesNum += m.linesNum;
                }
                curSpriteBatch.End();
            }
            Game.GraphicsDevice.RenderState.DepthBufferEnable = true;
            Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            
        }
    }
}
