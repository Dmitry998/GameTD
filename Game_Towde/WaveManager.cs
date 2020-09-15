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
    class WaveManager
    {
        private List<Tower> towers = new List<Tower>();
        private Player player;
        private Level level;
        private float exitTime;
        private Texture2D pathTexture;
        private Texture2D enemyTexture;
        private Texture2D healthBarTexture;
        private int amountEnemy = 7;//7
        private int amountWave = 10;//19
        private float timeBetweenW = 12;//10
        private bool rest = true;
        private float timer=0;
        private Wave curWave;
        private int numberWave = 0;
        private bool win = false;
        private int pathLength;
        private Queue<Vector2> path;
        private int[] priority;
        private int numberSkin = 0;

        private int baseHealth = 25;
        private float baseSpeed = 0.8f;
        private int baseValue = 5;


       // List<Texture2D> enemiesTexture = new List<Texture2D>();
        private Queue<Wave> waves = new Queue<Wave>();
        private bool drawWave = false;

        public bool Win
        {
            get { return win; }
        }

        public WaveManager(Player player, Level level,float exitTime,Texture2D enemyTexture,Texture2D healthBarTexture, Texture2D pathTexture)
        {
            this.player = player;
            this.level = level;
            this.exitTime = exitTime;
            this.enemyTexture = enemyTexture;
            //this.amountEnemy = amountEnemy;
            this.healthBarTexture = healthBarTexture;
            this.pathTexture = pathTexture;

            int k = 4;
            for (int i = 0; i < amountWave ; i++)
            {
                if (i >= 4)
                {
                    k += 2;
                }
                Wave wave = new Wave(exitTime, level, enemyTexture, healthBarTexture, player, amountEnemy+i, baseHealth + (i+2)*k, baseSpeed+(i*0.5f), baseValue+i,i+1,numberSkin);
                numberSkin++;
                if (numberSkin > 3)
                    numberSkin = 0;
                waves.Enqueue(wave);
            }
            //towers.Clear();// пока не понятно нужно ли
        }
        public int[] SetPriority
        {
            set { priority = value; }
        }
        public bool Rest
        {
            get { return rest; }
            set { rest = value; }
        }
        public void Update(GameTime gameTime)
        {
            if (rest)
            {

                if (numberWave >= 1)
                {
                    player.CurLVL = 2; // На 2ой волне начинаем 2ой уровень !!!
                }

                player.SetTower(); // пока можем ставить башни
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;


                foreach (Wave wave in waves) // Новое!!
                {
                    wave.SetPriority = priority;
                }


                if (timer > timeBetweenW)
                {
                    rest = false;
                    if (waves.Count() > 0)
                    {
                        curWave = waves.Dequeue();
                        //pathLength = curWave.GetLengthPath;
                        numberWave++;

                        towers = player.GetTowers;
                        foreach (Tower tower in towers)
                        {
                            tower.SetEnemies(curWave.GetEnemies);
                        }
                    }
                    /*else
                    {
                        win = true;// Игра пройдена
                        //player.Defeat = true;
                    }*/
                }
            }
            else
            {
                drawWave = true;
                curWave.Update(gameTime);
                pathLength = curWave.GetLengthPath;
                path = curWave.Path;
                if (curWave.EndWave)
                 {
                    if (waves.Count == 0)
                        win = true;
                    rest = true;
                    player.Gold += numberWave * 10;
                    timer = 0;
                    drawWave = false;
                }

            }
            foreach (Tower tower in towers)
            {
                tower.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (drawWave)
            {
                foreach (Vector2 pos in path)
                {
                    //spriteBatch.Draw(pathTexture, new Vector2(pos.X*64, pos.Y*64), Color.White);
                    spriteBatch.Draw(pathTexture, new Vector2(pos.X * 64 + pathTexture.Width/4, pos.Y * 64 + pathTexture.Height/4), null, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 1);
                }
                curWave.Draw(spriteBatch);
            }
            int t = (int)(timeBetweenW - timer); // раньше был float
            if (!win)
            {
                if (t > 0)
                {
                    spriteBatch.DrawString(font, "Время до волны: " + t.ToString(), new Vector2(520, 60), Color.Black);
                    spriteBatch.DrawString(font, "Стройте башни ", new Vector2(520, 100), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "Волна идет", new Vector2(520, 60), Color.Black);
                    spriteBatch.DrawString(font, "Длина пути: " + pathLength.ToString(), new Vector2(520, 100), Color.Black);
                }
                if (numberWave > 0)
                {
                    spriteBatch.DrawString(font, "Волна номер " + numberWave.ToString(), new Vector2(520, 80), Color.Black);
                }
                spriteBatch.DrawString(font, "Башни:", new Vector2(520, 120), Color.Black);
            }

        }

    }
}
