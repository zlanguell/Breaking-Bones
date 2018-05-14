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
    *** EnemySpawner
    *** Kevin Anderson, Blake Young
    ******************************************************
    *** This class was made to spawn enemies in a random fashion.
    *** This class contains an Update() method.
    ******************************************************
    *** 11/19/2015
    ******************************************************
    *** 11/19/2015
    *** - Class created.
    *** - Added constructor and Update() method.
    *** 11/20/2015
    *** - Modification of if statements.
    *** 12/01/2015
    *** - Finalized comments.
    ******************************************************
    */
    class EnemySpawner
    {
        // Declare class variables.
        private int randY;
        public List<EnemyObject> enemyObjects = new List<EnemyObject>();
        private Texture2D eagleTexture;
        private float spawn = 0;     
        public bool isTree = false;
        private Texture2D treeTexture;

        /*
        ******************************************************
        *** EnemySpawner
        *** Blake Young
        ******************************************************
        *** Required constructor for EnemySpawner.
        *** Method Inputs:
        ***     eagleTexture - Texture for eagle.
        ***     treeTexture - Texture for tree.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/19/2015
        ******************************************************
        */
        public EnemySpawner(Texture2D eagleTexture, Texture2D treeTexture)
        {
            this.eagleTexture = eagleTexture;
            this.treeTexture = treeTexture;
        }

        /*
        ******************************************************
        *** Update
        *** Kevin Anderson
        ******************************************************
        *** Randomly spawns eagles or trees using how much time has passed since the last spawn and random chance.
        *** Determines when to spwan or despawn an eagle or tree.
        *** Method Inputs:
        ***     gameTime - XNA timing variable.
        ***     random - A generated random from Game1
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/19/2015
        ******************************************************
        */
        public void Update(GameTime gameTime, Random random)
        {
            // Randomly generate randY for spawn location.
            randY = random.Next(0, 285);

            // If the random number generated is greater than 50, set isTree to false.
            // This means that an eagle will be spawned. Otherwise, isTree is set to true
            // and a tree is spawned.
            if (random.Next(100) >= 40)
                isTree = false;
            else
                isTree = true;

            // Set spawn based on the gameTime time.
            spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update each EnemyObject in the list of enemyObjects.
            foreach (EnemyObject s in enemyObjects)
                s.Update(gameTime);

            // If else if statment used to check if an eagle
            // or a tree has been spawned after 2 seconds.
            if (spawn >= 2 && !isTree)
            {
                // Set spawn to 0.
                spawn = 0;

                // If the number of eagles is less than 4, then add
                // a new eagle to the list of enemyObjects.
                if (enemyObjects.Count() < 4)
                    enemyObjects.Add(new EnemyObject(eagleTexture, new Vector2(900, randY), 32, 32, random, isTree));
            }
            else if (spawn >= 2 && isTree)
            {
                // Set spawn to 0.
                spawn = 0;

                // If the number of trees is less than 4, then add
                // a new eagle to the list of enemyObjects.
                if (enemyObjects.Count() < 4)
                    enemyObjects.Add(new EnemyObject(treeTexture, new Vector2(900, 300), 128, 81, random, isTree));
            }

            // For statmement used to determine if an EnemyObject needs 
            // to be removed. If it is not visible, it should be
            // removed from the list of enemyObjects.
            for (int i = 0; i < enemyObjects.Count; i++)
                if (!enemyObjects[i].isVisable)
                {
                    enemyObjects.RemoveAt(i);
                    i--;
                }
        }
    }
}
