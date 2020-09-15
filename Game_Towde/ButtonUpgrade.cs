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
    class ButtonUpgrade : Button
    {
        private Tower selectedTower = null;
        private Player player;
        private int priceForUpgarde;
        private int[,] map;
        float timer = 0;
        Color colorButton = Color.White;
        public ButtonUpgrade(Vector2 position, Texture2D texture, SpriteFont font, string message) : base(position, texture, font, message)
        {
        }

        public Tower GetSelectedTower
        {
            get { return selectedTower; }
            set { selectedTower = value; }
        }
        public int[,] Map
        {
            get { return map; }
            set { map = value; }
        }
        public Player GetPlayer
        {
            set { player = value; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (player != null)
            {
                if (selectedTower != null)
                {
                    if (selectedTower.LevelTower < 3)
                    {
                        priceForUpgarde = selectedTower.Value * selectedTower.LevelTower;

                        spriteBatch.Draw(texture, position,/*new Rectangle(0,0,texture.Width + 50, texture.Height),*/ Color.White);
                        string textButton = "Улучшить за " + priceForUpgarde.ToString();
                        spriteBatch.DrawString(font, textButton, this.GetCenterSprite, Color.Black);

                        if (pressed)
                        {
                            if (player.Gold >= priceForUpgarde)
                            {
                                player.Gold -= priceForUpgarde;
                                selectedTower.LevelTower++;
                                selectedTower.AddOrDeleteWeightToMap(map, false); // Сначала удаляем будто, а затем ставим заново с новым весом)
                                selectedTower.LevelUp();
                                selectedTower.AddOrDeleteWeightToMap(map, true);
                            }
                            colorButton = Color.Red;
                        }
                        else
                            colorButton = Color.White;
                    }
                    else
                        spriteBatch.DrawString(font, "Башня максимального уровня", new Vector2(position.X + 5, position.Y + 5), Color.Black);
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed && timer>0.2f)
            {
                float x = mouse.X;
                float y = mouse.Y;
                if ((x >= position.X && x < position.X + texture.Width) && (y >= position.Y && y < position.Y + texture.Height)) // нажали в пределах кнопки
                {
                    pressed = true;
                    if(selectedTower != null && selectedTower.LevelTower < 3 && player.Gold>=priceForUpgarde)
                        Game1.sounds["upgradeTower"].Play();
                    timer = 0;
                }
            }
            else
                pressed = false;
        }
    }
}
