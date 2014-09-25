namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Extensions;

    /// <summary>
    /// The camera controller.  Should be present in the main game only, 
    /// although multiple instances could be used for cinematic purposes. 
    /// 
    /// To modify a camera's property, use the following:
    ///     -   Camera.Following: sets the object for the camera to follow
    ///     -   ZoomIn(): zooms in a given amount
    ///     -   ZoomOut(): zooms out a given amount
    ///     -   Move(): moves the camera around
    ///     -   CenterOnFollowing(): focuses on Following, should be used like an update step.
    /// </summary>
    public class CameraController
    {
        private Matrix originScaleRotationMatrix;
        private Matrix translationMatrix;
        
        private Vector2 origin;
        private Vector2 position;
        private float zoom;        
        private float rotation;

        public CameraController()
        {
            this.Bounds = Vector2.Zero;
            this.Following = null;
            this.Zoom = 1;
            this.Position = Vector2.Zero;
            this.Rotation = 0;
            this.Origin = Vector2.Zero;
        }

        public CameraController(Vector2 position, IPosition following, float zoom, float rotation, Vector2 bounds)
        {
            this.Bounds = bounds;
            this.Following = following;
            this.Zoom = zoom;
            this.Position = position;
            this.Rotation = rotation;
            this.Origin = new Vector2(bounds.X / 2, bounds.Y / 2);
        }

        public Viewport CameraViewport { get; set; }
        
        public IPosition Following { get; set; }

        public float Zoom
        { 
            get
            {
                return this.zoom;
            } 

            private set
            {
                this.zoom = value;
                this.UpdateOriginRotateScale();
            }
        }

        public Vector2 Position
        {
            get
            {
                return this.position;
            }

            private set
            {
                this.position = value;
                this.translationMatrix = Matrix.CreateTranslation(new Vector3(this.position, 0)); // *Matrix.CreateRotationZ(-this.Rotation);
            }
        }

        public Vector2 Bounds { get; set; }

        public float Rotation
        {
            get
            {
                return this.rotation;
            }

            set
            {
                this.rotation = value;
                this.UpdateOriginRotateScale();
            }
        }

        public Vector2 Origin
        {
            get
            {
                return this.origin;
            }
            
            private set
            {
                this.origin = value;
                this.UpdateOriginRotateScale();
            }
        }

        public Matrix Transform
        {
            get
            {
                if (this.Following != null)
                {
                    return this.translationMatrix * this.originScaleRotationMatrix * Matrix.CreateTranslation(this.CameraViewport.ViewportSize.Width / 2, this.CameraViewport.ViewportSize.Height / 2, 0);
                }
                else
                {
                    return this.translationMatrix * this.originScaleRotationMatrix;
                }
            }
        }

        public void Move(Vector2 direction)
        {
            this.Position = this.Position + direction;
        }

        public void Rotate(float angle)
        {
            this.Rotation += angle;
        }

        public void ZoomIn(float magnitude)
        {
            if (magnitude > 0)
            {
                this.Zoom *= magnitude;
            }
        }

        public void ZoomOut(float magnitude)
        {
            if (magnitude > 0)
            {
                this.Zoom /= magnitude;
            }
        }

        public void Reset()
        {
            this.Position = Vector2.Zero;
            this.Rotation = 0f;
            this.Zoom = 1f;
        }

        public void CenterOnFollowing()
        {   
            // Define the camera's position as centered on the player (or other object, if so desired)
            if (this.Following != null)
            {
                // Vector2 RotatedFollowing = Vector2.Transform(this.Following.CenteredPosition, Matrix.CreateRotationZ(-1 * this.Rotation));
                this.Position = new Vector2(
                    -1 * this.Following.CenteredPosition.X,
                    -1 * this.Following.CenteredPosition.Y);
            }
        }

        private void UpdateOriginRotateScale()
        {
            this.originScaleRotationMatrix = Matrix.CreateTranslation(new Vector3(-this.Origin.X, -this.Origin.Y, 0)) *
                                             Matrix.CreateScale(this.Zoom) *
                                             Matrix.CreateRotationZ(this.Rotation);
                                             Matrix.CreateTranslation(new Vector3(this.Origin.X, this.Origin.Y, 0));
        }
    }
}