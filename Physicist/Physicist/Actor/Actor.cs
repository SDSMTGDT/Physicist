﻿namespace Physicist.MainGame.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using FarseerPhysics;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.MainGame.Controls;
    using Physicist.MainGame.Extensions;
    using Physicist.Types.Common;
    using Physicist.Types.Controllers;
    using Physicist.Types.Enums;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Util;

    public class Actor : PhysicistGameScreenItem, IActor
    {
        private Dictionary<string, GameSprite> sprites = new Dictionary<string, GameSprite>();
        private Dictionary<string, Guid> playingSounds = new Dictionary<string, Guid>();
        private AudioEmitter audioEmitter = new AudioEmitter();
        private Body body;
        private BodyInfo bodyInfo;
        private int health;

        public Actor()
        {
        }

        public Actor(string name)
        {
            this.RotatesWithWorld = true;
            this.PathManager = new PathManager(this);
            this.VisibleState = Visibility.Visible;
            this.IsEnabled = true;
            this.CanBeDamaged = true;
            this.Health = 1;
            this.Name = name;
        }

        public string Name { get; private set; }

        public bool CanBeDamaged { get; set; }

        // Farseer Structures
        public Body Body
        {
            get
            {
                return this.body;
            }

            set
            {
                if (value != null)
                {
                    if (this.body != null)
                    {
                        this.body.OnCollision -= this.OnCollision;
                    }

                    this.body = value;
                    this.body.UserData = this;
                    this.body.OnCollision += this.OnCollision;
                }
            }
        }
       
        // 2space variables
        public Vector2 Position
        {
            get
            {
                return this.body.Position.ToDisplayUnits();
            }

            set
            {
                this.body.Position = value.ToSimUnits();
            }
        }

        public Vector2 CenteredPosition
        {
            get
            {
                var centered = this.body.Position.ToDisplayUnits();

                // Assume that the first fixture is the rotation fixture
                FarseerPhysics.Collision.AABB aabb;
                this.body.FixtureList[0].GetAABB(out aabb, 0);

                centered.X -= aabb.Width;
                centered.Y -= aabb.Height;

                return centered;
            }
        }

        public float Rotation
        {
            get
            {
                return this.body.Rotation;
            }

            set
            {
                this.body.Rotation = value;
            }
        }

        public PathManager PathManager { get; set; }

        public Vector2 MovementSpeed { get; set; }

        // gameplay state variables
        public int Health
        {
            get
            {
                return this.health;
            }

            set
            {
                this.health = value;
                if (this.health <= 0)
                {
                    this.VisibleState = Visibility.Hidden;
                    this.IsEnabled = false;
                    this.body.CollidesWith = PhysicistCategory.None;
                }
            }
        }

        public bool IsEnabled { get; set; }

        public bool IsDead
        {
            get
            {
                return this.Health <= 0;
            }
        }

        // draw properties
        public Dictionary<string, GameSprite> Sprites
        {
            get
            {
                return this.sprites;
            }
        }

        public Visibility VisibleState { get; set; }

        protected BodyInfo BodyInfo
        {
            get
            {
                return this.bodyInfo;
            }
        }

        protected bool RotatesWithWorld { get; set; }

        protected Dictionary<string, Guid> PlayingSounds
        {
            get
            {
                return this.playingSounds;
            }
        }

        public virtual void Draw(ISpritebatch sb)
        {
            if (this.IsEnabled)
            {
                if (sb != null && this.VisibleState == Visibility.Visible)
                {
                    foreach (var sprite in this.Sprites.Values)
                    {
                        var effect = SpriteEffects.None;
                        if (sprite.CurrentAnimation.FlipHorizontal)
                        {
                            effect |= SpriteEffects.FlipHorizontally;
                        }

                        if (sprite.CurrentAnimation.FlipVertical)
                        {
                            effect |= SpriteEffects.FlipVertically;
                        }

                        // if the actor does not rotate with the world, keep it upright
                        float drawRotation = this.RotatesWithWorld ? this.Rotation + (-1 * this.Screen.ScreenRotation) : this.Rotation;

                        sb.Draw(
                            sprite.SpriteSheet,
                            this.Position - Vector2.Transform(this.bodyInfo.ShapeOffset, Matrix.CreateRotationZ(drawRotation)) + sprite.Offset,
                            sprite.CurrentSprite,
                            Color.White,
                            drawRotation,
                            Vector2.Zero,
                            1f,
                            effect,
                            sprite.Depth);
                    }
                }
            }
        }

        public virtual void AddSprite(GameSprite sprite)
        {
            if (sprite != null)
            {
                this.Sprites.Add(sprite.SpriteName, sprite);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            // update every sprite in the sprite collection
            if (this.IsEnabled)
            {
                foreach (var sprite in this.Sprites.Values)
                {
                    sprite.Update(gameTime);
                }

                this.PathManager.Update(gameTime);
            }
        }

        // Implementing Interface   
        public override XElement XmlSerialize()
        {
            // define the Actor element
            XElement actorElement = new XElement(
                "Actor",
                new XAttribute("class", "Actor"),
                new XAttribute("name", this.Name),
                new XAttribute("health", this.Health),
                new XAttribute("rotation", this.Rotation),
                new XAttribute("isEnabled", this.IsEnabled),
                new XAttribute("visibleState", this.VisibleState.ToString()),
                new XAttribute("canBeDamaged", this.CanBeDamaged),
                this.MovementSpeed.XmlSerialize("MovementSpeed"),
                new XElement("Sprites", this.sprites.Values.Select(sprite => sprite.XmlSerialize()).ToArray()),
                new XElement("BodyInfo", this.bodyInfo.XmlSerialize()));

            return actorElement;
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                // Create GameSprites out of the Deserialze functions in GameSprite
                var spriteElements = element.Element("Sprites");
                if (spriteElements != null)
                {
                    foreach (XElement gameSpriteEle in spriteElements.Elements("GameSprite"))
                    {
                        GameSprite gameSprite = new GameSprite();
                        gameSprite.XmlDeserialize(gameSpriteEle);
                        this.sprites.Add(gameSprite.SpriteName, gameSprite);
                    }
                }

                // ----------------------
                // Find the Body element
                XElement bodyElement = element.Element("BodyInfo");
                if (bodyElement != null)
                {
                    var bodyData = XmlBodyFactory.DeserializeBody(this.World, this.Map.Height, bodyElement.Elements().ElementAt(0));
                    this.Body = bodyData.Item1;
                    this.bodyInfo = bodyData.Item2;
                }

                this.PathManager = new PathManager(this);
                this.PathManager.Screen = this.Screen;
                XElement pathManagerEle = element.Element("PathManager");
                if (pathManagerEle != null)
                {
                    this.PathManager.XmlDeserialize(pathManagerEle);
                }

                // --------------------------------------------------------
                // Assign the new values to the Actor after body is created
                this.Name = element.GetAttribute("name", string.Empty);
                this.MovementSpeed = XmlDeserializeHelper.XmlDeserialize<Vector2>(element.Element("MovementSpeed"));
                this.Health = element.GetAttribute("health", 1);
                this.Rotation = element.GetAttribute("rotation", 0f);
                this.IsEnabled = element.GetAttribute("isEnabled", true);
                this.VisibleState = element.GetAttribute("visibleState", Visibility.Visible);
                this.RotatesWithWorld = element.GetAttribute("rotatesWithWorld", false);
                this.CanBeDamaged = element.GetAttribute("canBeDamaged", true);
            }
        }

        protected void StopSound(string name)
        {
            if (this.PlayingSounds.ContainsKey(name))
            {
                SoundController.StopSound(this.PlayingSounds[name]);
            }
        }

        protected void PauseSound(string name)
        {
            if (this.PlayingSounds.ContainsKey(name))
            {
                SoundController.PauseSound(this.PlayingSounds[name]);
            }
        }

        protected void ResumeSound(string name)
        {
            if (this.PlayingSounds.ContainsKey(name))
            {
                SoundController.ResumeSound(this.PlayingSounds[name]);
            }
        }

        protected bool SoundIsPlaying(string name)
        {
            if (this.PlayingSounds.ContainsKey(name))
            {
                return SoundController.SoundIsPlaying(this.PlayingSounds[name]);
            }

            return false;
        }

        protected virtual bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (this.IsEnabled && this.CanBeDamaged && fixtureB != null)
            {
                var collisionBody = fixtureB.Body;
                if (collisionBody != null)
                {
                    IDamage damagingBody = collisionBody.UserData as IDamage;
                    if (damagingBody != null)
                    {
                        this.Health -= damagingBody.AttackDamage;
                    }
                }
            }

            return this.IsEnabled;
        }

        protected void PlaySound(string name)
        {
            this.PlaySound(name, true, false, 1.0f, 0.0f, 0.0f);
        }

        protected void PlaySound(string name, bool localized, bool looping, float volume, float pitch, float pan)
        {
            if (!string.IsNullOrEmpty(name))
            {
                // check for every Guid in the sound controller.
                for (int i = this.PlayingSounds.Count - 1; i >= 0; i--)
                {
                    var sound = this.PlayingSounds.ElementAt(i);
                    if (!SoundController.ContainsInstance(sound.Value))
                    {
                        this.PlayingSounds.Remove(sound.Key);
                    }
                }

                Guid newGuid;

                if (localized)
                {
                    newGuid = SoundController.PlayLocalizedSound(name, this.audioEmitter, looping, volume, pitch, pan);
                }
                else
                {
                    newGuid = SoundController.PlaySound(name, looping, volume, pitch, pan);
                }

                if (this.PlayingSounds.ContainsKey(name))
                {
                    this.PlayingSounds[name] = newGuid;
                }
                else
                {
                    this.PlayingSounds.Add(name, newGuid);
                }
            }
        }
    }
}