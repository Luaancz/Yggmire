using Luaan.Yggmire.SharpClient.Graphics.VertexFormats;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Luaan.Yggmire.SharpClient.Actors
{
    public class TerrainActor : VisibleActor
    {
        SharpDX.Toolkit.Graphics.Buffer<VertexPositionNormalTexture> vertexBuffer;
        SharpDX.Toolkit.Graphics.Buffer<int> indexBuffer;

        int vertexCount;
        int indexCount;

        int patchWidth;
        int patchHeight;

        VertexInputLayout inputLayout;

        private Effect terrainEffect;
        private Texture2D texture;

        public TerrainActor(YggmireGame game)
            : base(game)
        {
            this.patchWidth = 255;
            this.patchHeight = 255;
        }

        public void Initialize()
        {
            Bitmap bmp = (Bitmap)Bitmap.FromFile("heightmap01.bmp");

            byte[,] bmpData = new byte[patchWidth + 1, patchHeight + 1];

            /*
            for (int x = 0; x <= patchWidth; x++ )
            {
                for (int y = 0; y <= patchHeight; y++)
                {
                    bmpData[x, y] = bmp.GetPixel(x, y).R;
                }
            }
            */

            terrainEffect = Game.Content.Load<Effect>("Terrain");
            texture = Game.Content.Load<Texture2D>("Dirt");

            vertexCount = (patchWidth + 1) * (patchHeight + 1);
            indexCount = patchWidth * patchHeight * 6;

            var vertices = new VertexPositionNormalTexture[vertexCount];
            var indices = new int[indexCount];

            var vertexLine = patchHeight + 1;

            var fwidth = (float)patchWidth / 2f;
            var fheight = (float)patchHeight / 2f;

            for (int y = 0; y < patchHeight + 1; y++)
            {
                var py = y * vertexLine;

                for (int x = 0; x < patchWidth + 1; x++)
                {
                    float dx = (bmpData[x < patchWidth ? x + 1 : x, y] - bmpData[x > 0 ? x - 1 : x, y]) / 255f;
                    float dy = (bmpData[x, y < patchHeight ? y + 1 : y] - bmpData[x, y > 0 ? y - 1 : y]) / 255f;

                    var normal = new Vector3(-dx, 1f, dy);
                    normal.Normalize();

                    vertices[x + py] = new VertexPositionNormalTexture { Position = new Vector3(x - fwidth, (bmpData[x, y] / 255f) * 10f, y - fheight), Normal = normal, TextureCoordinate = new Vector2(x, y) };
                }
            }

            for (int x = 0; x < patchWidth; x++)
            {
                for (int y = 0; y < patchHeight; y++)
                {
                    var topLeft = (x + y * vertexLine);

                    var startIndex = (x + y * patchHeight) * 6;

                    indices[startIndex] = topLeft;
                    indices[startIndex + 1] = (topLeft + vertexLine);
                    indices[startIndex + 2] = (topLeft + vertexLine + 1);
                    indices[startIndex + 3] = topLeft;
                    indices[startIndex + 4] = (topLeft + vertexLine + 1);
                    indices[startIndex + 5] = (topLeft + 1);
                }
            }

            vertexBuffer = SharpDX.Toolkit.Graphics.Buffer.Vertex.New(Game.GraphicsDevice, vertices);
            indexBuffer = SharpDX.Toolkit.Graphics.Buffer.Index.New(Game.GraphicsDevice, indices);           

            inputLayout = VertexInputLayout.FromBuffer(0, vertexBuffer);
        }
        
        protected override void RenderInternal(GameTime gameTime)
        {
            Game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            Game.GraphicsDevice.SetVertexInputLayout(inputLayout);
            Game.GraphicsDevice.SetIndexBuffer(indexBuffer, true);

            var world = Matrix.Scaling(1f);

            terrainEffect.Parameters["wvp"].SetValue(world * Game.Camera.ViewProjection);
            terrainEffect.Parameters["world"].SetValue(world);
            terrainEffect.Parameters["tex"].SetResource(texture);
            terrainEffect.Parameters["TextureSampler"].SetResource(Game.GraphicsDevice.SamplerStates.AnisotropicWrap);

            terrainEffect.CurrentTechnique.Passes[0].Apply();

            Game.GraphicsDevice.DrawIndexed(PrimitiveType.TriangleList, indexCount);
        }

        protected override void UpdateInternal(GameTime gameTime)
        {
            
        }
    }
}
