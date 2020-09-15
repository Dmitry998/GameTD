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
    class Level
    {
        private int width;
        private int height;
       public int [,] map = 
        {
        {1,0,0,0,0,0,0,0,0,0,0,1},
        {0,3,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,2,0},
        {0,0,0,0,0,0,0,0,0,0,0,0},
        {1,0,0,0,0,0,0,0,0,0,0,1},
        };

        /*public int[,] map =
        {
        {3,0,0,0,0,0,0,0,0,0,0,0,0,1},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},//3 
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},//30
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},//03
        {1,0,0,0,0,0,0,0,0,0,0,0,0,2},
        };//строки y столбцы х*/

        private List<Texture2D> mapTextures = new List<Texture2D>(); // Список текстур для генерации уровня.

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public bool ClickOnMAp(int x, int y)
        {
            if ((x >= 0 && x < width) && y >= 0 && y < height)
            {
                return true;
            }
            else
                return false;
        }
        public Level()
        {
            width = map.GetLength(0);
            height = map.GetLength(1);
        }
        public void AddTexture(Texture2D texture)
        {
            mapTextures.Add(texture);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    int indexTexture = map[x, y];
                    if (indexTexture >= mapTextures.Count)
                    {
                        texture = mapTextures[0];
                    }
                    else
                    {
                        texture = mapTextures[indexTexture];
                    }
                    spriteBatch.Draw(texture, new Rectangle(x * 64, y * 64, 64, 64), Color.White);
                }
            }
        }
    }
}
