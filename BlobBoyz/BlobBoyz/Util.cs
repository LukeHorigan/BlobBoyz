//Import statements
using System.Drawing;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using Accord.Video.FFMPEG;
using System.Collections.Generic;
using Accord.Imaging;

namespace BlobbyBoyz
{
    /*
     * Authors: David Chen and Aaron Gdanski
     * Purpose: General utility functions for the blobbyboyz program.
     */

    class Utils
    {
        /*
         * Purpose: Takes the Textfile at string filePath and processes it.
         * Parameters: string filePath - the filepath of the text file to process.
         * Returns: An array of Pairs.
         */
        public static Pair[] toInt(string filePath)
        {
            string[] strings = System.IO.File.ReadAllLines(filePath);
            Pair[] pairs = new Pair[strings.Length - 1];
            int index = 0;
            for (int i = 1; i < strings.Length; i++)
            {
                string[] splitString = strings[i].Split(' ');
                pairs[index] = new Pair(int.Parse(splitString[0]), int.Parse(splitString[1]));
                index++;
            }
            return pairs;
        }
        /*
         * Purpose: Executes a python file to produce text output file.
         * Parameters: None
         * Returns: None
         */
        public static void doPython()
        {
            ScriptEngine engine = Python.CreateEngine();
            engine.ExecuteFile(@"test.py");
            // this will produce myfile.txt file with integers
        }

        /*
         * Purpose: Takes a bitmap and converts it into a matrix of Color objects that represent each pixel.
         * Parameters: Bitmap bm - a bitmap of the image to convert.
         * Returns: a matrix of color values that represent the image's individual pixels.
         */
        public static Color[,] getMatrixFromBitMap(Bitmap bm)
        {
            Color[,] colors = new Color[bm.Width, bm.Height];
            for (int width = 0; width < bm.Width; width++)
            {
                for (int height = 0; height < bm.Height; height++)
                {
                    colors[width, height] = bm.GetPixel(width, height);
                }
            }
            return colors;
        }

        /*
         * Purpose: Takes a video file in .avi format and returns a list of a bitmap for each frame.
         * Parameters: string filePath - path to the .avi file
         * Return: a list of bitmaps for every frame.
         */
        public static List<Bitmap> GetAllFrames(string filePath)
        {
            string[] fileArray = filePath.Split('\\');
            string str = fileArray[fileArray.Length - 1];
            if (!str.Split('.')[1].Equals("avi"))
            {
                throw new NullReferenceException("Video File is wrong format. Use .avi");
            }
            VideoFileReader vfr = new VideoFileReader();
            vfr.Open(filePath);
            List<Bitmap> bitmapList = new List<Bitmap>();
            for (int i = 0; i < vfr.FrameCount; i++)
            {
                Bitmap bm = vfr.ReadVideoFrame();
                if (bm is null)
                {
                    break;
                }
                bitmapList.Add(bm);
                bm.Dispose();
            }
            vfr.Close();
            return bitmapList;
        }

        /*
         * Purpose: Saves all of the bitmaps in a list.
         * Parameters: List<Bitmap> bitmaps - list of bitmaps to be saved, string path - path of directory to save to.
         * Return: boolean; if failed false, if completed true.
         */

        public static bool saveBitMaps(List<Bitmap> bitmaps, string path)
        {
            try
            {
                foreach (Bitmap bm in bitmaps)
                {
                    bm.Save(path);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /*
         * Purpose: takes a blob array and saves it into specified folder.
         * Parameters: blobs - an array of blobs to save, savePath - path of directory to save to.
         * Return: boolean, true if complete, false if error.
         */

        public static bool saveBlobs(Blob[] blobs, string savePath)
        {
            try
            {
                foreach (Blob b in blobs)
                {
                    b.Image.ToManagedImage().Save(savePath);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }

    /*
     * Author: Aaron Gdanski
     * Purpose: Used for convienient storage of integer pairs.
     */
    class Pair
    {
        //Fields
        private int fileId, boolean;

        /*
         * Purpose: Constructor
         * Parameters: int fileId- the file identification number, int boolean - boolean value as an integer, either 0 or 1.
         */

        public Pair(int fileId, int boolean)
        {
            this.fileId = fileId;
            this.boolean = boolean;
        }

        /*
         * Purpose: Getter method for fileId.
         * Parameters: None
         * Returns: integer value fileId.
         */
        public int getFileId()
        {
            return fileId;
        }

        /*
         * Purpose: Getter method for integer boolean.
         * Returns: either 0 or 1 only, the integer representation of a boolean.
         */

        public int getBoolean()
        {
            return boolean;
        }
    }
}
