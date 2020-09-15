using System;
using System.Collections.Generic;
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
    class TowerTextureStorage
    {
        private int key;
        private Texture2D towerTexture;
        private Texture2D bulletTexture;

        public Texture2D TowerTexture
        { get { return towerTexture; } }

        public Texture2D BulletTexture
        { get { return bulletTexture; } }

        public int Key
        { get { return key; } }

        public TowerTextureStorage(int key, Texture2D towerTexture, Texture2D bulletTexture)
        {
            this.key = key;
            this.towerTexture = towerTexture;
            this.bulletTexture = bulletTexture;
        }
    }
}
