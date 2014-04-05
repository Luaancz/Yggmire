using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.SharpClient.Actors
{
    public abstract class PlacedActor : VisibleActor
    {
        Vector3 position;
        Vector3 scale;
        Quaternion orientation;

        bool dirtyWorld = true;
        Matrix world;

        public Matrix World
        {
            get
            {
                if (dirtyWorld)
                    UpdateWorldMatrix();

                return world;
            }
            protected set
            {
                world = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;

                dirtyWorld = true;
            }
        }

        public Vector3 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;

                dirtyWorld = true;
            }
        }

        public Quaternion Orientation
        {
            get
            {
                return this.orientation;
            }
            set
            {
                this.orientation = value;

                dirtyWorld = true;
            }
        }

        public PlacedActor(YggmireGame game)
            : base(game)
        {
            this.orientation = Quaternion.RotationYawPitchRoll(0, 0, 0);
            this.scale = Vector3.One;
        }

        /// <summary>
        /// Create the actor in world-space.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Update the World matrix with current data.
        /// </summary>
        void UpdateWorldMatrix()
        {
            world = Matrix.RotationQuaternion(orientation) * Matrix.Scaling(scale) * Matrix.Translation(position);
            dirtyWorld = false;
        }
    }
}
