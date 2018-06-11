using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YATest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using YATest.Utilities.CameraUtil;
using YATest.GameLogic;

namespace YATest.GameEngine
{
    class Panel2D : PanelDynamic, IControllable //NEEDS WORKING ON PERFORMANCE ISSUES
    {
        private Sprite[,] chessboard;
        private List<Sprite> pieces;
        private Texture2D boardTexture;
        private Texture2D piecesTexture;
        private Rectangle chessboardRectWhite;
        private Rectangle chessboardRectBlack;
        private Rectangle panelRect;
        private SpriteBatch spriteBatch;
        private Chessboard vChessboard;
        private MouseState curMouseState, oldMouseState;

        private int height = 330;

        public Panel2D(Game game, CompoundGameComponent parent)
            : base(game, parent, "Panels\\SideHandle3", "Panels\\CompletePane3", 330 ) 
        {
            LoadContent();
        }

        protected override void LoadContent()
        {
            boardTexture = Game.Content.Load<Texture2D>("Panels\\BoardTex");
            piecesTexture = Game.Content.Load<Texture2D>("Panels\\PiecesTex");

            chessboardRectWhite = new Rectangle(0 ,0, 18, 18);
            chessboardRectBlack = new Rectangle(19, 0, 18, 18);
            
            chessboard = new Sprite[8,8];
            for(int x=0; x<8; x++)
                for(int y=0; y<8; y++)
                {
                    chessboard[x, y] = new Sprite();
                    chessboard[x, y].Texture = boardTexture;
                    if ((x + y) % 2 == 0)
                        chessboard[x, y].SourceRect = chessboardRectWhite;
                    else
                        chessboard[x, y].SourceRect = chessboardRectBlack;
                }
            pieces = new List<Sprite>();
            panelRect = new Rectangle(Game.GraphicsDevice.Viewport.Width - 18*8, height + 5, 
                                        18 * 8, 18 * 8);

            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            vChessboard = (Chessboard)Game.Services.GetService(typeof(Chessboard));
            curMouseState = oldMouseState = Mouse.GetState();
            base.LoadContent();
        }

        private int curLevel;
        public override void Update(GameTime gameTime)
        {
            if (curLevel % 2 == 0)
                for (int x = 0; x < 8; x++)
                    for (int y = 0; y < 8; y++)
                        if ((x + y) % 2 == 0)
                            chessboard[x, y].SourceRect = chessboardRectWhite;
                        else
                            chessboard[x, y].SourceRect = chessboardRectBlack;
            else
                for (int x = 0; x < 8; x++)
                    for (int y = 0; y < 8; y++)
                        if ((x + y) % 2 == 0)
                            chessboard[x, y].SourceRect = chessboardRectBlack;
                        else
                            chessboard[x, y].SourceRect = chessboardRectWhite;
            
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    chessboard[x, y].Position = new Vector2((Game.GraphicsDevice.Viewport.Width - (18 * 8)) + (x * 18) - 8, height + 3 + (y * 18));

            pieces.Clear();
            //Get all pieces in the current level and add them to the pieces matrix
            for(int x=0; x<8; x++)
                for(int z=0; z<8; z++)
                    if (vChessboard.CheckerHasModel(x, curLevel, z) == true)
                    {
                        AbstractPiece ap = vChessboard.ModelAt(x, curLevel, z).LogicalPieceRef;
                        Sprite s = new Sprite() ;
                        s.SourceRect = getSpriteClip(ap.name, ap.player is Player1);
                        s.Texture = piecesTexture;
                        if(GameManager.getReference(null).isPlayer1Turn() == true)
                            s.Position = new Vector2((Game.GraphicsDevice.Viewport.Width - (18 * 8)) + (8 - x - 1) * 18 - 8, height + 3 + (8 - z - 1) * 18);
                        else
                            s.Position = new Vector2((Game.GraphicsDevice.Viewport.Width - (18 * 8)) + (x * 18) - 8, height + 3 + (z * 18));
                        pieces.Add(s);
                    }
            if (Blocked == false)
            {
                HandleKeyboardInput();
                HandleMouseInput();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if ((isOpened == true || isEnlarged == true) && isMovingRight == false)
            {
                Game.GraphicsDevice.RenderState.DepthBufferEnable = false;
                Game.GraphicsDevice.RenderState.AlphaBlendEnable = true;

                spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                for (int x = 0; x < 8; x++)
                    for (int y = 0; y < 8; y++)
                        chessboard[x, y].Draw(spriteBatch, 255);

                spriteBatch.End();
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                for (int i = 0; i < pieces.Count; i++)
                    pieces[i].Draw(spriteBatch, 255);
                spriteBatch.End();

                Game.GraphicsDevice.RenderState.DepthBufferEnable = true;
                Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            }
        }

        private Rectangle getSpriteClip(Utilities.ChessNames pieceName, bool isPlayer1)
        {
            return new Rectangle(((int)pieceName) * 17,(isPlayer1==true)?0:18,17,17);
        }

        #region IControllable Members

        private bool panelIsFocused = false;
        private bool controlsAreBlocked = false;

        private bool viewingIsActive = false;
        public void HandleMouseInput()
        {
            if ((isOpened == true || isEnlarged == true) && isMovingRight == false)
            {
                curMouseState = Mouse.GetState();

                if (panelRect.Contains(curMouseState.X, curMouseState.Y) == true)
                    if (curMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
                        panelIsFocused = !panelIsFocused;

                if (panelIsFocused == true)
                {
                    ActionScene ags = (ActionScene)Game.Services.GetService(typeof(ActionScene));
                    ags.blockControls();
                    this.controlsAreBlocked = false;
                    if (curMouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue - 100)
                        curLevel--;
                    if (curMouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue + 100)
                        curLevel++;
                    HandlemouseWheel();
                }
                else
                {
                        //reset controls
                        curLevel = vChessboard.CurrentLevel;
                        ActionScene ags = (ActionScene)Game.Services.GetService(typeof(ActionScene));
                        ags.unblockControls();
                        for (short y = 0; y < 8; y++)
                            vChessboard.SetPlaneVisibility(y, true);
                }
                oldMouseState = curMouseState;
            }
        }

        private void HandlemouseWheel()
        {
            if (curLevel > 7)
            {
                curLevel = 7;
                return;
            }
            if (curLevel < 0)
            {
                curLevel = 0;
                return;
            }
            for (short y = 0; y < 8; y++)
                vChessboard.SetPlaneVisibility(y, false);
            vChessboard.SetPlaneVisibility((short)curLevel, true);
        }

        public bool Blocked
        {
            get
            {
                return controlsAreBlocked;
            }
            set
            {
                controlsAreBlocked = value;
            }
        }

        #endregion
    }
}
