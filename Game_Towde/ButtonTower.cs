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
    class ButtonTower : Button
    {
        private int price;
        private int typeOfAction;
        public ButtonTower(Vector2 position, Texture2D texture, SpriteFont font,string message, int price, int typeOfAction) : base(position, texture, font, message)
        {
            this.price = price;
            this.typeOfAction = typeOfAction;
        }
       public int Price
       {
            get { return price; }
       }
       public int GetTypeofAction
       {
           get { return typeOfAction; }
       }
       public override void Draw(SpriteBatch spriteBatch, Color colorText)
       {
            MouseState mouse = Mouse.GetState();
            if (!pressed)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
            else
            {
                spriteBatch.Draw(texture, position, Color.Silver);
            }
            spriteBatch.DrawString(font, price.ToString(), position, colorText);
       }
       public override void Update(GameTime gameTime)
       {
            base.Update(gameTime);
       }
    }
}
