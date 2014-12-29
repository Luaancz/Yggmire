using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.SharpClient.Actors
{
    public class FreeCamera : CameraBase
    {
        Vector3 position;
        float yaw;
        float pitch;
        Quaternion orientation;

        public Vector3 Position
        {
            get 
            { 
                return this.position; 
            }
            set 
            {
                this.position = value;

                UpdateView();
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

                UpdateView();
            }
        }

        public FreeCamera(YggmireGame game) 
            : base(game)
        {
            this.position = Vector3.Zero;
            this.orientation = Quaternion.Identity;

            UpdateView();
        }

        private void UpdateView()
        {
            var lookAt = position + Vector3.Transform(Vector3.ForwardLH, orientation);

            View = Matrix.LookAtLH(position, lookAt, Vector3.Up);
        }

        bool KeyDown(Keys key)
        {
            return Game.InputManager.KeyboardState.IsKeyDown(key);
        }

        protected override void UpdateInternal(GameTime gameTime)
        {
            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var move = Vector3.Zero;
            var turn = Vector3.Zero;

            if (KeyDown(Keys.A)) move.X -= time;
            if (KeyDown(Keys.D)) move.X += time;
            if (KeyDown(Keys.W)) move.Z += time;
            if (KeyDown(Keys.S)) move.Z -= time;
            if (KeyDown(Keys.Space)) move.Y += time;
            if (KeyDown(Keys.LeftControl)) move.Y -= time;

            turn.X = Game.InputManager.MouseState.X - Game.InputManager.LastMouseState.X;
            turn.Y = Game.InputManager.MouseState.Y - Game.InputManager.LastMouseState.Y;

            if (move != Vector3.Zero)
            {
                this.position += Vector3.Transform(move * (KeyDown(Keys.LeftShift) ? 100f : 2f), orientation);
            }

            if (turn != Vector3.Zero && Game.InputManager.MouseState.RightButton.Pressed)
            {
                yaw += turn.X * 5f;
                pitch += turn.Y * 5f;
                
                this.orientation = Quaternion.RotationYawPitchRoll(yaw, pitch, 0f);
            }

            if (move != Vector3.Zero || turn != Vector3.Zero)
                UpdateView();
        }
    }
}
