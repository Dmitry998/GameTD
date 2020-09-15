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
    class Debaff
    {
        protected Enemy enemy;
        ///protected float lifeTime;
        protected bool endDebaff = false;

        public Debaff(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public bool EndDebaff
        {
            get { return endDebaff; }
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
