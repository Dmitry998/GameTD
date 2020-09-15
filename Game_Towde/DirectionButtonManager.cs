using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Towde
{
    class DirectionButtonManager
    {
        List<DirectionButton> directionButtons;
        float timer = 0;
        SoundEffect sound = Game1.sounds["button_pressed"];

        public DirectionButtonManager(List<DirectionButton> directionButtons)
        {
            this.directionButtons = directionButtons;
        }
        public void Update(GameTime gameTime)
        {
            int temp = -1;
            DirectionButton prevButton = null;
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (DirectionButton button in directionButtons)
            {
                button.Update(gameTime);
                if (button.Pressed)
                {
                    sound.Play();
                    temp = button.Priority;
                    prevButton = button;
                    if (timer >= 0.2f)
                    {
                        if (button.Priority < 3)
                        {
                            button.Priority++;
                        }
                        else
                            button.Priority = 0;
                        timer = 0;
                        prevButton = FindButtonWithPriority(button.Priority, button);
                        if (prevButton != null)
                            prevButton.Priority = temp;
                    }
                    button.Pressed = false;
                }
            }
        }
        public int[] GetPriority()
        {
            int[] priority = new int[directionButtons.Count];
            for (int i = 0; i < directionButtons.Count; i++)
            {
                priority[i] = directionButtons[i].Priority;
            }
            return priority;
        }

        private DirectionButton FindButtonWithPriority(int priority, DirectionButton currentButton)
        {
            foreach (DirectionButton button in directionButtons)
            {
                if (button.Priority == priority && button!=currentButton)
                    return button;
            }
            return null;
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (DirectionButton button in directionButtons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
