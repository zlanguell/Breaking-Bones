using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breaking_Bones
{
    /*
    ******************************************************
    *** EnemyObject
    *** Zachary Languell
    ******************************************************
    *** This class is used to manage enemies that can kill the player and result in end game state.
    ******************************************************
    *** 11/19/2015
    ******************************************************
    *** 11/19/2015
    *** - Created class and added functionality.
    *** 12/01/2015
    *** - Finalized comments.
    *******************************************************
    */
    class EnemyObject:GameObject 
    {
        //Declare variables needed.
        Boolean isTree;
        Random random;
        public float randX;
        private int treeType;
        float timer, interval = 100;
        public bool isVisable = true;


        /*
        ******************************************************
        *** EnemyObject
        *** Zachary Languell
        ******************************************************
        *** Required constructor for EnemyObject.
        *** Method Inputs:
        ***     newTexture - Texture of the object - image.
        ***     newPosition - Position of the enemy on the stage.
        ***     newFrameHeight - 0-128.
        ***     newFrameWidth - 0-128.
        ***     newRandom - Random generated in Game1.
        ***     newIsTree - True/False.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/19/2015
        ******************************************************
        */
        public EnemyObject(Texture2D newTexture, Vector2 newPosition, int newFrameHeight, int newFrameWidth, Random newRandom, bool newIsTree)
            : base(newTexture, newPosition, newFrameHeight, newFrameWidth)
        {
            texture = newTexture;
            position = newPosition;
            frameHeight = newFrameHeight;
            frameWidth = newFrameWidth;
            isTree = newIsTree;
            random = newRandom;

            // If statement used to check boolean isTree to determine if EnemyObject is a tree or an eagle.
            // True will set randX to -2, and false will set randX to -5.
            if (isTree)
                randX = -2;
            else
                randX = -5;

            // Create new Vector2 for velocity.
            velocity = new Vector2(randX, 0);

            // Generate a random number to determine treeType.
            treeType = random.Next(2);
        }

        /*
        ******************************************************
        *** Update
        *** Zachary Languell
        ******************************************************
        *** Updates the position and animation of an enemy object.
        *** Method Inputs:
        ***     gameTime - Game elapse time.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/19/2015
        ******************************************************
        */
        public override void Update(GameTime gameTime)
        {
            // Create new Rectangle or Vector2 objects as needed for animation.
            collisionRectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            interval = 300 / (-1 * velocity.X);

            // Animate EnemyObject.
            // Increment position with velocity.
            AnimateLeft(gameTime);
            position = position + velocity;

            // If statement that will set isVisable to false
            // if the condition is satisfied.
            if (position.X < (0 - texture.Width))
                isVisable = false;
        }

        /*
        ******************************************************
        *** Draw
        *** Zachary Languell
        ******************************************************
        *** Draw enemyObject texture onto the stage. Flips when picked up by player.
        *** Method Inputs:
        ***     spriteBatch - Draws onto stage.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/19/2015
        ******************************************************
        */
        public override void Draw(SpriteBatch spriteBatch)
        {
            // If else statement checks boolean isTree. If true, then draw a tree,
            // if false, tehn draw an eagle.
            if(isTree)
                spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, origin, 2.2f, SpriteEffects.None, 0);
        }

        /*
        ******************************************************
        *** AnimateLeft
        *** Zachary Languell
        ******************************************************
        *** Animates the enemy object based on the spritesheet and shifting sections of the image.
        *** Method Inputs:
        ***     gameTime - Game elapse time.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/19/2015
        ******************************************************
        */
        public void AnimateLeft(GameTime gameTime)
        {
            // If else statement to check teh boolean isTree.
            // If it is not a tree, then animate the eagle.
            // If it is a tree, then select a tree type to use.
            if (!isTree)
            {
                // Set timer to the gameTime time.
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;

                // If timer is greater than interval, then increment currentFrame
                // and set the time to 0.
                if (timer > interval)
                {
                    currentFrame++;
                    timer = 0;

                    // If the current frame exceeds 5, then reset currentFrame
                    // to 0.
                    if (currentFrame > 5)
                        currentFrame = 0;
                }
            }
            else
            {
                // Switch statement used on treeType to determine which tree to draw.
                switch (treeType)
                {
                    case 0:
                        currentFrame = 0;
                        break;
                    case 1:
                        currentFrame = 1;
                        break;
                }
            }
        }
    }
}
