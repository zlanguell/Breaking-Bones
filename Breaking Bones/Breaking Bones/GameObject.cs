/*
‘******************************************************
‘***  GameObject
‘***  Zach Languell, Kevin Anderson
‘******************************************************
‘*** Parent class that contains the base variables and functions for the childs, such as player, rock, and enemyObject
‘******************************************************
‘*** 11/03/2015
‘******************************************************
‘*** 11/03/2015
 *      Created Class
 *   11/8/2015
 *      Worked on the constructor
‘*******************************************************
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Breaking_Bones
{
    class GameObject
    {
        public Texture2D texture;
        public Rectangle rectangle;
        protected Vector2 origin;
        protected Vector2 velocity;
        public Vector2 position;
        protected Vector2 spawnLocation;
        public Rectangle collisionRectangle;
        public int currentFrame;
        public int frameHeight;
        public int frameWidth;

        /*
           ‘******************************************************
           ‘***  GameObject
           ‘***  Zach Languell, Kevin Anderson
           ‘******************************************************
           ‘*** Blank constructor created for ease of overriding
           ‘*** Method Inputs:
           ‘***    NA
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/3/2015
           ‘******************************************************
        */
        public GameObject()
        {

        }

        /*
           ‘******************************************************
           ‘***  GameObject
           ‘***  Zach Languell, Kevin Anderson
           ‘******************************************************
           ‘*** Require constructor for GameObject
           ‘*** Method Inputs:
           ‘***    newTexture - image of the gameobject instance
         *         newRectangle - rectangle of the texture
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/3/2015
           ‘******************************************************
        */

        public GameObject(Texture2D newText, Rectangle newRectangle)
        {
            texture = newText;
            rectangle = newRectangle; 
        }

        /*
           ‘******************************************************
           ‘***  GameObject
           ‘***  Zach Languell, Kevin Anderson
           ‘******************************************************
           ‘*** Another constructor for children classes
           ‘*** Method Inputs:
           ‘***    newTexture - image of the gameobject instance
         *         newPosition - position of the object on the stage - 0-800/0-600
         *         newFrameHeight - 0-128
         *         newFrameWidth - 0-128
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/3/2015
           ‘******************************************************
        */

        public GameObject(Texture2D newText, Vector2 newPosition, int newFrameHeight, int newFrameWidth )
        {
            texture = newText;
            position = newPosition;
            frameHeight=newFrameHeight;
            frameWidth=newFrameWidth;
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width,texture.Height);
        }

        /*
           ‘******************************************************
           ‘***  Draw
           ‘***  Zach Languell, Kevin Anderson
           ‘******************************************************
           ‘*** Draws objects to the stage
           ‘*** Method Inputs:
           ‘***    spriteBatch - to draw onto the stage
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/3/2015
           ‘******************************************************
        */

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        /*
           ‘******************************************************
           ‘***  Update
           ‘***  Zach Languell, Kevin Anderson
           ‘******************************************************
           ‘*** Created for children classes
           ‘*** Method Inputs:
           ‘***    NA
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/3/2015
           ‘******************************************************
        */

        virtual public void Update(GameTime gametime)
        {

        }

        /*
           ‘******************************************************
           ‘***  Spawner
           ‘***  Zach Languell, Kevin Anderson
           ‘******************************************************
           ‘*** Created for children classes
           ‘*** Method Inputs:
           ‘***    NA
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/3/2015
           ‘******************************************************
        */

        public virtual void Spawner()
        {

        }
    }
}
