#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using YATest.GameEngine; 
#endregion


namespace YATest.Utilities.CameraUtil
{
    public class GameCamera : BasicCamera
    {
        private Vector3 defaultPlayerOnePosition = new Vector3(0.05f, 8.3f, 16.0f);
        private Vector3 defaultPlayerTwoPosition = new Vector3(0.05f, 8.3f, -16.0f);
        private float P1xRotation = 0;
        private float P1yRotation = 0;
        private float P2xRotation = 0;
        private float P2yRotation = 0;
        private Quaternion P1CurrentXRotation = Quaternion.Identity;
        private Quaternion P1CurrentYRotation = Quaternion.Identity;
        private Quaternion P2CurrentXRotation = Quaternion.Identity;
        private Quaternion P2CurrentYRotation = Quaternion.Identity;

        private Vector3 upVector = Vector3.Up;
        private Vector3 P1xAxis = Vector3.UnitX;
        private Vector3 P1yAxis = Vector3.UnitY;
        private Vector3 P2xAxis = Vector3.UnitX;
        private Vector3 P2yAxis = Vector3.UnitY;


        private bool isPlayerOneCamera;
        private bool isLerping = false;

        private float P1UpDown = 0;
        private float P2UpDown = 0;
        private float P1Limit = 0;
        private float P2Limit = 0;
        private Vector3 zoomAmount = new Vector3(0.01f, 0.01f, 0.01f);
        public Vector3 currentPlayerOnePosition;
        public Vector3 currentPlayerTwoPosition;

        private Chessboard chessboard;

        public GameCamera(Game game, Viewport viewport): base(game) 
        {
            this.viewport = viewport;
            position = defaultPlayerOnePosition;
            worldMatrix = Matrix.Identity;
            aspectRatio = ((float)Viewport.Width / (float)Viewport.Height);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(40.0f), AspectRatio, NearPlane, FarPlane);
            Target = new Vector3(0, 0, 0);
            viewMatrix = Matrix.CreateLookAt(position, Target, Vector3.Up);
            oldMouseState = Mouse.GetState();
            newMouseState = Mouse.GetState();
            currentPlayerOnePosition = defaultPlayerOnePosition;
            currentPlayerTwoPosition = defaultPlayerTwoPosition;
            setPlayerOneCamera();

        }

        public void changeCamera()
        {
            if (!isPlayerOneCamera)
            {
                position = currentPlayerTwoPosition;
                setPlayerOneCamera();
            }
            else
            {
                position = currentPlayerOnePosition;
                setPlayerTwoCamera();
            }

        }
        public override void HandleKeyboardInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            chessboard = (Chessboard)Game.Services.GetService(typeof(Chessboard));
            if(keyState.IsKeyDown(Keys.I))
            {
                chessboard.Blocked = true;
            }
            if(keyState.IsKeyDown(Keys.O))
            {
                chessboard.Blocked = false;
            }
            // to see if the camera is moving while switching between turns .
            if (!isLerping)
            {
                if (keyState.IsKeyDown(Keys.D1))
                {
                    changeCamera();
                }



                ///////////////////////////////////////////////////////////////////////////////////////////////////
                // do the movement for the player One
                if (isPlayerOneCamera)
                {
                    // rotate the camera position around the YAxis
                    if (keyState.IsKeyDown(Keys.A))
                    {
                        // rotate up
                        P1CurrentYRotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateFromAxisAngle(P1yAxis, MathHelper.ToRadians(-1)));
                    }
                    else if (keyState.IsKeyDown(Keys.D))
                    {
                        // rotate down
                        P1CurrentYRotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateFromAxisAngle(P1yAxis, MathHelper.ToRadians(1)));
                    }
                    else
                    {
                        // no rotation
                        P1CurrentYRotation = Quaternion.Identity;
                    }

                    // apply the current yRotation to the xAxis
                    P1xAxis = Vector3.Transform(P1xAxis, Matrix.CreateFromQuaternion(P1CurrentYRotation));

                    ////////////////////////////////////////
                    //perform the zoom opeeration
                    if (keyState.IsKeyDown(Keys.Z))
                    {
                        // zoom out
                        currentPlayerOnePosition = Vector3.Lerp(currentPlayerOnePosition, target, -0.02f);
                        //position = currentPlayerOnePosition;
                    }
                    if (keyState.IsKeyDown(Keys.Q))
                    {
                        // zoom in
                        currentPlayerOnePosition = Vector3.Lerp(currentPlayerOnePosition, target, 0.02f);
                        //position = currentPlayerOnePosition;
                    }

                    if (keyState.IsKeyDown(Keys.W))
                    {
                        // layer up
                        
                        if (P1UpDown < 15)
                        {
                            P1UpDown += 0.15f;
                            currentPlayerOnePosition += new Vector3(0, 0.15f, 0);
                            target += new Vector3(0, 0.15f, 0);
                        }
                        
                    }
                    if (keyState.IsKeyDown(Keys.S))
                    {
                        // layer down
                        if (P1UpDown > 0)
                        {
                            P1UpDown -= 0.15f;
                            currentPlayerOnePosition -= new Vector3(0, 0.15f, 0);
                            target -= new Vector3(0, 0.15f, 0);
                        }
                    }

                }
                else
                {
                    // do the movement for the player Two
                    ///////////////////////////////////////////////////////////////////////////////////////////////////
                    // rotate the camera position around the YAxis
                    
                    if (keyState.IsKeyDown(Keys.A))
                    {
                        // rotate up
                        P2CurrentYRotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateFromAxisAngle(P2yAxis, MathHelper.ToRadians(-1)));
                    }
                    else if (keyState.IsKeyDown(Keys.D))
                    {
                        // rotate down
                        P2CurrentYRotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateFromAxisAngle(P2yAxis, MathHelper.ToRadians(1)));
                    }
                    else
                    {
                        // no rotation
                        P2CurrentYRotation = Quaternion.Identity;
                    }

                    // apply the current yRotation to the xAxis
                    P2xAxis = Vector3.Transform(P2xAxis, Matrix.CreateFromQuaternion(P2CurrentYRotation));

                    ////////////////////////////////////////
                    //perform the zoom opeeration
                    if (keyState.IsKeyDown(Keys.Z))
                    {
                        // zoom out
                        currentPlayerTwoPosition = Vector3.Lerp(currentPlayerTwoPosition, target, -0.02f);
                        //position = currentPlayerOnePosition;
                    }
                    if (keyState.IsKeyDown(Keys.Q))
                    {
                        // zoom in
                        currentPlayerTwoPosition = Vector3.Lerp(currentPlayerTwoPosition, target, 0.02f);
                        //position = currentPlayerOnePosition;
                    }

                    if (keyState.IsKeyDown(Keys.W))
                    {
                        // layer up
                        if (P2UpDown < 15)
                        {
                            P2UpDown += 0.15f;
                            currentPlayerTwoPosition += new Vector3(0, 0.15f, 0);
                            target += new Vector3(0, 0.15f, 0);
                        }
                    }
                    if (keyState.IsKeyDown(Keys.S))
                    {
                        // layer down
                        if (P2UpDown > 0)
                        {
                            P2UpDown -= 0.15f;
                            currentPlayerTwoPosition -= new Vector3(0, 0.15f, 0);
                            target -= new Vector3(0, 0.15f, 0);
                        }
                    }
                }
            }
            else
            {
                if (isPlayerOneCamera)
                    if (!setPlayerOneCamera())
                    {
                        UpdateViewMatrix();
                    }
                    else
                    {
                        isLerping = false;
                        //limit = 0;

                    }
                else
                    if (!setPlayerTwoCamera())
                    {
                        UpdateViewMatrix();
                    }
                    else
                    {
                        isLerping = false;
                        //limit = 0;
                    }
            }
            UpdateViewMatrix();

            base.HandleKeyboardInput();
        }

        public override void HandleMouseInput()
        {
            if (!isLerping)
                if (isPlayerOneCamera)
                {
                    newMouseState = Mouse.GetState();

                    // mouse right button is pressed .. do the rotation
                    if (newMouseState.RightButton == ButtonState.Pressed)
                    {

                        if (newMouseState != oldMouseState)
                        {
                            if (newMouseState.X > oldMouseState.X)
                            {
                                P1xRotation += (newMouseState.X - oldMouseState.X) / 3;
                            }
                            else if (newMouseState.X < oldMouseState.X)
                            {
                                P1xRotation -= (oldMouseState.X - newMouseState.X) / 3;
                            }

                            if (newMouseState.Y > oldMouseState.Y)
                            {
                                if (P1yRotation <= 36)
                                    P1yRotation += (newMouseState.Y - oldMouseState.Y) / 3;
                            }
                            else if (newMouseState.Y < oldMouseState.Y)
                            {
                                if (P1yRotation >= -36)
                                    P1yRotation -= (oldMouseState.Y - newMouseState.Y) / 3;
                            }

                            UpdateworldMatrix();
                        }
                    }

                    oldMouseState = newMouseState;
                }
                else
                {
                    newMouseState = Mouse.GetState();

                    // mouse right button is pressed .. do the rotation
                    if (newMouseState.RightButton == ButtonState.Pressed)
                    {

                        if (newMouseState != oldMouseState)
                        {
                            if (newMouseState.X > oldMouseState.X)
                            {
                                P2xRotation += (newMouseState.X - oldMouseState.X) / 3;
                            }
                            else if (newMouseState.X < oldMouseState.X)
                            {
                                P2xRotation -= (oldMouseState.X - newMouseState.X) / 3;
                            }

                            if (newMouseState.Y < oldMouseState.Y)
                            {
                                if (P2yRotation <= 36)
                                    P2yRotation += (oldMouseState.Y - newMouseState.Y) / 3;
                            }
                            else if (newMouseState.Y > oldMouseState.Y)
                            {
                                if (P2yRotation >= -36)
                                    P2yRotation -= (newMouseState.Y - oldMouseState.Y) / 3;
                            }

                            UpdateworldMatrix();
                        }
                    }

                    oldMouseState = newMouseState;
                }

            base.HandleMouseInput();
        }

        public bool setPlayerOneCamera()
        {
            isLerping = true;
            isPlayerOneCamera = true;

            position = Vector3.Lerp(position, currentPlayerOnePosition, 0.03f);

            if ((Math.Abs(position.X - currentPlayerOnePosition.X) <= 0.01) && (Math.Abs(position.Y - currentPlayerOnePosition.Y) <= 0.01) && (Math.Abs(position.Z - currentPlayerOnePosition.Z) <= 0.01))
            {

                return true;

            }

            UpdateViewMatrix();
            return false;
        }

        public bool setPlayerTwoCamera()
        {
            isLerping = true;
            isPlayerOneCamera = false;

            position = Vector3.Lerp(position, currentPlayerTwoPosition, 0.03f);

            if ((Math.Abs(position.X - currentPlayerTwoPosition.X) <= 0.01) && (Math.Abs(position.Y - currentPlayerTwoPosition.Y) <= 0.01) && (Math.Abs(position.Z - currentPlayerTwoPosition.Z) <= 0.01))
            {
                
                return true;
            }

            UpdateViewMatrix();
            return false;
        }

        private void UpdateworldMatrix()
        {
            Quaternion rotation;
            if (isPlayerOneCamera)
                rotation = Quaternion.Concatenate(Quaternion.CreateFromAxisAngle(P1yAxis, MathHelper.ToRadians(P1xRotation)), Quaternion.CreateFromAxisAngle(P1xAxis, MathHelper.ToRadians(P1yRotation)));
            else
                rotation = Quaternion.Concatenate(Quaternion.CreateFromAxisAngle(P2yAxis, MathHelper.ToRadians(P2xRotation)), Quaternion.CreateFromAxisAngle(P2xAxis, MathHelper.ToRadians(P2yRotation)));
            worldMatrix = Matrix.CreateFromQuaternion(rotation);
        }

        private void UpdateViewMatrix()
        {
            if (isPlayerOneCamera)
            {
                currentPlayerOnePosition = Vector3.Transform(currentPlayerOnePosition, Matrix.CreateFromQuaternion(Quaternion.Concatenate(P1CurrentXRotation, P1CurrentYRotation)));
                upVector = Vector3.Transform(upVector, Matrix.CreateFromQuaternion(Quaternion.Concatenate(P1CurrentXRotation, P1CurrentYRotation)));
                //currentPlayerOnePosition = position;
            }
            else
            {
                currentPlayerTwoPosition = Vector3.Transform(currentPlayerTwoPosition, Matrix.CreateFromQuaternion(Quaternion.Concatenate(P2CurrentXRotation, P2CurrentYRotation)));
                upVector = Vector3.Transform(upVector, Matrix.CreateFromQuaternion(Quaternion.Concatenate(P1CurrentXRotation, P1CurrentYRotation)));
            }
            UpdateworldMatrix();

            if (isLerping)
            {
                viewMatrix = Matrix.CreateLookAt(position, Target, upVector);
            }
            else
            {
                if (isPlayerOneCamera)
                    viewMatrix = Matrix.CreateLookAt(currentPlayerOnePosition, Target, Vector3.Up);
                else
                    viewMatrix = Matrix.CreateLookAt(currentPlayerTwoPosition, Target, Vector3.Up);
            }
        }

 




    }
}
