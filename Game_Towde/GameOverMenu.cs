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
    enum StateGameOvermenu : int { PlayAgain, InMainMenu, None};

    class StorageScoreAndName
    {
        private int score;
        private string name;
        public int Score { get { return score; } set { score = value; } }
        public string Name { get { return name; } set { name = value; } }
        public StorageScoreAndName(int score, string name)
        {
            this.score = score;
            this.name = name;
        }
    }



    class GameOverMenu
    {
        Player player;
        GameTime thisGameTime;
        List<StorageScoreAndName> scoreAndNAme = new List<StorageScoreAndName>();
        SpriteFont font;
        string[] fileRecords;
        List<Button> buttons = new List<Button>();
        StateGameOvermenu state;
        float timerForText = 0;
        bool addNewRecord = false;
        int amountRecords = 5;
        string name = null;

        public StateGameOvermenu State { get { return state; } set { state = value; } }

        public GameOverMenu(Player player, SpriteFont font, List<Button> buttons)
        {
            this.player = player;
            this.font = font;
            this.buttons = buttons;

            state = StateGameOvermenu.None;

            foreach (Button b in buttons)
            {
                b.Pressed = false;
            }

            try
            {
                fileRecords = File.ReadAllLines("Score.txt");
                Getrecords();
                if (player.Total > MinRecord() || fileRecords.Length<5)
                {
                    addNewRecord = true;
                    if (fileRecords.Length<5)
                    {
                        amountRecords = fileRecords.Length + 1; // старые записи + 1 новая добавленная
                    }
                }
            }
            catch
            {
                addNewRecord = true; // Если файла пока не существует
                amountRecords = 1;
            }
            try
            {
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "PathStatistic.txt";
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
            }
            catch { };
        }

        public void Update(GameTime gameTime)
        {
            thisGameTime = gameTime;
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Update(gameTime);
                if (buttons[i].Pressed)
                    state = (StateGameOvermenu)i;
            }
        }
        private void WriteScore(Player pl)
        {
            File.AppendAllText("Score.txt", pl.Total.ToString() + "|" + pl.Name + "\n");
        }
        private int MinRecord()
        {
            string stringMinRecord = fileRecords[fileRecords.Length-1];
            string temp="";
            int i = 0;
            while (stringMinRecord[i] != '|')
            {
                temp += stringMinRecord[i];
                i++;
            }

            int min = Convert.ToInt32(temp);
            return min;
        }

        public void Getrecords()
        {
            string [] tempArray = new string[fileRecords.Length];

            for (int i = 0; i < fileRecords.Length; i++)
            {
                string tempStr;
                int score;
                string number="";
                string name="";
                tempStr = fileRecords[i];
                int j = 0;
                while (tempStr[j]!='|')
                {
                    number += tempStr[j];
                    j++;
                }
                score = Convert.ToInt32(number);
                name = tempStr.Substring(j+1);
                scoreAndNAme.Add(new StorageScoreAndName(score, name));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Статистика построенных путей содержится в файле PathStatistic.txt", new Vector2(100, 450), Color.Black);
            if (addNewRecord)
            {
                if(!player.Defeat)
                    spriteBatch.DrawString(font, "Вы прошли игру! Вы набрали " + player.Total.ToString() + " очков. Введите свой ник.", new Vector2(100, 100), Color.Black);
                else
                    spriteBatch.DrawString(font, "Игра окончена! Вы набрали " + player.Total.ToString() + " очков. Введите свой ник.", new Vector2(100, 100), Color.Black);
                timerForText += (float)thisGameTime.ElapsedGameTime.TotalSeconds;
                KeyboardState state = Keyboard.GetState();
                StringBuilder sb = new StringBuilder();
                if (timerForText >= 0.1f)
                {
                    foreach (var key in state.GetPressedKeys())
                    {
                        if (key != Keys.Enter)
                        {
                            if (key == Keys.Back) // удаление
                            {
                                Game1.sounds["deleteTower"].Play();
                                if (name != null && name.Length>=1)
                                    name = name.Remove(name.Length-1);
                            }
                            else
                            {
                                Game1.sounds["buildingTower"].Play();
                                string rusLetter = Russifier.GetLetter(key);
                                sb.Append(rusLetter);
                                name += sb.ToString();
                                break;
                            }
                        }
                        else
                        {
                            player.Name = name;
                            addNewRecord = false;
                            scoreAndNAme.Add(new StorageScoreAndName(player.Total, player.Name));
                            scoreAndNAme.Sort(delegate (StorageScoreAndName sn1, StorageScoreAndName sn2)
                            { return sn1.Score.CompareTo(sn2.Score); });
                            scoreAndNAme.Reverse();
                            string records = "";
                            for (int j = 0; j < amountRecords; j++)// лучшие 5 результатов // так все scoreAndNAme.Count
                            {
                                records += scoreAndNAme[j].Score.ToString() + "|" + scoreAndNAme[j].Name + "\n";
                            }
                            File.WriteAllText("Score.txt", records);

                        }
                    }
                    timerForText = 0;
                }
                spriteBatch.DrawString(font, "Нажмите клавишу enter для подтверждения ника", new Vector2(100, 130), Color.Black);
                if (name != null)
                    spriteBatch.DrawString(font, "Ваш ник: " + name, new Vector2(100, 150), Color.Black);
                else
                    spriteBatch.DrawString(font, "Ваш ник: ", new Vector2(100, 150), Color.Black);
            }
            else
            {
                int i = 50;
                if(!player.Defeat)
                    spriteBatch.DrawString(font, "Вы прошли игру! Вы набрали " + player.Total.ToString() + " очков.", new Vector2(100, 100), Color.Black);
                else
                    spriteBatch.DrawString(font, "Игра окончена! Вы набрали " + player.Total.ToString() + " очков.", new Vector2(100, 100), Color.Black);
                spriteBatch.DrawString(font, "Лучшие результаты: ", new Vector2(100, 150), Color.Black);
                int amountViewRecords;
                if (scoreAndNAme.Count > 5)
                {
                    amountViewRecords = 5;
                }
                else
                {
                    amountViewRecords = scoreAndNAme.Count;
                }

                //Вывод на экран результатов
                for (int j = 0; j < amountViewRecords; j++) // scoreAndNAme.Count
                {
                    if (scoreAndNAme[j].Name == "")
                        scoreAndNAme[j].Name = "Без имени";
                    spriteBatch.DrawString(font,scoreAndNAme[j].Name +" набрал(а) " + scoreAndNAme[j].Score.ToString() + " очков", new Vector2(100, 150 + i), Color.Black);
                    i += 50;
                }
                foreach (Button button in buttons)
                {
                    button.Draw(spriteBatch, Color.Black);
                }
            }
        }
    }
}
