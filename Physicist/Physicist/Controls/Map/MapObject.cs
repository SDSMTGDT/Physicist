namespace Physicist.Controls
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Actors;
    using Physicist.Extensions;

    public class MapObject : IXmlSerializable
    {
        private Texture2D texture;

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
            this.texture = ContentController.Instance.GetContent<Texture2D>(textureRef);
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
                sb.Draw(this.texture, this.MapBodyInfo.Position - this.MapBodyInfo.ShapeOffset, Color.White);
            }
        }
       
        public XElement XmlSerialize()
        {
            var bodyXml = this.MapBodyInfo.XmlSerialize();
            bodyXml.Add(new XAttribute("textureref", this.TextureReference));
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
            this.TextureReference = element.Attribute("textureref").Value;
            this.texture = ContentController.Instance.GetContent<Texture2D>(this.TextureReference);
        }
    }
}
