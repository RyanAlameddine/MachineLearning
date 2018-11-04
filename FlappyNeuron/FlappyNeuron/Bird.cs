using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyNeuron
{
    class Bird : Sprite
    {
        public static float Gravity;
        public static float JumpPower;
        public static float XSpeed;
        public static float Scale;

        public Color Tint = Color.White;

        public float ySpeed;

        private double timeToJump = -1;

        public float fitness = -1;

        public NeuralNetwork.NeuralNetwork Brain;

        public Bird(Vector2 position, Texture2D texture, NeuralNetwork.NeuralNetwork brain) : base(position, texture)
        {
            Brain = brain;
        }

        public void Update(GameTime gameTime, bool jump)
        {
            if (timeToJump > 0)
            {
                timeToJump -= 1/60f;
            }
            else if(jump)
            {
                timeToJump = .1;
                ySpeed = JumpPower;
            }
            ySpeed += Gravity/60f;
            Position.Y += ySpeed;
        }

        public override void Update(GameTime gameTime)
        {
            Update(gameTime, false);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int layerDepth = Tint != Color.Red ? 0 : 1;
            spriteBatch.Draw(Texture, Position, null, Tint, 0f, Vector2.Zero, Scale, SpriteEffects.None, layerDepth);
        }
    }
}
