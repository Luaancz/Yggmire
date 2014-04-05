using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.SharpClient.Actors
{
    public abstract class VisibleActor : Actor
    {
        public bool IsVisible { get; set; }

        public VisibleActor(YggmireGame game)
            : base(game)
        {
            IsVisible = true;
        }

        public void Render(GameTime gameTime)
        {
            if (IsVisible)
                RenderInternal(gameTime);
        }

        protected abstract void RenderInternal(GameTime gameTime);
    }
}
