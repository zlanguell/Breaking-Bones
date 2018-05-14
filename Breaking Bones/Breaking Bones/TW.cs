using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breaking_Bones
{
    /*
    ******************************************************
    *** TW
    *** Blake Young
    ******************************************************
    *** The class was created to control the TW (Tom Wittaker) game entity's movement
    *** and basic AI.
    ******************************************************
    *** 11/10/2015
    ******************************************************
    *** 11/10/2015
    *** - Created the subclass derived from the base class GameObject.
    *** - TW will dash back and forth and is animated accoringly.
    *** 11/16/2015
    *** - Random TW movement.
    *** 12/01/2015
    *** - Finalized comments.
    *******************************************************
    */
    class TW : GameObject
    {
        // Declare variables needed.
        float timer, interval = 30;
        bool isMovingRight = true;
        Random myRandom = new Random();

        /*
        ******************************************************
        *** TW
        *** Blake Young
        ******************************************************
        *** Required constructor for TW.
        *** Method Inputs:
        ***     newText - The texture pack associated to TW.
        ***     newPosition - the initial position TW will be placed on the stage.
        ***     newFrameHeight - the height of the frame of each sprite on a sprite sheet. This allows animation.
        ***     newFrameWidth - the width of the frame of each sprite on a sprite sheet. This allows animation.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/10/2015
        ******************************************************
        */
        public TW(Texture2D newText, Vector2 newPosition, int newFrameHeight, int newFrameWidth)
            : base(newText, newPosition, newFrameHeight, newFrameWidth)
        {
            texture = newText;
            position = newPosition;
            frameHeight = newFrameHeight;
            frameWidth = newFrameWidth;
        }


        /*
        ******************************************************
        *** Update
        *** Blake Young, Zach Languell, Kevin Anderson
        ******************************************************
        *** To update TW's position, simulated velocity.
        *** Method Inputs:
        ***     gameTime - XNA timing variable.
        ***     playerIsDive - Boolean whose value is sent from Player, to Game1, to TW, referencing is the player is diving.
        ***     playerPosition - Vector2 whose value is sent from Player, to Game1, to TW, referencing the X and Y position of the player.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/10/2015
        ******************************************************
        */
        public void Update(GameTime gameTime, Boolean playerIsDive, Vector2 playerPosition)
        {
            // Creates Rectangle and Vector2 objects that will be used
            // to manage the collision of TW and how it is drawn
            // in-game. Sets position with velocity.
            rectangle = new Rectangle(currentFrame*frameWidth, 0, frameWidth, frameHeight);
            origin = new Vector2(rectangle.Width/2, rectangle.Height/2);
            position = position+velocity;
            collisionRectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

            // If-elseif-else statements used to determine the position and animation
            // to be used for TW.
            if (playerIsDive)
            {
                // If the player's X position is less than positions's X,
                // then animate right and increment position. Otherwise
                // animare left and increment position.
                if (playerPosition.X <= position.X)
                {
                    AnimateRight(gameTime);
                    position += new Vector2(2.6f, 0);
                }
                else
                {
                    AnimateLeft(gameTime);
                    position += new Vector2(-4.5f, 0);
                }
            }
            else if (isMovingRight)
            {
                // Animate right if TW is moving to the right.
                // Increment position.
                AnimateRight(gameTime);
                position += new Vector2(2.6f, 0);

                // If statement used to set isMovingRight to false
                // if the condition is satisfied.
                if (position.X >= (800 - (frameWidth / 2)) && isMovingRight)
                    isMovingRight = false;
            }
            else
            { 
                // Animate left if TW is moving to the left.
                // Increment position.
                AnimateLeft(gameTime);
                position += new Vector2(-4.5f, 0);
                
                // If statement used to set isMovingRight to true
                // if the condition is satisfied.
                if (position.X < (frameWidth / 2) && isMovingRight == false)
                    isMovingRight = true;
            }

            // If statements used to set position's X.
            if (position.X > (800 - (frameWidth / 2)))
                position.X = (800 - (frameWidth / 2));
            if (position.X < (frameWidth / 2))
                position.X = (frameWidth / 2);
           
            // Call the randomMovement method.
            randomMovement(); 
        }

        /*
        ******************************************************
        *** Draw
        *** Blake Young
        ******************************************************
        *** Used to draw textures on to the stage.
        *** Method Inputs:
        ***     spriteBatch - Of type SpriteBatch, it is used by XNA to draw textures.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/10/2015
        ******************************************************
        */
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,position,rectangle,Color.White,0f,origin,1.0f,SpriteEffects.None,0);
        }

        /*
        ******************************************************
        *** AnimateRight
        *** Blake Young
        ******************************************************
        *** Function meant to shift the animation of the texture to simulate running right.
        *** Method Inputs:
        ***     gameTime - XNA timing variable.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/10/2015
        ******************************************************
        */
        public void AnimateRight(GameTime gameTime)
        {
            // Sets timer to gameTime time.
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;

            // If statement executes if timer is greater than interval.
            // The current frame is incremented and the timer is set to 0.
            if(timer > interval)
            {
                currentFrame++;
                timer = 0;

                // If the current frame exceeds the max frames, set the
                // current frame back to 0.
                if (currentFrame > 7)
                    currentFrame = 0;
            }
        }

        /*
        ******************************************************
        *** AnimateLeft
        *** Blake Young
        ******************************************************
        *** Function meant to shift the animation of the texture to simulate running left.
        *** Method Inputs:
        ***     gameTime - XNA timing variable
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/10/2015
        ******************************************************
        */
        public void AnimateLeft(GameTime gameTime)
        {
            // Sets timer to gameTime time.
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;

            // If statement executes if timer is greater than interval.
            // The current frame is incremented and the timer is set to 0.
            if (timer > interval)
            {
                currentFrame--;
                timer = 0;

                // If the current frame exceeds the max frames, set the
                // current frame back to 0.
                if (currentFrame <8)
                    currentFrame = 15;
            }
        }

        /*
        ******************************************************
        *** randomMovement
        ***  lake Young
        ******************************************************
        *** Randomizes TW's movement so that he can be les predictable.
        *** Method Inputs:
        ***     NA
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/16/2015
        ******************************************************
        */
        private void randomMovement()
        {
            // Generate a random number.
            int randNum = myRandom.Next(0, 200);

            // If the random number is equal to 0,
            // then check the boolean isMovingRight.
            if (randNum == 0)
            {
                // If isMovingRight is true, then set
                // it to false, otherwise, set it to true.
                if (isMovingRight)
                    isMovingRight = false;
                else
                    isMovingRight = true;
            }
        }
    }
}
