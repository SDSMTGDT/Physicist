namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Actors;
    using Physicist.Extensions;

    public class MapObject : IXmlSerializable
    {
        private List<Tuple<Texture2D, Vector2>> textures = new List<Tuple<Texture2D, Vector2>>();
        private bool fill = false;

        public MapObject(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.XmlDeserialize(element);
        }

        public MapObject(Body body, BodyInfo bodyInfo, string textureRef)
        {
            this.TextureReference = textureRef;
            this.MapBody = body;
            this.MapBodyInfo = bodyInfo;
            this.textures.Add(new Tuple<Texture2D, Vector2>(ContentController.Instance.GetContent<Texture2D>(textureRef), Vector2.Zero));
        }

        public string TextureReference
        {
            get;
            private set;
        }

        public Body MapBody
        {
            get;
            private set;
        }

        public BodyInfo MapBodyInfo
        {
            get;
            private set;
        }

        public void Draw(SpriteBatch sb)
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
       
        public XElement XmlSerialize()
        {
            var bodyXml = this.MapBodyInfo.XmlSerialize();
            bodyXml.Add(new XAttribute("textureRef", this.TextureReference));
            bodyXml.Add(new XAttribute("class", this.GetType().FullName));
            return bodyXml;
        }

        public void XmlDeserialize(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var bodyData = XmlBodyFactory.DeserializeBody(element);
            this.MapBody = bodyData.Item1;
            this.MapBodyInfo = bodyData.Item2;
            this.TextureReference = element.Attribute("textureRef").Value;

            XAttribute fillAtt = element.Attribute("fill");
            if (fillAtt != null)
            {
                this.fill = bool.Parse(fillAtt.Value);
            }

            if (this.fill && this.MapBodyInfo.BodyCategory != Enums.BodyCategory.LoopShape)
            {
                foreach (var fixture in this.MapBody.FixtureList)
                {
                    Tuple<Texture2D, Vector2> textureInfo = new Tuple<Texture2D, Vector2>(
                                                AssetCreator.Instance.TextureFromShape(fixture.Shape, this.TextureReference, Color.White, 1f),
                                                this.FixtureOffset(fixture));

                    this.textures.Add(textureInfo);
                }
            }
            else
            {
                this.textures.Add(new Tuple<Texture2D, Vector2>(ContentController.Instance.GetContent<Texture2D>(this.TextureReference), Vector2.Zero));
            }
        }

        private Vector2 FixtureOffset(Fixture fixture)
        {
            Vector2 bodyCenter = this.MapBody.WorldCenter;
            FarseerPhysics.Common.Transform bodyTrans;
            this.MapBody.GetTransform(out bodyTrans);

            FarseerPhysics.Collision.AABB bounds;
            fixture.Shape.ComputeAABB(out bounds, ref bodyTrans, 0);
            Vector2 offset = Vector2.Zero;

            switch (this.MapBodyInfo.BodyCategory)
            {
                case Enums.BodyCategory.Rectangle:
                case Enums.BodyCategory.Capsule:
                case Enums.BodyCategory.Ellipse:
                case Enums.BodyCategory.Circle:
                    offset = bounds.Center - bodyCenter;
                    break;

                case Enums.BodyCategory.ChainShape:
                case Enums.BodyCategory.CompoundPolygon:
                case Enums.BodyCategory.Gear:
                case Enums.BodyCategory.Polygon:
                case Enums.BodyCategory.RoundedRectangle:
                case Enums.BodyCategory.SolidArc:
                    offset = bounds.LowerBound - this.MapBodyInfo.Position + this.MapBodyInfo.ShapeOffset;
                    break;
                
                case Enums.BodyCategory.LineArc:
                    offset = new Vector2(-this.MapBodyInfo.Radians / 2f, 0);
                    break;
            }

            return offset;
        }
    }
}
