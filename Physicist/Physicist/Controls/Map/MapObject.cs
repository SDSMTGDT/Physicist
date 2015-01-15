namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Actors;
    using Physicist.Extensions;

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
            this.MapBody = body;
            this.MapBodyInfo = bodyInfo;
            this.textures.Add(new Tuple<Texture2D, Vector2>(ContentController.Instance.GetContent<Texture2D>(textureRef), Vector2.Zero));
            this.MapBody.CollisionCategories = PhysicistCategory.Map1;
        }

        public string TextureReference { get; private set; }

        public Body MapBody 
        {
            get
            {
                return this.mapBody;
            }

            private set
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
                                this.MapBody.Rotation,
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
                this.MapBody = bodyData.Item1;
                this.MapBodyInfo = bodyData.Item2;
                this.TextureReference = element.GetAttribute("textureRef", string.Empty);

                this.fill = element.GetAttribute("fill", false);

                if (this.fill)
                {
                    if (this.MapBodyInfo.BodyCategory != Enums.BodyCategory.LoopShape)
                    {
                        foreach (var fixture in this.MapBody.FixtureList)
                        {
                            Tuple<Texture2D, Vector2> textureInfo = new Tuple<Texture2D, Vector2>(
                                                        AssetCreator.Instance.TextureFromShape(fixture.Shape, this.TextureReference, Color.White, 1f),
                                                        fixture.FixtureOffset(this.MapBodyInfo.BodyCategory, this.MapBodyInfo.Position, this.MapBodyInfo.ShapeOffset));

                            this.textures.Add(textureInfo);
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Error! Loop Shape is not supported for fill");
                    }
                }
                else
                {
                    this.textures.Add(new Tuple<Texture2D, Vector2>(ContentController.Instance.GetContent<Texture2D>(this.TextureReference), Vector2.Zero));
                }

                if (this.MapBody != null)
                {
                    this.MapBody.CollisionCategories = PhysicistCategory.Map1;
                }
            }
        }
    }
}
