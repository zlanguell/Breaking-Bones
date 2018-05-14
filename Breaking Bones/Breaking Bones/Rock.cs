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
    *** Rock
    *** Blake Young, Kevin Anderson, Zachary Languell, Holley Melchor
    ******************************************************
    *** This class was created to manage the rocks (game object) in
    *** the game. This class contains several methods, Draw(), Update(), and
    *** Animate().
    ******************************************************
    *** 11/20/2015
    ******************************************************
    *** 11/20/2015
    *** - Created the subclass derived from the base class GameObject.
    *** - Added constructor, Draw(), Update(), and Animate().
    *** 11/30/2015
    *** - Comments modified.
    *** 12/01/2015
    *** - Finalized comments.
    ******************************************************
    */
    class Rock:GameObject
    {
        // Declare class variables needed.
        Random random;
        public float randX;
        private int rockType;
        public bool isVisable = true;

        /*
        ******************************************************
        *** Rock
        *** Blake Young
        ******************************************************
        *** Constructor for the Rock game object.
        *** Method Inputs:
        ***     newTexture - The texture pack associated to Rock.
        ***     newPosition - The initial position a Rock will be placed on the stage.
        ***     newFrameHeight - The height of the frame of each sprite on a sprite sheet. Used for animation.
        ***     newFrameWidth - The width of the frame of each sprite on a sprite sheet. Used for animation.
        *** Return Values:
        ***     NA
        ******************************************************
        *** 11/20/2015
        ******************************************************
        */
        public Rock(Texture2D newTexture, Vector2 newPosition, int newFrameHeight, int newFrameWidth, Random newRandom)
            : base(newTexture, newPosition, newFrameHeight, newFrameWidth)
        {
            // Sets the values of the Game Object variables to the values passed to
            // the constructor.
            texture = newTexture;
            position = newPosition;
            frameHeight = newFrameHeight;
            frameWidth = newFrameWidth;
            random = newRandom;

            // Set randX to a value that will be used for the velocity
            // of the rock.
            randX = -2;
            velocity = new Vector2(randX, 0);

            // Randomly generate a number between 0 to 3 that will
            // determine which rock type to use from the sprite sheet.
            rockType = random.Next(4);
        }

        /*
        ******************************************************
        *** Update
        *** Kevin Anderson
        ******************************************************
        *** Used to update the position of a Rock and simulated velocity.
        *** Method Inputs:
        ***     gameTime - XNA timing variable.
        *** Return Values:
        ***     NA
        ******************************************************
        *** 11/20/2015
        ******************************************************
        */
        public override void Update(GameTime gameTime)
        {
            // Creates Rectangle and Vector2 objects that will be used
            // to manage the collision of the rock and how it is drawn
            // in-game.
            collisionRectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);

            // Call the Animate() method and pass it gameTime.
            Animate(gameTime);

            // Set the position of the rock based on its current
            // position plus its velocity.
            position = position + velocity;

            // If the rock's X position is less than the texture's width.
            if (position.X < (0 - texture.Width))
                isVisable = false;
        }

        /*
        ******************************************************
        *** Draw
        *** Holley Melchor
        ******************************************************
        *** Used to draw textures in-game.
        *** Method Inputs:
        ***     spriteBatch - XNA variable to draw textures.
        *** Return Values:
        ***     NA
        ******************************************************
        *** 11/20/2015
        ******************************************************
        */
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, origin, 2f, SpriteEffects.None, 0);
        }

        /*
        ******************************************************
        *** Animate
        *** Zachary Languell
        ******************************************************
        *** Used to select the rock type to be used from the sprite sheet.
        *** Method Inputs:
        ***     gameTime - XNA timing variable.
        *** Return Values:
        ***     NA
        ******************************************************
        *** 11/20/2015
        ******************************************************
        */
        public void Animate(GameTime gameTime)
        {
            // A switch is used to determine which frame in the
            // sprite sheet will be used for the rock type. The
            // variable rockType will be a value from 0 to 3.
            switch (rockType)
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
                case 3:
                    currentFrame = 3;
                    break;
            }
        }
    }
}