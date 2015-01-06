using Luaan.Yggmire.SharpClient.Graphics.VertexFormats;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.SharpClient.Actors
{
    public class BoxActor : PlacedActor
    {
        SharpDX.Toolkit.Graphics.Buffer<VertexPositionNormalTexture> vertexBuffer;
        SharpDX.Toolkit.Graphics.Buffer<short> indexBuffer;

        int vertexCount;
        int indexCount;

        VertexInputLayout inputLayout;
        private Effect effect;
        private Texture2D texture;
        
        public BoxActor(YggmireGame game)
            : base(game)
        {

        }

        private const int CubeFaceCount = 6;

        private static readonly Vector3[] faceNormals = new Vector3[CubeFaceCount]
                {
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, -1),
                    new Vector3(1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, -1, 0),
                };

        private static readonly Vector2[] textureCoordinates = new Vector2[4]
                {
                    new Vector2(1, 0),
                    new Vector2(1, 1),
                    new Vector2(0, 1),
                    new Vector2(0, 0),
                };

        public override void Initialize()
        {
            var vertices = new VertexPositionNormalTexture[CubeFaceCount * 4];
            var indices = new short[CubeFaceCount * 6];

            effect = Game.Content.Load<Effect>("Simple");
            texture = Game.Content.Load<Texture2D>("Crate");

            var size = 0.5f;

            vertexCount = 0;
            indexCount = 0;

            // Create each face in turn.
            for (int i = 0; i < CubeFaceCount; i++)
            {
                Vector3 normal = faceNormals[i];

                // Get two vectors perpendicular both to the face normal and to each other.
                Vector3 basis = (i >= 4) ? Vector3.UnitZ : Vector3.UnitY;

                Vector3 side1;
                Vector3.Cross(ref normal, ref basis, out side1);

                Vector3 side2;
                Vector3.Cross(ref normal, ref side1, out side2);

                // Six indices (two triangles) per face.
                short vbase = (short)(i * 4);
                indices[indexCount++] = (short)(vbase + 0);
                indices[indexCount++] = (short)(vbase + 2);
                indices[indexCount++] = (short)(vbase + 1);

                indices[indexCount++] = (short)(vbase + 0);
                indices[indexCount++] = (short)(vbase + 3);
                indices[indexCount++] = (short)(vbase + 2);

                // Four vertices per face.
                vertices[vertexCount++] = new VertexPositionNormalTexture((normal - side1 - side2) * size, normal, textureCoordinates[0]);
                vertices[vertexCount++] = new VertexPositionNormalTexture((normal - side1 + side2) * size, normal, textureCoordinates[1]);
                vertices[vertexCount++] = new VertexPositionNormalTexture((normal + side1 + side2) * size, normal, textureCoordinates[2]);
                vertices[vertexCount++] = new VertexPositionNormalTexture((normal + side1 - side2) * size, normal, textureCoordinates[3]);
            }

            vertexBuffer = SharpDX.Toolkit.Graphics.Buffer.Vertex.New(Game.GraphicsDevice, vertices);
            indexBuffer = SharpDX.Toolkit.Graphics.Buffer.Index.New(Game.GraphicsDevice, indices);

            inputLayout = VertexInputLayout.FromBuffer(0, vertexBuffer);
        }

        protected override void RenderInternal(GameTime gameTime)
        {
            Game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            Game.GraphicsDevice.SetVertexInputLayout(inputLayout);
            Game.GraphicsDevice.SetIndexBuffer(indexBuffer, false);

            var world = World;

            effect.Parameters["wvp"].SetValue(world * Game.Camera.ViewProjection);
            effect.Parameters["world"].SetValue(world);
            effect.Parameters["tex"].SetResource(texture);

            effect.CurrentTechnique.Passes[0].Apply();

            Game.GraphicsDevice.DrawIndexed(PrimitiveType.TriangleList, indexCount);
        }

        protected override void UpdateInternal(GameTime gameTime)
        {
            // Orientation *= Quaternion.RotationYawPitchRoll((float)gameTime.ElapsedGameTime.TotalSeconds, 0, 0);
        }
    }
}
