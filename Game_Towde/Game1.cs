using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Game_Towde
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Dictionary <string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        public static Dictionary<int, Texture2D> enemySkins = new Dictionary<int, Texture2D>();
        public static SoundEffect soundDeathRobot;
        public static SpriteFont font;
        public static int[] priority;


        float timerForPause = 0.1f;
        bool pause = false;


        Level level = new Level();
        Menu menu;
        GameOverMenu gameOverMenu;
        Player player;
        ButtonTower button;
        ButtonTower buttonFire;
        ButtonTower buttonFrost;
        ButtonUpgrade upGrade;
        List<TowerTextureStorage> towerTextureStorages = new List<TowerTextureStorage>();
        List<ButtonTower> buttons = new List<ButtonTower>();
        List<Button> buttonsMenu = new List<Button>();
        List<Button> buttonsMenuGameover = new List<Button>();
        List<DirectionButton> directionButtons = new List<DirectionButton>();
        DirectionButtonManager directionButtonManager;
        ButtonManager buttonManager;
        WaveManager waveManager;

        Texture2D healthBarTexture;
        Texture2D selectFrame;
        Texture2D pathTexture;
        Texture2D enemy1Texture;
        Texture2D upgradeButton;
        Song fonMusic;


        Texture2D grass;
        Texture2D hill;
        Texture2D start;
        Texture2D finish;

        string fileScore;
        bool ViewRecords = true;

        int widthScreen, heightScreen;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            widthScreen = level.Width * 64 + 510;
            heightScreen = level.Height * 64;

            graphics.PreferredBackBufferWidth = widthScreen;
            graphics.PreferredBackBufferHeight = heightScreen;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        private void WriteScore(Player pl)
        {
            File.AppendAllText(fileScore, pl.Total.ToString() + "|" + pl.Name + "\n");
        }

        protected override void LoadContent()
        {
            fileScore = "Score.txt";
            fonMusic = Content.Load<Song>("mainMusic");
            MediaPlayer.Play(fonMusic);
            MediaPlayer.Volume = 0.2f;//0.1
            SoundEffect.MasterVolume = 0.2f;
            MediaPlayer.IsRepeating = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("File");
            pathTexture = Content.Load<Texture2D>("path");
            grass = Content.Load<Texture2D>("grass");//0
            hill = Content.Load<Texture2D>("road7");
            start = Content.Load<Texture2D>("start");
            finish = Content.Load<Texture2D>("finish");
            enemy1Texture = Content.Load<Texture2D>("robot464");

            enemySkins.Add(0, enemy1Texture);
            enemySkins.Add(1, Content.Load<Texture2D>("robot64"));
            enemySkins.Add(2, Content.Load<Texture2D>("robot264"));
            enemySkins.Add(3, Content.Load<Texture2D>("robot364"));

            SoundEffect shot = Content.Load<SoundEffect>("arrowShot");
            sounds.Add("shot", shot);
            sounds.Add("frostShot", Content.Load<SoundEffect>("frostShot"));
            sounds.Add("fireShot", Content.Load<SoundEffect>("fireShot"));
            sounds.Add("button_pressed", Content.Load<SoundEffect>("button_pressed"));
            sounds.Add("buildingTower", Content.Load<SoundEffect>("buildingTower"));
            sounds.Add("deleteTower", Content.Load<SoundEffect>("deleteTower"));
            sounds.Add("upgradeTower", Content.Load<SoundEffect>("lvlup"));
            sounds.Add("enemyPassed", Content.Load<SoundEffect>("enemyPassed"));


            soundDeathRobot = Content.Load<SoundEffect>("deathRobot");

            Texture2D textureButTower = Content.Load<Texture2D>("towerButton");
            Texture2D textureButFrost = Content.Load<Texture2D>("buttonFrostTower");
            Texture2D textureButFire = Content.Load<Texture2D>("buttonFireTower");

            Texture2D bulletTexture = Content.Load<Texture2D>("bulletStandart");
            Texture2D bulletFireTexture = Content.Load<Texture2D>("bullet");
            Texture2D bulletFrostTexture = Content.Load<Texture2D>("snowFl");

            Texture2D towerTexture = Content.Load<Texture2D>("tower");
            Texture2D frostTowerTexture = Content.Load<Texture2D>("frostTower");
            Texture2D fireTowerTexture = Content.Load<Texture2D>("fireTower");

            healthBarTexture = Content.Load<Texture2D>("healthBar");
            selectFrame = Content.Load<Texture2D>("selectFrame");
            upgradeButton = Content.Load<Texture2D>("upgrade2");

            DirectionButton directionButtonLeft = new DirectionButton(new Vector2(level.Width * 64 + 150, level.Height * 64 - 670), pathTexture, font,"", 0);
            DirectionButton directionButtonRight = new DirectionButton(new Vector2(level.Width * 64 + 214, level.Height * 64 - 670), pathTexture, font,"", 1);
            DirectionButton directionButtonUp = new DirectionButton(new Vector2(level.Width * 64 + 182, level.Height * 64 - 734), pathTexture, font,"", 2);
            DirectionButton directionButtonDown = new DirectionButton(new Vector2(level.Width * 64 + 182, level.Height * 64 - 606), pathTexture, font,"", 3);

            ///directionButtons.Add(directionButtonRight); // 23.05
            directionButtons.Add(directionButtonLeft);
            directionButtons.Add(directionButtonRight);
            //directionButtons.Add(directionButtonUp);
            directionButtons.Add(directionButtonDown);
            directionButtons.Add(directionButtonUp);

            directionButtonManager = new DirectionButtonManager(directionButtons);

            towerTextureStorages.Add(new TowerTextureStorage(1, towerTexture, bulletTexture));
            towerTextureStorages.Add(new TowerTextureStorage(2, frostTowerTexture, bulletFrostTexture));
            towerTextureStorages.Add(new TowerTextureStorage(3, fireTowerTexture, bulletFireTexture)); 

            level.AddTexture(grass);
            level.AddTexture(hill);
            level.AddTexture(start);
            level.AddTexture(finish);
            upGrade = new ButtonUpgrade(new Vector2(level.Width * 64 + 10, level.Height * 64 - 60), upgradeButton, font,"");
            player = new Player(level,towerTextureStorages,selectFrame,upGrade);

            waveManager = new WaveManager(player, level, 2, enemy1Texture, healthBarTexture,pathTexture);

            button = new ButtonTower(new Vector2(level.Width * 64 + 10, level.Height * 64 - 630 ), textureButTower, font,"", ArrowTower.GetPrice, 1); // последний параметр это тип действия 1 - ставит башни обычные // 2 - морозные
            buttonFrost = new ButtonTower(new Vector2(level.Width * 64 + 10, level.Height * 64 - 560), textureButFrost, font,"",FrostTower.GetPrice, 2);
            buttonFire = new ButtonTower(new Vector2(level.Width * 64 + 10, level.Height * 64 - 490), textureButFire, font,"", FireTower.GetPrice, 3);

            Texture2D buttonPlaytexture = Content.Load<Texture2D>("buttonPlay1");
            Texture2D buttonExitTexture = Content.Load<Texture2D>("buttonExit1");
            Button startGameButton = new Button(new Vector2(widthScreen / 2 - buttonPlaytexture.Width / 2, heightScreen - 640), buttonPlaytexture, font, "");//upgradeButton
            Button exitButton = new Button(new Vector2(widthScreen / 2 - buttonExitTexture.Width / 2, heightScreen - 390), buttonExitTexture, font, "");

            Button playAgain = new Button(new Vector2(widthScreen / 2 - upgradeButton.Width / 2 + 10, heightScreen - 640), upgradeButton, font, "Играть еще раз");
            Button inMainMenu = new Button(new Vector2(widthScreen / 2 - upgradeButton.Width / 2 + 10, heightScreen - 570), upgradeButton, font, "В главное меню");

            buttonsMenuGameover.Add(playAgain);
            buttonsMenuGameover.Add(inMainMenu);

            buttonsMenu.Add(startGameButton);
            //buttonsMenu.Add(settingsButton);
            buttonsMenu.Add(exitButton);


            buttons.Add(buttonFrost);
            buttons.Add(button);
            buttons.Add(buttonFire);
            buttonManager = new ButtonManager(buttons,player);

            menu = new Menu(buttonsMenu);
            //gameOverMenu = new GameOverMenu(player, font, buttonsMenuGameover);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            timerForPause += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (MediaPlayer.Volume > 0)
                {
                    MediaPlayer.Volume -= 0.1f;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (MediaPlayer.Volume < 1)
                {
                    MediaPlayer.Volume += 0.1f;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                SoundEffect.MasterVolume = 0.5f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                SoundEffect.MasterVolume = 0f;
            }
            //player.Update(gameTime);
            if (menu.GetState == StateMenu.None)
            {
                menu.Update(gameTime);
            }
            else
            {
                if (menu.GetState == StateMenu.Exit)
                {
                    Exit();
                }
                if (menu.GetState == StateMenu.Game)
                {
                    if (!player.Defeat && waveManager.Win == false)
                    {

                        if (!Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            if (timerForPause >= 0.1f && Keyboard.GetState().IsKeyDown(Keys.P))
                            {
                                pause = !pause;
                            }
                            if (!pause)
                            {
                                player.Update(gameTime);
                                directionButtonManager.Update(gameTime);
                                priority = directionButtonManager.GetPriority();
                                waveManager.SetPriority = directionButtonManager.GetPriority();
                                waveManager.Update(gameTime);
                                buttonManager.Update(gameTime);
                            }
                        }
                        else
                        {
                            menu.GetState = StateMenu.None;
                            foreach (ButtonTower bt in buttons)
                            {
                                bt.Pressed = false;
                            }
                            level = new Level();
                            level.AddTexture(grass);
                            level.AddTexture(hill);
                            level.AddTexture(start);
                            level.AddTexture(finish);
                            menu = new Menu(buttonsMenu);
                            ViewRecords = true;
                            upGrade = new ButtonUpgrade(new Vector2(level.Width * 64 + 10, level.Height * 64 - 60), upgradeButton, font, "");
                            player = new Player(level, towerTextureStorages, selectFrame, upGrade);
                            waveManager = new WaveManager(player, level, 2, enemy1Texture, healthBarTexture, pathTexture);
                            buttonManager = new ButtonManager(buttons, player);
                        }
                    }
                    else
                    {
                        if (ViewRecords)
                        {
                            gameOverMenu = new GameOverMenu(player, font, buttonsMenuGameover);
                            ViewRecords = false;
                        }
                        gameOverMenu.Update(gameTime);
                        if (gameOverMenu.State == StateGameOvermenu.InMainMenu || gameOverMenu.State == StateGameOvermenu.PlayAgain)
                        {
                            /* if (gameOverMenu.CheckRecord())
                             {
                                 WriteScore(player);
                             }*/
                            foreach (ButtonTower bt in buttons)
                            {
                                bt.Pressed = false;
                            }
                            level = new Level();
                            level.AddTexture(grass);
                            level.AddTexture(hill);
                            level.AddTexture(start);
                            level.AddTexture(finish);
                            menu = new Menu(buttonsMenu);
                            if (gameOverMenu.State == StateGameOvermenu.PlayAgain)
                            {
                                menu.GetState = StateMenu.Game;
                            }
                            ViewRecords = true;
                            upGrade = new ButtonUpgrade(new Vector2(level.Width * 64 + 10, level.Height * 64 - 60), upgradeButton, font, "");
                            player = new Player(level, towerTextureStorages, selectFrame, upGrade);
                            waveManager = new WaveManager(player, level, 2, enemy1Texture, healthBarTexture, pathTexture);
                            buttonManager = new ButtonManager(buttons, player);
                            //gameOverMenu = new GameOverMenu(player, font, buttonsMenuGameover);
                        }
                    }
                    /*if (player.Lives <= 0)
                    {
                        Exit();
                    }*/
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (menu.GetState==StateMenu.None)
            {
                menu.Draw(spriteBatch);
            }
            else
            {
                if (!player.Defeat && !waveManager.Win)
                {
                    if(pause)
                        spriteBatch.DrawString(font, "ПАУЗА", new Vector2(level.Width * 64 + 100, level.Height), Color.Red);
                    level.Draw(spriteBatch);
                    player.Draw(spriteBatch, font);
                    waveManager.Draw(spriteBatch, font);
                    buttonManager.Draw(spriteBatch);
                    directionButtonManager.Draw(spriteBatch);
                    spriteBatch.DrawString(font, "Путевые координаты", new Vector2(level.Width * 64 + 150, level.Height), Color.Black);
                }
                else
                {
                    if(gameOverMenu!=null)
                    gameOverMenu.Draw(spriteBatch);
                }

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
