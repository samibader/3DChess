using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using YATest.Utilities.CameraUtil;
using Microsoft.Xna.Framework;
using YATest.Utilities.Effects;
using Microsoft.Xna.Framework.Input;

namespace YATest.GameEngine
{
    class MainBorderTexture : AbstractBorderTexture
    {

        private VertexPositionNormalTexture[] cornerVertices;
        private VertexPositionNormalTexture[] borderVertices;
        private VertexBuffer cornerVertexBuffer;
        private VertexBuffer borderVertexBuffer;
        private int[] indices;
        private int[] indices2;
        private IndexBuffer indexBuffer;
        private IndexBuffer indexBuffer2;
        private VertexDeclaration cornerVertexDeclaration;
        private VertexDeclaration borderVertexDeclaration;
        private GameCamera cam;
        private float amendedThickness;
        public AbstractEffect BorderEffect
        {
            get { return borderEffect; }
            set { borderEffect = value; }
        }

        private AbstractEffect borderEffect; //objects shall change the world matrix only!
        public AbstractEffect CornerEffect
        {
            get { return cornerEffect; }
            set { cornerEffect = value; }
        }

        private AbstractEffect cornerEffect; //objects shall change the world matrix only!
        private Texture2D texture;

        public MainBorderTexture(Game game, float thickness, float width, float bwidth, float height)
            : base(game, thickness, width, bwidth, height)
        {

            cam = (GameCamera)game.Services.GetService(typeof(BasicCamera));
            //borderEffect = new ReliefMappingEffect(game, Tint.Light);
            //cornerEffect = new ReliefMappingEffect(game, Tint.Light);
            borderEffect = new PhongEffect(game, BorderType.Border);
            cornerEffect = new PhongEffect(game, BorderType.Corner);
            LoadContent();

            generateChessboardTexture();
            //generateChecker();
        }

        protected override void LoadContent()
        {

            borderEffect.setOtherParams();

            cornerEffect.setOtherParams();



            base.LoadContent();
        }

        public override void Select()
        {
            cornerEffect.select();
        }

        public override void DeSelect()
        {
            cornerEffect.reset();
        }

        public override BoundingBox GetBoundingBox()
        {
            Vector3 lowerLeft = Vector3.Transform(cornerVertices[7].Position, World);
            Vector3 upperRight = Vector3.Transform(cornerVertices[0].Position, World);
            return new BoundingBox(lowerLeft, upperRight);
        }

        private void generateChessboardTexture()
        {
            cornerVertices = new VertexPositionNormalTexture[36];
            amendedThickness = Thickness + 0.2f;
            // border up 
            cornerVertices[0] = new VertexPositionNormalTexture(new Vector3(Width, amendedThickness, Height), Vector3.Up, new Vector2(1.0f, 1.0f));
            cornerVertices[1] = new VertexPositionNormalTexture(new Vector3(0.0f, amendedThickness, 0.0f), Vector3.Up, new Vector2(0.0f, 0.0f));
            cornerVertices[2] = new VertexPositionNormalTexture(new Vector3(0.0f, amendedThickness, Height), Vector3.Up, new Vector2(0.0f, 1.0f));

            cornerVertices[3] = new VertexPositionNormalTexture(new Vector3(Width, amendedThickness, Height), Vector3.Up, new Vector2(1.0f, 1.0f));
            cornerVertices[4] = new VertexPositionNormalTexture(new Vector3(Width, amendedThickness, 0.0f), Vector3.Up, new Vector2(1.0f, 0.0f));
            cornerVertices[5] = new VertexPositionNormalTexture(new Vector3(0.0f, amendedThickness, 0.0f), Vector3.Up, new Vector2(0.0f, 0.0f));

            cornerVertices[6] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Down, new Vector2(1.0f, 1.0f));
            cornerVertices[7] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Down, new Vector2(0.0f, 0.0f));
            cornerVertices[8] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, Height), Vector3.Down, new Vector2(0.0f, 1.0f));

            cornerVertices[9] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Down, new Vector2(1.0f, 1.0f));
            cornerVertices[10] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Down, new Vector2(1.0f, 0.0f));
            cornerVertices[11] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Down, new Vector2(0.0f, 0.0f));

            cornerVertices[12] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Backward, new Vector2(1.0f, 1.0f));
            cornerVertices[13] = new VertexPositionNormalTexture(new Vector3(Width, amendedThickness, Height), Vector3.Backward, new Vector2(1.0f, 0.0f));
            cornerVertices[14] = new VertexPositionNormalTexture(new Vector3(0.0f, amendedThickness, Height), Vector3.Backward, new Vector2(0.0f, 0.0f));

            cornerVertices[15] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Backward, new Vector2(1.0f, 1.0f));
            cornerVertices[16] = new VertexPositionNormalTexture(new Vector3(0.0f, amendedThickness, Height), Vector3.Backward, new Vector2(0.0f, 0.0f));
            cornerVertices[17] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, Height), Vector3.Backward, new Vector2(0.0f, 1.0f));

            cornerVertices[18] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, Height), Vector3.Left, new Vector2(1.0f, 1.0f));
            cornerVertices[19] = new VertexPositionNormalTexture(new Vector3(0.0f, amendedThickness, Height), Vector3.Left, new Vector2(1.0f, 0.0f));
            cornerVertices[20] = new VertexPositionNormalTexture(new Vector3(0.0f, amendedThickness, 0.0f), Vector3.Left, new Vector2(0.0f, 0.0f));

            cornerVertices[21] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, Height), Vector3.Left, new Vector2(1.0f, 1.0f));
            cornerVertices[22] = new VertexPositionNormalTexture(new Vector3(0.0f, amendedThickness, 0.0f), Vector3.Left, new Vector2(0.0f, 0.0f));
            cornerVertices[23] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Left, new Vector2(0.0f, 1.0f));

            cornerVertices[24] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Forward, new Vector2(1.0f, 1.0f));
            cornerVertices[25] = new VertexPositionNormalTexture(new Vector3(0.0f, amendedThickness, 0.0f), Vector3.Forward, new Vector2(0.0f, 0.0f));
            cornerVertices[26] = new VertexPositionNormalTexture(new Vector3(Width, amendedThickness, 0.0f), Vector3.Forward, new Vector2(1.0f, 0.0f));

            cornerVertices[27] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Forward, new Vector2(1.0f, 1.0f));
            cornerVertices[28] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Forward, new Vector2(0.0f, 1.0f));
            cornerVertices[29] = new VertexPositionNormalTexture(new Vector3(0.0f, amendedThickness, 0.0f), Vector3.Forward, new Vector2(0.0f, 0.0f));

            cornerVertices[30] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Right, new Vector2(1.0f, 1.0f));
            cornerVertices[31] = new VertexPositionNormalTexture(new Vector3(Width, amendedThickness, 0.0f), Vector3.Right, new Vector2(0.0f, 0.0f));
            cornerVertices[32] = new VertexPositionNormalTexture(new Vector3(Width, amendedThickness, Height), Vector3.Right, new Vector2(0.0f, 1.0f));

            cornerVertices[33] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Right, new Vector2(1.0f, 1.0f));
            cornerVertices[34] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Right, new Vector2(1.0f, 0.0f));
            cornerVertices[35] = new VertexPositionNormalTexture(new Vector3(Width, amendedThickness, 0.0f), Vector3.Right, new Vector2(0.0f, 0.0f));


            cornerVertexBuffer = new VertexBuffer(Game.GraphicsDevice, 36 * VertexPositionNormalTexture.SizeInBytes, BufferUsage.WriteOnly);


            //for (short i = 0; i < 36; i += 3)
            //{
            //    Vector3 v1 = cornerVertices[i].Position;
            //    Vector3 v2 = cornerVertices[i+1].Position;
            //    Vector3 v3 = cornerVertices[i+2].Position;
            //    Vector3 vu = v2 - v3;
            //    Vector3 vt = v2 - v1;
            //    Vector3 normal = Vector3.Cross(vu, vt);
            //    normal.Normalize();
            //    cornerVertices[i].Normal += normal;
            //    cornerVertices[i+1].Normal += normal;
            //    cornerVertices[i+2].Normal += normal;
            //}

            //for (short i = 0; i < 36; i++)
            //    cornerVertices[i].Normal.Normalize();

            cornerVertexBuffer.SetData<VertexPositionNormalTexture>(cornerVertices);
            cornerVertexDeclaration = new VertexDeclaration(Game.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            borderVertices = new VertexPositionNormalTexture[36];

            // border up 
            borderVertices[0] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, Thickness, Height), Vector3.Up, new Vector2(1.0f, 1.0f));
            borderVertices[1] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, 0.0f), Vector3.Up, new Vector2(0.0f, 0.0f));
            borderVertices[2] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, Height), Vector3.Up, new Vector2(0.0f, 1.0f));

            borderVertices[3] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, Thickness, Height), Vector3.Up, new Vector2(1.0f, 1.0f));
            borderVertices[4] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, Thickness, 0.0f), Vector3.Up, new Vector2(1.0f, 0.0f));
            borderVertices[5] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, 0.0f), Vector3.Up, new Vector2(0.0f, 0.0f));

            borderVertices[6] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, Height), Vector3.Down, new Vector2(1.0f, 1.0f));
            borderVertices[7] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Down, new Vector2(0.0f, 0.0f));
            borderVertices[8] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Down, new Vector2(0.0f, 1.0f));

            borderVertices[9] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, Height), Vector3.Down, new Vector2(1.0f, 1.0f));
            borderVertices[10] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, 0.0f), Vector3.Down, new Vector2(1.0f, 0.0f));
            borderVertices[11] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Down, new Vector2(0.0f, 0.0f));

            borderVertices[12] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, Height), Vector3.Backward, new Vector2(1.0f, 1.0f));
            borderVertices[13] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, Thickness, Height), Vector3.Backward, new Vector2(1.0f, 0.0f));
            borderVertices[14] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, Height), Vector3.Backward, new Vector2(0.0f, 0.0f));

            borderVertices[15] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, Height), Vector3.Backward, new Vector2(1.0f, 1.0f));
            borderVertices[16] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, Height), Vector3.Backward, new Vector2(0.0f, 0.0f));
            borderVertices[17] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Backward, new Vector2(0.0f, 1.0f));

            borderVertices[18] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Left, new Vector2(1.0f, 1.0f));
            borderVertices[19] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, Height), Vector3.Left, new Vector2(1.0f, 0.0f));
            borderVertices[20] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, 0.0f), Vector3.Left, new Vector2(0.0f, 0.0f));

            borderVertices[21] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Left, new Vector2(1.0f, 1.0f));
            borderVertices[22] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, 0.0f), Vector3.Left, new Vector2(0.0f, 0.0f));
            borderVertices[23] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Left, new Vector2(0.0f, 1.0f));

            borderVertices[24] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, 0.0f), Vector3.Forward, new Vector2(1.0f, 1.0f));
            borderVertices[25] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, 0.0f), Vector3.Forward, new Vector2(0.0f, 0.0f));
            borderVertices[26] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, Thickness, 0.0f), Vector3.Forward, new Vector2(1.0f, 0.0f));


            borderVertices[27] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, 0.0f), Vector3.Forward, new Vector2(1.0f, 1.0f));
            borderVertices[28] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Forward, new Vector2(0.0f, 1.0f));
            borderVertices[29] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, 0.0f), Vector3.Forward, new Vector2(0.0f, 0.0f));


            borderVertices[30] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, Height), Vector3.Right, new Vector2(1.0f, 1.0f));
            borderVertices[31] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, Thickness, 0.0f), Vector3.Right, new Vector2(0.0f, 0.0f));
            borderVertices[32] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, Thickness, Height), Vector3.Right, new Vector2(0.0f, 1.0f));


            borderVertices[33] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, Height), Vector3.Right, new Vector2(1.0f, 1.0f));
            borderVertices[34] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, 0.0f), Vector3.Right, new Vector2(1.0f, 0.0f));
            borderVertices[35] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, Thickness, 0.0f), Vector3.Right, new Vector2(0.0f, 0.0f));


            borderVertexBuffer = new VertexBuffer(Game.GraphicsDevice, 36 * VertexPositionNormalTexture.SizeInBytes, BufferUsage.WriteOnly);


            //for (short i = 0; i < 36; i += 3)
            //{
            //    Vector3 v1 = borderVertices[i].Position;
            //    Vector3 v2 = borderVertices[i + 1].Position;
            //    Vector3 v3 = borderVertices[i + 2].Position;
            //    Vector3 vu = v2 - v3;
            //    Vector3 vt = v2 - v1;
            //    Vector3 normal = Vector3.Cross(vu, vt);
            //    normal.Normalize();
            //    borderVertices[i].Normal += normal;
            //    borderVertices[i + 1].Normal += normal;
            //    borderVertices[i + 2].Normal += normal;
            //}

            //for (short i = 0; i < 36; i++)
            //    borderVertices[i].Normal.Normalize();

            borderVertexBuffer.SetData<VertexPositionNormalTexture>(borderVertices);
            borderVertexDeclaration = new VertexDeclaration(Game.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            //cornerVertices = new VertexPositionNormalTexture[12];
            ////without Thickness
            //cornerVertices[0] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, Height), Vector3.Zero, new Vector2(1.0f, 1.0f));
            //cornerVertices[1] = new VertexPositionNormalTexture(new Vector3(Width, 0.0f, 0.0f), Vector3.Zero, new Vector2(1.0f, 0.0f));
            //cornerVertices[2] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Zero, new Vector2(0.0f, 0.0f));

            //cornerVertices[3] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, Height), Vector3.Zero, new Vector2(0.0f, 1.0f));
            //cornerVertices[4] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, 0.0f, 0.0f), Vector3.Zero, new Vector2(0.0f, 0.0f));
            //cornerVertices[5] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth + Width, 0.0f, 0.0f), Vector3.Zero, new Vector2(1.0f, 0.0f));

            //// with thickness
            //cornerVertices[6] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, Height), Vector3.Zero, new Vector2(1.0f, 1.0f));
            //cornerVertices[7] = new VertexPositionNormalTexture(new Vector3(Width, Thickness, 0.0f), Vector3.Zero, new Vector2(1.0f, 0.0f));
            //cornerVertices[8] = new VertexPositionNormalTexture(new Vector3(0.0f, Thickness, 0.0f), Vector3.Zero, new Vector2(0.0f, 0.0f));

            //cornerVertices[9] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, Thickness, Height), Vector3.Zero, new Vector2(0.0f, 1.0f));
            //cornerVertices[10] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth, Thickness, 0.0f), Vector3.Zero, new Vector2(0.0f, 0.0f));
            //cornerVertices[11] = new VertexPositionNormalTexture(new Vector3(Width + Bwidth + Width, Thickness, 0.0f), Vector3.Zero, new Vector2(1.0f, 0.0f));

            //vertexBuffer = new VertexBuffer(Game.GraphicsDevice, 12 * VertexPositionNormalTexture.SizeInBytes, BufferUsage.WriteOnly);

            //indices = new int[] {
            //    // up
            //    0,1,2,
            //    3,4,0,
            //    0,4,1,
            //    5,4,3,
            //    //down
            //    6,7,8,
            //    9,10,6,
            //    6,10,7,
            //    11,10,9,
            //    //left
            //    2,0,8,
            //    8,0,6,
            //    //right
            //    3,5,11,
            //    11,9,3,
            //    //fornt
            //    0,3,9,
            //    0,9,6,
            //    //back
            //    2,5,11,
            //    2,11,8


            //};

            //indexBuffer = new IndexBuffer(Game.GraphicsDevice, 48 * sizeof(int), BufferUsage.WriteOnly, IndexElementSize.ThirtyTwoBits);
            //indexBuffer.SetData<int>(indices);

            //for (short i = 0; i < 48; i += 3)
            //{
            //    Vector3 v1 = cornerVertices[indices[i]].Position;
            //    Vector3 v2 = cornerVertices[indices[i + 1]].Position;
            //    Vector3 v3 = cornerVertices[indices[i + 2]].Position;
            //    Vector3 vu = v3 - v2;
            //    Vector3 vt = v2 - v1;
            //    Vector3 normal = Vector3.Cross(vu, vt);
            //    normal.Normalize();
            //    cornerVertices[indices[i]].Normal += normal;
            //    cornerVertices[indices[i + 1]].Normal += normal;
            //    cornerVertices[indices[i + 2]].Normal += normal;
            //}
            //for (short i = 0; i < 12; i++)
            //    cornerVertices[i].Normal.Normalize();

            //vertexBuffer.SetData<VertexPositionNormalTexture>(cornerVertices);
            //vertexDeclaration = new VertexDeclaration(Game.GraphicsDevice, VertexPositionNormalTexture.VertexElements);
        }

        public override void Draw(GameTime gameTime)
        {
            //cornerEffect.setLightPosition(new Vector3(cam.currentPlayerOnePosition.X, cam.currentPlayerOnePosition.Y, cam.currentPlayerOnePosition.Z));
            //Game.GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;//OPenGL Legacy!
            //cornerEffect.setWorldMatrix(World * cam.WorldMatrix);
            //cornerEffect.setProjectionMatrix(cam.ProjectionMatrix);
            //cornerEffect.setViewInverseMatrix(Matrix.Invert(cam.ViewMatrix));
            //cornerEffect.setViewMatrix(cam.ViewMatrix);
            //cornerEffect.setWorldInverseTransposeMatrix(Matrix.Transpose(Matrix.Invert(World * cam.WorldMatrix)));
            //cornerEffect.setWorldMatrix(World * cam.WorldMatrix);
            //cornerEffect.setWorldViewProjectionMatrix(World * cam.WorldMatrix * cam.ViewMatrix * cam.ProjectionMatrix);
            //cornerEffect.setWorldViewMatrix(World * cam.WorldMatrix * cam.ViewMatrix);
            //Game.GraphicsDevice.Vertices[0].SetSource(cornerVertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);

            //Game.GraphicsDevice.VertexDeclaration = cornerVertexDeclaration;
            //cornerEffect.Effect.Begin();
            //foreach (EffectPass pass in cornerEffect.Effect.CurrentTechnique.Passes)
            //{
            //    pass.Begin();

            //    //Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 60, 0, 20);
            //    Game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);
            //    pass.End();
            //}
            //cornerEffect.Effect.End();
            ////Game.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            //borderEffect.setLightPosition(new Vector3(cam.currentPlayerOnePosition.X, cam.currentPlayerOnePosition.Y, cam.currentPlayerOnePosition.Z));
            //borderEffect.setWorldMatrix(World * cam.WorldMatrix);
            //borderEffect.setProjectionMatrix(cam.ProjectionMatrix);
            //borderEffect.setViewInverseMatrix(Matrix.Invert(cam.ViewMatrix));
            //borderEffect.setViewMatrix(cam.ViewMatrix);
            //borderEffect.setWorldInverseTransposeMatrix(Matrix.Transpose(Matrix.Invert(World * cam.WorldMatrix)));
            //borderEffect.setWorldMatrix(World * cam.WorldMatrix);
            //borderEffect.setWorldViewProjectionMatrix(World * cam.WorldMatrix * cam.ViewMatrix * cam.ProjectionMatrix);
            //borderEffect.setWorldViewMatrix(World * cam.WorldMatrix * cam.ViewMatrix);

            //Game.GraphicsDevice.Vertices[0].SetSource(borderVertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);

            //Game.GraphicsDevice.VertexDeclaration = borderVertexDeclaration;
            //borderEffect.Effect.Begin();
            //foreach (EffectPass pass in borderEffect.Effect.CurrentTechnique.Passes)
            //{
            //    pass.Begin();
            //    Game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);
            //    pass.End();
            //}
            //borderEffect.Effect.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            Vector3 temp = borderEffect.Effect.Parameters["Lamp0Pos"].GetValueVector3();
            if (keyState.IsKeyDown(Keys.I))
            {
                borderEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X + 0.1f, temp.Y, temp.Z));
                cornerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X + 0.1f, temp.Y, temp.Z));
            }
            if (keyState.IsKeyDown(Keys.U))
            {
                borderEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X - 0.1f, temp.Y, temp.Z));
                cornerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X - 0.1f, temp.Y, temp.Z));
            }
            if (keyState.IsKeyDown(Keys.K))
            {
                borderEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y + 0.1f, temp.Z));
                cornerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y + 0.1f, temp.Z));
            }
            if (keyState.IsKeyDown(Keys.J))
            {
                borderEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y - 0.1f, temp.Z));
                cornerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y - 0.1f, temp.Z));
            }
            if (keyState.IsKeyDown(Keys.M))
            {
                borderEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y, temp.Z + 0.1f));
                cornerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y, temp.Z + 0.1f));
            }
            if (keyState.IsKeyDown(Keys.N))
            {
                borderEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y, temp.Z - 0.1f));
                cornerEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y, temp.Z - 0.1f));
            }
            //if (keyState.IsKeyDown(Keys.P))
            //    Console.WriteLine(borderEffect.Effect.Parameters["gLamp0Pos"].GetValueVector3().ToString());
            base.Update(gameTime);
        }
    }
}
