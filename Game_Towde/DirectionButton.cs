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
    class DirectionButton : Button
    {
        private int priority;
        public DirectionButton(Vector2 position, Texture2D texture, SpriteFont font,string message, int priority) : base(position, texture, font,message)
        {
            this.priority = priority;
        }
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.DrawString(font, priority.ToString(), new Vector2 (position.X + 32,position.Y+32), Color.Black);
        }
    }
}
