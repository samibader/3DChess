using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using YATest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using YATest.Utilities.CameraUtil;
using System.Collections.Generic;
using YATest.GameLogic;
using System;

namespace YATest.GameEngine
{
    class Chessboard : CompoundGameComponent , IControllable
    {
        #region Data Members
        private Checker[, ,] checkers;
        private ChessModel[] models;
        private AbstractBorderTexture[,] chessboardTexture;
        private KeyboardState curKeyState, oldKeyState;
        private MouseState curMouseState, oldMouseState;
        private bool controlsAreBlocked;

        private float spacingValue;

        //used for performance enhancment purposes
        private Checker curSelectedQuad;
        private Position curSelectedQuadPos;
        private Checker curHoveredQuad;
        private Position curHoveredQuadPos;
        private Position oldHoveredQuadPos;
        private List<Position> availableMoves;
        private List<Position> threatningMoves;
        private GameCamera cam;
        private Vector3 moveFrom;
        private Vector3 moveTo;

        private int currentLevel;

        public int CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }

        #endregion

        public Chessboard(Game game, CompoundGameComponent parent, Checker[, ,] checkers, ChessModel[] models, AbstractBorderTexture[,] chessboardTexture)
            : base(game, parent)
        {
            cam = (GameCamera)game.Services.GetService(typeof(BasicCamera));
            this.checkers = checkers;
            this.models = models;
            this.chessboardTexture = chessboardTexture;
            spacingValue = models[0].PrefferedModelHeight;
            currentLevel = 8;
            LoadContent();
            //SETTING ALPHA
            //Game.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha; // source rgb * source alpha
            //Game.GraphicsDevice.RenderState.AlphaSourceBlend = Blend.One; // don't modify source alpha
            //Game.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha; // dest rgb * (255 - source alpha)
            //Game.GraphicsDevice.RenderState.AlphaDestinationBlend = Blend.InverseSourceAlpha; // dest alpha * (255 - source alpha)
            //Game.GraphicsDevice.RenderState.BlendFunction = BlendFunction.Add; // add source and dest results
        }

        #region Accessing chessboard items methods

        public Checker CheckerAt(Utilities.Position pos)
        {
            return checkers[pos.x, pos.y, pos.z];
        }

        public Checker CheckerAt(int x, int y, int z)
        {
            return checkers[x, y, z];
        }

        public ChessModel ModelAt(Utilities.Position pos)
        {
            //the method passes through all models and check their position
            for (int i = 0; i < 32; i++)
                if (models[i].Position() == pos && models[i].IsCaptured() == false) //avoid shadow-worrier bug
                    return models[i];
            return null;
        }

        public ChessModel ModelAt(int x, int y, int z)
        {
            return ModelAt(new Utilities.Position(x, y, z));
        }

        public AbstractPiece LogicalModelAt(int x, int y, int z)
        {
            Position pos = new Position(x, y, z);
            for (int i = 0; i < 32; i++)
                if (models[i].Position() == pos) //avoid shadow-worrier bug
                    return models[i].LogicalPieceRef;
            return null;
        }

        #endregion

        #region Plane Visibility Tweaks

        private void activatedPlanesWithModelsOnly()
        {
            for (short y = 0; y < 8; y++)
                if (PlaneConstainsModels(y) == false)
                    SetPlaneVisibility(y, false);
        }

        private void activateAllPlanes()
        {
            for (short y = 0; y < 8; y++)
                if (PlaneConstainsModels(y) == false)
                    SetPlaneVisibility(y, true);
        }

        public bool PlaneConstainsModels(short planeIndex)
        {
            for (int x = 0; x < 8; x++)
                for (int z = 0; z < 8; z++)
                    if (CheckerHasModel(x, planeIndex, z) == true)
                        return true;
            return false;
        }

        public void SetPlaneVisibility(short planeIndex, bool visibility)
        {
            for (int x = 0; x < 8; x++)
                for (int z = 0; z < 8; z++)
                {
                    checkers[x, planeIndex, z].Visible = visibility;
                    if (CheckerHasModel(x, planeIndex, z) == true)
                    {
                        ModelAt(x, planeIndex, z).Visible = visibility;
                        ModelAt(x, planeIndex, z).FireParticles.Visible = visibility;
                    }
                }
            for (int i = 0; i < 4; i++)
                chessboardTexture[planeIndex, i].Visible = visibility;

        }

        private void ChangeSpacing(float increment)
        {
            if (increment > 0) //increasing
                if (spacingValue + increment > models[0].MaxModelHeight+1.2f)
                    return;
            if (increment < 0)
                if (spacingValue + increment < models[0].PrefferedModelHeight)
                    return;

            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    for (int z = 0; z < 8; z++)
                        checkers[x, y, z].World *= Matrix.CreateTranslation(0.0f, y * increment, 0.0f);
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 4; j++)
                    //chessboardTexture[i,j].World *= Matrix.CreateTranslation(0.0f, (i-4) * increment, 0.0f);
                    chessboardTexture[i, j].World *= Matrix.CreateTranslation(0.0f, i * increment, 0.0f);

            spacingValue += increment;
        }

        private bool isOpaque = false;
        private void ToggleTransparency()
        {
            isOpaque = !isOpaque;
            if (isOpaque == true)
            {

            }
            else
            {
            }

        }

        #endregion

        #region Overriden methods for drawing

        public override void Update(GameTime gameTime)
        {
            if (Blocked == false)
            {
                HandleKeyboardInput();
                HandleMouseInput();
            }
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    for (int z = 0; z < 8; z++)
                        subComponents.Add(checkers[x, y, z]);

            for (int i = 0; i < 32; i++)
                subComponents.Add(models[i]);
            
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 4; j++)
                    subComponents.Add(chessboardTexture[i, j]);

            curKeyState = oldKeyState = Keyboard.GetState();
            curMouseState = oldMouseState = Mouse.GetState();

            availableMoves = new List<Position>();
            threatningMoves = new List<Position>();
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        #endregion

        #region Handling Selection

        public void resetSelectedChecker()
        {
            if (curSelectedQuad != null)
            {
                curSelectedQuad.Reset();
                for (int i = 0; i < availableMoves.Count; i++)
                    CheckerAt(availableMoves[i]).Reset();
                availableMoves.Clear();
                curSelectedQuad = null;
                curSelectedQuadPos = Position.InvalidPosition;
            }
        }

        public void resetHoveredStuff()
        {
            if (curHoveredQuad != null)
            {
                curHoveredQuad.Dehighlight();
                curHoveredQuad = null;
                curHoveredQuadPos = Position.InvalidPosition;
            }
        }

        private bool quadBelongsToAvailableRoute(Position quadPos)
        {
            for (int i = 0; i < availableMoves.Count; i++)
                if (availableMoves[i] == quadPos)
                    return true;
            return false;
        }

        private Position GetQuadAtMousePosition()
        {
            Position closestQuad = Position.InvalidPosition;
            Vector3 rayStart = Game.GraphicsDevice.Viewport.Unproject(
                new Vector3(curMouseState.X, curMouseState.Y, 0.0f),
                //Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), Game.GraphicsDevice.Viewport.AspectRatio, 1.0f, 100.0f),
                cam.ProjectionMatrix,cam.ViewMatrix,
                //Matrix.CreateLookAt(new Vector3(0.0f, 9.0f, 9.0f), Vector3.Zero, Vector3.Up),
                cam.WorldMatrix);
            Vector3 rayEnd = Game.GraphicsDevice.Viewport.Unproject(
                new Vector3(curMouseState.X, curMouseState.Y, 1.0f),
                cam.ProjectionMatrix, cam.ViewMatrix,
                //Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), Game.GraphicsDevice.Viewport.AspectRatio, 1.0f, 100.0f),
                //Matrix.CreateLookAt(new Vector3(0.0f, 9.0f, 9.0f), Vector3.Zero, Vector3.Up),
                cam.WorldMatrix);
            Ray ray = new Ray(rayStart, Vector3.Normalize(rayEnd - rayStart));
            float? closestDistance = float.PositiveInfinity;
            float? distance;
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    for (int z = 0; z < 8; z++)
                    {
                        if (checkers[x, y, z].Visible == true)
                        {
                            distance = ray.Intersects(checkers[x, y, z].GetBoundingBox());
                            if (distance < closestDistance)
                            {
                                closestQuad.x = x;
                                closestQuad.y = y;
                                closestQuad.z = z;
                                currentLevel = y;
                                closestDistance = distance;
                            }
                        }
                    }
            return closestQuad;
        }

        private void HandleHovering()
        {
            Position closestQuad = GetQuadAtMousePosition();
            if (closestQuad == Position.InvalidPosition)
            {
                for (int i = 0; i < threatningMoves.Count; i++)
                    CheckerAt(threatningMoves[i]).Reset();
                return; //i.e. we have no contact
            }
            else
            {
                curHoveredQuad = CheckerAt(closestQuad);
                curHoveredQuadPos = (Position)closestQuad.Clone();

                //Handle threatning moves here
                if (quadBelongsToAvailableRoute(curHoveredQuadPos) == true)
                {
                    if (curHoveredQuadPos != oldHoveredQuadPos)
                    {
                        for (int i = 0; i < threatningMoves.Count; i++)
                            CheckerAt(threatningMoves[i]).Reset();
                        threatningMoves.Clear();
                        threatningMoves = GameLogic.Chessboard.getReference().getThreatningPiece(curHoveredQuadPos, GameManager.getReference(null).curPlayer());
                    }
                    for (int i = 0; i < threatningMoves.Count; i++)
                        CheckerAt(threatningMoves[i]).Threatning();
                }
                else
                {
                    for (int i = 0; i < threatningMoves.Count; i++)
                        CheckerAt(threatningMoves[i]).Reset();
                    threatningMoves.Clear();
                }

                curHoveredQuad.Highlight();
                oldHoveredQuadPos = curHoveredQuadPos;
            }
        }

        private void HandleSelection()
        {
            Console.WriteLine("KIC" + KingInCheck(new Player1()));
            Position closestQuad = GetQuadAtMousePosition();
            if (closestQuad == Position.InvalidPosition)
                return; //i.e. we have no contact
            else
            {
                if (CheckerHasModel(closestQuad) == true)
                    if (curSelectedQuad == null)
                    {
                        if (ModelAt(closestQuad).LogicalPieceRef.player != GameManager.getReference(null).curPlayer()) //We're trying to select a piece that is not ours!
                            return;
                        curSelectedQuad = CheckerAt(closestQuad);
                        curSelectedQuadPos = closestQuad;
                        curSelectedQuad.Select();
                        availableMoves = ModelAt(closestQuad).LogicalPieceRef.getAvailableMoves();
                        for (int i = 0; i < availableMoves.Count; i++)
                            CheckerAt(availableMoves[i]).AvailableRoute();
                        //System.Console.WriteLine("Piece Selected : " + curSelectedQuadPos.ToString());
                    }
                    else
                    {
                        if (curSelectedQuadPos == closestQuad)
                        {
                            curSelectedQuad.Deselect();
                            resetSelectedChecker();
                            //System.Console.WriteLine("Piece Deselected : " + curSelectedQuadPos.ToString());
                        }
                        else //it's not the previously selected quad
                        {
                            if (ModelAt(closestQuad).LogicalPieceRef.player == YATest.Utilities.GameManager.getReference(null).curPlayer()) //the same player
                            {
                                //just deselect and select that player
                                curSelectedQuad.Deselect();
                                resetSelectedChecker();
                                HandleSelection(); //recursive call to handle selection of the new piece
                            }
                            else //there is a model on this quad, it doesn't belong to us (because it's on the available route), capture it
                            {
                                if (quadBelongsToAvailableRoute(closestQuad) == true)
                                {
                                    ModelAt(closestQuad).LogicalPieceRef.IsCaptured = true;

                                    // Nassouh has made changes
                                    ModelAt(curSelectedQuadPos).MoveTo(closestQuad);
                                    ModelAt(curSelectedQuadPos).LogicalPieceRef.isSelected = true;

                                    // ModelAt(curSelectedQuadPos).LogicalPieceRef.moveTo(closestQuad);
                                    // end of changes


                                    curSelectedQuad.Deselect();
                                    resetSelectedChecker();
                                    //toggle players
                                    //GameManager.getReference(null).toggleTurn(false);
                                }
                            }
                        }
                    }
                else //no selected model in this new selection
                {
                    if (quadBelongsToAvailableRoute(closestQuad) == true) //check if the quad belong to a available route
                    {
                        if (CheckerHasModel(closestQuad) == false) //if the checker is empty, then move to it
                        {
                            //toggle players
                            moveFrom = new Vector3(curSelectedQuadPos.x, curSelectedQuadPos.y, curSelectedQuadPos.z);

                            // Nassouh has made changes
                            ModelAt(curSelectedQuadPos).MoveTo(closestQuad);
                            ModelAt(curSelectedQuadPos).LogicalPieceRef.isSelected = true;
                            // moveTo = new Vector3(closestQuad.x, closestQuad.y, closestQuad.z);
                            // ModelAt(curSelectedQuadPos).LogicalPieceRef.moveTo(closestQuad);
                            // end of changes
                            curSelectedQuad.Deselect();
                            resetSelectedChecker();
                           // GameManager.getReference(null).toggleTurn(false);
                        }
                    }
                    else //the quad doesn't belong to available route, Just deselect
                    {
                        if(curSelectedQuad != null)
                            curSelectedQuad.Deselect();
                        resetSelectedChecker();
                    }
                }
            }
        }

        #endregion

        #region IControllable Members

        private bool planeVisibilityToggle = false;
        public void HandleKeyboardInput()
        {
            curKeyState = Keyboard.GetState();

            BasicCamera bc = (BasicCamera)Game.Services.GetService(typeof(BasicCamera));
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) == true)
                bc.Blocked = true;
            else
                bc.Blocked = false;

            if (curKeyState.IsKeyDown(Keys.B) && oldKeyState.IsKeyUp(Keys.B))
            {
                planeVisibilityToggle = !planeVisibilityToggle;
                if (planeVisibilityToggle == true)
                    activatedPlanesWithModelsOnly();
                else
                    activateAllPlanes();
            }

            if (curKeyState.IsKeyDown(Keys.Z) && oldKeyState.IsKeyUp(Keys.Z))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) == true)
                    History.getReference().undo();
            }
            if (curKeyState.IsKeyDown(Keys.Y) && oldKeyState.IsKeyUp(Keys.Y))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) == true)
                    History.getReference().redo();
            }
            oldKeyState = curKeyState;
        }

        private void ToggleCheckersVisibility()
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    for (int z = 0; z < 8; z++)
                    {
                        checkers[x, y, z].Visible = !checkers[x, y, z].Visible;
                        checkers[x, y, z].Enabled = !checkers[x, y, z].Enabled;
                    }
        }

        private void ToggleModelsVisibility()
        {
            for (int x = 0; x < 32; x++)
            {
                models[x].Visible = !models[x].Visible;
                models[x].Enabled = !models[x].Enabled;
            }
        }

        public void HandleMouseInput()
        {

            resetHoveredStuff();
            curMouseState = Mouse.GetState();
            if (curMouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue)
                ChangeSpacing(-0.1f);
            if (curMouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue)
                ChangeSpacing(0.1f);
            if (curMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                HandleSelection();
            if (curMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Released)
                HandleHovering();   
            oldMouseState = curMouseState;
        }

        public bool Blocked
        {
            get{ return controlsAreBlocked; }
            set { controlsAreBlocked = value;}
        }

        #endregion

        #region CheckerHasModel

        public bool CheckerHasModel(Utilities.Position checkerPosition)
        {
            for (int i = 0; i < 32; i++)
                if (models[i].Position() == checkerPosition)
                    if (models[i].IsCaptured() == false)
                        return true;
            return false;
        }

        public bool CheckerHasModel(int x, int y, int z)
        {
            return CheckerHasModel(new Utilities.Position(x, y, z));
        }
        #endregion

        public int GetCapturedPieces(AbstractPlayer p)
        {
            int result = -1;
            foreach (ChessModel m in models)
            {
                if (m.LogicalPieceRef.player == p && m.IsCaptured())
                {
                    result++;
                }
            }
            return result;

        }

        public bool KingInCheck(AbstractPlayer p)
        {
            Position kingPos;
            if (p is Player1)
                kingPos = models[16].LogicalPieceRef.position;
            else kingPos = models[19].LogicalPieceRef.position;

            for (int i = 0; i < 32; i++)
            {
                if (p.GetType().ToString() != models[i].LogicalPieceRef.player.GetType().ToString())
                {
                    foreach (Position pos in models[i].LogicalPieceRef.getAvailableMoves())
                    {
                        if (pos == kingPos) return true;

                    }
                }

            }
            return false;

        }

    }
}
