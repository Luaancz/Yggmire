using Luaan.Yggmire.Core.Structures;
using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Actors;
using Luaan.Yggmire.SharpClient.Actors;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Luaan.Yggmire.SharpClient
{
    public class YggmireGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteFont arial16Font;

        private FreeCamera camera;

        public CameraBase Camera { get { return this.camera; } }

        private Model model;
       
        private TerrainActor terrain;

        private readonly InputManager inputManager;
        public InputManager InputManager { get { return this.inputManager; } }

        private readonly ISessionGrain session;
        public ISessionGrain Session { get { return this.session; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="YggmireGame" /> class.
        /// </summary>
        public YggmireGame(ISessionGrain session)
        {
            this.session = session;

            graphicsDeviceManager = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            inputManager = new InputManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            inputManager.Initialize();
            graphicsDeviceManager.PreferMultiSampling = true;
        }
        
        List<PlacedActor> localActors = new List<PlacedActor>();
        ConcurrentQueue<PlacedActor> addedActors = new ConcurrentQueue<PlacedActor>();

        public void AddZone(ZonePosition zoneOffset)
        {
            var terrain = new TerrainActor(this);
            terrain.Position = new Vector3(zoneOffset.Position.X * 255, 0, zoneOffset.Position.Y * 255);
            terrain.Initialize();

            addedActors.Enqueue(terrain);
        }

        public void DropZone(ZonePosition zoneOffset)
        {
            // TODO
        }

        public void AddWorldItem(ZonePosition zoneOffset, WorldItem item)
        {
            var swi = item as StaticWorldItem;

            PlacedActor actor;

            switch (item.PrototypeId)
            {
                case 1: actor = new TreeActor(this); break;
                case 2: actor = new BoxActor(this); break;
                default: return;
            }

            actor.Position = new Vector3(zoneOffset.Position.X * 255 + swi.Position.X / 1000f, 0, zoneOffset.Position.Y * 255 + swi.Position.Y / 1000f);
            actor.Scale *= 2.5f;
            actor.Initialize();

            addedActors.Enqueue(actor);
        }

        protected override void LoadContent()
        {
            // Loads a sprite font
            // The [Arial16.xml] file is defined with the build action [ToolkitFont] in the project
            arial16Font = Content.Load<SpriteFont>("Arial16");

            model = Content.Load<Model>("Ship");

            // Enable default lighting on model.
            BasicEffect.EnableDefaultLighting(model, true);

            camera = new FreeCamera(this);
            camera.Position = new Vector3(0, 5, -25);
            
            base.LoadContent();   
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            inputManager.Update();

            camera.Update(gameTime);
            
            // Not thread-safe, but we don't really care if a couple of actors
            // are not added until the next update.
            if (addedActors.Count > 0)
            {
                PlacedActor actor;

                while (addedActors.TryDequeue(out actor))
                {
                    localActors.Add(actor);
                }
            }

            foreach (var actor in localActors)
                actor.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            // Use time in seconds directly
            var time = (float)gameTime.TotalGameTime.TotalSeconds;
            
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            foreach (var actor in localActors)
                actor.Render(gameTime);

            base.Draw(gameTime);
        }
    }
}
