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
    /*
    ******************************************************
    *** ScoringObject
    *** Kevin Anderson, Blake Young, Zachary Languell, Holley Melchor
    ******************************************************
    *** This class was created to manage the scoring objects (turtles, bones) in
    *** the game. This class contains several methods, Update(), Draw(), AnimateLeft(),
    *** dropObject(), and setStateCarried(). 
    ******************************************************
    *** 11/10/2015
    ******************************************************
    *** 11/10/2015
    *** - Created the subclass derived from the base class GameObject.
    *** - Added constructor.
    *** 11/11/2015
    *** - Added the Draw() method.
    *** 11/12/2015
    *** - Modified constructor, Update(), and Draw() methods.
    *** 11/17/2015
    *** - Added the AnimateLeft() and dropObject() methods.
    *** - Updated Draw() and Update() methods.
    *** 11/19/2015
    *** - Added enumerations for ScoreState.
    *** - Added setStateCarried() method.
    *** - Modified Draw() method.
    *** 12/01/2015
    *** - Finalized comments.
    ******************************************************
    */
    class ScoringObject:GameObject
    {
        // Declare class variables.
        public int score;
        public bool isCarried = false;        
        public bool isVisable = true;
        public bool isbone, forceDrop;
        public Vector2 lastPosition;
        private int boneType;
        public float randX;
        private SpriteEffects flipEffect = SpriteEffects.FlipHorizontally;
        public ScoreState scoreState;
        public bool movingLeft = false;

        // Declare enumeration states for ScoreState.
        public enum ScoreState
        {
            Walking,
            Carried,
            Dropped,
            Dead
        }

        // Declare variables needed for animation.
        float timer, interval = 100;
        Random random;

        /*
        ******************************************************
        *** ScoringObject
        *** Holley Melchor
        ******************************************************
        *** Constructor for ScoringObject game object.
        *** Method Inputs:
        ***     newTexture - The texture pack associated to the ScoringObject.
        ***     newPosition - The initial position the ScoringObject will be placed on the stage.
        ***     newFrameHeight - The height of the frame of each sprite on a sprite sheet. Used for animation.
        ***     newFrameWidth - The width of the frame of each sprite on a sprite sheet. Used for animation.
        ***     random - The variable used to determine which bone type to use.
        ***     isBone - The variable that will return true if ScoringObject is a bone. Returns false if it isn't.
        *** Return Values:
        ***     NA
        ******************************************************
        *** 11/10/2015
        ******************************************************
        */
        public ScoringObject(Texture2D newTexture, Vector2 newPosition, int newFrameHeight, int newFrameWidth, Random random, bool isBone)
            : base(newTexture, newPosition, newFrameHeight, newFrameWidth)
        {
            // Set the GameObject variables and class variables to the values passed to the
            // constructor.
            texture = newTexture;
            position = newPosition;
            frameHeight = newFrameHeight;
            frameWidth = newFrameWidth;
            this.isbone = isBone;

            // Set the class variable score to 25 as a base score that will be
            // multiplied in Game1 based on collision detection.
            this.score = 25;

            // Set the value of forceDrop to be used in the dropObject() method.
            forceDrop = false;

            // If else statement checks if the ScoringObject is a bone. If it is,
            // randX is set to -2. If not, then a random float is generated for randX.
            if (isBone)
                randX = -2;
                            
            else
                randX = random.Next(-4, -1);
            
            // If statement used to decrement randX if the ScoringObject is not a bone
            // and randX is equal to -2.
            if (!isBone && randX == -2)
                randX -= .2f;

            // Sets the random variable in the class to the random variable that was passed.
            this.random = random;

            // Creates a new Vector2 for velocity of the ScoringObject.
            velocity = new Vector2(randX, 0);

            // Based on a random number from 0 - 2, the type of bone that will be determined.
            boneType = random.Next(3);

        }

        /*
        ******************************************************
        *** Update
        *** Kevin Anderson
        ******************************************************
        *** Handles the position and visibility of each ScoringObject as the game continously updates.
        *** Method Inputs:
        ***     gameTime - XNA timing variable.
        ***     playerPosition - The position of the player.
        *** Return Values:
        ***     NA
        ******************************************************
        *** 11/10/2015
        ******************************************************
        */
        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
           // Creates Rectangle and Vector2 objects that will be used
           // to manage the collision of the ScoringObject and how it is drawn
           // in-game.
           collisionRectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
           rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
           origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);

           // Calculate the interval needed for animation.
           interval = 300 / (-1 * velocity.X);

           // Call AnimateLeft() and pass it gameTime.
           AnimateLeft(gameTime);

           // A switch is used based on scoreState to update position of the ScoringObject and
           // its visibility. Each case has variations in calculating position and/or visibility.
           switch (scoreState)
           {
                case ScoreState.Walking: 
                    position = position + velocity;
                    break;
                case ScoreState.Carried:
                    if (this.movingLeft)
                    {
                        position.X = playerPosition.X + 20;
                        position.Y = playerPosition.Y + 22;
                    }
                    else
                    {
                        position.X = playerPosition.X - 20;
                        position.Y = playerPosition.Y + 22;
                    }
                    break;
                case ScoreState.Dropped:
                    dropObject(forceDrop);
                    break;
                case ScoreState.Dead:
                    this.position.X = -10;
                    this.isVisable = false;
                    break;
            }

            // If the position of the ScoringObject texture is greater than its X position, sets
            // isVisable to false.
            if (position.X < (0 - texture.Width))
                isVisable = false;
        }

        /*
        ******************************************************
        *** Draw
        *** Kevin Anderson
        ******************************************************
        *** Used to draw the ScoringObject textures based on the scoreState.
        *** Method Inputs:
        ***     spriteBatch - XNA variable to draw textures.
        *** Return Values:
        ***     NA
        ******************************************************
        *** 11/11/2015
        ******************************************************
        */
        public override void Draw(SpriteBatch spriteBatch)
        {
            // A switch statement is used to determine how the ScoringObject
            // will be drawn. Each case is determined by the enumerated scoreState.
            switch(scoreState)
            {
                case ScoreState.Walking:
                    spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
                    break;
                case ScoreState.Carried:
                    if(Keyboard.GetState().IsKeyDown(Keys.Left))
                        spriteBatch.Draw(texture, position, rectangle, Color.White, (float)Math.PI, origin, 1.0f, flipEffect, 0);
                    else
                        spriteBatch.Draw(texture, position, rectangle, Color.White, (float)Math.PI, origin, 1.0f, SpriteEffects.None, 0);
                    break;
                case ScoreState.Dropped:
                        spriteBatch.Draw(texture, position, rectangle, Color.White, (float)Math.PI, origin, 1.0f, SpriteEffects.None, 0);
                    break;
                case ScoreState.Dead:
                    break;
            }
        }

        /*
        ******************************************************
        *** AnimateLeft
        *** Blake Young
        ******************************************************
        *** Handles the animation of a SocringObject.
        *** Method Inputs:
        ***     gameTime - XNA timing variable.
        *** Return Values:
        ***     NA
        ******************************************************
        *** 11/17/2015
        ******************************************************
        */
        public void AnimateLeft(GameTime gameTime)
        {
            // If else statement that checks if the ScoringObject is a bone or not.
            // If it is not a bone, the current frame for a turtle is updated.
            // If it is a bone, then use a switch statement to determine which 
            // type of bone to use.
            if (!isbone)
            {
                // Initialize the tiemr based on the time from gameTime.
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;

                // If the timer is greater than the interval, then increment
                // the current frame and set the timer to 0.
                if (timer > interval)
                {
                    currentFrame++;
                    timer = 0;

                    // If the current frame is greater than 1, reset
                    // the current frame to 0.
                    if (currentFrame > 1)
                        currentFrame = 0;
                }
            }
            else
            {
                // A switch is used on boneType to determine which type
                // of bone is used.
                switch(boneType)
                {
                    case 0:
                        currentFrame = 0;
                        break;
                    case 1:
                        currentFrame = 1;
                        break;
                    case 2:
                        currentFrame = 2;
                        break;
                }
            }  
        }

        /*
        ******************************************************
        *** dropObject
        *** Zachary Languell
        ******************************************************
        *** Handles when a ScoringObject is dropped.
        *** Method Inputs:
        ***     forceDrop - A boolean variable that determines if a ScoringObject needs to force dropped.
        *** Return Values:
        ***     NA
        ******************************************************
        *** 11/17/2015
        ******************************************************
        */
        public void dropObject(bool forceDrop)
        {
            // Sets the ScoringObject forceDrop variable to the boolean that was passed to the method.
            this.forceDrop = forceDrop;

            // If forceDrop is true, then score is set to 0.
            if (forceDrop)
                this.score = 0;

            // The variable scoreState is set to a dropped state.
            scoreState = ScoreState.Dropped;

            // If else statement is used to change the Y position of
            // the ScoringObject if the Y position is less than 600 + frameHeight
            // otherwise set isVisable to false.
            if (this.position.Y < 600 + frameHeight)
                this.position.Y += 6;
            else
                isVisable = false;           
        }

        /*
        ******************************************************
        *** setStateCarried
        *** Kevin Anderson
        ******************************************************
        *** Method sets the scoreState to carried.
        *** Method Inputs:
        ***     NA
        *** Return Values:
        ***     NA
        ******************************************************
        *** 11/19/2015
        ******************************************************
        */
        public void setStateCarried()
        {
            scoreState = ScoreState.Carried;
        }
    }
}
