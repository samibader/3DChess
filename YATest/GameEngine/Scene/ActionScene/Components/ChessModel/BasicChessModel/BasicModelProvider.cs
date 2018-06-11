using Microsoft.Xna.Framework;
using YATest.Utilities;
using YATest.GameLogic;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.GameEngine
{
    class BasicModelProvider : ModelProvider
    {
        public BasicModelProvider(Game game)
            : base(game)
        {
            loadModels("Models/Basic");
            adjustOrientation();
        }
        protected override void adjustOrientation()
        {
            modelsMatrices[(int)ChessNames.Pawn] = Matrix.CreateScale(3.5f, 4.5f, 3.5f) * Matrix.CreateTranslation(0.25f, 0.3f, 0.24f);
            modelsMatrices[(int)ChessNames.Rook] = Matrix.CreateScale(3.5f, 4.5f, 4.0f) * Matrix.CreateTranslation(0.25f, 0.43f, 0.27f);
            modelsMatrices[(int)ChessNames.King] = Matrix.CreateScale(0.0025f, 0.0025f, 0.002f) * Matrix.CreateRotationX(MathHelper.ToRadians(-90.0f)) * Matrix.CreateTranslation(0.25f, 0.68f, 0.25f);
            modelsMatrices[(int)ChessNames.Bishop] = Matrix.CreateScale(2.5f, 2.5f, 3.0f) * Matrix.CreateRotationX(MathHelper.ToRadians(-90.0f)) * Matrix.CreateTranslation(0.2f, 0.7f, 0.3f);
            modelsMatrices[(int)ChessNames.Knight] = Matrix.CreateScale(0.015f, 0.015f, 0.015f) * Matrix.CreateRotationY(MathHelper.ToRadians(45.0f)) * Matrix.CreateTranslation(0.26f, 0.45f, 0.28f);
            modelsMatrices[(int)ChessNames.Queen] = Matrix.CreateScale(2.5f, 2.5f, 3.0f) * Matrix.CreateTranslation(0.34f, 0.7f, 0.25f);
        } //* Matrix.CreateRotationX(1.57f)
    }
}
