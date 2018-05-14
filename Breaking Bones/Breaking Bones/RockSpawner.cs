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
   *** RockSpawner
   *** Kevin Anderson
   ******************************************************
   *** This class was made to spawn rocks in a random fashion.
   *** This class contains an Update() method.
   ******************************************************
   *** 11/20/2015
   ******************************************************
   *** 11/20/2015
   *** - Class created.
   *** - Added constructor and Update() method.
   *** 12/01/2015
   *** - Finalized comments.
   ******************************************************
   */
   class RockSpawner
   {
        // Declare class variables and other variables needed.
        private int randY;
        public List<Rock> rockObjects = new List<Rock>();
        private Texture2D texture;
        private float spawn = 0;
        int frameWidth = 0;
        int frameHeight = 0;

        /*
        ******************************************************
        *** RockSpawner
        *** Kevin Anderson
        ******************************************************
        *** Required constructor for RockSpawner.
        *** Method Inputs:
        ***     newTexture - Texture for rocks.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/20/2015
        ******************************************************
        */
        public RockSpawner(Texture2D newTexture)
        {
            texture = newTexture;
            frameHeight = texture.Height;
            frameWidth = texture.Width / 4;
        }

        /*
        ******************************************************
        ***  Update
        ***  Kevin Anderson
        ******************************************************
        *** Randomly spawns rocks using how much time has passed since the last spawn and random chance.
        *** Determines when to spwan or despawn a rock.
        *** Method Inputs:
        ***     gameTime - XNA timing variable.
        ***     random - A generated random from Game1.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/20/2015
        ******************************************************
        */
        public void Update(GameTime gameTime, Random random)
        {
            // Set the spawn based on the gameTime time.
            spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update each rock in the list of rockObjects.
            foreach (Rock s in rockObjects)
                s.Update(gameTime);

            // If statement checks if a second has passed. Then
            // it will check the number of rockObjects to determine
            // if a rock needs to be added.
            if (random.Next(150) == 1 && spawn > .7)//a second has passed
            {
                spawn = 0;

                // If the count of rockObjects is less than 2, then
                // add a new rock to the list of rockObjects.
                if (rockObjects.Count() < 2)
                    rockObjects.Add(new Rock(texture, new Vector2(900, 325), 32, 32, random));
            }

            // For statmement used to determine if a rock needs 
            // to be removed. If it is not visible, it should be
            // removed from the list of rockObjects.
            for (int i = 0; i < rockObjects.Count; i++)
                if (!rockObjects[i].isVisable)
                {
                    rockObjects.RemoveAt(i);
                    i--;
                }
        }
    }
}