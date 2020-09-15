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
    class Button : Sprite
    {
        protected bool pressed = false;
        protected SpriteFont font;
        protected string message;

        public Button(Vector2 position, Texture2D texture,SpriteFont font, string message) : base(position, texture)
        {
            this.message = message;
            this.font = font;
        }
        public bool Pressed
        {
            get { return pressed; }
            set { pressed = value; }
        }

        public void PressedB()
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                float x = mouse.X;
                float y = mouse.Y;
                if ((x >= position.X && x < position.X + texture.Width) && (y >= position.Y && y < position.Y + texture.Height)) // нажали в пределах кнопки
                {
                    Game1.sounds["button_pressed"].Play();
                    pressed = true;
                }
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch, Color colorText)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.DrawString(font, message, this.GetCenterSprite, colorText);
        }
        public override void Update(GameTime gameTime)
        {
            PressedB();
            base.Update(gameTime);
        }
    }
}
