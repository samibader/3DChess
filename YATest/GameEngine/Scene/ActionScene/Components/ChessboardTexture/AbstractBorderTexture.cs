using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace YATest.GameEngine
{
    abstract class AbstractBorderTexture : DrawableGameComponent
    {
        private float thickness;

        public float Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }
        /// <summary>
        /// Width of the checker, corresponds to its X axis
        /// </summary>
        /// 

        public abstract BoundingBox GetBoundingBox();

        private float width;

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        private float bwidth;

        public float Bwidth
        {
            get { return bwidth; }
            set { bwidth = value; }
        }
        /// <summary>
        /// Height of the checker, corresponds to its Z axis
        /// </summary>
        private float height;

        public float Height
        {
            get { return height; }

        }

        private Matrix world;

        public Matrix World
        {
            get { return world; }
            set { world = value; }
        }
        protected AbstractBorderTexture(Game game, float thickness, float width, float bwidth, float height)
            : base(game)
        {
            this.thickness = thickness;
            this.width = width;
            this.height = height;
            this.bwidth = bwidth;
        }

        public abstract void Select();
        public abstract void DeSelect();
    }
}
