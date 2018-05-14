/*
‘******************************************************
‘***  Game1
‘***  Blake Young, Zach Languell, Holley Melchor, Kevin Anderson
‘******************************************************
‘*** This class was made to manage the game, the state changes, and the interactivity amongst its entities
‘******************************************************
‘*** 11/3/2015
‘******************************************************
‘*** 11/3/2015
 *      Created game1
 *   11/5/2015
 *      Scrolling background added
 *   11/8/2015
 *      Improved background scrolling
 *   11/10/2015
 *      Added player, Tw, and music
 *      Enum states
 *   11/11/2015
 *      Enum for draw
 *      Menu system
 *   11/12/2015
 *      Buttons
 *      Spawning scoring objects
 *   11/17/2015
 *      Beginning Collision
 *      More menu artifacts
 *   11/19/2015
 *      Sound Effects
 *      Enemy objects spawning
 *      More detecting work
 *   11/20/2015
 *      Tutorial page added
 *      Finishing up collision detection
 *   11/24/2015
 *      Improved buttons added
 *      Removed old buttons
 *      Bug removal
 *   11/25/2015
 *      Bugs
 *   11/30/2015
 *      Bug removal
‘*******************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Breaking_Bones
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //GameState Enumeration
        public enum GameState 
        {
            mainMenu,
            Options,
            gamePlay,
            leaderBoard,
            About,
            tutorial
        };        

        //Sounds and Music
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BackgroundMusicPlayer bgmPlayer;
        SoundEffect playerDivesSound;
        SoundEffect pickUpSound;
        SoundEffect gainPointSound;
        SoundEffect turtlePickUp;
        SoundEffect turtleDeath;
        
        //Variable Declaration
        GameState gameState;
        bool wasdKeysUsed;
        ScrollingBkg background;
        Player player;
        TW tw;
        GameObject logo;
        GameObject aboutPage, leaderboardPage, optionsPage;
        ScoringObjectSpawner scoringObjectSpawner;
        EnemySpawner enemySpawner;
        RockSpawner rockSpawner;
        Random rnd = new Random();
        int playerScore = 0;
        int remainingTime = 0;
        SpriteFont scoreFont;
        Leaderboard leaderBoard;
        double collisionTimer;
        Button btnPlay, btnLeaderboard, btnOptions, btnAbout;

        Button btnControlScheme;


        //----------Tutorial Page Variables -----------
        Texture2D tutorialPage;
        Rectangle tutorialPageRectangle;
        int time = 0;
        bool isSpacePressed = false;
        //---------------------------------------------

        /*
            ‘******************************************************
            ‘***  Game1
            ‘***  Zach Languell, Kevin Anderson, Holley Melchor, Blake Young
            ‘******************************************************
            ‘*** Required constructor for Game1
            ‘*** Method Inputs:
            ‘***    NA
            ‘*** Return value:
            ‘***    NA
            ‘******************************************************
            ‘*** 11/03/2015
            ‘******************************************************
         */

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
        }

        /*
           ‘******************************************************
           ‘***  Initialize
           ‘***  Zach Languell, Kevin Anderson, Holley Melchor, Blake Young
           ‘******************************************************
           ‘*** Required function for Game1
           ‘*** Method Inputs:
           ‘***    NA
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/03/2015
           ‘******************************************************
        */

        protected override void Initialize()
        {
            gameState = GameState.mainMenu;
            wasdKeysUsed = false;
            base.Initialize();
        }

        /*
           ‘******************************************************
           ‘***  LoadContent
           ‘***  Zach Languell, Kevin Anderson, Holley Melchor, Blake Young
           ‘******************************************************
           ‘*** Required constructor for Game1, and loads content of intialized variables
           ‘*** Method Inputs:
           ‘***    NA
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/03/2015
           ‘******************************************************
        */

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            System.IO.Stream waveFileStream = TitleContainer.OpenStream(@"Content\lamBack2.wav");
            bgmPlayer = new BackgroundMusicPlayer(waveFileStream);
            switch(gameState)
            { 

                case GameState.mainMenu:
                    background = new ScrollingBkg(Content.Load<Texture2D>("background"), new Rectangle(0, 0, 2047, 600));
                    logo = new GameObject(Content.Load<Texture2D>("Menus\\breakingBonesLogo"), new Rectangle(180, 110, 494, 217));
                    btnPlay = new Button(Content.Load<Texture2D>("Menus\\playButton"), new Vector2(260, 380), 188, 63, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                    btnLeaderboard = new Button(Content.Load<Texture2D>("Menus\\leaderboardButton"), new Vector2(410, 450), 472, 58, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                    btnOptions = new Button(Content.Load<Texture2D>("Menus\\optionsButton"), new Vector2(510, 380), 301, 59, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                    btnAbout = new Button(Content.Load<Texture2D>("Menus\\aboutButton"), new Vector2(110, 565), 234, 59, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);

                    break;
                case GameState.gamePlay:                  
                   background = new ScrollingBkg(Content.Load<Texture2D>("background"), new Rectangle(0, 0, 2047, 600));
                   tw = new TW(Content.Load<Texture2D>("tw"), new Vector2(450, 415), 128, 128);
                   player = new Player(Content.Load<Texture2D>("lammergeier"), new Vector2(50, 50), 64, 64, wasdKeysUsed);                   
                   scoringObjectSpawner = new ScoringObjectSpawner(Content.Load<Texture2D>("turtle"), Content.Load<Texture2D>("bones"));
                   enemySpawner = new EnemySpawner(Content.Load<Texture2D>("eagle"), Content.Load<Texture2D>("tree"));
                   rockSpawner = new RockSpawner(Content.Load<Texture2D>("rocks"));
                   scoreFont = Content.Load<SpriteFont>("SpriteFont1");
                   playerDivesSound = Content.Load<SoundEffect>("SFX\\PlayerDiving");
                   pickUpSound = Content.Load<SoundEffect>("SFX\\PlayerPickUpItem2");
                   turtlePickUp = Content.Load<SoundEffect>("SFX\\TurtlePickUp");
                   gainPointSound = Content.Load<SoundEffect>("SFX\\GainPoints");
                   turtleDeath = Content.Load<SoundEffect>("SFX\\DyingTurtle");
                   playerScore = 0;
                   break;
                case GameState.leaderBoard:
                   leaderBoard = new Leaderboard(playerScore, Content.Load<SpriteFont>("SpriteFont1"));
                   playerScore = 0;
                   background = new ScrollingBkg(Content.Load<Texture2D>("background"), new Rectangle(0, 0, 2047, 600));
                   leaderboardPage = new GameObject(Content.Load<Texture2D>("Menus\\leaderboardPage"), new Rectangle(60, 53, 680, 500));
                   btnPlay = new Button(Content.Load<Texture2D>("Menus\\playButton"), new Vector2(250, 30), 188, 63, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                   btnOptions = new Button(Content.Load<Texture2D>("Menus\\optionsButton"), new Vector2(500, 30), 301, 59, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                   btnAbout = new Button(Content.Load<Texture2D>("Menus\\aboutButton"), new Vector2(110, 565), 234, 59, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                   
                  
                   break;
                case GameState.Options:
                   background = new ScrollingBkg(Content.Load<Texture2D>("background"), new Rectangle(0, 0, 2047, 600));
                   optionsPage = new GameObject(Content.Load<Texture2D>("Menus\\optionsPage"), new Rectangle(60, 53, 680, 500));
                   btnPlay = new Button(Content.Load<Texture2D>("Menus\\playButton"), new Vector2(250, 30), 188, 63, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                   btnLeaderboard = new Button(Content.Load<Texture2D>("Menus\\leaderboardButton"), new Vector2(550, 565), 472, 58, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                   btnAbout = new Button(Content.Load<Texture2D>("Menus\\aboutButton"), new Vector2(110, 565), 234, 59, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                   btnControlScheme = new Button(Content.Load<Texture2D>("Menus\\controlButton"), new Vector2(520,230),309,77,Content.Load<SoundEffect>("SFX\\SelectBlip2"), 4, wasdKeysUsed);
                   break;
                case GameState.About:
                   background = new ScrollingBkg(Content.Load<Texture2D>("background"), new Rectangle(0, 0, 2047, 600));
                   aboutPage = new GameObject(Content.Load<Texture2D>("Menus\\aboutPage"), new Rectangle(60, 53, 680, 500));
                   btnPlay = new Button(Content.Load<Texture2D>("Menus\\playButton"), new Vector2(250, 30), 188, 63, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                   btnLeaderboard = new Button(Content.Load<Texture2D>("Menus\\leaderboardButton"), new Vector2(550, 565), 472, 58, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                   btnOptions = new Button(Content.Load<Texture2D>("Menus\\optionsButton"), new Vector2(500, 30), 301, 59, Content.Load<SoundEffect>("SFX\\SelectBlip2"), 2);
                   break;
                case GameState.tutorial:
                   background = new ScrollingBkg(Content.Load<Texture2D>("background"), new Rectangle(0, 0, 2047, 600));
                   tutorialPage = Content.Load<Texture2D>("Menus\\Tutorial");
                   scoreFont = Content.Load<SpriteFont>("SpriteFont1");
                   tutorialPageRectangle = new Rectangle(0, 0, 800, 600);
                   break;
            }
        }
        /*
           ‘******************************************************
           ‘***  UnloadContent
           ‘***  Zach Languell, Kevin Anderson, Holley Melchor, Blake Young
           ‘******************************************************
           ‘*** Required Function for Game1, this unloads content from the stage
           ‘*** Method Inputs:
           ‘***    NA
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/03/2015
           ‘******************************************************
        */
        protected override void UnloadContent()
        {
            spriteBatch.Dispose();
        }

        /*
           ‘******************************************************
           ‘***  Update
           ‘***  Zach Languell, Kevin Anderson, Holley Melchor, Blake Young
           ‘******************************************************
           ‘*** Required function for Game1, this updates every 60 times per second, and manages the enumeration and what updates.
           ‘*** Method Inputs:
           ‘***    gameTime
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/03/2015
           ‘******************************************************
        */
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            
            switch (gameState)
            {
                case GameState.mainMenu:
                    MouseState mouseState = Mouse.GetState();


                    btnPlay.Update();
                    btnLeaderboard.Update();
                    btnOptions.Update();
                    btnAbout.Update();
                
                    if (btnPlay.isClicked())
                        buttonStateChange(GameState.tutorial);   
                    else if (btnLeaderboard.isClicked())
                        buttonStateChange(GameState.leaderBoard);
                    else if(btnOptions.isClicked())
                        buttonStateChange(GameState.Options);
                    else if (btnAbout.isClicked())
                        buttonStateChange(GameState.About);

                break;
                case GameState.gamePlay:
                    bgmPlayer.play();
                    background.Update(gameTime);
                    player.Update(gameTime, playerDivesSound, pickUpSound);
                    scoringObjectSpawner.Update(gameTime, rnd, player.Position);
                    enemySpawner.Update(gameTime, rnd);
                    rockSpawner.Update(gameTime, rnd);
                    tw.Update(gameTime, player.IsDiving(), player.Position);
                    detectCollision(gameTime);
                    break;
                case GameState.Options:
                    btnPlay.Update();
                    btnLeaderboard.Update();
                    btnAbout.Update();
                    btnControlScheme.Update();

                    if (btnPlay.isClicked())
                        buttonStateChange(GameState.tutorial);
                    else if (btnLeaderboard.isClicked())
                        buttonStateChange(GameState.leaderBoard);
                    else if (btnAbout.isClicked())
                        buttonStateChange(GameState.About);
                    else if (btnControlScheme.isClicked() && btnControlScheme.clicked)
                        changeControlScheme();
                        
                    break;
                case GameState.leaderBoard:
                    btnPlay.Update();
                    btnOptions.Update();
                    btnAbout.Update();               
                    if (btnPlay.isClicked())
                        buttonStateChange(GameState.tutorial);   
                    else if(btnOptions.isClicked())
                        buttonStateChange(GameState.Options);
                    else if (btnAbout.isClicked())
                        buttonStateChange(GameState.About);
                        break;
                case GameState.About:
                    btnPlay.Update();
                    btnLeaderboard.Update();
                    btnOptions.Update();                    
                
                    if (btnPlay.isClicked())
                        buttonStateChange(GameState.tutorial);   
                    else if (btnLeaderboard.isClicked())
                        buttonStateChange(GameState.leaderBoard);
                    else if(btnOptions.isClicked())
                        buttonStateChange(GameState.Options);
                                     
                    break;
                case GameState.tutorial:
                    
                    time += gameTime.ElapsedGameTime.Milliseconds;
                    remainingTime = 15 - (time / 1000);
                    if(time > 15000)
                    {
                        gameState = GameState.gamePlay;
                        UnloadContent();
                        LoadContent();
                        time = 0;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && !isSpacePressed)
                    {
                        time = 15001;
                        isSpacePressed = true;
                    }
                    else
                        isSpacePressed = false;                    
                    break;
            }
                
            base.Update(gameTime);
        }
        /*
           ‘******************************************************
           ‘***  Draw
           ‘***  Zach Languell, Kevin Anderson, Holley Melchor, Blake Young
           ‘******************************************************
           ‘*** Required function for Game1, draws objects onto the stage
           ‘*** Method Inputs:
           ‘***    gameTime
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/03/2015
           ‘******************************************************
        */
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            switch(gameState)
            {
                case GameState.mainMenu:
                    spriteBatch.Begin();
                    background.Draw(spriteBatch);
                    btnPlay.Draw(spriteBatch);
                    btnAbout.Draw(spriteBatch);
                    btnOptions.Draw(spriteBatch);
                    btnLeaderboard.Draw(spriteBatch);
                    logo.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.gamePlay:
                    spriteBatch.Begin();
                    background.Draw(spriteBatch);
                    foreach (Rock r in rockSpawner.rockObjects)
                        r.Draw(spriteBatch);
                    foreach (EnemyObject e in enemySpawner.enemyObjects)
                        e.Draw(spriteBatch);
                    foreach (ScoringObject s in scoringObjectSpawner.scoringObjects)
                        s.Draw(spriteBatch);
                    tw.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    spriteBatch.DrawString(scoreFont, "Score: " + playerScore, new Vector2(10, 0), Color.Orange);
                    spriteBatch.End();                    
                    break;
                case GameState.leaderBoard:
                    spriteBatch.Begin();
                    background.Draw(spriteBatch);
                    leaderboardPage.Draw(spriteBatch);
                    btnPlay.Draw(spriteBatch);
                    btnAbout.Draw(spriteBatch);
                    btnOptions.Draw(spriteBatch);
                    leaderBoard.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.Options:
                    spriteBatch.Begin();
                    background.Draw(spriteBatch);
                    optionsPage.Draw(spriteBatch);
                    btnPlay.Draw(spriteBatch);
                    btnAbout.Draw(spriteBatch);
                    btnLeaderboard.Draw(spriteBatch);
                    btnControlScheme.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.About:
                    spriteBatch.Begin();
                    background.Draw(spriteBatch);
                    aboutPage.Draw(spriteBatch);
                    btnPlay.Draw(spriteBatch);
                    btnOptions.Draw(spriteBatch);
                    btnLeaderboard.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.tutorial:
                    spriteBatch.Begin();
                    background.Draw(spriteBatch);
                    spriteBatch.Draw(tutorialPage, tutorialPageRectangle, Color.White);
                    spriteBatch.DrawString(scoreFont, "Time: " + remainingTime, new Vector2(700, 375), Color.OrangeRed);
                    spriteBatch.End();
                    break;
            }            
            base.Draw(gameTime);
        }

        /*
           ‘******************************************************
           ‘***  detectCollision
           ‘***  Zach Languell, Kevin Anderson, Holley Melchor, Blake Young
           ‘******************************************************
           ‘*** The purpose of this function is to detect collision amongst interactable objects as dictated from requirements
           ‘*** Method Inputs:
           ‘***    gameTime
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/20/2015
           ‘******************************************************
        */
        protected void detectCollision(GameTime gameTime)
        {
            collisionTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (player.carriedObject == null && player.IsDiving())
            {
                foreach (ScoringObject s in scoringObjectSpawner.scoringObjects)
                    if (player.collisionRectangle.Intersects(s.collisionRectangle))
                    {
                        //if(pixelPerfect(player.texture, s.texture, player.collisionRectangle, s.collisionRectangle, player.rectangle, s.rectangle))
                        //{
                            player.IsDiveComplete = true;
                            player.carriedObject = s;
                            s.setStateCarried();
                            s.lastPosition.Y = s.position.Y;
                            if (s.isbone)
                                pickUpSound.Play();
                            else
                                turtlePickUp.Play();
                            break;
                       // }

                        
                    }
            } 
            else if(!player.IsDiving())
            {
                foreach (EnemyObject e in enemySpawner.enemyObjects)
                    if(player.collisionRectangle.Intersects(e.collisionRectangle))
                    {
                        //if (pixelPerfect(player.texture, e.texture, player.collisionRectangle, e.collisionRectangle, player.rectangle, e.rectangle))
                       // {
                            if (player.carriedObject != null)
                                player.takeDamage();

                            else if (collisionTimer > 500)
                            {
                                gameState = GameState.leaderBoard;
                                bgmPlayer.stop();
                                UnloadContent();
                                LoadContent();
                            }
                            collisionTimer = 0;
                            break;
                        //}
                    }
            }

            foreach (ScoringObject s in scoringObjectSpawner.scoringObjects)
                if (s.collisionRectangle.Intersects(tw.collisionRectangle) && s.scoreState == Breaking_Bones.ScoringObject.ScoreState.Dropped)
                {
                    if (s.scoreState == Breaking_Bones.ScoringObject.ScoreState.Dropped)
                    {
                        s.scoreState = Breaking_Bones.ScoringObject.ScoreState.Dead;
                        if(!s.forceDrop)
                            gainPointSound.Play();
                        if (s.isbone)
                            playerScore += s.score*3;
                        else
                            playerScore += s.score*10;
                    }                    
                    break;
                }

            foreach (ScoringObject s in scoringObjectSpawner.scoringObjects)
                foreach (Rock r in rockSpawner.rockObjects)
                    if (s.collisionRectangle.Intersects(r.collisionRectangle) && s.scoreState == Breaking_Bones.ScoringObject.ScoreState.Dropped)
                    {
                        s.scoreState = Breaking_Bones.ScoringObject.ScoreState.Dead;
                        if (s.isbone)
                        {
                            if(!s.forceDrop)
                             gainPointSound.Play();
                            playerScore += s.score*3;
                        }
                        else
                        {
                            turtleDeath.Play();
                            playerScore += s.score;
                        }
                        break;
                    }                    
        }

        /*
           ‘******************************************************
           ‘***  buttonStateChange
           ‘***  Zach Languell, Kevin Anderson, Holley Melchor, Blake Young
           ‘******************************************************
           ‘*** This function changed the gamestate depending on the button clicked
           ‘*** Method Inputs:
           ‘***    gs
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/24/2015
           ‘******************************************************
        */

        void buttonStateChange(GameState gs)
        {
            Console.WriteLine(gameState);
            gameState = gs;
            UnloadContent();
            LoadContent();
         
        }

        /*
           ‘******************************************************
           ‘***  changeControlScheme
           ‘***  Zach Languell, Kevin Anderson, Holley Melchor, Blake Young
           ‘******************************************************
           ‘*** Changes the controls from WASD or Arrows
           ‘*** Method Inputs:
           ‘***    NA
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/22/2015
           ‘******************************************************
        */

        public void changeControlScheme()
        {
            Console.WriteLine(wasdKeysUsed);
            if (wasdKeysUsed == false)
                wasdKeysUsed = true;
            else
                wasdKeysUsed = false;
        }

        /*
           ‘******************************************************
           ‘***  pixelPerfect
           ‘***  Zach Languell, Blake Young
           ‘******************************************************
           ‘*** Pixel perfect collison amongst interactible objects (incomplete)
           ‘*** Method Inputs:
           ‘***    object1 - texture of one gameobject
         *         object2 - texture of the second gameobject
         *         rectangle1 - collisionrectangle of the first object
         *         rectangle2 - collisionrectangle fo the second object
         *         rect1 - rectangle from the first object
         *         rect2 - rectangle from the second object.
           ‘*** Return value:
           ‘***    True/False
           ‘******************************************************
           ‘*** 11/30/2015
           ‘******************************************************
        */

        public bool pixelPerfect(Texture2D object1, Texture2D object2, Rectangle rectangle1, Rectangle rectangle2, Rectangle rect1, Rectangle rect2)
        {
            
            Color[] object1Color = new Color[object1.Width * object1.Height];
           
            Color[] object2Color = new Color[object2.Width * object2.Height];
            object1.GetData<Color>(0, rect1, object1Color, 0, (rectangle1.Width * rectangle1.Height));

            object2.GetData<Color>(0, rect2, object2Color, 0, (rectangle2.Width * rectangle2.Height));


            int top, bottom, left, right;

            top = Math.Max(rectangle1.Top, rectangle2.Top);
            left = Math.Max(rectangle1.Left, rectangle2.Left);
            bottom = Math.Min(rectangle1.Bottom, rectangle2.Bottom);
            right = Math.Min(rectangle1.Right, rectangle2.Right);

            for(int i = top; i < bottom; i++)
            {
                for(int j = left; j < right; j++)
                {
                    Color A = object1Color[(i - rectangle1.Top) * (rectangle1.Width) + (j - rectangle1.Left)];
                    Color B = object2Color[(i - rectangle2.Top) * (rectangle2.Width) + (j - rectangle2.Left)];

                    if (A.A != 0 && B.A != 0)
                        return true;
                }
            }
            return false;

        }
    }
}
