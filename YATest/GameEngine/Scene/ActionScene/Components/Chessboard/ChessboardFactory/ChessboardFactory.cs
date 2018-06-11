using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace YATest.GameEngine
{
    abstract class ChessboardFactory
    {
        protected Game game;
        protected string gameFile;

        protected CheckerFactory checkerFactory;
        protected ChessModelFactory chessModelFactory;
        protected AbstractBorderTextureFactory chessboardTextureFactory;

        private Checker[, ,] checkers;
        private ChessModel[] models;

        private AbstractBorderTexture[,] chessboardTexture;

        private Vector3 calibrationVector;

        protected GameLogic.Chessboard logicalChessboard;
        protected GameEngine.Chessboard chessboard;

        protected ChessboardFactory(Game game, string gameFile)
        {
            this.game = game;
            this.gameFile = gameFile;

            //Initiate Checkers Matrix
            checkers = new Checker[8, 8, 8];

            //Initiate Models Matrix
            models = new ChessModel[32];

            chessboardTexture = new AbstractBorderTexture[8, 4];
        }

        protected void init()
        {
            //Build Logical Chessboard
            GameLogic.Chessboard.resetReference();
            logicalChessboard = GameLogic.Chessboard.getReference();

            //Read XML file
            GameLogic.ChessboardBuilder.readXML(gameFile);

            //Fill the logical chessboard with data read from XML file
            GameLogic.ChessboardBuilder.fillChessboard();

            //Init chessboard
            chessboard = null;

            //Create the Checkers Matrix
            createCheckersMatrix();

            //Now, create the models, they'll allign themselves (in their Update method)
            createAndAllignModels();

            // Chessboard Texture

            createChessboardTexture();

            //Adjust the visuals for the Checkers Matrix
            adjustVisuals();
        }

        private void createChessboardTexture()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 4; j++)
                    //if (j % 2 == 0)
                    chessboardTexture[i, j] = chessboardTextureFactory.CreateBorderTexture(checkers[0, 0, 0].Thickness, checkers[0, 0, 0].Width, 8 * checkers[0, 0, 0].Width, checkers[0, 0, 0].Height);
            //else
            //    chessboardTexture[i, j] = chessboardTextureFactory.CreateMainChessboardTexture(0.04f, 0.7f,0.7f, 0.7f*9f);

        }


        private void createCheckersMatrix()
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    for (int z = 0; z < 8; z++)
                        if ((x + y + z) % 2 == 0)
                            checkers[x, y, z] = checkerFactory.CreateDarkChecker();
                        else
                            checkers[x, y, z] = checkerFactory.CreateLightChecker();
        }

        private void adjustVisuals()
        {
            //We have to take into account the Effect for the Checker (which is static)
            //as well as the effect of the models.
            //We have to set the viewing and projection matrices of the models and checkers.
            //which must be read from the camera!
            //I think we should register the Camera in the serives, then we can call it
            //to get the right projection and viewing matrices.
            //But: what about world matrix?
            //Perhaps we should only modify it here. or ... in the Update method of this class
            //Since it is inherited from DrawableGameComponent?

            //Let's for a start adject the world matrix of the Checkers.
            float chessboardWidth = checkers[0,0,0].Width * 8;
            float chessboardHeight = checkers[0, 0, 0].Height * 8;
            float chessboardThickness = (checkers[0, 0, 0].Thickness + models[0].PrefferedModelHeight) * 8.0f;
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    for (int z = 0; z < 8; z++)
                        checkers[x, y, z].World = Matrix.CreateTranslation(x * checkers[0, 0, 0].Width - (chessboardWidth / 2.0f), 
                                                                           y * (checkers[0, 0, 0].Thickness + models[0].PrefferedModelHeight) - (chessboardThickness / 2.5f),
                                                                           z * checkers[0, 0, 0].Height - (chessboardHeight / 2.0f));
            //Calibrate models

            for (int i = 0; i < 8; i++)
            {
                chessboardTexture[i, 0].World = (Matrix.CreateTranslation(-1 * checkers[0, 0, 0].Width - (chessboardWidth / 2.0f),
                                                                       i * (checkers[0, 0, 0].Thickness + models[0].PrefferedModelHeight) - (chessboardThickness / 2.5f),
                                                                       -1 * checkers[0, 0, 0].Height - (chessboardHeight / 2.0f))) * Matrix.CreateRotationY(MathHelper.ToRadians(90.0f));
                chessboardTexture[i, 1].World = (Matrix.CreateTranslation(-1 * checkers[0, 0, 0].Width - (chessboardWidth / 2.0f),
                                                           i * (checkers[0, 0, 0].Thickness + models[0].PrefferedModelHeight) - (chessboardThickness / 2.5f),
                                                           -1 * checkers[0, 0, 0].Height - (chessboardHeight / 2.0f))) * Matrix.CreateRotationY(MathHelper.ToRadians(180.0f));
                chessboardTexture[i, 2].World = (Matrix.CreateTranslation(-1 * checkers[0, 0, 0].Width - (chessboardWidth / 2.0f),
                                                           i * (checkers[0, 0, 0].Thickness + models[0].PrefferedModelHeight) - (chessboardThickness / 2.5f),
                                                           -1 * checkers[0, 0, 0].Height - (chessboardHeight / 2.0f))) * Matrix.CreateRotationY(MathHelper.ToRadians(270.0f));
                chessboardTexture[i, 3].World = (Matrix.CreateTranslation(-1 * checkers[0, 0, 0].Width - (chessboardWidth / 2.0f),
                                                           i * (checkers[0, 0, 0].Thickness + models[0].PrefferedModelHeight) - (chessboardThickness / 2.5f),
                                                           -1 * checkers[0, 0, 0].Height - (chessboardHeight / 2.0f)));
            }


            calibrationVector = new Vector3(checkers[0, 0, 0].Width / 6.0f, 0.0f, checkers[0, 0, 0].Height / 6.0f); //Ops, it's related to models size!
            int modelsCnt = 0;
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    for (int z = 0; z < 8; z++)
                        if (GameLogic.Chessboard.getReference()[x, y /*I guess this should be swapped with z*/, z] != null)
                        {
                            models[modelsCnt].CalibrationMatrix = Matrix.CreateTranslation(calibrationVector);
                            modelsCnt++;
                        }
        }

        private void createAndAllignModels()
        {
            int modelsCnt = 0;
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    for (int z = 0; z < 8; z++)
                        if (GameLogic.Chessboard.getReference()[x, y /*I guess this should be swapped with z*/, z] != null)
                            models[modelsCnt++] = chessModelFactory.CreateChessModel(GameLogic.Chessboard.getReference()[x, y, z]); //here as well
        }

        public Chessboard getChessboard(CompoundGameComponent parent) //factory method h3h3h3
        {
            if (chessboard == null)
            {
                chessboard = new Chessboard(game, parent, checkers, models,chessboardTexture);
                if (game.Services.GetService(typeof(Chessboard)) != null)
                    game.Services.RemoveService(typeof(Chessboard));
                game.Services.AddService(typeof(Chessboard), chessboard);
            }
            return chessboard;
        }
    }
}
