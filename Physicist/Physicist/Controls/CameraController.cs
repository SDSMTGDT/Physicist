namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Physicist.Actors;
    using Physicist.Extensions;

    public class CameraController
    {
        private Matrix originScaleRotationMatrix;
        
        private Matrix originMatrix;
        
        private Matrix scaleMatrix;
        
        private Matrix rotationMatrix;
        
        private Vector2 origin;
        
        private float zoom;
        
        private float rotation;

        public CameraController()
        {
            this.Following = null;
            this.Zoom = 1;
            this.Position = Vector2.Zero;
            this.Rotation = 0;
            this.Origin = Vector2.Zero;
        }

        public CameraController(Vector2 position, Vector2 origin, Actor following, float zoom, float rotation)
        {
            this.Following = following;
            this.Zoom = zoom;
            this.Position = position;
            this.Rotation = rotation;
            this.Origin = origin;
        }

        public Viewport CameraViewport { get; set; }
        
        public Actor Following { get; set; }

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
        
        public Vector2 Position { get; set; }

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
                var translationMatrix = Matrix.CreateTranslation(new Vector3(this.Position.X, this.Position.Y, 0));
                return translationMatrix * this.originScaleRotationMatrix;
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
            this.Position = new Vector2(Math.Max(0, this.Following.Position.X - (this.CameraViewport.ViewportSize.Width / 2)), Math.Max(0, this.Following.Position.Y - (this.CameraViewport.ViewportSize.Height / 2)));
        }
    }
}