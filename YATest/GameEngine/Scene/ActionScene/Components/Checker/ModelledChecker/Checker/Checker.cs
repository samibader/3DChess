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
    public enum Tint { Dark, Light, None };

    public enum BorderType { Border, Corner, None };

    abstract class Checker : DrawableGameComponent
    {
        /// <summary>
        /// Thickness of the checker, corresponds to its Y axis
        /// </summary>
        private float thickness;

        public float Thickness
        {
            get { return thickness; }
        }
        /// <summary>
        /// Width of the checker, corresponds to its X axis
        /// </summary>
        private float width;

        public float Width
        {
            get { return width; }
        }
        /// <summary>
        /// Height of the checker, corresponds to its Z axis
        /// </summary>
        private float height;

        public float Height
        {
            get { return height; }
        }

        protected bool isAvailableRoute;

        protected bool isSelected;

        private Matrix world;

        public Matrix World
        {
            get { return world; }
            set { world = value; }
        }

        protected Checker(Game game, float thickness, float width, float height) : base(game)
        {
            this.thickness = thickness;
            this.width = width;
            this.height = height;
        }

        public abstract BoundingBox GetBoundingBox();

        public virtual void Select()
        {
            isSelected = true;
        }

        public virtual void Deselect()
        {
            isSelected = false;
        }

        public abstract void AvailableRoute();

        public abstract void Highlight();

        public abstract void Dehighlight();

        public abstract void Threatning();

        public abstract void Reset();

        //public enum Tint {Dark, Light};

        public bool IsAvailableRoute()
        {
            return isAvailableRoute;
        }

        public bool IsSelected()
        {
            return isSelected;
        }
    }
}
