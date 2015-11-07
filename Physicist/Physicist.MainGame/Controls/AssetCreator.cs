namespace Physicist.MainGame.Controls
{
    using System;
    using System.Collections.Generic;
    using FarseerPhysics;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Common;
    using FarseerPhysics.Common.Decomposition;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.MainGame.Extensions;
    using Physicist.Types.Controllers;

    // Taken From Farseer Examples modified to fit purposes 
    public sealed class AssetCreator : IDisposable
    {
        private const int CircleSegments = 32;

        private static AssetCreator instance = null;
        private static object lockObject = new object();
        private GraphicsDevice device;
        private BasicEffect effect;

        private AssetCreator()
        {
            this.IsInitialized = false;
        }

        public static AssetCreator Instance 
        {
            get
            {
                if (AssetCreator.instance == null)
                {
                    lock (AssetCreator.lockObject)
                    {
                        if (AssetCreator.instance == null)
                        {
                            AssetCreator.instance = new AssetCreator();
                        }
                    }
                }

                return AssetCreator.instance;
            }
        }

        public bool IsInitialized { get; private set; }

        public static Vector2 CalculateOrigin(Body body)
        {
            Vector2 origin = Vector2.Zero;

            if (body != null)
            {
                Vector2 leftBound = new Vector2(float.MaxValue);
                Transform trans;
                body.GetTransform(out trans);

                for (int i = 0; i < body.FixtureList.Count; ++i)
                {
                    for (int j = 0; j < body.FixtureList[i].Shape.ChildCount; ++j)
                    {
                        AABB bounds;
                        body.FixtureList[i].Shape.ComputeAABB(out bounds, ref trans, j);
                        Vector2.Min(ref leftBound, ref bounds.LowerBound, out leftBound);
                    }
                }

                // calculate body offset from its center and add a 1 pixel border
                // because we generate the textures a little bigger than the actual body's fixtures
                origin = ConvertUnits.ToDisplayUnits(body.Position - leftBound) + new Vector2(1f);
            }

            return origin;
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            if (AssetCreator.Instance != null)
            {
                this.device = graphicsDevice;
                this.effect = new BasicEffect(this.device);
                this.IsInitialized = true;
            }
        }

        public void Dispose()
        {
            this.device.Dispose();
            this.effect.Dispose();
        }

        public Texture2D TextureFromShape(Shape shape, string textureRef, Color color, float materialScale)
        {
            Texture2D shapeTexture = null;
            if (this.IsInitialized && shape != null)
            {
                switch (shape.ShapeType)
                {
                    case ShapeType.Circle:
                        shapeTexture = this.CircleTexture(shape.Radius, textureRef, color, materialScale);
                        break;

                    case ShapeType.Polygon:
                        shapeTexture = this.TextureFromVertices(((PolygonShape)shape).Vertices, textureRef, color, materialScale);
                        break;

                    case ShapeType.Chain:
                        ChainShape chain = shape as ChainShape;
                        if (chain != null)
                        {
                            shapeTexture = this.TextureFromVertices(AssetCreator.CreateChainVertices(chain), textureRef, color, materialScale);
                        }     
                   
                        break;
                }
            }

            return shapeTexture;
        }
      
        public Texture2D TextureFromVertices(IEnumerable<Vector2> vertices, string textureRef, Color color, float materialScale)
        {
            Texture2D texture = null;

            if (this.IsInitialized)
            {
                // copy vertices
                Vertices verts = new Vertices(vertices);

                // scale to display units (i.e. pixels) for rendering to texture
                Vector2 scale = ConvertUnits.ToDisplayUnits(Vector2.One);
                verts.Scale(ref scale);

                // translate the boundingbox center to the texture center
                // because we use an orthographic projection for rendering later
                AABB vertsBounds = verts.GetAABB();
                verts.Translate(-vertsBounds.Center);

                List<Vertices> decomposedVerts;
                if (!verts.IsConvex())
                {
                    decomposedVerts = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Delauny);
                }
                else
                {
                    decomposedVerts = new List<Vertices>();
                    decomposedVerts.Add(verts);
                }

                List<VertexPositionColorTexture[]> verticesFill = new List<VertexPositionColorTexture[]>(decomposedVerts.Count);
                Texture2D material = ContentController.Instance.GetTextureMaterial(this.device, textureRef);

                materialScale /= material.Width;

                for (int i = 0; i < decomposedVerts.Count; ++i)
                {
                    verticesFill.Add(new VertexPositionColorTexture[3 * (decomposedVerts[i].Count - 2)]);
                    for (int j = 0; j < decomposedVerts[i].Count - 2; ++j)
                    {
                        // fill vertices
                        verticesFill[i][3 * j].Position = new Vector3(decomposedVerts[i][0], 0f);
                        verticesFill[i][(3 * j) + 1].Position = new Vector3(decomposedVerts[i].NextVertex(j), 0f);
                        verticesFill[i][(3 * j) + 2].Position = new Vector3(decomposedVerts[i].NextVertex(j + 1), 0f);
                        verticesFill[i][3 * j].TextureCoordinate = decomposedVerts[i][0] * materialScale;
                        verticesFill[i][(3 * j) + 1].TextureCoordinate = decomposedVerts[i].NextVertex(j) * materialScale;
                        verticesFill[i][(3 * j) + 2].TextureCoordinate = decomposedVerts[i].NextVertex(j + 1) * materialScale;
                        verticesFill[i][3 * j].Color = verticesFill[i][(3 * j) + 1].Color = verticesFill[i][(3 * j) + 2].Color = color;
                    }
                }

                /* calculate outline
                VertexPositionColor[] verticesOutline = new VertexPositionColor[2 * verts.Count];
                for (int i = 0; i < verts.Count; ++i)
                {
                    verticesOutline[2 * i].Position = new Vector3(verts[i], 0f);
                    verticesOutline[(2 * i) + 1].Position = new Vector3(verts.NextVertex(i), 0f);
                    verticesOutline[2 * i].Color = verticesOutline[(2 * i) + 1].Color = Color.Black;
                }
                */

                Vector2 vertsSize = new Vector2(vertsBounds.UpperBound.X - vertsBounds.LowerBound.X, vertsBounds.UpperBound.Y - vertsBounds.LowerBound.Y);
                texture = this.RenderTexture((int)Math.Ceiling(vertsSize.X), (int)Math.Ceiling(vertsSize.Y), material, verticesFill);
            }

            return texture;
        }

        public Texture2D CircleTexture(float radius, string textureRef, Color color, float materialScale)
        {
            Texture2D texture = null;
            if (this.IsInitialized)
            {
                texture = this.EllipseTexture(radius, radius, textureRef, color, materialScale);
            }

            return texture;
        }

        public Texture2D EllipseTexture(float radiusX, float radiusY, string textureRef, Color color, float materialScale)
        {
            Texture2D texture = null;
            if (this.IsInitialized)
            {
                VertexPositionColorTexture[] verticesFill = new VertexPositionColorTexture[3 * (CircleSegments - 2)];
                
                // VertexPositionColor[] verticesOutline = new VertexPositionColor[2 * CircleSegments];
                const float SegmentSize = MathHelper.TwoPi / CircleSegments;
                float theta = SegmentSize;

                Texture2D material = ContentController.Instance.GetTextureMaterial(this.device, textureRef);

                radiusX = ConvertUnits.ToDisplayUnits(radiusX);
                radiusY = ConvertUnits.ToDisplayUnits(radiusY);
                materialScale /= material.Width;

                Vector2 start = new Vector2(radiusX, 0f);

                for (int i = 0; i < CircleSegments - 2; ++i)
                {
                    Vector2 p1 = new Vector2(radiusX * (float)Math.Cos(theta), radiusY * (float)Math.Sin(theta));
                    Vector2 p2 = new Vector2(
                                             radiusX * (float)Math.Cos(theta + SegmentSize),
                                             radiusY * (float)Math.Sin(theta + SegmentSize));

                    // fill vertices
                    verticesFill[3 * i].Position = new Vector3(start, 0f);
                    verticesFill[(3 * i) + 1].Position = new Vector3(p1, 0f);
                    verticesFill[(3 * i) + 2].Position = new Vector3(p2, 0f);
                    verticesFill[3 * i].TextureCoordinate = start * materialScale;
                    verticesFill[(3 * i) + 1].TextureCoordinate = p1 * materialScale;
                    verticesFill[(3 * i) + 2].TextureCoordinate = p2 * materialScale;
                    verticesFill[3 * i].Color = verticesFill[(3 * i) + 1].Color = verticesFill[(3 * i) + 2].Color = color;

                    /* outline vertices
                    *
                    if (i == 0)
                    {
                        verticesOutline[0].Position = new Vector3(start, 0f);
                        verticesOutline[1].Position = new Vector3(p1, 0f);
                        verticesOutline[0].Color = verticesOutline[1].Color = Color.Black;
                    }

                    if (i == CircleSegments - 3)
                    {
                        verticesOutline[(2 * CircleSegments) - 2].Position = new Vector3(p2, 0f);
                        verticesOutline[(2 * CircleSegments) - 1].Position = new Vector3(start, 0f);
                        verticesOutline[(2 * CircleSegments) - 2].Color = verticesOutline[(2 * CircleSegments) - 1].Color = Color.Black;
                    }

                    verticesOutline[(2 * i) + 2].Position = new Vector3(p1, 0f);
                    verticesOutline[(2 * i) + 3].Position = new Vector3(p2, 0f);
                    verticesOutline[(2 * i) + 2].Color = verticesOutline[(2 * i) + 3].Color = Color.Black;
                    */

                    theta += SegmentSize;
                }

                texture = this.RenderTexture((int)(radiusX * 2f), (int)(radiusY * 2f), material, verticesFill);
            }

            return texture;
        }

        /// <summary>
        /// Makes a set of vertices for the chain shape 
        /// by creating two offset vertices perpendicular 
        /// to the averaged direction from the current vertex
        /// and it's two connected vertices. Returned vertices 
        /// are in counterclockwise order
        /// </summary>
        /// <param name="chain">chain shape to work on</param>
        /// <returns>new Triangularizable Vertices for chain shape</returns>
        private static Vertices CreateChainVertices(ChainShape chain)
        {
            Matrix rotMat = Matrix.CreateRotationZ(MathHelper.PiOver2);
            Vertices chainTextVerts = new Vertices(chain.Vertices.Count * 2);
            List<Vector2> posChainTextVerts = new List<Vector2>(chain.Vertices.Count);
            List<Vector2> negChainTextVerts = new List<Vector2>(chain.Vertices.Count);
            for (int i = 0; i < chain.Vertices.Count; i++)
            {
                var curVertex = chain.Vertices[i];
                Vector2 direction = Vector2.Zero;
                Vector2 distance = Vector2.Zero;
                if (i == 0)
                {
                    distance = chain.Vertices[i + 1] - curVertex;
                }
                else if (i < chain.Vertices.Count - 1)
                {
                    distance = (chain.Vertices[i + 1] - curVertex) + (curVertex - chain.Vertices[i - 1]);
                }
                else
                {
                    distance = curVertex - chain.Vertices[i - 1];
                }

                direction = Vector2.Normalize(Vector2.Transform(distance, rotMat)).ToSimUnits();
                posChainTextVerts.Add(curVertex + (direction * 2f));
                negChainTextVerts.Insert(0, curVertex - (direction * 2f));
            }

            chainTextVerts.AddRange(posChainTextVerts);
            chainTextVerts.AddRange(negChainTextVerts);

            return chainTextVerts;
        }

        private Texture2D RenderTexture(int width, int height, Texture2D material, VertexPositionColorTexture[] verticesFill)
        {
            // Removed Paramter -> , VertexPositionColor[] verticesOutline
            List<VertexPositionColorTexture[]> fill = new List<VertexPositionColorTexture[]>(1);
            fill.Add(verticesFill);
            return this.RenderTexture(width, height, material, fill);
        }

        private Texture2D RenderTexture(int width, int height, Texture2D material, List<VertexPositionColorTexture[]> verticesFill)
        {
            // Removed Parameter -> , VertexPositionColor[] verticesOutline
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0f);
            PresentationParameters pp = this.device.PresentationParameters;
            RenderTarget2D texture = null;
            RenderTarget2D tempTarget = null;

            try
            {
                tempTarget = new RenderTarget2D(this.device, width, height, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
                this.device.RasterizerState = RasterizerState.CullNone;
                this.device.SamplerStates[0] = SamplerState.LinearWrap;

                this.device.SetRenderTarget(tempTarget);
                this.device.Clear(Color.Transparent);
                this.effect.Projection = Matrix.CreateOrthographic(width, -height, 0f, 1f);
                this.effect.View = halfPixelOffset;

                // render shape;
                this.effect.TextureEnabled = true;
                this.effect.Texture = material;
                this.effect.VertexColorEnabled = true;
                this.effect.Techniques[0].Passes[0].Apply();
                for (int i = 0; i < verticesFill.Count; ++i)
                {
                    this.device.DrawUserPrimitives(PrimitiveType.TriangleList, verticesFill[i], 0, verticesFill[i].Length / 3);
                }

                this.effect.TextureEnabled = false;
                this.effect.Techniques[0].Passes[0].Apply();

                // render outline                
                // this.device.DrawUserPrimitives(PrimitiveType.LineList, verticesOutline, 0, verticesOutline.Length / 2);
                this.device.SetRenderTarget(null);

                texture = tempTarget;
                tempTarget = null;
            }
            catch (InvalidOperationException)
            {
                texture = null;
            }
            finally
            {
                if (tempTarget != null)
                {
                    tempTarget.Dispose();
                }
            }

            return texture;
        }
    }
}