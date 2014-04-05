using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.SharpClient.Actors
{
    public abstract class CameraBase : Actor
    {
        private bool dirtyViewProjection = true;

        private Matrix view;
        private Matrix projection;
        private Matrix viewProjection;

        public Matrix View 
        {
            get 
            { 
                return view;
            }
            protected set 
            {
                view = value;
                dirtyViewProjection = true;
            }
        }

        public Matrix Projection 
        {
            get 
            { 
                return projection; 
            }
            protected set 
            {
                projection = value;
                dirtyViewProjection = true;
            }
        }

        public Matrix ViewProjection
        {
            get 
            {
                if (dirtyViewProjection)
                {
                    Matrix.Multiply(ref view, ref projection, out viewProjection);
                    dirtyViewProjection = false;
                }

                return viewProjection;
            }
        }

        public CameraBase(YggmireGame game)
            : base(game)
        {
            Projection = Matrix.PerspectiveFovLH(0.9f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 1f, 10000.0f);
        }
    }
}
