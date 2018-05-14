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
    *** ScrollingBkg
    *** Blake Young, Kevin Anderson, Zachary Languell, Holley Melchor
    ******************************************************
    *** This sub class was created to create a moving background that loops infinitely. 
    *** This gives the impression of flying.
    ******************************************************
    *** 11/06/2015
    ******************************************************
    *** 11/06/2015
    ***  - Created subclass derived from GameObject.
    ***  - Added functionality for looping and simulated velocity.
    *** 12/01/2015
    ***  - Finalized comments.
    *******************************************************
    */
    class ScrollingBkg : GameObject
    {
        // Declare variables needed.
        Rectangle rectangleTwo = new Rectangle(2047, 0, 2047, 600);

        /*
        ******************************************************
        *** ScrollingBkg
        *** Zachary Languell, Holley Melchor
        ******************************************************
        *** Required constructor for ScrollingBkg.
        *** Method Inputs:
        ***     newText - Texture2D of the background image.
        ***     newPosition - Vector2 representing the position of the image.
        ***     newFrameHeight - Should be the height of the texture ex: 32px
        ***     newFrameWidth - Should be the width of a single instance of a image on a sprite sheet. ex: 32px
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/06/2015
        ******************************************************
        */
        public ScrollingBkg(Texture2D newTexture, Rectangle newRectangle)
        {
            texture = newTexture;
            rectangle = newRectangle;
            position.X = 0;
            position.Y = 0;
        }

        /*
        ******************************************************
        ***  Update
        ***  Blake Young, Kevin Anderson, Zach Languell
        ******************************************************
        *** Update is used to move the background texture from right to left, simulating movement.
        *** Method Inputs:
        ***     gameTime - XNA timing variable
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/06/2015
        ******************************************************
        */
        public override void Update(GameTime gametime)
        {
            // If else if statement is used to adjust
            // the X value of rectangle and rectangleTwo.
            if (rectangle.X + texture.Width <= 0)
            {
                rectangle.X = rectangleTwo.X + texture.Width;
            }
            else if (rectangleTwo.X + texture.Width<= 0)
            {
                rectangleTwo.X = rectangle.X + texture.Width;
            }
            
            // Decrements both rectangle and rectangleTwo by 2.
            rectangle.X -= 2;
            rectangleTwo.X -= 2;         
        }

        /*
        ******************************************************
        ***  Draw
        ***  Blake Young, Kevin Anderson, Zach Languell
        ******************************************************
        *** Draw method draws the texture and rectangle onto the stage.
        *** Method Inputs:
        ***     spriteBatch - Required by XNA to draw textures onto the stage.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/06/2015
        ******************************************************
        */
        public override void Draw(SpriteBatch spriteBatch)
        {        
            spriteBatch.Draw(texture, rectangle, Color.White);
            spriteBatch.Draw(texture, rectangleTwo, Color.White);            
        }
    }
}
