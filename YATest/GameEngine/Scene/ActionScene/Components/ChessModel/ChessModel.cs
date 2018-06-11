using Microsoft.Xna.Framework;
using YATest.Utilities;
using YATest.GameLogic;
using Microsoft.Xna.Framework.Graphics;
using YATest.Utilities.CameraUtil;
using YATest.Utilities.Effects;
using Microsoft.Xna.Framework.Input;
using ParticleClassLibrary;
using System;

namespace YATest.GameEngine
{
    abstract class ChessModel : DrawableGameComponent
    {
        //Changes by nassouh
        bool c=true;
        private float alpha = 1.0f;
        private bool fadeIn = false;

        public bool FadeIn
        {
            get { return fadeIn; }
            set { fadeIn = value; }
        }
        private bool fadeOut = false;
        private Position newPos;
        private float alphaStep = 0.02f;
        private bool isCapturing = false;

        public bool IsCapturing
        {
            get { return isCapturing; }
            set
            {
                isCapturing = value;
                fadeOut = true;
            }
        }

        private bool isMoving = false;
        Chessboard owner;
        // end of changes

        protected AbstractPiece logicalPieceRef; //Here, the piece will get all the information
        protected Model model;
        protected ModelProvider modelProvider;
        private Matrix worldMatrix;
        private Matrix calibrationMatrix;
        public Matrix CalibrationMatrix
        {
            get { return calibrationMatrix; }
            set { calibrationMatrix = value; }
        }

        private float increment = 0.01f;
        protected Texture2D texture;
        private GameCamera cam;
        protected float maxModelHeight = 1.5f;
        protected float prefferedModelHeight = 10.5f;

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        private FireParticleSystem fireParticles;

        public FireParticleSystem FireParticles
        {
            get { return fireParticles; }
            set { fireParticles = value; }
        }
        Random random = new Random();


        public float PrefferedModelHeight
        {
            get { return prefferedModelHeight; }
        }

        public float MaxModelHeight
        {
            get { return maxModelHeight; }
        }

        private BasicEffect modelEffect;

        public BasicEffect ModelEffect
        {
            get { return modelEffect; }
            set { modelEffect = value; }
        }

        // extra for capturing
        private int modelIndex = -1;
        public Plane shadowPlane;
        Vector3 shadowLightDir;
        Matrix shadow;

        #region Properties
        public AbstractPiece LogicalPieceRef
        {
            get { return logicalPieceRef; }
            set { logicalPieceRef = value; }
        }
        #endregion
        //Maybe we should add later some methods like: Highlight, Select, and Reset. Just like the checker.
        //But that needs extra handling using dedicated Effect.
        public ChessModel(Game game, AbstractPiece logicalPieceRef, ModelProvider modelProvider)
            : base(game)
        {
            this.logicalPieceRef = logicalPieceRef;
            this.modelProvider = modelProvider;
            // ModelEffect = new PhongEffect(game, logicalPieceRef);
            ModelEffect = new BasicEffect(game.GraphicsDevice, null);

            //System.Console.WriteLine(ModelEffect.Effect.GetHashCode());
            // ModelEffect.setOtherParams();
            if (logicalPieceRef.player is Player1)
                fireParticles = new FireParticleSystem(game, Tint.Dark);
            else
                fireParticles = new FireParticleSystem(game, Tint.Light);

            fireParticles.DrawOrder = 500;
            cam = (GameCamera)game.Services.GetService(typeof(BasicCamera));

            Game.Components.Add(fireParticles);

            shadowPlane = new Plane(new Vector3(-5, 0, -5), new Vector3(5, 0, -5), new Vector3(5, 0, 5));
            shadowPlane.Normal = new Vector3(0, -8, 0);

            shadowLightDir = new Vector3(-2.1f, -2f, -2.1f);
            shadow = Matrix.CreateShadow(shadowLightDir, shadowPlane);
            modelEffect.LightingEnabled = true;

            modelEffect.TextureEnabled = true;
            modelEffect.EnableDefaultLighting();
            if (logicalPieceRef.player is Player1)
            {
                texture = game.Content.Load<Texture2D>("White");
            }
            else texture = game.Content.Load<Texture2D>("Black");
            //game.Components.Add(fireParticles);
        }

        public Utilities.Position Position()
        {
            return logicalPieceRef.position;
        }

        public bool IsCaptured()
        {
            return logicalPieceRef.IsCaptured;
        }

        void UpdateFire()
        {
            const int fireParticlesPerFrame = 500;


            Matrix m1 = modelProvider.getModelMatrix(logicalPieceRef.name);
            Matrix m2 = owner.CheckerAt(logicalPieceRef.position).World;
            Matrix m3 = owner.CheckerAt(newPos).World;
            Matrix original = m1 * m2 * calibrationMatrix;
            Matrix target = m1 * m3 * calibrationMatrix;
            // Create a number of fire particles, randomly positioned around a circle.

            for (int i = 0; i < fireParticlesPerFrame; i++)
            {
                fireParticles.AddParticle(Vector3.Transform(Vector3.Zero, original * cam.WorldMatrix), Vector3.Zero);
                original = Matrix.Lerp(original, target, 0.01f);
            }



            // Create one smoke particle per frmae, too.
            //smokePlumeParticles.AddParticle(RandomPointOnCircle(), Vector3.Zero);
        }





        public override void Update(GameTime gameTime)
        {
            owner = (Chessboard)Game.Services.GetService(typeof(Chessboard));






            //KeyboardState keyState = Keyboard.GetState();
            //Vector4 temp = modelEffect.Effect.Parameters["gLamp0DirPos"].GetValueVector4();
            //if (keyState.IsKeyDown(Keys.I))
            //    modelEffect.Effect.Parameters["gLamp0DirPos"].SetValue(new Vector4(temp.X + 0.1f, temp.Y, temp.Z,temp.W));
            //if (keyState.IsKeyDown(Keys.U))
            //    modelEffect.Effect.Parameters["gLamp0DirPos"].SetValue(new Vector4(temp.X - 0.1f, temp.Y, temp.Z, temp.W));
            //if (keyState.IsKeyDown(Keys.K))
            //    modelEffect.Effect.Parameters["gLamp0DirPos"].SetValue(new Vector4(temp.X, temp.Y + 0.1f, temp.Z, temp.W));
            //if (keyState.IsKeyDown(Keys.J))
            //    modelEffect.Effect.Parameters["gLamp0DirPos"].SetValue(new Vector4(temp.X, temp.Y - 0.1f, temp.Z, temp.W));
            //if (keyState.IsKeyDown(Keys.M))
            //    modelEffect.Effect.Parameters["gLamp0DirPos"].SetValue(new Vector4(temp.X, temp.Y, temp.Z + 0.1f, temp.W));
            //if (keyState.IsKeyDown(Keys.N))
            //    modelEffect.Effect.Parameters["gLamp0DirPos"].SetValue(new Vector4(temp.X, temp.Y, temp.Z - 0.1f, temp.W));
            //if (logicalPieceRef.name == ChessNames.Pawn)
            //{
            //    i = i + increment;
            //    if (i > 0.8)
            //        increment = -0.01f;
            //    if (i < 0)
            //        increment = 0.01f;
            //    ModelEffect.Effect.Parameters["gKs"].SetValue(i);
            //}

            KeyboardState keyState = Keyboard.GetState();
            //Vector3 temp = modelEffect.Effect.Parameters["Lamp0Pos"].GetValueVector3();
            //if (keyState.IsKeyDown(Keys.I))
            //    modelEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X + 0.1f, temp.Y, temp.Z));
            //if (keyState.IsKeyDown(Keys.U))
            //    modelEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X - 0.1f, temp.Y, temp.Z));
            //if (keyState.IsKeyDown(Keys.K))
            //    modelEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y + 0.1f, temp.Z));
            //if (keyState.IsKeyDown(Keys.J))
            //    modelEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y - 0.1f, temp.Z));
            //if (keyState.IsKeyDown(Keys.M))
            //    modelEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y, temp.Z + 0.1f));
            //if (keyState.IsKeyDown(Keys.N))
            //    modelEffect.Effect.Parameters["Lamp0Pos"].SetValue(new Vector3(temp.X, temp.Y, temp.Z - 0.1f));
            //if (keyState.IsKeyDown(Keys.P))
            //    Console.WriteLine(CheckerEffect.Effect.Parameters["Lamp0Pos"].GetValueVector3().ToString());



            UpdateFire();
            base.Update(gameTime);
        }


        public void MoveTo(Position moveTo)
        {

            
            newPos = moveTo;
            fadeOut = true;
            owner.Blocked = true;
        }

        public override void Draw(GameTime gameTime)
        {
            owner = (Chessboard)Game.Services.GetService(typeof(Chessboard));
            Matrix m1 = modelProvider.getModelMatrix(logicalPieceRef.name);
            Matrix m2 = owner.CheckerAt(logicalPieceRef.position).World;
            Matrix m3 = owner.CheckerAt(newPos).World;
            if (IsCaptured() == true)
            {
                if (c)
                {

                    m1 = modelProvider.getModelMatrix(logicalPieceRef.name);
                    m2 = Matrix.Identity;
                    if (logicalPieceRef.player is Player2)
                    {
                        m2 = owner.CheckerAt(new Position(0, 0, 0)).World * Matrix.CreateTranslation(-1 - owner.GetCapturedPieces(logicalPieceRef.player) / 8, 0, owner.GetCapturedPieces(logicalPieceRef.player) % 8);
                    }
                    else
                    {
                        m2 = owner.CheckerAt(new Position(0, 0, 0)).World * Matrix.CreateTranslation(8 + owner.GetCapturedPieces(logicalPieceRef.player) / 8, 0, owner.GetCapturedPieces(logicalPieceRef.player) % 8);
                    }

                    worldMatrix = m1 * m2 * calibrationMatrix;
                    Visible = true;
                    fireParticles.Visible = false;

                }
                c = false;
            }
            else if (fadeIn)
            {
                worldMatrix = Matrix.Lerp(worldMatrix, m1 * m3 * calibrationMatrix, 0.3f);
            }
            else
            {
                worldMatrix = m1 * m2 * calibrationMatrix;
                fireParticles.Visible = true;
            }
            modelEffect.Projection = cam.ProjectionMatrix;
            modelEffect.View = cam.ViewMatrix;
            modelEffect.World = worldMatrix * cam.WorldMatrix;
            modelEffect.Alpha = alpha;
            modelEffect.Texture = texture;


            //Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            if (fadeOut)
            {
                if (alpha > 0.2) // begin fade out
                    alpha -= alphaStep;
                else
                {
                    fadeOut = false; // end fade out 

                    fadeIn = true;
                    logicalPieceRef.moveTo(newPos);
                    
                }
            }

            //if (IsCapturing)
            //{
            //    if (modelIndex > 51)
            //        modelIndex -= 50;
            //    else
            //    {
            //        logicalPieceRef.isCaptured = true;
            //        isCapturing = false;
            //    }

            //}
            if (fadeIn)
            {

                if (alpha < 0.9f)
                    alpha += alphaStep;
                else
                {
                    fadeIn = false;
                    owner.Blocked = false;
                    logicalPieceRef.isSelected = false;
                    GameManager.getReference(null).toggleTurn(false);
                }
            }



            foreach (ModelMesh mesh in model.Meshes)
            {
                vertexBuffer = mesh.VertexBuffer;
                indexBuffer = mesh.IndexBuffer;
            }

            if (modelIndex == -1)
                modelIndex = indexBuffer.SizeInBytes / 6;


            //if (modelIndex > 31)
            //    modelIndex -= 30;

            Game.GraphicsDevice.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
            Game.GraphicsDevice.Indices = indexBuffer;

            Game.GraphicsDevice.VertexDeclaration = new VertexDeclaration(
                Game.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            //Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
            modelEffect.Begin();
            foreach (EffectPass pass in modelEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                //graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, buffer.SizeInBytes, 0, 2730);
                Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.SizeInBytes, 0, modelIndex);
                //Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.SizeInBytes, 0, modelIndex);

                pass.End();
            }
            modelEffect.End();



            //foreach (ModelMesh mesh in model.Meshes)
            //{
            //    foreach (ModelMeshPart part in mesh.MeshParts)
            //    {
            //        part.Effect = ModelEffect;
            //    }
            //}

            //Game.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            // Game.GraphicsDevice.RenderState.DestinationBlend = Blend.DestinationAlpha;
            //foreach (ModelMesh mesh in model.Meshes)
            //{
            //    foreach (Effect eff in mesh.Effects)
            //    {
            //        modelEffect.Projection = cam.ProjectionMatrix;
            //        modelEffect.View = cam.ViewMatrix;
            //        modelEffect.World = worldMatrix * cam.WorldMatrix;
            //        modelEffect.Alpha = alpha;
            //        modelEffect.Texture = texture;
            //    }
            //    mesh.Draw();
            //}
            // Game.GraphicsDevice.RenderState.SourceBlend = Blend.One;
            // Game.GraphicsDevice.RenderState.DestinationBlend = Blend.Zero;
            fireParticles.SetCamera(cam.ViewMatrix, cam.ProjectionMatrix);
            if (logicalPieceRef.isSelected)

                fireParticles.Visible = true;
            else
                fireParticles.Visible = false;
            // draw the shadow

            //Game.GraphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0, 0);
            //Game.GraphicsDevice.RenderState.StencilEnable = true;
            //// Draw on screen if 0 is the stencil buffer value           
            //Game.GraphicsDevice.RenderState.ReferenceStencil = 0;
            //Game.GraphicsDevice.RenderState.StencilFunction =
            //    CompareFunction.Equal;
            //// Increment the stencil buffer if we draw
            //Game.GraphicsDevice.RenderState.StencilPass =
            //    StencilOperation.Increment;
            //// Setup alpha blending to make the shadow semi-transparent
            //Game.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            //Game.GraphicsDevice.RenderState.SourceBlend =
            //    Blend.SourceAlpha;
            //Game.GraphicsDevice.RenderState.DestinationBlend =
            //    Blend.InverseSourceAlpha;

            //foreach (ModelMesh mesh in model.Meshes)
            //{
            //    foreach (Effect eff in mesh.Effects)
            //    {

            //        modelEffect.setProjectionMatrix(cam.ProjectionMatrix);
            //        modelEffect.setViewInverseMatrix(Matrix.Invert(cam.ViewMatrix));
            //        modelEffect.setViewMatrix(cam.ViewMatrix);
            //        modelEffect.setWorldInverseTransposeMatrix(Matrix.Transpose(Matrix.Invert(worldMatrix * cam.WorldMatrix * shadow)));
            //        modelEffect.setWorldMatrix(worldMatrix * cam.WorldMatrix * shadow);
            //        modelEffect.setWorldViewProjectionMatrix(worldMatrix * cam.WorldMatrix * shadow * cam.ViewMatrix * cam.ProjectionMatrix);

            //        modelEffect.setWorldViewMatrix(worldMatrix * cam.WorldMatrix * shadow * cam.ViewMatrix);
            //    }
            //    mesh.Draw();
            //}

            // Game.GraphicsDevice.RenderState.StencilEnable = false;
            // turn alpha blending off
            // Game.GraphicsDevice.RenderState.AlphaBlendEnable = false;


            base.Draw(gameTime);
        }
    }
}
