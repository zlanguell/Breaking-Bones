/*
‘******************************************************
‘***  Player
‘***  Kevin Anderson, Blake Young
‘******************************************************
‘*** This class was made so that the user has a player controlled entity for the game
‘******************************************************
‘*** 11/8/2015
‘******************************************************
‘*** 11/8/2015
 *      Created Class
 *      Added constructor
 *      Added update function
 *   11/10/2015
 *      Added Draw Function
 *      Added Keyboard controls
 *      Animation
 *      Diving
 *   11/11/2015
 *      Getters and setters
 *   11/17/2015
 *      Animate left and right
 *      Player boundries
 *   11/19/2015
 *      Added enum states
 *   11/24/2015
 *      Added player movement function
 *      Added take damage function
‘*******************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Breaking_Bones
{
    class Player : GameObject
    {        
        Boolean isDiveComplete=true;
        KeyboardState newKeyboardState;
        Vector2 lastPosition;
        public ScoringObject carriedObject = null;
        float timer, interval = 30;
        double stateChange = 0;
        public PlayerState playerState;
        protected bool wasdKeysUsed;
        public enum PlayerState
        {
            freeFlight,
            diving,
            hasObject
        }

        /*
            ‘******************************************************
            ‘***  Player
            ‘***  Blake Young, Kevin Anderson
            ‘******************************************************
            ‘*** Required constructor for Player
            ‘*** Method Inputs:
            ‘***    newText - The texture pack associated to TW
         *          newPosition - the initial position TW will be placed on the stage
         *          newFrameHeight - the height of the frame of each sprite on a sprite sheet. This allows animation
         *          newFrameWidth - the width of the frame of each sprite on a sprite sheet. This allows animation
         *          wasdKeysUsed - true/false
            ‘*** Return value:
            ‘*** NA
            ‘******************************************************
            ‘*** 11/8/2015
            ‘******************************************************
         */

        public Player(Texture2D newText, Vector2 newPosition, int newFrameHeight, int newFrameWidth, bool wasdKeysUsed)
            : base(newText, newPosition, newFrameHeight, newFrameWidth)
        {
            texture = newText;
            position = newPosition;
            frameHeight = newFrameHeight;
            frameWidth = newFrameWidth;
            playerState = PlayerState.freeFlight;
            this.wasdKeysUsed = wasdKeysUsed;
        }

        /*
            ‘******************************************************
            ‘***  Update
            ‘***  Kevin Anderson
            ‘******************************************************
            ‘*** Updates player movements, animation and states
            ‘*** Method Inputs:
            ‘***    gameTime - game time elapse
         *          divingSound - SoundEffect - playerDiving
         *          pickUpSound - sound effect - pickUpSound2
            ‘*** Return value:
            ‘*** NA
            ‘******************************************************
            ‘*** 11/8/2015
            ‘******************************************************
         */

        public void Update(GameTime gameTime, SoundEffect divingSound, SoundEffect pickUpSound)
        {
            newKeyboardState = Keyboard.GetState();
            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            collisionRectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            stateChange += gameTime.ElapsedGameTime.TotalMilliseconds;
            switch (playerState)
            {
                case PlayerState.freeFlight:

                    playerMovement(gameTime);                   

                    if (newKeyboardState.IsKeyDown(Keys.Space)&&stateChange>500)
                    {
                        stateChange = 0;
                        playerState = PlayerState.diving;
                        isDiveComplete = false;
                        lastPosition = new Vector2(position.X, position.Y);                        
                        divingSound.Play();
                    }
                    break;

                case PlayerState.diving:                    
                        
                     if (this.position.Y < 600 && isDiveComplete == false)
                         this.position.Y += 6;
                     if (this.position.Y >= 599)                            
                         isDiveComplete = true;                         

                    if (this.position.Y > lastPosition.Y && isDiveComplete == true)
                            this.position.Y -= 6;
                    if (this.position.Y <= lastPosition.Y && isDiveComplete == true && this.carriedObject == null)
                        playerState = PlayerState.freeFlight;
                    else if (this.position.Y <= lastPosition.Y && isDiveComplete == true && this.carriedObject != null)
                        playerState = PlayerState.hasObject;                   
                    break;

                case PlayerState.hasObject:
                    playerMovement(gameTime);
                    if (newKeyboardState.IsKeyDown(Keys.Space))
                    {
                        if (carriedObject != null)
                            carriedObject.dropObject(false);
                        carriedObject = null;
                        playerState = PlayerState.freeFlight;
                        stateChange = 0;
                    }
                    position += new Vector2(0, 2);                                   
                    break;
            }
            if(wasdKeysUsed && isDiveComplete)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    AnimateLeft(gameTime);
                    if (carriedObject != null)
                        carriedObject.movingLeft = true;
                }
                else
                {
                    if (carriedObject!=null)
                        carriedObject.movingLeft = false;
                    AnimateRight(gameTime);
                }
            }
            else if(isDiveComplete)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    AnimateLeft(gameTime);
                    if(carriedObject!=null)
                     carriedObject.movingLeft = true;
                }
                else
                {
                    if (carriedObject!=null)
                        carriedObject.movingLeft = false;
                    AnimateRight(gameTime);
                }
            }
        }

        /*
            ‘******************************************************
            ‘***  Player
            ‘***  Blake Young
            ‘******************************************************
            ‘*** Draws the player onto the stage
            ‘*** Method Inputs:
            ‘***    spriteBatch - XNA drawing variable
            ‘*** Return value:
            ‘*** NA
            ‘******************************************************
            ‘*** 11/10/2015
            ‘******************************************************
         */
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,position,rectangle,Color.White,0f,origin,1.0f,SpriteEffects.None,0);
        }

        /*
            ‘******************************************************
            ‘***  AnimateRight
            ‘***  Blake Young
            ‘******************************************************
            ‘*** Selects the frames where the player is facing right
            ‘*** Method Inputs:
            ‘***    gameTime - game time elapse
            ‘*** Return value:
            ‘*** NA
            ‘******************************************************
            ‘*** 11/17/2015
            ‘******************************************************
         */
        public void AnimateRight(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 7)
                    currentFrame = 0;
            }
        }

        /*
            ‘******************************************************
            ‘***  AnimateLeft
            ‘***  Blake Young
            ‘******************************************************
            ‘*** Selects the frames where the player is facing left
            ‘*** Method Inputs:
            ‘***    gameTime - game time elapse
            ‘*** Return value:
            ‘*** NA
            ‘******************************************************
            ‘*** 11/17/2015
            ‘******************************************************
         */

        public void AnimateLeft(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (timer > interval)
            {
                currentFrame--;
                timer = 0;
                if (currentFrame < 9)
                    currentFrame = 17;
            }
        }

        /*
            ‘******************************************************
            ‘***  Position get/set
            ‘***  Blake Young
            ‘******************************************************
            ‘*** Getter/Setter for position
            ‘*** Method Inputs:
            ‘***    NA
            ‘*** Return value:
            ‘***    position
            ‘******************************************************
            ‘*** 11/11/2015
            ‘******************************************************
         */

        public Vector2 Position
        {
            get { return position; }
            set { ; }
        }

        /*
            ‘******************************************************
            ‘***  IsDiving get/set
            ‘***  Blake Young
            ‘******************************************************
            ‘*** Getter/Setter for bool isDiving
            ‘*** Method Inputs:
            ‘***    NA
            ‘*** Return value:
            ‘***    True/False
            ‘******************************************************
            ‘*** 11/11/2015
            ‘******************************************************
         */

        public Boolean IsDiving()
        {
            bool isTrue = true;
            if (this.playerState != PlayerState.diving)
                isTrue=false;
            return isTrue;
        }

        /*
            ‘******************************************************
            ‘***  takeDamage
            ‘***  Zach Languell
            ‘******************************************************
            ‘*** Drops ScoringObject if holding one
            ‘*** Method Inputs:
            ‘***    NA
            ‘*** Return value:
            ‘***    NA
            ‘******************************************************
            ‘*** 11/24/2015
            ‘******************************************************
         */

        public void takeDamage()
        {
            if (carriedObject != null)
                carriedObject.dropObject(true);
            carriedObject = null;
            playerState = PlayerState.freeFlight;
            stateChange = 0;
        }

        /*
            ‘******************************************************
            ‘***  IsDivingComplete get.set
            ‘***  Zach Languell
            ‘******************************************************
            ‘*** Getter.Setter for isDiveComplete
            ‘*** Method Inputs:
            ‘***    NA
            ‘*** Return value:
            ‘***    returns isDiveComplete
            ‘******************************************************
            ‘*** 11/24/2015
            ‘******************************************************
         */

        public Boolean IsDiveComplete
        {
            get { return isDiveComplete; }
            set { isDiveComplete = value; }
        }

        /*
            ‘******************************************************
            ‘***  playerMovement
            ‘***  Zach Languell
            ‘******************************************************
            ‘*** player movement in a function for cleanliness
            ‘*** Method Inputs:
            ‘***    gameTime - elapse game time
            ‘*** Return value:
            ‘***    NA
            ‘******************************************************
            ‘*** 11/24/2015
            ‘******************************************************
         */

        public void playerMovement(GameTime gameTime)
        {
            if (wasdKeysUsed)
            {
                //Normal player movement on the screen
                if (newKeyboardState.IsKeyDown(Keys.S))
                    this.position.Y += 3;
                if (newKeyboardState.IsKeyDown(Keys.W))
                    this.position.Y -= 4;
                if (newKeyboardState.IsKeyDown(Keys.D))
                    this.position.X += 3;
                if (newKeyboardState.IsKeyDown(Keys.A))
                    this.position.X -= 4;
                //---------------------------------------

                
            }
            else
            {
                //Normal player movement on the screen
                if (newKeyboardState.IsKeyDown(Keys.Down))
                    this.position.Y += 3;
                if (newKeyboardState.IsKeyDown(Keys.Up))
                    this.position.Y -= 4;
                if (newKeyboardState.IsKeyDown(Keys.Right))
                    this.position.X += 3;
                if (newKeyboardState.IsKeyDown(Keys.Left))
                    this.position.X -= 4;
                //---------------------------------------

                
            }
            //Prevents player to go below horizon 
            if (position.Y > 300 - (frameHeight / 2))
                position.Y = 300 - (frameHeight / 2);
            if (position.Y < 0 + (frameHeight / 2))
                position.Y = 0 + (frameHeight / 2);
            if (position.X > 800 - (frameWidth / 2))
                position.X = 800 - (frameWidth / 2);
            if (position.X < 0 + (frameWidth / 2))
                position.X = 0 + (frameWidth / 2);
            //---------------------------------------

        }      

    }
}
