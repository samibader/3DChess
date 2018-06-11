using Microsoft.Xna.Framework;
using YATest.Utilities;
using YATest.GameLogic;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.GameEngine
{
    abstract class ModelProvider
    {
        protected Model[] models;
        protected Matrix[] modelsMatrices;
        private Game game;

        protected ModelProvider(Game game)
        {
            models = new Model[6];
            modelsMatrices = new Matrix[6];
            this.game = game;
        }

        protected void loadModels(string relativePath)
        {
            models[(int)ChessNames.Pawn] = game.Content.Load<Model>(relativePath + "/Pawn");
            models[(int)ChessNames.Bishop] = game.Content.Load<Model>(relativePath + "/Bishop");
            models[(int)ChessNames.King] = game.Content.Load<Model>(relativePath + "/King");
            models[(int)ChessNames.Knight] = game.Content.Load<Model>(relativePath + "/Knight");
            models[(int)ChessNames.Rook] = game.Content.Load<Model>(relativePath + "/Rook");
            models[(int)ChessNames.Queen] = game.Content.Load<Model>(relativePath + "/Queen");
        }

        /// <summary>
        /// This function corrects the orientation of the models (stored in modelsMatrices)
        /// </summary>
        protected abstract void adjustOrientation();

        public Model getModel(ChessNames name)
        {
            return models[(int)name];
        }

        public Matrix getModelMatrix(ChessNames name)
        {
            return modelsMatrices[(int)name];
        }
    }
}
