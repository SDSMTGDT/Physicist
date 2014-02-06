namespace Physicist.Control
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using FarseerPhysics;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Physicist.Actors;
    using Physicist.Enums;

    public class MapObject
    {
        private BodyInfo bodyInfo;
        private Body mapBody;

        public void Deserialize(XElement element)
        {
            this.bodyInfo = new BodyInfo(element);

            switch (this.bodyInfo.Category)
            {
                // case bodycategory.breakablebody:                   
                //    this.mapbody = bodyfactory.createbreakablebody(maingame.world, this.bodyinfo.vertices.elementat(0), this.bodyinfo.density, this.bodyinfo.position);
                //    break;
                case BodyCategory.Capsule:
                    this.mapBody = BodyFactory.CreateCapsule(MainGame.World, this.bodyInfo.Height, this.bodyInfo.TopRadius, this.bodyInfo.TopEdge, this.bodyInfo.BottomRadius, this.bodyInfo.BottomEdge, this.bodyInfo.Density, this.bodyInfo.Position);
                    break;

                case BodyCategory.ChainShape:
                    this.mapBody = BodyFactory.CreateChainShape(MainGame.World, this.bodyInfo.Vertices.ElementAt(0), this.bodyInfo.Position);
                    break;

                case BodyCategory.Circle:
                    this.mapBody = BodyFactory.CreateCircle(MainGame.World, this.bodyInfo.Radius, this.bodyInfo.Density, this.bodyInfo.Position);
                    break;

                case BodyCategory.CompoundPolygon:
                    this.mapBody = BodyFactory.CreateCompoundPolygon(MainGame.World, this.bodyInfo.Vertices.ToList(), this.bodyInfo.Density, this.bodyInfo.Position);
                    break;

                case BodyCategory.Edge:
                    this.mapBody = BodyFactory.CreateEdge(MainGame.World, this.bodyInfo.Start, this.bodyInfo.End);
                    break;

                case BodyCategory.Ellipse:
                    this.mapBody = BodyFactory.CreateEllipse(MainGame.World, this.bodyInfo.XRadius, this.bodyInfo.YRadius, this.bodyInfo.Edges, this.bodyInfo.Density, this.bodyInfo.Position);
                    break;

                case BodyCategory.Gear:
                    this.mapBody = BodyFactory.CreateGear(MainGame.World, this.bodyInfo.Radius, this.bodyInfo.NumberOfTeeth, this.bodyInfo.TipPercentage, this.bodyInfo.ToothHeight, this.bodyInfo.Density);
                    this.mapBody.Position = this.bodyInfo.Position;
                    break;

                case BodyCategory.LineArc:
                    this.mapBody = BodyFactory.CreateLineArc(MainGame.World, this.bodyInfo.Radians, this.bodyInfo.Sides, this.bodyInfo.Radius, this.bodyInfo.Position, this.bodyInfo.Angle, this.bodyInfo.Closed);
                    break;

                case BodyCategory.LoopShape:
                    this.mapBody = BodyFactory.CreateLoopShape(MainGame.World, this.bodyInfo.Vertices.ElementAt(0), this.bodyInfo.Position);
                    break;

                case BodyCategory.Polygon:
                    this.mapBody = BodyFactory.CreatePolygon(MainGame.World, this.bodyInfo.Vertices.ElementAt(0), this.bodyInfo.Density, this.bodyInfo.Position);
                    break;

                case BodyCategory.Rectangle:
                    this.mapBody = BodyFactory.CreateRectangle(MainGame.World, this.bodyInfo.Width, this.bodyInfo.Height, this.bodyInfo.Density, this.bodyInfo.Position);
                    break;

                case BodyCategory.RoundedRectangle:
                    this.mapBody = BodyFactory.CreateRoundedRectangle(MainGame.World, this.bodyInfo.Width, this.bodyInfo.Height, this.bodyInfo.XRadius, this.bodyInfo.YRadius, this.bodyInfo.Segments, this.bodyInfo.Density, this.bodyInfo.Position);
                    break;

                case BodyCategory.SolidArc:
                    this.mapBody = BodyFactory.CreateSolidArc(MainGame.World, this.bodyInfo.Density, this.bodyInfo.Radians, this.bodyInfo.Sides, this.bodyInfo.Radius, this.bodyInfo.Position, this.bodyInfo.Angle);
                    break;
            }
        }
    }
}
