using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game_Towde
{
    class SlowDebaff : Debaff
    {

        private float timer;
        private float indexSlow;
        public SlowDebaff(Enemy enemy, float indexSlow) : base(enemy)
        {
            this.indexSlow = indexSlow;
            if (enemy.Speed >= 0.1)
            {
                enemy.Speed = enemy.Speed * indexSlow;
            }
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer < 2)
            {
                enemy.Color = Color.Blue;
            }
            else
                endDebaff = true;
            base.Update(gameTime);
        }
    }
}
