using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyNeuron
{
    abstract class Sprite
    {
        public Vector2 Position;
        public Texture2D Texture;

        public Sprite(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
