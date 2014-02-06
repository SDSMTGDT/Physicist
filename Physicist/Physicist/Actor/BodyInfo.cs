namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using FarseerPhysics.Common;
    using Microsoft.Xna.Framework;
    using Physicist.Enums;

    public class BodyInfo
    {
        private BodyCategory? category = null;
        private List<Vertices> vertices = null;
        private Vector2? position = null;
        private float? density = null;
        private float? height = null;
        private float? width = null;
        private float? radius = null;
        private float? xradius = null;
        private float? yradius = null;
        private float? radians = null;
        private float? angle = null;
        private int? sides = null;
        private int? segments = null;
        private int? edges = null;
        private bool? closed = null;
        private float? topradius = null;
        private float? bottomradius = null;
        private int? topedge = null;
        private int? bottomedge = null;
        private Vector2? start = null;
        private Vector2? end = null;
        private float? tippercentage = null;
        private float? toothheight = null;
        private int? numberofteeth = null;

        public BodyInfo(XElement element)
        {
            this.XmlDeserialize(element);
        }

        // Global to all Body
        public BodyCategory Category
        {
            get { return this.category.Value; }
        }

        public Vector2 Position 
        {
            get { return this.position.Value; } 
        }
        
        // Common
        public IEnumerable<Vertices> Vertices 
        {
            get { return this.vertices; }
        }

        public float Density 
        { 
            get { return this.density.Value; } 
        }

        public float Height 
        {
            get { return this.height.Value; } 
        }

        public float Width 
        {
            get { return this.width.Value; } 
        }

        public float Radius 
        { 
            get { return this.radius.Value; }
        }

        // Spherical shapes
        public float XRadius 
        {
            get { return this.xradius.Value; } 
        }

        public float YRadius 
        {
            get { return this.yradius.Value; } 
        }

        public float Radians 
        {
            get { return this.radians.Value; } 
        }

        public float Angle 
        {
            get { return this.angle.Value; } 
        }

        public int Sides 
        {
            get { return this.sides.Value; } 
        }

        public int Segments 
        {
            get { return this.segments.Value; } 
        }

        public int Edges 
        {
            get { return this.edges.Value; } 
        }

        public bool Closed 
        {
            get { return this.closed.Value; } 
        }
        
        // Capsule
        public float TopRadius 
        {
            get { return this.topradius.Value; } 
        }

        public float BottomRadius 
        {
            get { return this.bottomradius.Value; } 
        }

        public int TopEdge 
        {
            get { return this.topedge.Value; } 
        }

        public int BottomEdge 
        {
            get { return this.bottomedge.Value; } 
        }

        // Edge
        public Vector2 Start 
        {
            get { return this.start.Value; } 
        }

        public Vector2 End 
        {
            get { return this.end.Value; } 
        }

        // Gear
        public float TipPercentage 
        {
            get { return this.tippercentage.Value; } 
        }

        public float ToothHeight 
        {
            get { return this.toothheight.Value; } 
        }

        public int NumberOfTeeth 
        {
            get { return this.numberofteeth.Value; } 
        }

        public XElement XmlSerialize()
        {
            XElement bodyInfoXml = new XElement(Enum.GetName(typeof(BodyCategory), this.category.Value));

            if (this.position.HasValue)
            {
                bodyInfoXml.Add(new XElement("position", new XAttribute("x", this.position.Value.X), new XAttribute("y", this.position.Value.Y)));
            }

            if (this.vertices != null)
            {
                XElement verts = new XElement("vertices");
                foreach (var vert in this.vertices)
                {
                    XElement vertice = new XElement("vertice");
                    foreach (var vector in vert)
                    {
                        vertice.Add(new XElement("Vector2", new XAttribute("x", vector.X), new XAttribute("y", vector.Y)));
                    }

                    verts.Add(vertice);
                }

                bodyInfoXml.Add(verts);
            }

            if (this.density.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("density", this.density.Value));
            }

            if (this.height.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("height", this.height.Value));
            }

            if (this.width.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("width", this.width.Value));
            }

            if (this.radius.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("radius", this.radius.Value));
            }

            if (this.xradius.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("xRadius", this.xradius.Value));
            }

            if (this.yradius.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("yRadius", this.yradius.Value));
            }

            if (this.radians.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("radians", this.radians.Value));
            }

            if (this.angle.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("angle", this.angle.Value));
            }

            if (this.sides.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("sides", this.sides.Value));
            }

            if (this.segments.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("segments", this.segments.Value));
            }

            if (this.edges.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("edges", this.edges.Value));
            }

            if (this.topradius.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("topRad", this.topradius.Value));
            }

            if (this.bottomradius.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("botRad", this.bottomradius.Value));
            }

            if (this.topedge.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("topEdge", this.topedge.Value));
            }

            if (this.bottomedge.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("botEdge", this.bottomedge.Value));
            }

            if (this.start.HasValue)
            {
                bodyInfoXml.Add(new XElement("start", new XAttribute("x", this.start.Value.X), new XAttribute("y", this.start.Value.Y)));
            }

            if (this.end.HasValue)
            {
                bodyInfoXml.Add(new XElement("end", new XAttribute("x", this.end.Value.X), new XAttribute("y", this.end.Value.Y)));
            }

            if (this.closed.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("closed", this.closed.Value));
            }

            if (this.tippercentage.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("tipPercentage", this.tippercentage.Value));
            }

            if (this.toothheight.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("toothHeight", this.toothheight.Value));
            }

            if (this.numberofteeth.HasValue)
            {
                bodyInfoXml.Add(new XAttribute("numberOfTeeth", this.numberofteeth.Value));
            }

            return bodyInfoXml;
        }

        public void XmlDeserialize(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            BodyCategory cat = (BodyCategory)Enum.Parse(typeof(BodyCategory), element.Name.LocalName);

            if (cat != BodyCategory.Edge)
            {
                this.position = new Vector2(
                                        float.Parse(element.Element("position").Attribute("x").Value, CultureInfo.CurrentCulture),
                                        float.Parse(element.Element("position").Attribute("y").Value, CultureInfo.CurrentCulture));
            }

            switch (cat)
            {
                case BodyCategory.BreakableBody:
                    this.MakeBreakableBody(element);
                    break;

                case BodyCategory.Capsule:
                    this.MakeCapsule(element);
                    break;

                case BodyCategory.ChainShape:
                    this.MakeChainShape(element);
                    break;

                case BodyCategory.Circle:
                    this.MakeCircle(element);
                    break;

                case BodyCategory.CompoundPolygon:
                    this.MakeCompoundPolygon(element);
                    break;

                case BodyCategory.Edge:
                    this.MakeEdge(element);
                    break;

                case BodyCategory.Ellipse:
                    this.MakeEllipse(element);
                    break;

                case BodyCategory.Gear:
                    this.MakeGear(element);
                    break;

                case BodyCategory.LineArc:
                    this.MakeLineArc(element);
                    break;

                case BodyCategory.LoopShape:
                    this.MakeLoopShape(element);
                    break;

                case BodyCategory.Polygon:
                    this.MakePolygon(element);
                    break;

                case BodyCategory.Rectangle:
                    this.MakeRectangle(element);
                    break;

                case BodyCategory.RoundedRectangle:
                    this.MakeRoundedRectangle(element);
                    break;

                case BodyCategory.SolidArc:
                    this.MakeSolidArc(element);
                    break;
            }
        }
      
        private void MakeBreakableBody(XElement element)
        {
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            Vertices verts = new Vertices();
            foreach (var vert in element.Element("verticies").Elements())
            {
                verts.Add(new Vector2(
                                        float.Parse(vert.Attribute("x").Value, CultureInfo.CurrentCulture),
                                        float.Parse(vert.Attribute("y").Value, CultureInfo.CurrentCulture)));
            }

            this.vertices = new List<Vertices>() { verts };
        }

        private void MakeCapsule(XElement element)
        {
            this.height = float.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture);
            this.width = float.Parse(element.Attribute("width").Value, CultureInfo.CurrentCulture);
            this.topradius = float.Parse(element.Attribute("topRadius").Value, CultureInfo.CurrentCulture);
            this.bottomradius = float.Parse(element.Attribute("bottomRadius").Value, CultureInfo.CurrentCulture);
            this.topedge = int.Parse(element.Attribute("topEdge").Value, CultureInfo.CurrentCulture);
            this.bottomedge = int.Parse(element.Attribute("bottomEdge").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
        }

        private void MakeChainShape(XElement element)
        {
            Vertices verts = new Vertices();
            foreach (var vert in element.Element("verticies").Elements())
            {
                verts.Add(new Vector2(
                                        float.Parse(vert.Attribute("x").Value, CultureInfo.CurrentCulture),
                                        float.Parse(vert.Attribute("y").Value, CultureInfo.CurrentCulture)));
            }

            this.vertices = new List<Vertices>() { verts };
        }

        private void MakeCircle(XElement element)
        {
            this.radians = float.Parse(element.Attribute("radians").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
        }

        private void MakeCompoundPolygon(XElement element)
        {
            this.vertices = new List<Vertices>();
            foreach (var verticeslist in element.Element("vertices").Elements())
            {
                Vertices nextVerts = new Vertices();
                foreach (var vert in verticeslist.Elements())
                {
                    nextVerts.Add(new Vector2(
                                float.Parse(vert.Attribute("x").Value, CultureInfo.CurrentCulture),
                                float.Parse(vert.Attribute("y").Value, CultureInfo.CurrentCulture)));
                }

                this.vertices.Add(nextVerts);
            }

            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
        }

        private void MakeEdge(XElement element)
        {
            this.start = new Vector2(
                                float.Parse(element.Element("start").Attribute("x").Value, CultureInfo.CurrentCulture),
                                float.Parse(element.Element("start").Attribute("y").Value, CultureInfo.CurrentCulture));

            this.end = new Vector2(
                                float.Parse(element.Element("end").Attribute("x").Value, CultureInfo.CurrentCulture),
                                float.Parse(element.Element("end").Attribute("y").Value, CultureInfo.CurrentCulture));
        }

        private void MakeEllipse(XElement element)
        {
            this.xradius = float.Parse(element.Attribute("xRadius").Value, CultureInfo.CurrentCulture);
            this.yradius = float.Parse(element.Attribute("yRadius").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.edges = int.Parse(element.Attribute("edges").Value, CultureInfo.CurrentCulture);
        }

        private void MakeGear(XElement element)
        {
            this.radius = float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture);
            this.numberofteeth = int.Parse(element.Attribute("numberOfTeeth").Value, CultureInfo.CurrentCulture);
            this.tippercentage = float.Parse(element.Attribute("tipPercentage").Value, CultureInfo.CurrentCulture);
            this.toothheight = float.Parse(element.Attribute("toothHeight").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
        }

        private void MakeLineArc(XElement element)
        {
            this.radians = float.Parse(element.Attribute("radians").Value, CultureInfo.CurrentCulture);
            this.sides = int.Parse(element.Attribute("sides").Value, CultureInfo.CurrentCulture);
            this.radius = float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture);
            this.angle = float.Parse(element.Attribute("angle").Value, CultureInfo.CurrentCulture);
            this.closed = bool.Parse(element.Attribute("closed").Value);
        }

        private void MakeLoopShape(XElement element)
        {
            Vertices verts = new Vertices();
            foreach (var vert in element.Element("verticies").Elements())
            {
                verts.Add(new Vector2(
                                        float.Parse(vert.Attribute("x").Value, CultureInfo.CurrentCulture),
                                        float.Parse(vert.Attribute("y").Value, CultureInfo.CurrentCulture)));
            }

            this.vertices = new List<Vertices>() { verts };
        }

        private void MakePolygon(XElement element)
        {
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            Vertices verts = new Vertices();
            foreach (var vert in element.Element("verticies").Elements())
            {
                verts.Add(new Vector2(
                                        float.Parse(vert.Attribute("x").Value, CultureInfo.CurrentCulture),
                                        float.Parse(vert.Attribute("y").Value, CultureInfo.CurrentCulture)));
            }

            this.vertices = new List<Vertices>() { verts };
        }

        private void MakeRectangle(XElement element)
        {
            this.height = float.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture);
            this.width = float.Parse(element.Attribute("width").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
        }

        private void MakeRoundedRectangle(XElement element)
        {
            this.width = float.Parse(element.Attribute("width").Value, CultureInfo.CurrentCulture);
            this.height = float.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture);
            this.xradius = float.Parse(element.Attribute("xRadius").Value, CultureInfo.CurrentCulture);
            this.yradius = float.Parse(element.Attribute("yRadius").Value, CultureInfo.CurrentCulture);
            this.segments = int.Parse(element.Attribute("segments").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
        }

        private void MakeSolidArc(XElement element)
        {
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.radians = float.Parse(element.Attribute("radians").Value, CultureInfo.CurrentCulture);
            this.sides = int.Parse(element.Attribute("sides").Value, CultureInfo.CurrentCulture);
            this.radius = float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture);
            this.angle = float.Parse(element.Attribute("angle").Value, CultureInfo.CurrentCulture);
        }
    }
}
