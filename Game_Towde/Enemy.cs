using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Game_Towde
{
    class Enemy : Sprite
    {

        private int danger = 0;

        private int health;
        private int currentHealth; 
        private float speed;
        private float startSpeed;
        private int damage;
        private int bounty; 
        private bool alive = true;
        private string elapsedTime;
        private Texture2D healthBarTexture;

        //private int PathLength;// длина пути

        private SpriteEffects spriteEffect = SpriteEffects.None;

        private List<Debaff> debaffs = new List<Debaff>();
        private Modifications modification = Modifications.Nothing;
        private Color color = Color.White;

        private float totalElapsed = 0;//Время, которое прошло с начала отображения текущего кадра
        private int numberOfFrame = 0;//Номер фрейма
        private int frames;
        private List<Tower> towers = new List<Tower>();
        private Player player;
        private Queue<Vector2> path = new Queue<Vector2>(); // путь 

        protected Rectangle spriteRectangle;
        private int[,] map;
        private int[,] mapWave;
        private int[,] passedCell;

        public int[,] GetMapWave
        {
            get { return mapWave;  }
        }
       

        public Modifications SetModifications
        {
            set { modification = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public void AddDebaff(Debaff debaff)
        {
            debaffs.Add(debaff);
        }
        public int Danger { get { return danger; } }

        public Enemy(Texture2D texture, Texture2D healthBarTexture,  int health, float speed, int bounty,int frames, Player player) : base(texture)
        {
            startSpeed = speed;
            this.healthBarTexture = healthBarTexture;
            this.health = health;
            this.currentHealth = health;
            this.speed = speed;
            this.bounty = bounty;
            this.frames = frames;
            this.player = player;
            towers = player.GetTowers;
            damage = 5; // урон пока у всех одинковый
        }

        private bool CellWithTower(int x, int y)
        {
            foreach (Tower tower in towers)
            {
                if (tower.Position.X == x * 64 && tower.Position.Y == y * 64)
                    return true;
            }
            return false;
        }

        public int GetLengthPath
        {
            get { return path.Count; }
        }
        
        public Queue<Vector2> Path
        {
            get { return path; }
            set { path = value; }
        }

        public float Speed
        {
            set { speed = value; }
            get { return speed; }
        }
        public float StartSpeed
        {
            get { return startSpeed; }
        }
        public int CurrentHeatlth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public double PecentHealth
        {
            get { return (double)currentHealth / (double)health; }
        }

        public int Bounty
        {
            get { return bounty; }
        }
 

        public bool Isdead
        {
            get { return !alive; } 
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }


        public string GetPathLayingTime { get { return elapsedTime; } } // время на прокладку трассы.

        public void SetMap(int [,] map)
        {
            this.map = map;
        }

        public void Setpath(int [] priorityNumber)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            passedCell = new int[width,height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    passedCell[x, y] = 0;
                }
            }
            Stopwatch stopWatch = new Stopwatch(); // время прокладки трассы
            bool [] priority = new bool [4];
            Vector2[] priorityVector = new Vector2[4];
            int[,] nmap = new int[width, height]; // карта для распространения волны
            Vector2 start = new Vector2();
            Vector2 finish = new Vector2();

            stopWatch.Start();
            //Заполнение карты для волнового алгоритма
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    switch (map[x, y])
                    {
                        case 0:
                            nmap[x, y] = -1;// Ещё не прошли там
                            break;
                        case 1:
                            nmap[x, y] = -2;// Препятствие
                            break;
                        case 2:
                            nmap[x, y] = -1;// Старт
                            start = new Vector2(x, y);
                            break;
                        case 3:
                            nmap[x, y] = -1;// Финиш
                            finish = new Vector2(x, y);
                            break;
                        default:
                            nmap[x, y] = -1;//Если что-то другое, то будет как неизвестность
                            break;
                    }
                }
            }
            //установим врага на позицию старта

            position = new Vector2(start.X * 64, start.Y * 64);
            int step = 0;
            bool add = true;
            nmap[(int)finish.X, (int)finish.Y] = step;
            passedCell[(int)finish.X, (int)finish.Y] = 1;
            while (add)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (nmap[x, y] == step)
                        {
                            if (x + 1 < width && nmap[x + 1, y] != -2 && ((passedCell[x + 1, y] == 1 && nmap[x + 1, y] > step + 1 + map[x + 1, y]) || nmap[x + 1, y] == -1))
                            {
                                nmap[x + 1, y] = step + 1 + map[x + 1, y];
                                passedCell[x + 1, y] = 1;
                            }
                            if (x - 1 >= 0 && nmap[x - 1, y] != -2 && ((passedCell[x - 1, y] == 1 && nmap[x - 1, y] > step + 1 + map[x - 1, y]) || nmap[x - 1, y]==-1))
                            {
                                nmap[x - 1, y] = step + 1 + map[x - 1, y];
                                passedCell[x - 1, y] = 1;
                            }
                            if (y + 1 < height && nmap[x, y + 1] != -2 && ((passedCell[x, y+1] == 1 && nmap[x, y+1] > step + 1 + map[x, y+1]) || nmap[x, y + 1]==-1))
                            {
                                nmap[x, y + 1] = step + 1 + map[x, y + 1];
                                passedCell[x, y + 1] = 1;
                            }
                            if (y - 1 >= 0 && nmap[x, y - 1] != -2 && ((passedCell[x, y-1] == 1 && nmap[x, y-1] > step + 1 + map[x, y-1]) || nmap[x, y - 1]==-1))
                            {
                                nmap[x, y - 1] = step + 1 + map[x, y - 1];
                                passedCell[x, y - 1] = 1;
                            }
                        }
                    }
                }
                step++;
                if (nmap[(int)start.X, (int)start.Y] != -1)
                {
                    add = false;
                    nmap[(int)start.X, (int)start.Y] = step;
                    mapWave = nmap;
                    path.Enqueue(start);
                }
            }
            // Ищем сам путь
            priorityNumber = Game1.priority;
            int minStep = width * height + 10000;
            Vector2 current = start;
            while (minStep > 0)
            {
                minStep = width * height + 10000;
                if (current.X - 1 >= 0)
                {
                    if (nmap[(int)current.X - 1, (int)current.Y] >= 0 && nmap[(int)current.X - 1, (int)current.Y] < minStep)
                        minStep = nmap[(int)current.X - 1, (int)current.Y];
                }
                if (current.X + 1 < width)
                {
                    if (nmap[(int)current.X + 1, (int)current.Y] >= 0 && nmap[(int)current.X + 1, (int)current.Y] < minStep)
                        minStep = nmap[(int)current.X + 1, (int)current.Y];
                }
                if (current.Y + 1 < height)
                {
                    if (nmap[(int)current.X, (int)current.Y + 1] >= 0 && nmap[(int)current.X, (int)current.Y + 1] < minStep)
                        minStep = nmap[(int)current.X, (int)current.Y + 1];
                }
                if (current.Y - 1 >= 0)
                {
                    if (nmap[(int)current.X, (int)current.Y - 1] >= 0 && nmap[(int)current.X, (int)current.Y - 1] < minStep)
                        minStep = nmap[(int)current.X, (int)current.Y - 1];
                }

                bool left = current.X - 1 >= 0 && nmap[(int)current.X - 1, (int)current.Y] == minStep;
                Vector2 leftVector = new Vector2(current.X - 1, current.Y);
                bool right = current.X + 1 < width && nmap[(int)current.X + 1, (int)current.Y] == minStep;
                Vector2 rightVector = new Vector2(current.X + 1, current.Y);
                bool up = current.Y + 1 < height && nmap[(int)current.X, (int)current.Y + 1] == minStep;
                Vector2 upVector = new Vector2(current.X, current.Y + 1);
                bool down = current.Y - 1 >= 0 && nmap[(int)current.X, (int)current.Y - 1] == minStep;
                Vector2 downVector = new Vector2(current.X, current.Y - 1);

                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0: priority[priorityNumber[i]] = left;
                            priorityVector[priorityNumber[i]] = leftVector;
                            break;
                        case 1:
                            priority[priorityNumber[i]] = right;
                            priorityVector[priorityNumber[i]] = rightVector;
                            break;
                        case 2:
                            priority[priorityNumber[i]] = up;
                            priorityVector[priorityNumber[i]] = upVector;
                            break;
                        case 3:
                            priority[priorityNumber[i]] = down;
                            priorityVector[priorityNumber[i]] = downVector;
                            break;

                    }
                }

                if (priority[0])
                {
                    current = priorityVector[0];
                    path.Enqueue(current);
                }
                else if (priority[1])
                {
                    current = priorityVector[1];
                    path.Enqueue(current);
                }
                else if (priority[2])
                {
                    current = priorityVector[2];
                    path.Enqueue(current);
                }
                else if (priority[3])
                {
                    current = priorityVector[3];
                    path.Enqueue(current);
                }
            }

            stopWatch.Stop();
            elapsedTime = stopWatch.Elapsed.ToString() + " миллисекунд";
            //elapsedTime = stopWatch.ElapsedMilliseconds.ToString() + " милисекунд";
            //var before = DateTime.Now;
            //var spendTime = DateTime.Now - before;
            //elapsedTime = spendTime.Ticks.ToString() + " тактов";
            //elapsedTime = stopWatch.ElapsedTicks.ToString() + " тактов";

            List<Vector2> trace = path.ToList();
            for (int i = 0; i < trace.Count; i++)
            {
                foreach (Tower tower in towers)
                {
                    if (trace[i] == new Vector2((tower.Position.X) / 64, (tower.Position.Y) / 64))
                        danger += 5 + tower.LevelTower;

                    if (trace[i] == new Vector2((tower.Position.X) / 64 - 1, (tower.Position.Y) / 64 - 1))
                        danger+=tower.LevelTower;
                    else if (trace[i] == new Vector2((tower.Position.X) / 64 - 1, (tower.Position.Y) / 64))
                        danger += tower.LevelTower;
                    else if (trace[i] == new Vector2((tower.Position.X) / 64, (tower.Position.Y) / 64 - 1))
                        danger += tower.LevelTower;
                    else if (trace[i] == new Vector2((tower.Position.X) / 64 , (tower.Position.Y) / 64 + 1))
                        danger += tower.LevelTower;
                    else if (trace[i] == new Vector2((tower.Position.X) / 64 + 1, (tower.Position.Y) / 64))
                        danger += tower.LevelTower;
                    else if (trace[i] == new Vector2((tower.Position.X) / 64 + 1, (tower.Position.Y) / 64 + 1))
                        danger += tower.LevelTower;
                    else if (trace[i] == new Vector2((tower.Position.X) / 64 - 1, (tower.Position.Y) / 64 + 1))
                        danger += tower.LevelTower;
                    else if (trace[i] == new Vector2((tower.Position.X) / 64 + 1, (tower.Position.Y) / 64 - 1))
                        danger += tower.LevelTower;
                }
            }

        }
        private void Move()
        {

            if (path.Count > 0)
            {
                Vector2 curPoint = path.Peek();
                curPoint.X *= 64;
                curPoint.Y *= 64;

                if (position.X - curPoint.X > 0) // Если равно нулю, то мы ничего не меняем
                {
                    spriteEffect = SpriteEffects.None;
                }
                else if (position.X - curPoint.X < 0)
                {
                    spriteEffect = SpriteEffects.FlipHorizontally;
                }

                if (Vector2.Distance(position, curPoint) < speed)
                {
                    position = curPoint;
                    if (/*map[(int)curPoint.X/64, (int)curPoint.Y/64] == 20*/ CellWithTower((int)curPoint.X / 64, (int)curPoint.Y / 64)) // если путь прошел через башню, то надо уменьшить прочность у башни
                    {
                        foreach (Tower tower in towers)
                        {
                            if (tower.Position == curPoint)
                            {
                                //speed = speed / 4;
                                speed = speed / 4;
                                startSpeed = startSpeed / 4; // Если нет дебафов, то скорость вернется к начальной, поэтому снижаем начальную
                                tower.Strength -= damage;
                                if (tower.Strength <= 0) // Надо вынести отсюда
                                {
                                    Game1.sounds["deleteTower"].Play();
                                    player.RemoveTower(tower);
                                    tower.AddOrDeleteWeightToMap(map, false);
                                    break;
                                }

                            }
                        }
                    }
                    path.Dequeue();
                }
                else
                {
                    Vector2 direction = curPoint - position;
                    direction.Normalize();//вектор того же направления но единичной длины
                    Vector2 velocity = Vector2.Multiply(direction, speed);
                    position += velocity;
                }
            }
            else
                alive = false;
        }

        public void Animation(float tmer)
        {
            const int framesPerSec = 9;
            const int widthFrame = 64;//ширина кадра
            float timePerFrame = (float)1 / framesPerSec; // время на кадр

            totalElapsed += tmer;

            if (totalElapsed > timePerFrame)
            {
                if (numberOfFrame == frames - 1)
                {
                    numberOfFrame = 0;
                }
                else
                {
                    numberOfFrame++;
                }
                spriteRectangle = new Rectangle((int)widthFrame * numberOfFrame, 0, 64, 64);
                totalElapsed = 0;
            }

        }
        public override void Update(GameTime gameTime)
        {
            float tmer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Move();
            if (currentHealth <= 0)
            {
                alive = false;
                Game1.soundDeathRobot.Play();
            }
            Animation(tmer);

            //Debaff(gameTime);

            if (debaffs.Count != 0)
            {
                Debaff debaffForDelete = null;
                foreach (Debaff debaff in debaffs)
                {
                    debaff.Update(gameTime);
                    if (debaff.EndDebaff)
                    {
                        debaffForDelete = debaff;
                    }
                }
                if (debaffForDelete != null)
                {
                    debaffs.Remove(debaffForDelete);
                }
            }
            else
            {
                color = Color.White;
                speed = startSpeed;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Isdead)
            {
                Vector2 positionHealthBar = new Vector2(position.X + 12, position.Y - 15);
                int widthHealthBar = 40;
                int heightHealthBar = 8;
                int widthHealthBarCurrent = (int)(widthHealthBar * this.PecentHealth);
                spriteBatch.Draw(healthBarTexture, positionHealthBar, new Rectangle(0, 0, widthHealthBar, heightHealthBar), Color.Gray);
                spriteBatch.Draw(healthBarTexture, positionHealthBar, new Rectangle(0, 0, widthHealthBarCurrent, heightHealthBar), Color.Red);
                spriteBatch.Draw(texture, position, spriteRectangle, color, 0, Vector2.Zero, 0.7f, spriteEffect,0);
            }
        }
    }
}
