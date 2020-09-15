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
    class Bullet : Sprite
    {

        private Enemy target;
        private float speed = 20; // скорость пули // надо вынести в башню наверное
        private float slowIndex;
        private int fireDamage;
        private bool alive = true;
        private int damage;
        private Modifications modification;
        private Tower towerParent;

        public Bullet(Vector2 position, Texture2D texture, Enemy target, int damage, Modifications modification, Tower towerParent) : base(position, texture)
        {
            this.target = target;
            this.damage = damage;
            this.modification = modification;
            this.towerParent = towerParent;
        }
        public override void Update(GameTime gameTime)
        {
            if (target != null)
            {
                if (Vector2.Distance(position, target.Position) >= speed)
                {
                    Vector2 direction = target.Position - this.position;
                    direction.Normalize();
                    Vector2 velocity = Vector2.Multiply(direction, speed);
                    position += velocity;
                }
                else
                {
                    if (alive)
                    {
                        target.CurrentHeatlth -= damage;
                        switch(modification)
                        {
                            case Modifications.Slowdown:
                                if(towerParent.LevelTower == 1)
                                {
                                    slowIndex = 0.85f;
                                }
                                if (towerParent.LevelTower == 2)
                                {
                                    slowIndex = 0.65f;
                                }
                                if (towerParent.LevelTower == 3)
                                {
                                    slowIndex = 0.3f;
                                }
                                target.AddDebaff(new SlowDebaff(target,slowIndex));
                                break;
                            case Modifications.Burn:
                                if (towerParent.LevelTower == 1)
                                {
                                    fireDamage = 1;
                                }
                                if (towerParent.LevelTower == 2)
                                {
                                    fireDamage = 3;
                                }
                                if (towerParent.LevelTower == 3)
                                {
                                    fireDamage = 5;
                                }
                                target.AddDebaff(new FireDebaff(target,fireDamage));
                                break;
                            default:
                                break;
                        }
                        alive = false;
                    }
                }
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                // spriteBatch.Draw(texture, position, spriteRectangle, Color.White);
                spriteBatch.Draw(texture, position, new Rectangle(0,0,texture.Width,texture.Height), Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            }
        }
    }
}
