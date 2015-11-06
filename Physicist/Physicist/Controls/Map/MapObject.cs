namespace Physicist.MainGame.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.MainGame.Actors;
    using Physicist.Types.Controllers;
    using Physicist.Types.Enums;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Util;
    using Physicist.MainGame.Extensions;

    public class MapObject : PhysicistGameScreenItem, IMapObject
    {
        private List<Tuple<Texture2D, Vector2>> textures = new List<Tuple<Texture2D, Vector2>>();
        private bool fill = false;
        private Body mapBody = null;

        public MapObject()
        {
        }

        public MapObject(Body body, BodyInfo bodyInfo, string textureRef)
        {
            this.TextureReference = textureRef;
            this.Body = body;
            this.MapBodyInfo = bodyInfo;
            this.textures.Add(new Tuple<Texture2D, Vector2>(ContentController.Instance.GetContent<Texture2D>(textureRef), Vector2.Zero));
            this.Body.CollisionCategories = PhysicistCategory.Map1;
        }

        public string TextureReference { get; private set; }

        public Body Body 
        {
            get
            {
                return this.mapBody;
            }

            set
            {
                this.mapBody = value;
                if (value != null)
                {
                    this.mapBody.UserData = this;
                }
            }
        }

        public BodyInfo MapBodyInfo { get; private set; }

        public void Draw(ISpritebatch sb)
        {
            if (sb != null)
            {
                foreach (var texture in this.textures)
                {
                    if (texture.Item1 != null)
                    {
                        sb.Draw(
                                texture.Item1,
                                this.MapBodyInfo.Position + texture.Item2,
                                null,
                                Color.White,
                                this.Body.Rotation,
                                this.MapBodyInfo.ShapeOffset,
                                1f,
                                SpriteEffects.None,
                                this.fill ? 0.6f : .5f);
                    }
                }
            }
        }
       
        public override XElement XmlSerialize()
        {
            var bodyXml = this.MapBodyInfo.XmlSerialize();
            bodyXml.Add(
                new XAttribute("textureRef", this.TextureReference),
                new XAttribute("class", this.GetType().FullName));

            return bodyXml;
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                var bodyData = XmlBodyFactory.DeserializeBody(this.World, this.Map.Height, element);
                this.Body = bodyData.Item1;
                this.MapBodyInfo = bodyData.Item2;
                this.TextureReference = element.GetAttribute("textureRef", string.Empty);

                this.fill = element.GetAttribute("fill", false);

                if (this.fill)
                {
                    if (this.MapBodyInfo.BodyCategory != BodyCategory.LoopShape)
                    {
                        foreach (var fixture in this.Body.FixtureList)
                        {
                            Tuple<Texture2D, Vector2> textureInfo = new Tuple<Texture2D, Vector2>(
                                                        AssetCreator.Instance.TextureFromShape(fixture.Shape, this.TextureReference, Color.White, 1f),
                                                        fixture.FixtureOffset(this.MapBodyInfo.BodyCategory, this.MapBodyInfo.Position, this.MapBodyInfo.ShapeOffset));

                            this.textures.Add(textureInfo);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error! Loop Shape is not supported for fill");
                    }
                }
                else
                {
                    this.textures.Add(new Tuple<Texture2D, Vector2>(ContentController.Instance.GetContent<Texture2D>(this.TextureReference), Vector2.Zero));
                }

                if (this.Body != null)
                {
                    this.Body.CollisionCategories = PhysicistCategory.Map1;
                }
            }
        }
    }
}
