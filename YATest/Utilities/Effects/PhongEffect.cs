using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using YATest.GameLogic;
using Microsoft.Xna.Framework.Graphics;
using YATest.GameEngine;

namespace YATest.Utilities.Effects
{
    class PhongEffect : AbstractEffect
    {
        TexturesLibrary textureLibrary;
        private Texture2D temp;

        #region Constructors
        public PhongEffect(Game game, AbstractPiece piece)
            : base(game, piece)
        {
            Effect = Game.Content.Load<Effect>("Effects/Phong").Clone(game.GraphicsDevice);
            textureLibrary = (TexturesLibrary)game.Services.GetService(typeof(TexturesLibrary));
        }

        public PhongEffect(Game game, Tint colorTint)
            : base(game, colorTint)
        {
            Effect = Game.Content.Load<Effect>("Effects/Phong").Clone(game.GraphicsDevice);
            textureLibrary = (TexturesLibrary)game.Services.GetService(typeof(TexturesLibrary));
        }

        public PhongEffect(Game game, BorderType borderType)
            : base(game, borderType)
        {
            Effect = Game.Content.Load<Effect>("Effects/Phong").Clone(game.GraphicsDevice);
            textureLibrary = (TexturesLibrary)game.Services.GetService(typeof(TexturesLibrary));
        }

        #endregion

        public override void setViewMatrix(Microsoft.Xna.Framework.Matrix viewMatrix)
        {
            Effect.Parameters["ViewIXf"].SetValue(viewMatrix);
        }

        public override void setProjectionMatrix(Microsoft.Xna.Framework.Matrix projectionMatrix)
        {
            return;
        }

        public override void setWorldMatrix(Microsoft.Xna.Framework.Matrix worldMatrix)
        {
            Effect.Parameters["WorldXf"].SetValue(worldMatrix);
        }

        public override void setWorldViewProjectionMatrix(Microsoft.Xna.Framework.Matrix worldViewProjectionMatrix)
        {
            Effect.Parameters["WvpXf"].SetValue(worldViewProjectionMatrix);
        }

        public override void setWorldInverseTransposeMatrix(Microsoft.Xna.Framework.Matrix worldInverseTransposeMatrix)
        {
            Effect.Parameters["WorldITXf"].SetValue(worldInverseTransposeMatrix);
        }

        public override void setViewInverseMatrix(Microsoft.Xna.Framework.Matrix viewInverseMatrix)
        {
            return;
        }

        public override void setOtherParams()
        {
            Effect.Parameters["EnvTexture"].SetValue(textureLibrary.PhongEnvTexture);
            if (Piece != null)
            {
                if (Piece.player is Player1)
                {
                    Effect.Parameters["AmbiColor"].SetValue(new Vector3(0.3f, 0.3f, 0.3f));
                    Effect.Parameters["ColorTexture"].SetValue(Game.Content.Load<Texture2D>("Textures/gold111"));
                }
                else if (Piece.player is Player2)
                {
                    Effect.Parameters["AmbiColor"].SetValue(new Vector3(0.3f, 0.3f, 0.3f));
                    Effect.Parameters["ColorTexture"].SetValue(Game.Content.Load<Texture2D>("Textures/sil"));
                }
            }
            else if (ColorTint != Tint.None)
            {
                if (ColorTint == Tint.Dark)
                {
                    Effect.Parameters["ColorTexture"].SetValue(textureLibrary.PhongColorTextureDark);
                }
                else if (ColorTint == Tint.Light)
                {
                    Effect.Parameters["ColorTexture"].SetValue(textureLibrary.PhongColorTextureLight);
                }
            }
            else if (BorderType != BorderType.None)
            {
                if (BorderType == BorderType.Border)
                {
                    Effect.Parameters["ColorTexture"].SetValue(textureLibrary.PhongColorTextureBorder);
                }
                else
                {
                    Effect.Parameters["ColorTexture"].SetValue(textureLibrary.PhongColorTextureCorner);
                }
            }


            Effect.Parameters["NormalTexture"].SetValue(textureLibrary.PhongNormalTexture);
        }

        public override void highlight()
        {
            temp = Effect.Parameters["ColorTexture"].GetValueTexture2D();
            Effect.Parameters["ColorTexture"].SetValue(textureLibrary.PhongHighLighted);
        }

        public override void deHighlight()
        {
            Effect.Parameters["ColorTexture"].SetValue(temp);
        }

        public override void select()
        {
            Effect.Parameters["ColorTexture"].SetValue(textureLibrary.PhongSelected);
        }

        public override void reset()
        {
            if (ColorTint == Tint.Light)
                Effect.Parameters["ColorTexture"].SetValue(textureLibrary.PhongColorTextureLight);
            else
                Effect.Parameters["ColorTexture"].SetValue(textureLibrary.PhongColorTextureDark);
        }

        public override void availableRoute()
        {
            Effect.Parameters["ColorTexture"].SetValue(textureLibrary.PhongAvailable);
        }


        public override void setWorldViewMatrix(Matrix worldViewMatrix)
        {
            return;
        }

        public override void setLightPosition(Vector3 pos)
        {
            Effect.Parameters["Lamp0Pos"].SetValue(pos);
        }

        public override void threatning()
        {
            Effect.Parameters["ColorTexture"].SetValue(textureLibrary.PhongThreatened);
        }
    }
}
