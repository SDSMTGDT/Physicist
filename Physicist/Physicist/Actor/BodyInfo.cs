namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;
    using Physicist.Enums;
    using Physicist.Extensions;

    public class BodyInfo
    {
        private static Dictionary<BodyCategory, MethodInfo> makeBodyHash = new Dictionary<BodyCategory, MethodInfo>();

        // Construction information
        private int mapHeight;

        // Body information
        private BodyCategory? bodyCategory = null;
        private List<Vertices> vertexList = null;
        private Vector2? position = null;
        private Vector2? shapeOffset = null;
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
        private float? topRadius = null;
        private float? bottomRadius = null;
        private int? topedge = null;
        private int? bottomEdge = null;
        private Vector2? start = null;
        private Vector2? end = null;
        private float? tipPercentage = null;
        private float? toothHeight = null;
        private int? numberOfTeeth = null;

        // Non-null Defaults
        private bool fixedRotation = false;
        private Category collidesWith = FarseerPhysics.Dynamics.Category.All;
        private BodyType bodyType = BodyType.Static;
        private float friction = 0f;
        private float rotation = 0f;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Handles Changes to underlying enum")]
        static BodyInfo()
        {
            // Create a simple dictionary to associate bodycategories with 'factory' methods 
            Enum.GetValues(typeof(BodyCategory)).Cast<BodyCategory>().ToList().ForEach(
                cat => makeBodyHash.Add(cat, typeof(BodyInfo).GetMethod("Make" + cat.ToString(), BindingFlags.Instance | BindingFlags.NonPublic)));
        }

        public BodyInfo()
        {
        }

        public BodyInfo(int mapHeight)
        {
            this.mapHeight = mapHeight;
        }

        // Global to all Body
        public FarseerPhysics.Dynamics.Category CollidesWith
        {
            get { return this.collidesWith; }
        }

        public float Rotation
        {
            get { return this.rotation; }
        }

        public BodyType BodyType
        {
            get { return this.bodyType; }
        }

        public bool FixedRotation
        {
            get { return this.fixedRotation; }
        }

        public float Friction
        {
            get { return this.friction; }
        }

        public BodyCategory BodyCategory
        {
            get { return this.bodyCategory.Value; }
        }

        public Vector2 Position
        {
            get { return this.position.Value; }
            set { this.position = value; }
        }

        public Vector2 ShapeOffset
        {
            get { return this.shapeOffset.Value; }
        }

        // Common
        public IEnumerable<Vertices> Vertices
        {
            get { return this.vertexList; }
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
            get { return this.topRadius.Value; }
        }

        public float BottomRadius
        {
            get { return this.bottomRadius.Value; }
        }

        public int TopEdge
        {
            get { return this.topedge.Value; }
        }

        public int BottomEdge
        {
            get { return this.bottomEdge.Value; }
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
            get { return this.tipPercentage.Value; }
        }

        public float ToothHeight
        {
            get { return this.toothHeight.Value; }
        }

        public int NumberOfTeeth
        {
            get { return this.numberOfTeeth.Value; }
        }

        public XElement XmlSerialize()
        {
            XElement bodyInfoXml = new XElement(
                this.bodyCategory.Value.ToString(),
                ExtensionMethods.XmlSerialize(new Vector2(this.Position.X, this.mapHeight - this.Position.Y), "Position"),
                new XAttribute("rotation", this.rotation),
                new XAttribute("collidesWith", this.collidesWith.ToString()),
                new XAttribute("fixedRotation", this.fixedRotation),
                new XAttribute("bodyType", this.bodyType.ToString()),
                new XAttribute("friction", this.friction));

            if (this.vertexList != null)
            {
                XElement vertices = new XElement(
                    "VertexList",
                    this.vertexList.Select(vertex => new XElement(
                                    "Vertices", 
                                    vertex.Select(vector => ExtensionMethods.XmlSerialize(vector, "Vector2")))));

                bodyInfoXml.Add(this.vertexList.Count > 1 ? vertices : vertices.Element("Verticies"));
            }

            bodyInfoXml.TryAddNullableAttributeToXml("density", this.density);
            bodyInfoXml.TryAddNullableAttributeToXml("height", this.height);
            bodyInfoXml.TryAddNullableAttributeToXml("width", this.width);
            bodyInfoXml.TryAddNullableAttributeToXml("radius", this.radius);
            bodyInfoXml.TryAddNullableAttributeToXml("xRadius", this.xradius);
            bodyInfoXml.TryAddNullableAttributeToXml("yRadius", this.yradius);
            bodyInfoXml.TryAddNullableAttributeToXml("radians", this.radians);
            bodyInfoXml.TryAddNullableAttributeToXml("angle", this.angle);
            bodyInfoXml.TryAddNullableAttributeToXml("sides", this.sides);
            bodyInfoXml.TryAddNullableAttributeToXml("segments", this.segments);
            bodyInfoXml.TryAddNullableAttributeToXml("edges", this.edges);
            bodyInfoXml.TryAddNullableAttributeToXml("topRadius", this.topRadius);
            bodyInfoXml.TryAddNullableAttributeToXml("bottomRadius", this.bottomRadius);
            bodyInfoXml.TryAddNullableAttributeToXml("topEdge", this.topedge);
            bodyInfoXml.TryAddNullableAttributeToXml("bottomEdge", this.bottomEdge);
            bodyInfoXml.TryAddNullableAttributeToXml("closed", this.closed);
            bodyInfoXml.TryAddNullableAttributeToXml("tipPercentage", this.tipPercentage);
            bodyInfoXml.TryAddNullableAttributeToXml("numberOfTeeth", this.numberOfTeeth);
            bodyInfoXml.TryAddNullableAttributeToXml("toothHeight", this.toothHeight);

            if (this.start.HasValue)
            {
                bodyInfoXml.Add(new XElement("start", new XAttribute("x", this.start.Value.X), new XAttribute("y", this.start.Value.Y)));
            }

            if (this.end.HasValue)
            {
                bodyInfoXml.Add(new XElement("end", new XAttribute("x", this.end.Value.X), new XAttribute("y", this.end.Value.Y)));
            }

            return bodyInfoXml;
        }

        public void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.bodyCategory = (BodyCategory)Enum.Parse(typeof(BodyCategory), element.Name.LocalName);

                XElement posEle = element.Element("Position");
                Vector2 designPosition = Vector2.Zero;
                if (posEle != null)
                {
                    designPosition = ExtensionMethods.XmlDeserializeVector2(posEle);
                }

                this.position = new Vector2(designPosition.X, this.mapHeight - designPosition.Y);

                this.collidesWith = element.GetAttribute("collidesWith", Category.All);

                this.fixedRotation = element.GetAttribute("fixedRotation", false);

                this.bodyType = element.GetAttribute("bodyType", BodyType.Static);

                this.friction = element.GetAttribute("friction", 0f);

                this.rotation = element.GetAttribute("rotation", 0f);

                BodyInfo.makeBodyHash[this.bodyCategory.Value].Invoke(this, new object[] { element });
            }
        }

        private void MakeBreakableBody(XElement element)
        {
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            Vertices verts = new Vertices();
            float xmax = float.MinValue, ymax = float.MinValue;
            float xmin = float.MaxValue, ymin = float.MaxValue;

            foreach (var vert in element.Element("VertexList").Elements())
            {
                Vector2 nextVect = ExtensionMethods.XmlDeserializeVector2(vert);
                verts.Add(nextVect);
                xmax = MathHelper.Max(nextVect.X, xmax);
                ymax = MathHelper.Max(nextVect.Y, ymax);
                xmin = MathHelper.Min(nextVect.X, xmin);
                ymin = MathHelper.Min(nextVect.Y, ymin);
            }

            this.vertexList = new List<Vertices>() { verts };

            this.shapeOffset = new Vector2(xmax - xmin, ymax - ymin) / 2.0f;
        }

        private void MakeCapsule(XElement element)
        {
            this.height = float.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture);
            this.topRadius = float.Parse(element.Attribute("topRadius").Value, CultureInfo.CurrentCulture);
            this.bottomRadius = float.Parse(element.Attribute("bottomRadius").Value, CultureInfo.CurrentCulture);
            this.topedge = int.Parse(element.Attribute("topEdge").Value, CultureInfo.CurrentCulture);
            this.bottomEdge = int.Parse(element.Attribute("bottomEdge").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.shapeOffset = new Vector2(MathHelper.Max(this.bottomRadius.Value, this.topRadius.Value) * 2.0f, this.height.Value) / 2.0f;
        }

        private void MakeChainShape(XElement element)
        {
            Vertices verts = new Vertices();

            foreach (var vert in element.Element("Vertices").Elements())
            {
                verts.Add(ExtensionMethods.XmlDeserializeVector2(vert));
            }

            this.vertexList = new List<Vertices>() { verts };

            this.shapeOffset = Vector2.Zero;
        }

        private void MakeCircle(XElement element)
        {
            this.radius = float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.shapeOffset = new Vector2(this.radius.Value, this.radius.Value);
        }

        private void MakeCompoundPolygon(XElement element)
        {
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);

            this.vertexList = new List<Vertices>();

            foreach (var verticeslist in element.Element("VertexList").Elements())
            {
                Vertices nextVerts = new Vertices();
                foreach (var vert in verticeslist.Elements())
                {
                    nextVerts.Add(ExtensionMethods.XmlDeserializeVector2(vert));
                }

                this.vertexList.Add(nextVerts);
            }

            this.shapeOffset = Vector2.Zero;
        }

        private void MakeEdge(XElement element)
        {
            this.start = ExtensionMethods.XmlDeserializeVector2(element.Element("Start"));

            this.end = ExtensionMethods.XmlDeserializeVector2(element.Element("End"));

            this.shapeOffset = Vector2.Zero;
        }

        private void MakeEllipse(XElement element)
        {
            this.xradius = float.Parse(element.Attribute("xRadius").Value, CultureInfo.CurrentCulture);
            this.yradius = float.Parse(element.Attribute("yRadius").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.edges = int.Parse(element.Attribute("edges").Value, CultureInfo.CurrentCulture);

            this.shapeOffset = new Vector2(this.xradius.Value, this.yradius.Value);
        }

        private void MakeGear(XElement element)
        {
            this.radius = float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture);
            this.numberOfTeeth = int.Parse(element.Attribute("numberOfTeeth").Value, CultureInfo.CurrentCulture);
            this.tipPercentage = float.Parse(element.Attribute("tipPercentage").Value, CultureInfo.CurrentCulture);
            this.toothHeight = float.Parse(element.Attribute("toothHeight").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.shapeOffset = new Vector2(this.radius.Value + this.toothHeight.Value, this.radius.Value + this.toothHeight.Value);
        }

        private void MakeLineArc(XElement element)
        {
            this.radians = float.Parse(element.Attribute("radians").Value, CultureInfo.CurrentCulture);
            this.sides = int.Parse(element.Attribute("sides").Value, CultureInfo.CurrentCulture);
            this.radius = float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture);
            this.angle = float.Parse(element.Attribute("angle").Value, CultureInfo.CurrentCulture);
            this.closed = bool.Parse(element.Attribute("closed").Value);

            this.shapeOffset = new Vector2(this.radius.Value, this.radius.Value);
        }

        private void MakeLoopShape(XElement element)
        {
            Vertices verts = new Vertices();

            foreach (var vert in element.Element("Vertices").Elements())
            {
                verts.Add(ExtensionMethods.XmlDeserializeVector2(vert));
            }

            this.vertexList = new List<Vertices>() { verts };

            this.shapeOffset = Vector2.Zero;
        }

        private void MakePolygon(XElement element)
        {
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            Vertices verts = new Vertices();

            foreach (var vert in element.Element("Vertices").Elements())
            {
                verts.Add(ExtensionMethods.XmlDeserializeVector2(vert));
            }

            this.vertexList = new List<Vertices>() { verts };

            this.shapeOffset = Vector2.Zero;
        }

        private void MakeRectangle(XElement element)
        {
            this.height = float.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture);
            this.width = float.Parse(element.Attribute("width").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);

            this.shapeOffset = new Vector2(this.width.Value, this.height.Value) / 2.0f;
        }

        private void MakeRoundedRectangle(XElement element)
        {
            this.width = float.Parse(element.Attribute("width").Value, CultureInfo.CurrentCulture);
            this.height = float.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture);
            this.xradius = float.Parse(element.Attribute("xRadius").Value, CultureInfo.CurrentCulture);
            this.yradius = float.Parse(element.Attribute("yRadius").Value, CultureInfo.CurrentCulture);
            this.segments = int.Parse(element.Attribute("segments").Value, CultureInfo.CurrentCulture);
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);

            this.shapeOffset = new Vector2(this.width.Value, this.height.Value) / 2.0f;
        }

        private void MakeSolidArc(XElement element)
        {
            this.density = float.Parse(element.Attribute("density").Value, CultureInfo.CurrentCulture);
            this.radians = float.Parse(element.Attribute("radians").Value, CultureInfo.CurrentCulture);
            this.sides = int.Parse(element.Attribute("sides").Value, CultureInfo.CurrentCulture);
            this.radius = float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture);
            this.angle = float.Parse(element.Attribute("angle").Value, CultureInfo.CurrentCulture);

            this.shapeOffset = new Vector2(this.radius.Value, this.radius.Value);
        }
    }
}
