namespace Physicist.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Physicist.Actors;
    using Physicist.Enums;

    public static class XmlBodyFactory
    {
        private static World world;

        public static void SetWorld(World value)
        {
            XmlBodyFactory.world = value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Physics Engine Tracks objects")]
        public static Tuple<Body, BodyInfo> DeserializeBody(XElement element)
        {
            BodyInfo bodyInfo = null;
            Body body = null;

            bodyInfo = new BodyInfo(element);

            switch (bodyInfo.BodyCategory)
            {
                case BodyCategory.Capsule:
                    body = BodyFactory.CreateCapsule(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.Height.ToSimUnits(),
                                                        bodyInfo.TopRadius.ToSimUnits(),
                                                        bodyInfo.TopEdge,
                                                        bodyInfo.BottomRadius.ToSimUnits(),
                                                        bodyInfo.BottomEdge,
                                                        bodyInfo.Density,
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.ChainShape:
                    body = BodyFactory.CreateChainShape(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.Vertices.ElementAt(0).ToSimUnits(),
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.Circle:
                    body = BodyFactory.CreateCircle(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.Radius.ToSimUnits(),
                                                        bodyInfo.Density,
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.CompoundPolygon:
                    List<FarseerPhysics.Common.Vertices> convertedVerts = new List<FarseerPhysics.Common.Vertices>();
                    foreach (var vert in bodyInfo.Vertices)
                    {
                        convertedVerts.Add(vert.ToSimUnits());
                    }

                    body = BodyFactory.CreateCompoundPolygon(
                                                        XmlBodyFactory.world,
                                                        convertedVerts,
                                                        bodyInfo.Density.ToSimUnits(),
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.Edge:
                    body = BodyFactory.CreateEdge(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.Start.ToSimUnits(),
                                                        bodyInfo.End.ToSimUnits());
                    body.Position = bodyInfo.Position;
                    break;

                case BodyCategory.Ellipse:
                    body = BodyFactory.CreateEllipse(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.XRadius.ToSimUnits(),
                                                        bodyInfo.YRadius.ToSimUnits(),
                                                        bodyInfo.Edges,
                                                        bodyInfo.Density,
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.Gear:
                    body = BodyFactory.CreateGear(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.Radius.ToSimUnits(),
                                                        bodyInfo.NumberOfTeeth,
                                                        bodyInfo.TipPercentage.ToSimUnits(),
                                                        bodyInfo.ToothHeight.ToSimUnits(),
                                                        bodyInfo.Density);

                    body.Position = bodyInfo.Position.ToSimUnits();
                    break;

                case BodyCategory.LineArc:
                    body = BodyFactory.CreateLineArc(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.Radians,
                                                        bodyInfo.Sides,
                                                        bodyInfo.Radius.ToSimUnits(),
                                                        bodyInfo.Position.ToSimUnits(),
                                                        bodyInfo.Angle,
                                                        bodyInfo.Closed);
                    break;

                case BodyCategory.LoopShape:
                    body = BodyFactory.CreateLoopShape(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.Vertices.ElementAt(0).ToSimUnits(),
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.Polygon:
                    body = BodyFactory.CreatePolygon(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.Vertices.ElementAt(0).ToSimUnits(),
                                                        bodyInfo.Density,
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.Rectangle:
                    body = BodyFactory.CreateRectangle(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.Width.ToSimUnits(),
                                                        bodyInfo.Height.ToSimUnits(),
                                                        bodyInfo.Density,
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.RoundedRectangle:
                    body = BodyFactory.CreateRoundedRectangle(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.Width.ToSimUnits(),
                                                        bodyInfo.Height.ToSimUnits(),
                                                        bodyInfo.XRadius.ToSimUnits(),
                                                        bodyInfo.YRadius.ToSimUnits(),
                                                        bodyInfo.Segments,
                                                        bodyInfo.Density,
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.SolidArc:
                    body = BodyFactory.CreateSolidArc(
                                                        XmlBodyFactory.world,
                                                        bodyInfo.Density,
                                                        bodyInfo.Radians,
                                                        bodyInfo.Sides,
                                                        bodyInfo.Radius.ToSimUnits(),
                                                        bodyInfo.Position.ToSimUnits(),
                                                        bodyInfo.Angle);

                    break;
            }

            body.BodyType = bodyInfo.BodyType;
            body.FixedRotation = bodyInfo.FixedRotation;
            body.Rotation = bodyInfo.Rotation;

            return new Tuple<Body, BodyInfo>(body, bodyInfo);
        }
    }
}
