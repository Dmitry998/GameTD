using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Game_Towde
{
    class ArrowTower : Tower
    {
        protected static int price = 10;
        public ArrowTower(Vector2 position, Texture2D texture,  Texture2D bulletTexture) : base(position, texture, bulletTexture)
        {
            Sound = Game1.sounds["shot"];
            damage = 5;
            weightCEll = 40; // вес клетки для волнового алгоритма
            weightCEllAround = 4; // вес клетки для волнового алгоритма  вокруг башни
            value = price;
            radius = 96;
            strength = 10;
            timeBetweenShots = 1f;
            timer = timeBetweenShots;
            startStrength = strength;
        }
        public override void LevelUp()
        {
            base.LevelUp();
        }
        public override void Shoot(GameTime gameTime)
        {
            base.Shoot(gameTime);
        }

        public static int GetPrice
        {
            get { return price; }
        }

        public override void AddBullet(Vector2 position, Texture2D bulletTexture, Enemy enemy, int damage)
        {
            Bullet bullet = new Bullet(position, bulletTexture, enemy, damage, Modifications.Nothing,this);
            bullets.Add(bullet);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
