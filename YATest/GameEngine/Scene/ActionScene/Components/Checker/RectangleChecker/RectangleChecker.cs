using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using YATest.Utilities;
using YATest.Utilities.CameraUtil;
using YATest.Utilities.Effects;
using System;

namespace YATest.GameEngine
{
    class RectangleChecker : Checker
    {
        private Tint colorTint;
        private VertexPositionNormalTexture[] checkerVertices;
        private VertexBuffer vertexBuffer;
        private VertexDeclaration vertexDeclaration;

        private AbstractEffect checkerEffect;
        public AbstractEffect CheckerEffect
        {
            get { return checkerEffect; }
            set { checkerEffect = value; }
        }

        private GameCamera cam;


        TexturesLibrary textureLibrary;

        public RectangleChecker(Game game, Tint colorTint)
            : base(game, 0.2f, 1.0f, 1.0f)
        {
            this.colorTint = colorTint;
            cam = (GameCamera)game.Services.GetService(typeof(BasicCamera));
            textureLibrary = (TexturesLibrary)game.Services.GetService(typeof(TexturesLibrary));
            checkerEffect = new PhongEffect(game, colorTint);
            CheckerEffect.setOtherParams();
            LoadContent();
            generateChecker();
        }

        protected override void LoadContent()
        {

            base.LoadContent();
        }

        private void generateChecker()
        {
            checkerVertices = new VertexPositionNormalTexture[36];

            checkerVertices[0] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, Height), Vector3.Down, new Vector2(1.0f, 1.0f));
            checkerVertices[1] = new VertexPositionNormalTexture(new Vector3(0.0f, Thickness, 0.0f), Vector3.Down, new Vector2(0.0f, 0.0f));
            checkerVertices[2] = new VertexPositionNormalTexture(new Vector3(0.0f, Thickness, Height), Vector3.Down, new Vector2(0.0f, 1.0f));

            checkerVertices[3] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, Height), Vector3.Down, new Vector2(1.0f, 1.0f));
            checkerVertices[4] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, 0.0f), Vector3.Down, new Vector2(1.0f, 0.0f));
            checkerVertices[5] = new VertexPositionNormalTexture(new Vector3(0.0f, Thickness, 0.0f), Vector3.Down, new Vector2(0.0f, 0.0f));

            checkerVertices[6] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Up, new Vector2(1.0f, 1.0f));
            checkerVertices[7] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Up, new Vector2(0.0f, 0.0f));
            checkerVertices[8] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, Height), Vector3.Up, new Vector2(0.0f, 1.0f));

            checkerVertices[9] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Up, new Vector2(1.0f, 1.0f));
            checkerVertices[10] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Up, new Vector2(1.0f, 0.0f));
            checkerVertices[11] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Up, new Vector2(0.0f, 0.0f));

            checkerVertices[12] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Forward, new Vector2(1.0f, 1.0f));
            checkerVertices[13] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, Height), Vector3.Forward, new Vector2(1.0f, 0.0f));
            checkerVertices[14] = new VertexPositionNormalTexture(new Vector3(0.0f, Thickness, Height), Vector3.Forward, new Vector2(0.0f, 0.0f));

            checkerVertices[15] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Forward, new Vector2(1.0f, 1.0f));
            checkerVertices[16] = new VertexPositionNormalTexture(new Vector3(0.0f, Thickness, Height), Vector3.Forward, new Vector2(0.0f, 0.0f));
            checkerVertices[17] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, Height), Vector3.Forward, new Vector2(0.0f, 1.0f));

            checkerVertices[18] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Backward, new Vector2(1.0f, 1.0f));
            checkerVertices[19] = new VertexPositionNormalTexture(new Vector3(0.0f, Thickness, 0.0f), Vector3.Backward, new Vector2(0.0f, 0.0f));
            checkerVertices[20] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, 0.0f), Vector3.Backward, new Vector2(1.0f, 0.0f));

            checkerVertices[21] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Backward, new Vector2(1.0f, 1.0f));
            checkerVertices[22] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Backward, new Vector2(0.0f, 1.0f));
            checkerVertices[23] = new VertexPositionNormalTexture(new Vector3(0.0f, Thickness, 0.0f), Vector3.Backward, new Vector2(0.0f, 0.0f));

            checkerVertices[24] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, Height), Vector3.Right, new Vector2(1.0f, 1.0f));
            checkerVertices[25] = new VertexPositionNormalTexture(new Vector3(0.0f, Thickness, Height), Vector3.Right, new Vector2(1.0f, 0.0f));
            checkerVertices[26] = new VertexPositionNormalTexture(new Vector3(0.0f, Thickness, 0.0f), Vector3.Right, new Vector2(0.0f, 0.0f));

            checkerVertices[27] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, Height), Vector3.Right, new Vector2(1.0f, 1.0f));
            checkerVertices[28] = new VertexPositionNormalTexture(new Vector3(0.0f, Thickness, 0.0f), Vector3.Right, new Vector2(0.0f, 0.0f));
            checkerVertices[29] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Right, new Vector2(0.0f, 1.0f));



            checkerVertices[30] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Left, new Vector2(1.0f, 1.0f));
            checkerVertices[31] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, 0.0f), Vector3.Left, new Vector2(0.0f, 0.0f));
            checkerVertices[32] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, Height), Vector3.Left, new Vector2(0.0f, 1.0f));

            checkerVertices[33] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Left, new Vector2(1.0f, 1.0f));
            checkerVertices[34] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Left, new Vector2(1.0f, 0.0f));
            checkerVertices[35] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, 0.0f), Vector3.Left, new Vector2(0.0f, 0.0f));

            vertexBuffer = new VertexBuffer(Game.GraphicsDevice, 36 * VertexPositionNormalTexture.SizeInBytes, BufferUsage.WriteOnly);
            for (short i = 0; i < 36; i++)
                checkerVertices[i].Normal.Normalize();

            vertexBuffer.SetData<VertexPositionNormalTexture>(checkerVertices);
            vertexDeclaration = new VertexDeclaration(Game.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

        }

        public override BoundingBox GetBoundingBox()
        {
            Vector3 lowerLeft = Vector3.Transform(checkerVertices[7].Position, World);
            Vector3 upperRight = Vector3.Transform(checkerVertices[0].Position, World);
            return new BoundingBox(lowerLeft, upperRight);
        }

        public override void Select()
        {
            CheckerEffect.select();
            base.Select();
        }

        public override void Highlight()
        {
            CheckerEffect.highlight();
        }

        public override void Dehighlight()
        {
            CheckerEffect.deHighlight();
        }

        public override void AvailableRoute()
        {
            CheckerEffect.availableRoute();
        }

        public override void Reset()
        {
            CheckerEffect.reset();
        }

        public override void Update(GameTime gameTime)
        {
            //KeyboardState keyState = Keyboard.GetState();
            //Vector3 temp = checkerEffect.Effect.Parameters["Lamp0Pos"].GetValueVector3();
            //if (keyState.IsKeyDown(Keys.I))
            //    CheckerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X + 0.1f, temp.Y, temp.Z));
            //if (keyState.IsKeyDown(Keys.U))
            //    CheckerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X - 0.1f, temp.Y, temp.Z));
            //if (keyState.IsKeyDown(Keys.K))
            //    CheckerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y + 0.1f, temp.Z));
            //if (keyState.IsKeyDown(Keys.J))
            //    CheckerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y - 0.1f, temp.Z));
            //if (keyState.IsKeyDown(Keys.M))
            //    CheckerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y, temp.Z + 0.1f));
            //if (keyState.IsKeyDown(Keys.N))
            //    CheckerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y, temp.Z - 0.1f));
            //if (keyState.IsKeyDown(Keys.P))
            //    Console.WriteLine(CheckerEffect.Effect.Parameters["Lamp0Pos"].GetValueVector3().ToString());
            //base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.RenderState.CullMode = CullMode.None;

            CheckerEffect.setProjectionMatrix(cam.ProjectionMatrix);
            CheckerEffect.setViewInverseMatrix(Matrix.Invert(cam.ViewMatrix));
            CheckerEffect.setViewMatrix(cam.ViewMatrix);
            CheckerEffect.setWorldInverseTransposeMatrix(Matrix.Transpose(Matrix.Invert(World * cam.WorldMatrix)));
            CheckerEffect.setWorldMatrix(World * cam.WorldMatrix);
            CheckerEffect.setWorldViewProjectionMatrix(World * cam.WorldMatrix * cam.ViewMatrix * cam.ProjectionMatrix);
            checkerEffect.setWorldViewMatrix(World * cam.WorldMatrix * cam.ViewMatrix);

            Game.GraphicsDevice.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
            Game.GraphicsDevice.VertexDeclaration = vertexDeclaration;
            CheckerEffect.Effect.Begin();
            foreach (EffectPass pass in CheckerEffect.Effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                Game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);
                pass.End();
            }
            CheckerEffect.Effect.End();

            base.Draw(gameTime);
        }

        public override void Threatning()
        {
            checkerEffect.threatning();
        }


    }
}
