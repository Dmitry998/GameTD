using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Game_Towde
{
    class Wave
    {
        private Player player;
        private float exitTime; // время выхода очередного монстра
        private Level level;
        private Texture2D enemyTexture;
        private Texture2D healthBarTexture;
        private int amountEnemy;
        private List<Enemy> enemies = new List<Enemy>();
        private float timer;
        private int numberSkin = 0;
        private int health;
        private int numberWave;
        private float speed;
        private int value;
        private int pathLength;
        private Queue<Vector2> path;
        private Queue<Vector2> pathCur;

        private int numberCurrentEnemy = 1;

        // private int[] priority = {3,1,2,0}; // left right up down
        private int[] priority; // left right up down
        bool endWave = false;
        public bool EndWave
        {
            get { return endWave; }
        }

        public Wave(float exitTime, Level level, Texture2D texture, Texture2D healthBarTexture, Player player, int amountEnemy, int health, float speed, int value, int numberWave, int numberSkin)
        {
            this.amountEnemy = amountEnemy;
            this.exitTime = exitTime;
            this.level = level;
            this.player = player;
            enemyTexture = texture;
            this.healthBarTexture = healthBarTexture;
            timer = exitTime; // первый враг выходит без задержки
            this.health = health;
            this.speed = speed;
            this.value = value;
            this.numberWave = numberWave;
            this.numberSkin = numberSkin;
        }

        public List<Enemy> GetEnemies
        {
            get { return enemies; }
        }

        private void WritePathStatistic(int pathLength, Enemy enemy)
        {
            File.AppendAllText("PathStatistic.txt", "Волна номер " + numberWave.ToString() + "; Враг номер " + numberCurrentEnemy.ToString() + "; Длина пути " + pathLength.ToString() + "; Время затраченное на прокладку пути: " + enemy.GetPathLayingTime + "; Опасность пути: " + enemy.Danger.ToString() + "; Количество башен на карте " + player.GetTowers.Count + "; \n");
        }
        private void AddEnemy()
        {
            enemyTexture = Game1.enemySkins[numberSkin];
            Enemy enemy = new Enemy(enemyTexture,healthBarTexture, health, speed, value, 3,player);
            enemies.Add(enemy);
            enemy.SetMap(level.map);
            enemy.Setpath(priority);
            pathLength = enemy.GetLengthPath;
            path = enemy.Path;
            pathCur = new Queue<Vector2>(path);
            WritePathStatistic(path.Count,enemy);
            numberCurrentEnemy++;
        }

        public int[] SetPriority
        {
            set { priority = value; }
        }

        public int GetLengthPath
        {
            get { return pathLength; }
        }

        public Queue<Vector2> Path
        {
            get { return path; }
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= exitTime && amountEnemy>0)
            {
                AddEnemy();
                amountEnemy--;
                timer = 0;
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];

                enemy.Update(gameTime);
                if (enemy.Isdead)
                {
                    if (enemy.CurrentHeatlth > 0)
                    {
                        Game1.sounds["enemyPassed"].Play();
                        player.Lives--;
                    }
                    else
                    {
                        player.Gold += enemy.Bounty;
                        player.Total += enemy.Bounty * 10;
                    }
                    enemies.Remove(enemy);
                    i--;
                    if (amountEnemy==0 && enemies.Count() == 0) // enemies.Count() == 0 && timer>=exitTime
                    {
                        endWave = true;
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
            if (enemies.Count > 0)
            {
                int indentionX = 0;
                int indentionY = 0;
                for (int i = level.Height - 1; i >= 0; i--)
                {
                    for (int j = 0; j < level.Width; j++)
                    {
                        Color colorText = Color.Black;
                        string txt = "";
                        Enemy curEnemy = enemies[enemies.Count - 1];
                        if (curEnemy.GetMapWave[j, i] == -2)
                            txt = "X";
                        else
                            txt = curEnemy.GetMapWave[j, i].ToString(); // берем матрицу последнего добавленного врага
                        /*foreach (Vector2 vector in curEnemy.Path)
                        {
                            if (vector.X == i && vector.Y == j)
                            {
                                colorText = Color.Orange;
                                break;
                            }
                                
                        }*/
                        foreach (Vector2 vector in pathCur)
                        {
                            if (vector.X == j && vector.Y == i)
                            {
                                colorText = Color.Red;
                                break;
                            }
                        }
                        spriteBatch.DrawString(Game1.font, txt, new Vector2(level.Width * 64 + 310 + indentionX, level.Height * 64 - 250 + indentionY), colorText);
                        indentionX += 25;
                    }
                    indentionX = 0;
                    indentionY -= 25;
                }
            }
        }
    }
}
