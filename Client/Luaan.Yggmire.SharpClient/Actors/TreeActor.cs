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
    public class TreeActor : PlacedActor
    {
        Model model;
        Effect effect;
        Texture2D texture;

        public TreeActor(YggmireGame game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            effect = Game.Content.Load<Effect>("Simple");
            model = Game.Content.Load<Model>("Models/Trees/Tree-2");
            texture = Game.Content.Load<Texture2D>("Models/Trees/Tree-2.Texture.tkb");
        }

        protected override void UpdateInternal(GameTime gameTime)
        {
            
        }

        protected override void RenderInternal(GameTime gameTime)
        {
            var world = World;

            effect.Parameters["wvp"].SetValue(world * Game.Camera.ViewProjection);
            effect.Parameters["world"].SetValue(world);
            effect.Parameters["tex"].SetResource(texture);
            effect.Parameters["TextureSampler"].SetResource(Game.GraphicsDevice.SamplerStates.AnisotropicWrap);

            effect.CurrentTechnique.Passes[0].Apply();

            model.Meshes[0].Draw(Game.GraphicsDevice, null, effect);
        }
    }
}
