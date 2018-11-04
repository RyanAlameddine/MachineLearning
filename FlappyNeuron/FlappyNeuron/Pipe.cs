using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyNeuron
{
    class Pipe : Sprite
    {
        public static int gapHeightOffset;
        public static int pairSeparationWidth;

        public Rectangle topBox;
        public Rectangle bottomBox;

        public Pipe(int y, Texture2D texture) : base(new Vector2(1800, y), texture)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(Position.X, Position.Y + gapHeightOffset), null, Color.White, 0, new Vector2(0, 0), Vector2.One, SpriteEffects.None, .5f);
            spriteBatch.Draw(Texture, new Vector2(Position.X + Texture.Width, Position.Y - gapHeightOffset), null, Color.White, (float) Math.PI, new Vector2(0, 0), Vector2.One, SpriteEffects.None, .5f);
            //spriteBatch.Draw(Texture, new Vector2(Position.X, Position.Y + gapHeightOffset), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.FlipVertically, 0);
        }

        public override void Update(GameTime gameTime)
        {
            Position.X -= Bird.XSpeed;
            topBox = new Rectangle((int)Position.X, (int)Position.Y - gapHeightOffset - Texture.Height, Texture.Width, Texture.Height);
            bottomBox = new Rectangle((int)Position.X, (int)Position.Y + gapHeightOffset, Texture.Width, Texture.Height);
        }
    }
}
