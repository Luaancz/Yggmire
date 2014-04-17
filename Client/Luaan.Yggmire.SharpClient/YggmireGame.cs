using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.SharpClient.Actors;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System;
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
            graphicsDeviceManager.PreferredBackBufferWidth = 1600;
            graphicsDeviceManager.PreferredBackBufferHeight = 1000;

            Content.RootDirectory = "Content";

            inputManager = new InputManager(this);
        }

        protected override void Initialize()
        {
            Window.Title = "Yggmire Client";

            base.Initialize();

            inputManager.Initialize();
        }

        BoxActor box;
        TreeActor tree;

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
            
            terrain = new TerrainActor(this);
            terrain.Initialize();

            box = new BoxActor(this);
            box.Initialize();
            box.Position = new Vector3(10, 10, 0);
            box.Scale *= 3;

            tree = new TreeActor(this);
            tree.Initialize();
            tree.Position = new Vector3(0, 0, 0);
            tree.Scale *= 3;

            base.LoadContent();   
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            inputManager.Update();

            camera.Update(gameTime);
            terrain.Update(gameTime);

            box.Update(gameTime);
            tree.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            // Use time in seconds directly
            var time = (float)gameTime.TotalGameTime.TotalSeconds;
            
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);           

            terrain.Render(gameTime);

            box.Render(gameTime);
            tree.Render(gameTime);

            base.Draw(gameTime);
        }
    }
}
