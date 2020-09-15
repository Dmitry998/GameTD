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
    enum Modifications : int { Nothing, Slowdown, Burn };

    class Tower : Sprite
    {
        protected SoundEffect sound;
        protected Color colorTower;
        protected int levelTower = 1;
        protected int damage;
        protected int weightCEll;
        protected int weightCEllAround;
        protected int value;
        protected int radius;
        protected int strength;
        protected int startStrength;
        protected float timeBetweenShots; // время перезарядки
        protected float timer;
        protected List<Enemy> enemies;
        protected List<Bullet> bullets = new List<Bullet>();
        protected Texture2D bulletTexture;

        public SoundEffect Sound { get { return sound; } set { sound = value; } }

        public Tower(Vector2 position, Texture2D texture, Texture2D bulletTexture) : base(position, texture)
        {
            /*this.damage = damage;
            this.value = value;
            this.radius = radius;
            strength = 10;*/
            this.bulletTexture = bulletTexture;
        }
        public int WeightCell
        {
            get { return weightCEll; }
            set { weightCEll = value; }
        }
        public int WeightCellAround
        {
            get { return weightCEllAround; }
            set { weightCEllAround = value; }
        }
        public int LevelTower
        {
            get { return levelTower; }
            set { levelTower = value; }
        }
        public Texture2D GetTexture
        {
            get { return texture; }
        }
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public int Radius
        {
            get { return radius; }
        }
        public int Value
        {
            get { return value; }
        }

        public int StartStrength
        {
            get { return startStrength; }
        }
        public int Strength
        {
            get { return strength; }
            set { strength = value; }
        }
        public Vector2 Position
        {
            get { return position; }
        }
        public virtual void LevelUp()
        {
            if (levelTower > 1)
            {
                damage += 2;
                radius += 64;
                value += 5 * levelTower;
                weightCEll += 20;
                WeightCellAround += 4;
                if (GetType() == typeof(ArrowTower))
                    timeBetweenShots -= 0.2f;

                if (strength != startStrength)
                {
                    strength = startStrength + 5;
                }
                else
                    strength += 5;
                startStrength = strength;
            }
        }

        public void SetEnemies(List<Enemy> enemies)
        {
            this.enemies = enemies;
        }

        public  virtual void Shoot(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (enemies != null)
            {
                if (timer >= timeBetweenShots)
                {
                    if (enemies.Count > 0)
                    {
                        foreach (Enemy enemy in enemies)
                        {
                            if (Vector2.Distance(this.position, enemy.Position) <= radius) // если враг в радиусе башни
                            {
                                //SoundEffect mySound = Game1.sounds["shot"];
                                sound.Play();
                                AddBullet(position, bulletTexture, enemy, damage);
                                timer = 0;
                                break;
                            }
                        }
                    }
                }
            }
            if (bullets != null && bullets.Count > 0)
            {
                foreach (Bullet bullet in bullets)
                {
                    bullet.Update(gameTime);
                }
            }
        }

        public virtual void AddBullet(Vector2 position, Texture2D bulletTexture, Enemy enemy, int damage)
        {
        }

        public void AddOrDeleteWeightToMap(int [,] map, bool add)
        {
            if (add)
            {
                map[(int)Position.X / 64, (int)Position.Y / 64] += WeightCell;
            }
            else
                map[(int)Position.X / 64, (int)Position.Y / 64] -= WeightCell;
            int x = -1;
            int y = -1;

            int cellX = (int)Position.X / 64;
            int cellY = (int)Position.Y / 64;
            for (int i = 1; i < 10; i++)
            {
                if (cellX + x < map.GetLength(0) && cellX + x >= 0 && cellY + y < map.GetLength(1) && cellY + y >= 0)
                {
                    if (map[cellX + x, cellY + y] != 1 && map[cellX + x, cellY + y] != 2 && map[cellX + x, cellY + y] != 3)
                    {
                        if (x != 0 || y != 0)
                        {
                            if (add)
                            {
                                map[cellX + x, cellY + y] += WeightCellAround; // вес клеток вокруг башни;
                            }
                            else
                                map[cellX + x, cellY + y] -= WeightCellAround;
                        }
                    }

                }
                if (i % 3 == 0)
                {
                    x++;
                    y = -1;
                }
                else
                {
                    y++;
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            Shoot(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (strength > 0)
            {
                switch (levelTower)
                {
                    case 1:
                        colorTower = Color.White;
                        break;
                    case 2:
                        if(GetType() == typeof(ArrowTower))
                            colorTower = Color.Silver;
                        if (GetType() == typeof(FrostTower))
                            colorTower = Color.BlueViolet;
                        if (GetType() == typeof(FireTower))
                            colorTower = Color.Orange;
                        break;
                    case 3:
                        if (GetType() == typeof(ArrowTower))
                            colorTower = Color.Goldenrod;
                        if (GetType() == typeof(FrostTower))
                            colorTower = Color.Blue;
                        if (GetType() == typeof(FireTower))
                            colorTower = Color.Red;
                        break;
                    default:
                        colorTower = Color.White;
                        break;

                }
                spriteBatch.Draw(texture, position, colorTower);
                //spriteBatch.Draw(texture, position, null, colorTower, 0, Vector2.Zero, 1f, SpriteEffects.None, 0); / / надо разобраться с глубиной (последний параметр)
                foreach (Bullet bullet in bullets)
                {
                    bullet.Draw(spriteBatch);
                }
            }
        }
    }
}
