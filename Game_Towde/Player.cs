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
    class Player
    {
        private int lives = 5;
        private int gold = 150;
        private int total=0;
        private int curLvl = 1;
        private Level level;
        private Vector2 selectCell;
        private Texture2D selectFrame;
        private Texture2D towerTexture;
        private Texture2D bulletTexture;
        private List<TowerTextureStorage> towerTextureStorages;
        private ButtonUpgrade upgradeButton;

        private Tower nowAddTower;

        private Tower viewTower = null;
        //private Dictionary<int, Texture2D> towersTexture = new Dictionary<int, Texture2D>();

        private int typeOfAction = -1; // действие которого нет (ничего не можем сделать)

        private List<Tower> towers = new List<Tower>();
        public int Lives { get { return lives; } set { lives = value; } }
        public int Gold { get { return gold; } set { gold = value; } }
        public int Total { get { return total; } set { total = value; } }
        public string Name { get; set; }

        public Player(Level level, List<TowerTextureStorage> towerTextureStorages, Texture2D selectFrame, ButtonUpgrade upgrade) // потом лучше сделать список текстур и выбирать в зависимости от башни текстуру
        {
            File.Delete("PathStatistic.txt");

            this.level = level;
            this.towerTextureStorages = towerTextureStorages;
            this.selectFrame = selectFrame;
            this.upgradeButton = upgrade;
            upgrade.GetPlayer = this;
        }
        public int SetTypeOfAction
        {
            set { typeOfAction = value; }
        }
        public int CurLVL
        {
            get { return curLvl; }
            set { curLvl = value; }
        }
        public List<Tower> GetTowers
        {
            get { return towers; }
        }
        public void RemoveTower(Tower tower)
        {
            towers.Remove(tower);
        }

        public bool Defeat
        {
            get
            {
                if (lives <= 0)
                    return true;
                else
                    return false;
            }
        }
        public void SelectTower()
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                int cellX = mouse.X/64;
                int cellY = mouse.Y/64;
                if (level.ClickOnMAp(cellX, cellY))
                {
                    selectCell = new Vector2(cellX * 64, cellY * 64);
                    foreach (Tower tower in towers)
                    {
                        if (tower.Position == new Vector2(cellX * 64, cellY * 64))
                        {
                            viewTower = tower;
                            upgradeButton.GetSelectedTower = viewTower;
                            upgradeButton.Map = level.map;
                            break;
                        }
                        else
                        {
                            upgradeButton.GetSelectedTower = null;
                            viewTower = null;
                        }
                    }
                }
            }
        }

        public void SetTower()
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                int cellX = mouse.X/64;
                int cellY = mouse.Y/64;
                if(typeOfAction == 1 && gold >= ArrowTower.GetPrice || typeOfAction == 2 && gold >= FrostTower.GetPrice || typeOfAction == 3 && gold >= FireTower.GetPrice) // действие установки башен 1 -обычные 2 фрост
                {
                    if (level.ClickOnMAp(cellX, cellY))
                    {
                        if (!CellWithTower(cellX,cellY) && level.map[cellX,cellY]!=2 && level.map[cellX, cellY] != 3 && level.map[cellX, cellY] != 1) // пустое место или рядом с башней (лучше противоположное условие сделать)
                        {
                            cellX = cellX * 64;
                            cellY = cellY * 64;
                            Vector2 pos = new Vector2(cellX, cellY);
                            foreach(TowerTextureStorage tTS in towerTextureStorages)
                            {
                                if (typeOfAction == tTS.Key)
                                {
                                    towerTexture = tTS.TowerTexture;
                                    bulletTexture = tTS.BulletTexture;
                                    break;
                                }
                            }
                            switch (typeOfAction)
                            {
                                case 1:
                                    nowAddTower = new ArrowTower(pos, towerTexture,  bulletTexture);
                                    break;
                                case 2:
                                    nowAddTower = new FrostTower(pos, towerTexture,  bulletTexture);
                                    break;
                                case 3:
                                    nowAddTower = new FireTower(pos, towerTexture,  bulletTexture);
                                    break;
                            }
                            towers.Add(nowAddTower);
                            gold -= nowAddTower.Value;
                            nowAddTower.AddOrDeleteWeightToMap(level.map, true);
                            Game1.sounds["buildingTower"].Play();
                        }
                        else
                        {
                            ;//Клетка занята или в ней нельзя строить
                        }
                    }
                }
            }
            if (mouse.RightButton == ButtonState.Pressed) // Продажа башни
            {
                int cellX = mouse.X / 64;
                int cellY = mouse.Y / 64;
                if (level.ClickOnMAp(cellX, cellY))
                {
                    if (CellWithTower(cellX,cellY))
                    {
                        foreach (Tower tower in towers)
                        {
                            if (new Vector2(cellX, cellY) == new Vector2 (tower.Position.X/64, tower.Position.Y/64))
                            {
                                RemoveTower(tower);
                                Game1.sounds["deleteTower"].Play();
                                tower.AddOrDeleteWeightToMap(level.map, false);
                                gold += tower.Value / 2;
                                if (tower == upgradeButton.GetSelectedTower)
                                {
                                    upgradeButton.GetSelectedTower = null;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
        private bool CellWithTower(int x, int y)
        {
            foreach (Tower tower in towers)
            {
                if (tower.Position.X == x*64 && tower.Position.Y == y*64)
                    return true;
            }
            return false;
        }
        public void Update(GameTime gameTime)
        {
            SelectTower();
            upgradeButton.Update(gameTime);
            /*foreach (Tower tower in towers)
            {
                tower.LevelUp();
            }*/
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            int indentionX = 0;
            int indentionY = 0;
            for (int i = level.Height-1; i >= 0; i--)
            {
                for (int j =0; j<level.Width;j++)
                {
                    string txt = "";
                    switch (level.map[j, i])
                    {
                        case 1: txt = "X";
                            break;
                        case 2: txt = "S";
                            break;
                        case 3: txt = "F";
                            break;
                        default: txt = level.map[j, i].ToString();
                            break;
                    }
                    if (j == (int)selectCell.X / 64 && i == (int)selectCell.Y / 64)
                    {
                        spriteBatch.DrawString(font, txt, new Vector2(level.Width * 64 + 100 + indentionX, level.Height * 64 - 250 + indentionY), Color.Red);//290
                    }
                    else
                    spriteBatch.DrawString(font, txt, new Vector2(level.Width * 64 + 100 + indentionX, level.Height * 64 - 250 + indentionY), Color.Black);
                    indentionX += 25; 
                }
                indentionX = 0;
                indentionY -= 25;
            }
            upgradeButton.Draw(spriteBatch);
            spriteBatch.Draw(selectFrame, selectCell, new Rectangle(0, 0, 64, 64), Color.White);
            string descriptionTower;
            if (viewTower != null)
            {
                foreach (Tower tower in towers)
                {
                    if (viewTower == tower)
                    {
                        spriteBatch.Draw(viewTower.GetTexture, new Vector2(level.Width * 64 + 10, level.Height * 64 - 220), Color.White);
                        descriptionTower = "Уровень башни " + viewTower.LevelTower + "\nУрон башни "+viewTower.Damage.ToString() +"\nПрочность башни "+ viewTower.Strength.ToString()+"/"+viewTower.StartStrength.ToString() + "\nРадиус башни "+(viewTower.Radius/ 64).ToString() + " клеток" + "\nЦена при продаже " + (viewTower.Value / 2).ToString() + "\n(Для продажи нажмите на башню правой кнопкой мыши)";
                        spriteBatch.DrawString(font, descriptionTower, new Vector2(level.Width*64 + 10, level.Height*64 - 150), Color.Black);
                        break;
                    }
                }
;           }
            string text;
            if (!Defeat)
            {
                text = string.Format("Жизни : {0} \nДеньги : {1}\nОчки: {2} ", lives, gold, total);
                spriteBatch.DrawString(font, text, new Vector2(level.Width*64 + 10, level.Height), Color.Black);
            }
            else
            {
                text = string.Format("ВЫ ПРОИГРАЛИ\nВы набрали {0} очков", total);

                spriteBatch.DrawString(font, text, new Vector2(level.Width * 64 + 10, level.Height), Color.White);
            }
            foreach (Tower tower in towers)
            {
                tower.Draw(spriteBatch);
            }
        }
    }
}
