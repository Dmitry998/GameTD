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
    enum StateMenu : int { Game, Exit,None };

    class Menu
    {
        float timer = 0;
        private List<Button> buttons;
        StateMenu state;

        public StateMenu GetState
        {
            get { return state; }
            set { state = value; }
        }

        public Menu(List<Button> buttons)
        {
            this.buttons = buttons;
            foreach (Button button in buttons)
            {
                button.Pressed = false;
            }
            state = StateMenu.None;
        }

        public void Update(GameTime gameTime)
        {
            timer+= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > 0.1f)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].Update(gameTime);
                    if (buttons[i].Pressed)
                        state = (StateMenu)i;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch, Color.Black);
            }
        }
    }
}
