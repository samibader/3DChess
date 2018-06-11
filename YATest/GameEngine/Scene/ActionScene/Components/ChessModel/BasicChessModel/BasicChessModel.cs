using Microsoft.Xna.Framework;
using YATest.Utilities;
using YATest.GameLogic;
using Microsoft.Xna.Framework.Graphics;
using YATest.Utilities.CameraUtil;

namespace YATest.GameEngine
{
    class BasicChessModel : ChessModel
    {

        public BasicChessModel(Game game, AbstractPiece logicalPieceRef, ModelProvider modelProvider)
            : base(game, logicalPieceRef, modelProvider)
        {
            createModel();

            maxModelHeight = 1.2f;
            prefferedModelHeight = 0.9f;
        }

        private void createModel()
        {
            if (LogicalPieceRef.player is Player1)
                texture = Game.Content.Load<Texture2D>("Models/Basic/Player1Texture");
            else
                texture = Game.Content.Load<Texture2D>("Models/Basic/Player2Texture");
            model = modelProvider.getModel(logicalPieceRef.name);

        }
    }
}