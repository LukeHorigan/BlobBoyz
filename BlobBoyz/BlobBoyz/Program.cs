/*
 Authors: 
 Kevin Barkevich

 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using Accord;
using Accord.Imaging;
using Accord.Imaging.Filters;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace BlobBoyz
{

    class Program
    {
        public const int MAX_FRAME_ARR_SIZE = 60 * 60 * 10;
        public const int MAX_BLOBS_PER_FRAME = 255;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("Please input a file name.");
            string filePath = Console.ReadLine();
            Console.WriteLine("Input a dot frequency threshhold");
            int dotThresh = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Input a blob density threshhold");
            int blobThresh = Convert.ToInt32(Console.ReadLine());

            FileStream fileStream = File.Open(filePath, FileMode.Open);
            Bitmap pic = (Bitmap)Bitmap.FromStream(fileStream);

            Blob[] blobArray = new Blob[BlobBoyz.Program.MAX_BLOBS_PER_FRAME];
            blobArray = BlobTracking.getBlobs(pic, dotThresh, blobThresh);

        }
    }
}

