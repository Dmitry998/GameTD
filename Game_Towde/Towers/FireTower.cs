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
    class FireTower : Tower
    {
        protected static int price = 25;
        public FireTower(Vector2 position, Texture2D texture,  Texture2D bulletTexture) : base(position, texture,  bulletTexture)
        {
            Sound = Game1.sounds["fireShot"];
            damage = 8;
            value = price;
            radius = 200;
            strength = 15;
            timeBetweenShots = 3.5f;
            timer = timeBetweenShots;
            startStrength = strength;
            price = value;
            weightCEll = 60;
            weightCEllAround = 6;
        }

        public override void Shoot(GameTime gameTime)
        {
            base.Shoot(gameTime);
        }

        public override void AddBullet(Vector2 position, Texture2D bulletTexture, Enemy enemy, int damage)
        {
            Bullet bullet = new Bullet(position, bulletTexture, enemy, damage, Modifications.Burn,this);
            bullets.Add(bullet);
        }

        public static int GetPrice
        {
            get { return price; }
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
