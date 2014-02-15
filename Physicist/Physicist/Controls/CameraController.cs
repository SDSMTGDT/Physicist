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

        private Matrix originMatrix;
        
        private Matrix scaleMatrix;
        
        private Matrix rotationMatrix;
        
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

        public CameraController(Vector2 position, Vector2 origin, IPosition following, float zoom, float rotation, Vector2 bounds)
        {
            this.Bounds = bounds;
            this.Following = following;
            this.Zoom = zoom;
            this.Position = position;
            this.Rotation = rotation;
            this.Origin = origin;
        }

        public Viewport CameraViewport { get; set; }
        
        public IPosition Following { get; set; }

        public float Zoom
        { 
            get
            {
                return this.zoom;
            } 

            set
            {
                this.zoom = value;
                this.scaleMatrix = Matrix.CreateScale(new Vector3(this.Zoom, this.Zoom, 1));
                this.originScaleRotationMatrix = this.rotationMatrix * this.scaleMatrix * this.originMatrix;
            }
        }

        public Vector2 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
                this.translationMatrix = Matrix.CreateTranslation(new Vector3(this.Position.X, this.Position.Y, 0));
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
                this.rotationMatrix = this.rotationMatrix = Matrix.CreateRotationZ(this.Rotation);
                this.originScaleRotationMatrix = this.rotationMatrix * this.scaleMatrix * this.originMatrix;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return this.origin;
            }
            
            set
            {
                this.origin = value;
                this.originMatrix = Matrix.CreateTranslation(new Vector3(this.Origin.X, this.Origin.Y, 0));
                this.originScaleRotationMatrix = this.rotationMatrix * this.scaleMatrix * this.originMatrix;
            }
        }

        public Matrix Transform
        {
            get
            {
                return this.translationMatrix * this.originScaleRotationMatrix;
            }
        }

        public void Move(Vector2 direction)
        {
            this.Position += direction;
        }

        public void Rotate(float angle)
        {
            this.Rotation += angle;
        }

        public void ZoomIn(float magnitude)
        {
            this.Zoom += magnitude;
        }

        public void ZoomOut(float magnitude)
        {
            this.Zoom -= magnitude;
        }

        public void CenterOnFollowing()
        {
            // Define the camera's position as centered on the player (or other object, if so desired)
            this.Position = new Vector2(
                (-1) * Math.Min(this.Bounds.X - this.CameraViewport.ViewportSize.Width, Math.Max(0, this.Following.Position.X - ((this.CameraViewport.ViewportSize.Width / 2) / this.Zoom))), 
                (-1) * Math.Min(this.Bounds.Y - this.CameraViewport.ViewportSize.Height, Math.Max(0, this.Following.Position.Y - ((this.CameraViewport.ViewportSize.Height / 2) / this.Zoom))));
        }
    }
}