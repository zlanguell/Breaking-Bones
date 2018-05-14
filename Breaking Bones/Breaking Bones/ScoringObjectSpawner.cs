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
    /*
    ******************************************************
    *** ScoringObjectSpawner
    *** Kevin Anderson, Blake Young, Zachary Languell
    ******************************************************
    *** This class was made to spawn ScoringObjects(turtles, bones) in a random fashion.
    *** This class contains an Update() method.
    ******************************************************
    *** 11/12/2015
    ******************************************************
    *** 11/12/2015
    *** - Class created.
    *** - Added constructor and Update() method.
    *** 11/19/2015
    *** - Modified class variables, constructor, and the Update() method.
    *** 12/01/2015
    *** - Finalized comments.
    ******************************************************
    */
    class ScoringObjectSpawner
    {
        // Declare class variables.
        private int randY;
        public List<ScoringObject> scoringObjects = new List<ScoringObject>();
        private Texture2D turtleTexture;
        private float spawn = 0;   
        public bool isBone = false;
        private Texture2D boneTexture;

        /*
        ******************************************************
        *** ScoringObjectSpawner
        *** Kevin Anderson
        ******************************************************
        *** Required constructor for ScoringObjectSpawner
        *** Method Inputs:
        ***     turtleTexture - Texture for turtle.
        ***     boneTexture - Texture for bone.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/12/2015
        ******************************************************
        */
        public ScoringObjectSpawner(Texture2D turtleTexture, Texture2D boneTexture)
        {
            // Sets the texture class variables to the textures that were passed to the constructor.
            this.turtleTexture = turtleTexture;
            this.boneTexture = boneTexture;
        }

        /*
        ******************************************************
        *** Update
        *** Blake Young, Zachary Languell
        ******************************************************
        *** Randomly spawns turtles or bones using how much time has passed since the last spawn and random chance.
        *** Method Inputs:
        ***     gameTime - XNA timing variable.
        ***     random - A generated random from Game1.
        ***     newPlayerPosition - The position of the player.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/12/2015
        ******************************************************
        */
        public void Update(GameTime gameTime, Random random, Vector2 newPlayerPosition) //when to spawn/despawn
        {
            // Randomly generate randY for spawn location.
            randY = random.Next(375, 568);

            // Initialize a Vector2 variable based on newPlayerPosition.
            Vector2 playerPosition = newPlayerPosition;

            // If the random number generated is greater than 50, set isBone to false.
            // This means that a turtle will be spawned. Otherwise, isBone is set to true
            // and a bone is spawned.
            if (random.Next(100) >= 50)
                isBone = false;
            else
                isBone = true;

            // Set spawn based on the gameTime time.
            spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update each ScoringObject in the list of scoringObjects.
            foreach (ScoringObject s in scoringObjects)
                s.Update(gameTime, playerPosition);

            // If else if statment used to check if a turtle
            // or a bone has been spawned after every second.
            if (spawn >= 1 && !isBone)
            {
                // Set spawn to 0.
                spawn = 0;

                // If the number of turtles is less than 8, then add
                // a new turtle to the list of scoringObjects.
                if (scoringObjects.Count() < 8)
                    scoringObjects.Add(new ScoringObject(turtleTexture, new Vector2(900, randY), 14, 32, random, isBone));
            }
            else if(spawn >= 1 && isBone)
            {
                // Set spawn to 0.
                spawn = 0;

                // If the number of bones is less than 9, then add
                // a new bone to the list of ScoringObjects.
                if (scoringObjects.Count() < 8)
                    scoringObjects.Add(new ScoringObject(boneTexture, new Vector2(900, randY), 32, 32, random, isBone));
            }

            // For statmement used to determine if a ScoringObject needs 
            // to be removed. If it is not visible, it should be
            // removed from the list of scoringObjects.
            for (int i = 0; i < scoringObjects.Count; i++)
                if (!scoringObjects[i].isVisable)
                {
                    scoringObjects.RemoveAt(i);
                    i--;
                }
        }
    }
}
