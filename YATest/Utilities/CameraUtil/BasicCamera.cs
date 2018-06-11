#region Using
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 
#endregion

namespace YATest.Utilities.CameraUtil
{
    public abstract class BasicCamera : GameComponent, IControllable
    {
        public BasicCamera(Game game) : base(game) { }

        #region Protected Members
        protected Matrix viewMatrix;
        protected Matrix projectionMatrix;
        protected Viewport viewport;
        //protected Vector3 cameraPosition;
        protected MouseState newMouseState;
        protected MouseState oldMouseState;
        protected KeyboardState newKeyboardState;
        protected KeyboardState oldKeyboardState;
        protected float nearPlane = 1.0f;
        protected float farPlane = 1000.0f;
        protected float aspectRatio;
        protected Vector3 position;
        protected Vector3 lookAt;
        protected Matrix worldMatrix;
        protected bool controlsAreBlocked;
        protected Vector3 target;



        #endregion

        #region Properties
        
        protected Vector3 Target
        {
            get { return target; }
            set { target = value; }
        }

        public Matrix WorldMatrix
        {
            get { return worldMatrix; }
            set { worldMatrix = value; }
        }

        public Matrix ViewMatrix
        {
            set
            {
                viewMatrix = value;
            }
            get
            {
                return viewMatrix;
            }
        }

        public Matrix ProjectionMatrix
        {
            set
            {
                projectionMatrix = value;
            }
            get
            {
                return projectionMatrix;
            }
        }

        public Viewport Viewport
        {
            set
            {
                viewport = value;
            }
            get
            {
                return viewport;
            }
        }

        public Vector3 Position
        {
            set
            {
                position = value;
            }
            get
            {
                return position;
            }
        }

        public MouseState OldMouseState
        {
            set
            {
                oldMouseState = value;
            }
            get
            {
                return oldMouseState;
            }
        }

        public MouseState NewMouseState
        {
            set
            {
                newMouseState = value;
            }
            get
            {
                return newMouseState;
            }
        }

        public KeyboardState OldKeyboardState
        {
            set
            {
                oldKeyboardState = value;
            }
            get
            {
                return oldKeyboardState;
            }
        }

        public KeyboardState NewKeyboardState
        {
            set
            {
                newKeyboardState = value;
            }
            get
            {
                return newKeyboardState;
            }
        }

        public float NearPlane
        {
            set
            {
                nearPlane = value;
            }
            get
            {
                return nearPlane;
            }
        }

        public float FarPlane
        {
            set
            {
                farPlane = value;
            }
            get
            {
                return farPlane;
            }
        }

        public float AspectRatio
        {
            set
            {
                aspectRatio = value;
            }
            get
            {
                return aspectRatio;
            }
        }

        public Vector3 LookAt
        {
            set
            {
                lookAt = value;
            }
            get
            {
                return lookAt;
            }
        } 
        #endregion

        //public abstract void update();

        #region IControllable Members

        public override void Update(GameTime gameTime)
        {

            if (Blocked == false)
            {
                HandleKeyboardInput();
                HandleMouseInput();
            }
            base.Update(gameTime);
        }

        public virtual void HandleKeyboardInput()
        {
            ;
        }

        public virtual void HandleMouseInput()
        {
            ;
        }

        public bool Blocked
        {
            get
            {
                return controlsAreBlocked;
            }
            set
            {
                controlsAreBlocked = value;
            }
        }

        #endregion
    }

    public enum CameraBehavior { SpectatorCamera, SpectatorCameraWithoutMouse };
}
