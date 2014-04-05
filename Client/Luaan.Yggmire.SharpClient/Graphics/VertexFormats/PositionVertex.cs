using SharpDX;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.SharpClient.Graphics.VertexFormats
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PositionVertex
    {
        [VertexElement("SV_POSITION")]
        public Vector3 Position;

        public static readonly int Size = 12;
    }
}
