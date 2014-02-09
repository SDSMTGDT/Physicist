﻿namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Physicist.Enums;
    using Physicist.Extensions;

    public class BodyInfo
    {
        private BodyCategory? bodycategory = null;
        private List<Vertices> vertices = null;
        private Vector2? position = null;
        private Vector2? shapeoffset = null;
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
        
        // Non-null Defaults
        private bool fixedrotation = false;
        private Category collideswith = FarseerPhysics.Dynamics.Category.All;
        private BodyType bodytype = BodyType.Static;
        private float friction = 0f;

        public BodyInfo(XElement element)
        {
            this.XmlDeserialize(element);
        }

        // Global to all Body
        public FarseerPhysics.Dynamics.Category CollidesWith
        {
            get { return this.collideswith; }
        }

        public BodyType BodyType
        {
            get { return this.bodytype; }
        }

        public bool FixedRotation
        {
            get { return this.fixedrotation; }
        }

        public float Friction
        {
            get { return this.friction; }
        }

        public BodyCategory BodyCategory
        {
            get { return this.bodycategory.Value; }
        }

        public Vector2 Position 
        {
            get { return this.position.Value; }
            set { this.position = value; }
        }

        public Vector2 ShapeOffset
        {
            get { return this.shapeoffset.Value; }
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
            XElement bodyInfoXml = new XElement(Enum.GetName(typeof(BodyCategory), this.bodycategory.Value));

            bodyInfoXml.Add(ExtensionMethods.XmlSerialize(this.position.Value, "position"));

            if (this.CollidesWith != FarseerPhysics.Dynamics.Category.All)
            {
                bodyInfoXml.Add(new XAttribute("collidesWith", this.collideswith.ToString())); 
            }

            if (this.fixedrotation)
            {
                bodyInfoXml.Add(new XAttribute("fixedrotation", this.fixedrotation));
            }

            if (this.bodytype != FarseerPhysics.Dynamics.BodyType.Static)
            {
                bodyInfoXml.Add(new XAttribute("bodytype", this.bodytype.ToString()));
            }

            if (this.friction != 0f)
            {
                bodyInfoXml.Add(new XAttribute("friction", this.friction));
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

            this.bodycategory = (BodyCategory)Enum.Parse(typeof(BodyCategory), element.Name.LocalName);

            this.position = ExtensionMethods.XmlDeserializeVector2(element.Element("position"));

            XAttribute collidesWithEle = element.Attribute("collidesWith");
            if (collidesWithEle != null) 
            {
                this.collideswith = (Category)Enum.Parse(typeof(Category), collidesWithEle.Value);
            }

            XAttribute fixedRotEle = element.Attribute("fixedrotation");
            if (fixedRotEle != null)
            {
                this.fixedrotation = bool.Parse(fixedRotEle.Value);
            }

            XAttribute bodyTypeEle = element.Attribute("bodytype");
            if (bodyTypeEle != null)
            {
                this.bodytype = (BodyType)Enum.Parse(typeof(BodyType), bodyTypeEle.Value);
            }

            XAttribute frictionEle = element.Attribute("friction");
            if (frictionEle != null)
            {
                this.friction = float.Parse(frictionEle.Value, CultureInfo.CurrentCulture);
            }

            switch (this.bodycategory)
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
            float xmax = float.MinValue, ymax = float.MinValue;
            float xmin = float.MaxValue, ymin = float.MaxValue;

            foreach (var vert in element.Element("vertices").Elements())
            {
                Vector2 nextVect = ExtensionMethods.XmlDeserializeVector2(vert);
                verts.Add(nextVect);
                xmax = MathHelper.Max(nextVect.X, xmax);
                ymax = MathHelper.Max(nextVect.Y, ymax);
                xmin = MathHelper.Min(nextVect.X, xmin);
                ymin = MathHelper.Min(nextVect.Y, ymin);
            }

            this.vertices = new List<Vertices>() { verts };

            this.shapeoffset = new Vector2(xmax - xmin, ymax - ymin) / 2.0f;
        }

        private void MakeCapsule(XElement element)
        {
            this.height = float.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture);
            this.topradius = float.Parse(element.Attribute("topRadius").Value, CultureInfo.CurrentCulture);
            this.bottomradius = float.Parse(element.Attribute("bottomRadius").Value, CultureInfo.CurrentCulture);
            this.topedge = int.Parse(element.Attribute("topEdge").Value, CultureInfo.CurrentCulture);
            this.bottomedge = int.Parse(element.Attribute("bottomEdge").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.shapeoffset = new Vector2(MathHelper.Max(this.bottomradius.Value, this.topradius.Value) * 2.0f, this.height.Value) / 2.0f;
        }

        private void MakeChainShape(XElement element)
        {
            Vertices verts = new Vertices();

            foreach (var vert in element.Element("vertice").Elements())
            {
                verts.Add(ExtensionMethods.XmlDeserializeVector2(vert));
            }

            this.vertices = new List<Vertices>() { verts };

            this.shapeoffset = Vector2.Zero;
        }

        private void MakeCircle(XElement element)
        {
            this.radius = float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.shapeoffset = new Vector2(this.radius.Value, this.radius.Value);
        }

        private void MakeCompoundPolygon(XElement element)
        {
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);

            this.vertices = new List<Vertices>();

            foreach (var verticeslist in element.Element("vertices").Elements())
            {
                Vertices nextVerts = new Vertices();
                foreach (var vert in verticeslist.Elements())
                {
                    nextVerts.Add(ExtensionMethods.XmlDeserializeVector2(vert));
                }

                this.vertices.Add(nextVerts);
            }

            this.shapeoffset = Vector2.Zero;
        }

        private void MakeEdge(XElement element)
        {
            this.start = ExtensionMethods.XmlDeserializeVector2(element.Element("start"));

            this.end = ExtensionMethods.XmlDeserializeVector2(element.Element("end"));

            this.shapeoffset = Vector2.Zero;
        }

        private void MakeEllipse(XElement element)
        {
            this.xradius = float.Parse(element.Attribute("xRadius").Value, CultureInfo.CurrentCulture);
            this.yradius = float.Parse(element.Attribute("yRadius").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.edges = int.Parse(element.Attribute("edges").Value, CultureInfo.CurrentCulture);

            this.shapeoffset = new Vector2(this.xradius.Value, this.yradius.Value);
        }

        private void MakeGear(XElement element)
        {
            this.radius = float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture);
            this.numberofteeth = int.Parse(element.Attribute("numberOfTeeth").Value, CultureInfo.CurrentCulture);
            this.tippercentage = float.Parse(element.Attribute("tipPercentage").Value, CultureInfo.CurrentCulture);
            this.toothheight = float.Parse(element.Attribute("toothHeight").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.shapeoffset = new Vector2(this.radius.Value + this.toothheight.Value, this.radius.Value + this.toothheight.Value);
        }

        private void MakeLineArc(XElement element)
        {
            this.radians = float.Parse(element.Attribute("radians").Value, CultureInfo.CurrentCulture);
            this.sides = int.Parse(element.Attribute("sides").Value, CultureInfo.CurrentCulture);
            this.radius = float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture);
            this.angle = float.Parse(element.Attribute("angle").Value, CultureInfo.CurrentCulture);
            this.closed = bool.Parse(element.Attribute("closed").Value);

            this.shapeoffset = new Vector2(this.radius.Value, this.radius.Value);
        }

        private void MakeLoopShape(XElement element)
        {
            Vertices verts = new Vertices();

            foreach (var vert in element.Element("vertice").Elements())
            {
                verts.Add(ExtensionMethods.XmlDeserializeVector2(vert));
            }

            this.vertices = new List<Vertices>() { verts };

            this.shapeoffset = Vector2.Zero;
        }

        private void MakePolygon(XElement element)
        {
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            Vertices verts = new Vertices();

            foreach (var vert in element.Element("vertice").Elements())
            {
                verts.Add(ExtensionMethods.XmlDeserializeVector2(vert));
            }

            this.vertices = new List<Vertices>() { verts };

            this.shapeoffset = Vector2.Zero;
        }

        private void MakeRectangle(XElement element)
        {
            this.height = float.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture);
            this.width = float.Parse(element.Attribute("width").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);

            this.shapeoffset = new Vector2(this.width.Value, this.height.Value) / 2.0f;
        }

        private void MakeRoundedRectangle(XElement element)
        {
            this.width = float.Parse(element.Attribute("width").Value, CultureInfo.CurrentCulture);
            this.height = float.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture);
            this.xradius = float.Parse(element.Attribute("xRadius").Value, CultureInfo.CurrentCulture);
            this.yradius = float.Parse(element.Attribute("yRadius").Value, CultureInfo.CurrentCulture);
            this.segments = int.Parse(element.Attribute("segments").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);

            this.shapeoffset = new Vector2(this.width.Value, this.height.Value) / 2.0f;
        }

        private void MakeSolidArc(XElement element)
        {
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.radians = float.Parse(element.Attribute("radians").Value, CultureInfo.CurrentCulture);
            this.sides = int.Parse(element.Attribute("sides").Value, CultureInfo.CurrentCulture);
            this.radius = float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture);
            this.angle = float.Parse(element.Attribute("angle").Value, CultureInfo.CurrentCulture);

            this.shapeoffset = new Vector2(this.radius.Value, this.radius.Value);
        }
    }
}
