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
    class WaveAlgorithm
    {
        private int[,] map;
        private Queue<Vector2> path = new Queue<Vector2>(); // путь 
        private Vector2 start = new Vector2();
        public WaveAlgorithm(int [,] map)
        {
            this.map = map;
        }
        public void StartPosition(Enemy enemy)
        {
            enemy.Position = new Vector2(start.X * 64, start.Y * 64);
        }
        public Queue<Vector2> SetPath()
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            int[,] nmap = new int[width, height]; // карта для распространения волны
            start = new Vector2();
            Vector2 finish = new Vector2();

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
                        case 4:
                            nmap[x, y] = -1;// Пространство вокруг башни
                            break;
                        case 20://////////////////////////////// БАШНЯ!
                            nmap[x, y] = -1;
                            break;
                        case 10:
                            nmap[x, y] = -1;// Пространство вокруг башни // поменял на 10 вместо 4
                            break;
                        default:
                            nmap[x, y] = -1;//Если что-то другое, то будет как неизвестность
                            break;
                    }
                }
            }
            //установим врага на позицию старта
            //position = new Vector2(start.X * 64, start.Y * 64); Перед выходом каждого врага ставим на позицию старта
            // Распространение волны
            int step = 0; // текущий шаг
            bool add = true;
            nmap[(int)finish.X, (int)finish.Y] = step;//Начинаем с финиша

            while (add)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (nmap[x, y] == step)
                        {
                            if (x + 1 < width && nmap[x + 1, y] != -2 && (nmap[x + 1, y] > step - 1 || nmap[x + 1, y] == -1)) //не выходит ли за границу / не препятствие / пришли от туда- или если это не перв шаг
                            {
                                nmap[x + 1, y] = step + 1 + map[x + 1, y];
                            }
                            if (x - 1 >= 0 && nmap[x - 1, y] != -2 && (nmap[x - 1, y] > step - 1 || nmap[x - 1, y] == -1))
                            {
                                nmap[x - 1, y] = step + 1 + map[x - 1, y];
                            }
                            if (y + 1 < height && nmap[x, y + 1] != -2 && (nmap[x, y + 1] > step - 1 || nmap[x, y + 1] == -1))
                            {
                                nmap[x, y + 1] = step + 1 + map[x, y + 1];
                            }
                            if (y - 1 >= 0 && nmap[x, y - 1] != -2 && (nmap[x, y - 1] > step - 1 || nmap[x, y - 1] == -1))
                            {
                                nmap[x, y - 1] = step + 1 + map[x, y - 1];
                            }
                        }
                    }
                }

                // 14.02.20 Смотрим клетку в которую переходим и смотрим ее соседей и если она граничит с башней добавляем ей вес (Так получится если она граничит сразу с несколькими башнями, то проходить мимо 2 рядом стоящих будет очень невыгодно)
                step++;
                if (nmap[(int)start.X, (int)start.Y] != -1)
                {
                    add = false; // Волна дошла до нужной точки
                }
                else if (step > width * height * 1000)
                {
                    ;
                }
            }
            // Ищем сам путь
            Vector2 current = start;

            while (step >= 0)
            {
                step--;
                //Приоритет (24 варианта (24*4 = 96 строк ))

                bool left = current.X - 1 >= 0 && nmap[(int)current.X - 1, (int)current.Y] == step;
                Vector2 leftVector = new Vector2(current.X - 1, current.Y);
                bool right = current.X + 1 < width && nmap[(int)current.X + 1, (int)current.Y] == step;
                Vector2 rightVector = new Vector2(current.X + 1, current.Y);
                bool up = current.Y + 1 < height && nmap[(int)current.X, (int)current.Y + 1] == step;
                Vector2 upVector = new Vector2(current.X, current.Y + 1);
                bool down = current.Y - 1 >= 0 && nmap[(int)current.X, (int)current.Y - 1] == step;
                Vector2 downVector = new Vector2(current.X, current.Y - 1);

                bool priority0 = left;
                Vector2 priorityVector0 = leftVector;
                bool priority1 = up;
                Vector2 priorityVector1 = upVector;
                bool priority2 = right;
                Vector2 priorityVector2 = rightVector;
                bool priority3 = down;
                Vector2 priorityVector3 = downVector;

                /*bool priority0 = down;
                Vector2 priorityVector0 = downVector;
                bool priority1 = left;
                Vector2 priorityVector1 = leftVector;
                bool priority2 = right;
                Vector2 priorityVector2 = rightVector;
                bool priority3 = up;
                Vector2 priorityVector3 = upVector;*/



                if (priority0)
                {
                    current = priorityVector0;
                    path.Enqueue(current);
                }
                else if (priority1)
                {
                    current = priorityVector1;
                    path.Enqueue(current);
                }
                else if (priority2)
                {
                    current = priorityVector2;
                    path.Enqueue(current);
                }
                else if (priority3)
                {
                    current = priorityVector3;
                    path.Enqueue(current);
                }
            }
            return path;
        }
    }
}
