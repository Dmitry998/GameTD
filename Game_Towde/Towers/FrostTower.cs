using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Towde
{
    class FrostTower : Tower
    {
        protected static int price = 15;
        public FrostTower(Vector2 position, Texture2D texture,  Texture2D bulletTexture) : base(position, texture,  bulletTexture)
        {
            Sound = Game1.sounds["frostShot"];
            damage = 2;
            value = price;
            radius = 200;
            strength = 5;
            timeBetweenShots = 2.5f;
            timer = timeBetweenShots;
            startStrength = strength;
            price = value;
            weightCEll = 50;
            weightCEllAround = 5;
        }

        public override void Shoot(GameTime gameTime)
        {
            base.Shoot(gameTime);
        }

        public override void AddBullet(Vector2 position, Texture2D bulletTexture, Enemy enemy, int damage)
        {
            Bullet bullet = new Bullet(position, bulletTexture,  enemy, damage, Modifications.Slowdown,this);
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
