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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Physics Engine Tracks objects")]
        public static Tuple<Body, BodyInfo> DeserializeBody(World world, int mapHeight, XElement element)
        {
            BodyInfo bodyInfo = null;
            Body body = null;

            bodyInfo = new BodyInfo(mapHeight);
            bodyInfo.XmlDeserialize(element);

            switch (bodyInfo.BodyCategory)
            {
                case BodyCategory.Capsule:
                    body = BodyFactory.CreateCapsule(
                                                        world,
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
                                                        world,
                                                        bodyInfo.Vertices.ElementAt(0).ToSimUnits(),
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.Circle:
                    body = BodyFactory.CreateCircle(
                                                        world,
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
                                                        world,
                                                        convertedVerts,
                                                        bodyInfo.Density.ToSimUnits(),
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.Edge:
                    body = BodyFactory.CreateEdge(
                                                        world,
                                                        bodyInfo.Start.ToSimUnits(),
                                                        bodyInfo.End.ToSimUnits());
                    body.Position = bodyInfo.Position;
                    break;

                case BodyCategory.Ellipse:
                    body = BodyFactory.CreateEllipse(
                                                        world,
                                                        bodyInfo.XRadius.ToSimUnits(),
                                                        bodyInfo.YRadius.ToSimUnits(),
                                                        bodyInfo.Edges,
                                                        bodyInfo.Density,
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.Gear:
                    body = BodyFactory.CreateGear(
                                                        world,
                                                        bodyInfo.Radius.ToSimUnits(),
                                                        bodyInfo.NumberOfTeeth,
                                                        bodyInfo.TipPercentage.ToSimUnits(),
                                                        bodyInfo.ToothHeight.ToSimUnits(),
                                                        bodyInfo.Density);

                    body.Position = bodyInfo.Position.ToSimUnits();
                    break;

                case BodyCategory.LineArc:
                    body = BodyFactory.CreateLineArc(
                                                        world,
                                                        bodyInfo.Radians,
                                                        bodyInfo.Sides,
                                                        bodyInfo.Radius.ToSimUnits(),
                                                        bodyInfo.Position.ToSimUnits(),
                                                        bodyInfo.Angle,
                                                        bodyInfo.Closed);
                    break;

                case BodyCategory.LoopShape:
                    body = BodyFactory.CreateLoopShape(
                                                        world,
                                                        bodyInfo.Vertices.ElementAt(0).ToSimUnits(),
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.Polygon:
                    body = BodyFactory.CreatePolygon(
                                                        world,
                                                        bodyInfo.Vertices.ElementAt(0).ToSimUnits(),
                                                        bodyInfo.Density,
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.Rectangle:
                    body = BodyFactory.CreateRectangle(
                                                        world,
                                                        bodyInfo.Width.ToSimUnits(),
                                                        bodyInfo.Height.ToSimUnits(),
                                                        bodyInfo.Density,
                                                        bodyInfo.Position.ToSimUnits());
                    break;

                case BodyCategory.RoundedRectangle:
                    body = BodyFactory.CreateRoundedRectangle(
                                                        world,
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
                                                        world,
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
