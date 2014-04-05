using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.SharpClient.Actors
{
    public abstract class Actor
    {
        protected YggmireGame Game { get; private set; }

        public bool IsActive { get; set; }

        public Actor(YggmireGame game)
        {
            Game = game;
            IsActive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (IsActive)
                UpdateInternal(gameTime);
        }

        protected abstract void UpdateInternal(GameTime gameTime);
    }
}
