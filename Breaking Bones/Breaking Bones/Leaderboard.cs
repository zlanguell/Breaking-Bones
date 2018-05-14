/*
‘******************************************************
‘***  Leaderboard
‘***  Zach Languell and Kevin Anderson
‘******************************************************
‘*** The class was created to manage the leaderboard results, organize the scores, and save them
‘*** on a local file.
‘******************************************************
‘*** 11/24/2015
‘******************************************************
‘*** 11/24/2015
 *      Created Class
 *      Added writing to file and retrieval from file
 *      Display score
‘*******************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Breaking_Bones
{

    class Leaderboard
    {
        
        public int score;
        public int[] scores = new int[11];
        private String filePath = (@"Content\x1b5");
        TextBox name1 = new TextBox();
        StreamReader reader;
        StreamWriter writer;
        SpriteFont font;

        /*
            ‘******************************************************
            ‘***  Leaderboard
            ‘***  Zach Languell and Kevin Anderson
            ‘******************************************************
            ‘*** Required constructor for Leaderboard
            ‘*** Method Inputs:
            ‘***    integer score - number greater than 0
         *          SpriteFont newFont - font file for using fonts in XNA
            ‘*** Return value:
            ‘***    NA
            ‘******************************************************
            ‘*** 11/24/2015
            ‘******************************************************
         */

        public Leaderboard(int score, SpriteFont newFont)
        {
            this.score = score;
            readScores();
            updateScores(score);
            writeScores();
            font = newFont;
        }

        /*
           ‘******************************************************
           ‘***  Draw
           ‘***  Zach Languell
           ‘******************************************************
           ‘*** Draws the leaderboard scores on the leaderboard menu
           ‘*** Method Inputs:
           ‘***    spriteBatch - required by XNA to draw an image
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/24/2015
           ‘******************************************************
        */

        public void Draw(SpriteBatch spriteBatch)
        {
            int changeY = 160;
            for(int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(font, scores[i].ToString(), new Vector2(400, changeY+=30), Color.Black);
            }
            
        }

        /*
           ‘******************************************************
           ‘***  readScores
           ‘***  Zach Languell
           ‘******************************************************
           ‘*** Retrieves the scores from a file and inserts in an array.
           ‘*** Method Inputs:
           ‘***    NA
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/24/2015
           ‘******************************************************
        */
        public void readScores()
        {
            try 
            { 
                reader = new StreamReader(filePath);
                for (int i = 0; i < 10; i++)
                    scores[i] = Convert.ToInt32(reader.ReadLine());
                reader.Close();
            }
            catch(Exception e)
            {
                for (int i = 0; i < 10; i++)
                    scores[i] = i;
                writeScores();
            }
            
        }

        /*
           ‘******************************************************
           ‘***  updateScores
           ‘***  Kevin Anderson
           ‘******************************************************
           ‘*** Adds the score just earned by a player and then sorts the array in decending order
           ‘*** Method Inputs:
           ‘***    integer score - number greater than 0
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/24/2015
           ‘******************************************************
        */

        public void updateScores(int score)
        {
            scores[10] = score;
            Array.Sort(scores);
            Array.Reverse(scores);
        }

        /*
           ‘******************************************************
           ‘***  writeScores
           ‘***  Kevin Anderson
           ‘******************************************************
           ‘*** Writes the scoring in the array to a file
           ‘*** Method Inputs:
           ‘***    NA
           ‘*** Return value:
           ‘***    NA
           ‘******************************************************
           ‘*** 11/24/2015
           ‘******************************************************
        */

        public void writeScores()
        {
            try
            {
                writer = new StreamWriter(filePath);
                for (int i = 0; i < 10; i++)
                    writer.WriteLine(scores[i]);
                writer.Close();    
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
                    
        }
    }   
}
