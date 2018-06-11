using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using YATest.GameEngine;
using YATest.GameLogic;
using YATest.Utilities.CameraUtil;

namespace YATest.Utilities.Effects
{
    abstract class AbstractEffect : DrawableGameComponent
    {
        // Constructor


        public AbstractEffect(Game game, Tint tint)
            : base(game)
        {
            colorTint = tint;
        }

        public AbstractEffect(Game game, AbstractPiece piece)
            : base(game)
        {
            this.piece = piece;
        }

        public AbstractEffect(Game game, BorderType borderType)
            : base(game)
        {
            this.borderType = borderType;
        }

        // Main Effect Member
        private Effect effect;
        private Tint colorTint = Tint.None;
        private BorderType borderType = BorderType.None;
        private AbstractPiece piece;

        // Main Effect Methods
        public abstract void setViewMatrix(Matrix viewMatrix);
        public abstract void setProjectionMatrix(Matrix projectionMatrix);
        public abstract void setWorldMatrix(Matrix worldMatrix);
        public abstract void setWorldViewProjectionMatrix(Matrix worldViewProjectionMatrix);
        public abstract void setWorldInverseTransposeMatrix(Matrix worldInverseTransposeMatrix);
        public abstract void setViewInverseMatrix(Matrix viewInverseMatrix);
        public abstract void setOtherParams();
        public abstract void setWorldViewMatrix(Matrix worldViewMatrix);

        public abstract void highlight();
        public abstract void deHighlight();
        public abstract void select();
        public abstract void reset();
        public abstract void availableRoute();
        public abstract void threatning();
        public abstract void setLightPosition(Vector3 pos);


        // Main properties
        public Effect Effect
        {
            get { return effect; }
            set { effect = value; }
        }

        public Tint ColorTint
        {
            get { return colorTint; }
            set { colorTint = value; }
        }
        public AbstractPiece Piece
        {
            get { return piece; }
            set { piece = value; }
        }

        public BorderType BorderType
        {
            get { return borderType; }
            set { borderType = value; }
        }
    }
}
