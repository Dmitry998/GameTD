using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Towde
{
    class Sprite
    {
        protected Vector2 position;
        protected Texture2D texture;
        //protected Rectangle spriteRectangle;

        public Vector2 GetCenterSprite
        {
            get { return new Vector2(position.X + texture.Width /6 ,position.Y  + texture.Height/2); }
        }
        public Sprite(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }
        public Sprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Будет переопрделен в наследнике
        }

        public virtual void Update(GameTime gameTime)
        {
            // Будет переопрделен в наследнике
        }

    }
}
