using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    *** Button
    *** Blake Young, Holley Melchor
    ******************************************************
    *** This class was written to have functionality for the menu
    *** buttons. This class contains Update(), Draw(), isClicked() methods.
    ******************************************************
    *** 11/24/2015
    ******************************************************
    *** 11/24/2015
    *** - Created class.
    *** - Clicking function.
    *** - Animation basics.
    *** - Sounds.
    *** 11/25/2015
    *** - Added functionality for options button.
    *** 12/01/2015
    *** - Finalized comments.
    ******************************************************
    */
    class Button
    {
        // Declare class variables.
        Texture2D texture;
        Rectangle rectangle;
        Rectangle collisionRectangle;
        Vector2 origin;
        int frameHeight;
        int frameWidth;
        int currentFrame;
        Vector2 position;
        bool play = true;
        MouseState mouseState;
        Rectangle mouseRectangle;
        SoundEffect sound;
        int numFrames;
        public bool clicked = false;
        MouseState lastMouseState;
        public bool wasdKeysUsed;


        /*
        ******************************************************
        *** Button
        *** Blake Young
        ******************************************************
        *** Required constructor for Button.
        *** Method Inputs:
        ***     newTexture - Texture of button.
        ***     newPosition - Position on stage - 0-800, 0-600.
        ***     newFrameWidth - 0 - 128.
        ***     newFrameHeight - 0 - 128.
        ***     newSound - Sound file.
        ***     newNumFrames - 0 - 4.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/24/2015
        ******************************************************
        */
        public Button(Texture2D newTexture, Vector2 newPosition, int newFrameWidth, int newFrameHeight, SoundEffect newSound, int newNumFrames)
        {
            texture = newTexture;
            position = newPosition;
            frameWidth = newFrameWidth;
            frameHeight = newFrameHeight;
            numFrames = newNumFrames;
            collisionRectangle = new Rectangle((int)position.X - (frameWidth / numFrames), (int)position.Y - (frameHeight / numFrames), frameWidth, frameHeight);
            sound = newSound;
        }

        /*
        ******************************************************
        ***  Button
        ***  Holley Melchor
        ******************************************************
        *** Overridden constructor for Button for the special button in options.
        *** Method Inputs:
        ***     newTexture - Texture of button.
        ***     newPosition - Position on stage - 0-800, 0-600.
        ***     newFrameWidth - 0 - 128.
        ***     newFrameHeight - 0 - 128.
        ***     newSound - Sound file.
        ***     newNumFrames - 0 - 4.
        ***     newWasdKeysUsed  - true/false.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/24/2015
        ******************************************************
        */
        public Button(Texture2D newTexture, Vector2 newPosition, int newFrameWidth, int newFrameHeight, SoundEffect newSound, int newNumFrames, bool newWasdKeysUsed)
        {
            texture = newTexture;
            position = newPosition;
            frameWidth = newFrameWidth;
            frameHeight = newFrameHeight;
            numFrames = newNumFrames;
            collisionRectangle = new Rectangle((int)position.X - (frameWidth / 2), (int)position.Y - (frameHeight / 2), frameWidth, frameHeight);
            sound = newSound;
            wasdKeysUsed = newWasdKeysUsed;
            clicked = !wasdKeysUsed;
        }

        /*
        ******************************************************
        *** Update
        *** Blake Young
        ******************************************************
        *** Update function is used to check for changes in in current frame, hovering, and sound display.
        *** Method Inputs:
        ***     NA
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/24/2015
        ******************************************************
        */
        public void Update()
        {
            // Create any new Rectangle or Vector2 objects needed.
            // Set mouseState by getting the state of the mouse.
            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            mouseState = Mouse.GetState();
            mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 2, 2);

            // If else if statement used to check which update to perform. If
            // numFrames is equal to 2, then menu buttons are being updated.
            // If numFrames is equal to 4, then the control button in the options
            // menu is being updated.
            if(numFrames == 2)
            {
                // If the mouse has a collision with the menu rectangle, then play
                // the hovering sound. Otherwise, set play to true and set the current
                // frame back to 0.
                if (mouseRectangle.Intersects(collisionRectangle))
                {
                    currentFrame = 1;

                    if (play)
                    {
                        sound.Play();
                        play = false;
                    }
                }
                else
                {
                    play = true;
                    currentFrame = 0;
                }
            }
            else if (numFrames == 4)
            {
                // If the mouse has a collision with the menu rectangle and it is not clicked, then play
                // the hovering sound and set the currentFrame to 1. Else if not clicked set play to true and set the current
                // frame back to 0.
                if (mouseRectangle.Intersects(collisionRectangle)&& !clicked)
                {
                    currentFrame = 1;

                    if (play)
                    {
                        sound.Play();
                        play = false;
                    }
                }
                else if(!clicked)
                {
                    play = true;
                    currentFrame = 0;
                }

                // If the mouse has a collision with the menu rectangle and it is clicked, then play
                // the hovering sound and set the currentFrame to 3. Else if it is clicked set play to true and set the current
                // frame to 2.
                if (mouseRectangle.Intersects(collisionRectangle) && clicked)
                {
                    currentFrame = 3;

                    if (play)
                    {
                        sound.Play();
                        play = false;
                    }
                }
                else if(clicked)
                {
                    play = true;
                    currentFrame = 2;
                }
            }

            // If the left mouse button is pressed and the mouse intersects with the collision rectangle, then
            // set the lastMouseState by getting the state of the mouse.
            if (mouseState.LeftButton == ButtonState.Pressed && mouseRectangle.Intersects(collisionRectangle))
                lastMouseState = Mouse.GetState();

            // If the left mouse button is released and lastMouseState is pressed, then check the boolean clicked.
            if (mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
            {
                // If else statement used to switch the value of clicked.
                if (!clicked)
                    clicked = true;
                else
                    clicked = false;

                // Set lastMouseState by getting the state of the mouse.
                lastMouseState = Mouse.GetState();
            }
        }

        /*
        ******************************************************
        *** Draw
        *** Blake Young
        ******************************************************
        *** Drawing the buttons onto the stage.
        *** Method Inputs:
        ***     spriteBatch - XNA drawing variable.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/24/2015
        ******************************************************
        */
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }

        /*
        ******************************************************
        *** Button
        *** Holley Melchor
        ******************************************************
        *** Function designed to tell the user if a button is clicked.
        *** Method Inputs:
        ***     NA
        *** Return value:
        ***     True/False - Tells if the button is clicked.
        ******************************************************
        *** 11/24/2015
        ******************************************************
        */
        public bool isClicked()
        {
            // If statement returns true if left mouse button is pressed and the mouse intersects with the collision rectangle.
            if (mouseState.LeftButton == ButtonState.Pressed && mouseRectangle.Intersects(collisionRectangle))
            {
                return true;
            }
            return false;
        }

    }
}
