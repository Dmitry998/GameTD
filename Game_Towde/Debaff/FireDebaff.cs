using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game_Towde
{
    class FireDebaff : Debaff
    {
        private int fireDamage;
        private float timer;
        private Color prevColor;
        //private float tickrate = 0.7f;

        public FireDebaff(Enemy enemy, int fireDamage) : base(enemy)
        {
            prevColor = enemy.Color;
            this.fireDamage = fireDamage;
        }

        public override void Update(GameTime gameTime)
        {
            timer++;
            if (timer <= 300)
            {
                if (timer == 100 || timer == 200 || timer == 300)
                {
                    enemy.Color = Color.Red;
                    enemy.CurrentHeatlth -= fireDamage;
                }
                else
                {
                    enemy.Color = prevColor;
                }
            }
            else
            {
                endDebaff = true;
            }
            base.Update(gameTime);
        }
    }
}
