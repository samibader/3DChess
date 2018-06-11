using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.Utilities
{
    class Sprite
    {
        private Texture2D texture;
        private Rectangle sourceRect;
        private float rotation = 0.0f;
        private float scale = 1.0f;
        private Vector2 position;

        #region Properties
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch, byte transparency)
        {
            spriteBatch.Draw(texture, position, sourceRect, new Color(255, 255, 255, transparency), rotation, new Vector2(0, 0), scale, SpriteEffects.None, 0.0f);
        }
    }
}
