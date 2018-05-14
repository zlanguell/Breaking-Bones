using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Breaking_Bones
{
    /*
    ******************************************************
    *** BackgroundMusicPlayer
    *** Kevin Anderson
    ******************************************************
    *** This class was written to improve the background audio in the game.
    ******************************************************
    *** 11/13/2015
    ******************************************************
    *** 11/13/2015
    *** - Created class.
    *** 12/01/2015
    *** - Finalized comments.
    *******************************************************
     * Inspired from MSDN code
     * https://msdn.microsoft.com/en-us/library/ff827591.aspx
    */
    class BackgroundMusicPlayer
    {
        // Declare variables needed.
        int position;
        int count;
        byte[] byteArray;
        int chunkID;
        int fileSize;
        int riffType;
        int fmtID;
        int fmtSize;
        int fmtCode;
        int channels;
        int sampleRate;
        int fmtAvgBPS;
        int fmtBlockAlign;
        int bitDepth;
        int fmtExtraSize;
        int dataID;
        int dataSize;
        System.IO.Stream waveFileStream;
        BinaryReader reader;
        DynamicSoundEffectInstance dynamicSound;

        /*
        ******************************************************
        *** BackgroundMusicPlayer
        *** Kevin Anderson
        ******************************************************
        *** Required constructor for BackgroundMusicPlayer.
        *** Method Inputs:
        ***     waveFileStream - Music file.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/13/2015
        ******************************************************
        */
        public BackgroundMusicPlayer(System.IO.Stream waveFileStream)
        {
            this.waveFileStream = waveFileStream;
            reader = new BinaryReader(this.waveFileStream);
            chunkID = reader.ReadInt32();
            fileSize = reader.ReadInt32();
            riffType = reader.ReadInt32();
            fmtID = reader.ReadInt32();
            fmtSize = reader.ReadInt32();
            fmtCode = reader.ReadInt16();
            channels = reader.ReadInt16();
            sampleRate = reader.ReadInt32();
            fmtAvgBPS = reader.ReadInt32();
            fmtBlockAlign = reader.ReadInt16();
            bitDepth = reader.ReadInt16();
            if (fmtSize == 18)
            {
                // Read any extra values.
                fmtExtraSize = reader.ReadInt16();
                reader.ReadBytes(fmtExtraSize);
            }
            dataID = reader.ReadInt32();
            dataSize = reader.ReadInt32();

            byteArray = reader.ReadBytes(dataSize);
            dynamicSound = new DynamicSoundEffectInstance(sampleRate, (AudioChannels)channels);
            count = dynamicSound.GetSampleSizeInBytes(TimeSpan.FromMilliseconds(100));
            dynamicSound.BufferNeeded += new EventHandler<EventArgs>(DynamicSound_BufferNeeded);
        }

        /*
        ******************************************************
        ***  DynamicSound_BufferNeeded
        ***  Kevin Anderson
        ******************************************************
        *** Required function for BackgroundMusicPlayer.
        *** Method Inputs:
        ***     sender - Object.
        ***     e - Event data.
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/13/2015
        ******************************************************
        */
        public void DynamicSound_BufferNeeded(object sender, EventArgs e)
        {
            dynamicSound.SubmitBuffer(byteArray, position, count / 2);
            dynamicSound.SubmitBuffer(byteArray, position + count / 2, count / 2);

            position += count;

            if (position + count > byteArray.Length)
                position = 0;
        }

        /*
        ******************************************************
        *** play
        *** Kevin Anderson
        ******************************************************
        *** Begins the music.
        *** Method Inputs:
        ***     NA
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/13/2015
        ******************************************************
        */
        public void play()
        {
            dynamicSound.Play();
        }

        /*
        ******************************************************
        *** Stop
        *** Kevin Anderson
        ******************************************************
        *** Ends the music.
        *** Method Inputs:
        ***     NA
        *** Return value:
        ***     NA
        ******************************************************
        *** 11/13/2015
        ******************************************************
        */
        public void stop()
        {
            dynamicSound.Stop();
        }
    }
}
