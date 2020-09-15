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
    class ButtonManager
    {
        private List<ButtonTower> buttons = new List<ButtonTower>(); // список кнопок 
        private Player player;
        Color colorText;
        public ButtonManager(List<ButtonTower> buttons, Player player)
        {
            this.buttons = buttons;
            this.player = player;
        }

        public void Update(GameTime gameTime)
        {
            foreach (ButtonTower button in buttons)
            {
               /* if (player.CurLVL < 2)
                {
                    if (button.GetTypeofAction == 1)
                    {
                        button.Update(gameTime);
                    }
                }
                else
                {*/
                    button.Update(gameTime);
               // }
                if (button.Pressed)
                {
                    ButtonTower buttonThis = button;
                    foreach (ButtonTower b in buttons)
                    {
                        if (b != button)
                        {
                            b.Pressed = false;
                        }
                    }
                    player.SetTypeOfAction = button.GetTypeofAction;
                }
                //button.Pressed = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ButtonTower button in buttons)
            {
                if (player.Gold >= button.Price)
                {
                    colorText = Color.Yellow;
                }
                else
                {
                    colorText = Color.Red;
                }

                /*if (player.CurLVL < 2) // Если уровень игрока меньше чем 2 то мы можем строить только башни 1 типа
                {
                    if (button.GetTypeofAction == 1)
                    {
                        button.Draw(spriteBatch, colorText);
                    }
                }
                else // Если больше, то любые */
                    button.Draw(spriteBatch, colorText);
            }
        }

    }
}

