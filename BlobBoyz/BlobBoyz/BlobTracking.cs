/*
 Author:
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



namespace BlobBoyz
{

    class BlobTracking
    {
        const int CONSIDERED_AT_TOP_RANGE = 50;

        /*
         Pre:
         Post: Whether or not the blob is a duplicate of a previously found blob is returned as a bool (true: duplicate, false: not duplicate) and if it is the data in the list is updated
         Purpose: Takes in a blob as a parameter and sifts through the list of previously located "active" blobs, checking to see if it is the same blob in an updated position (within
                    inputted margins of errors for size and position)
                    Additionally, optionally, if the blob is at the top of the frame, it is removed from the array.
         */
        public static bool IsBlobFrameDup(/* Blob to be tested*/Accord.Imaging.Blob inBlob, /*Array of existing blobs*/Accord.Imaging.Blob[] lastBlobs,
            /*Number of horizontal pixels moved per frame*/int currXPPF, /*Number of vertical pixels moved per frame*/int currYPPF, /*Margin of error for size in pixels*/int errorMarginSize,
            /*Margin of error for position in pixels*/int errorMarginPos, /*Whether or not to update the lastBlobs list*/bool updateLastBlobs)
        {
            // Initialize blob we can work with
            Accord.Imaging.Blob myBlob = new Accord.Imaging.Blob(inBlob);

            int totExistingBlobs = lastBlobs.Length;
            Accord.Imaging.Blob currBlob;
            bool hasBeenFound = false;

            int inc = 0;
            while (inc < totExistingBlobs && !hasBeenFound)
            {
                currBlob = lastBlobs[inc];

                // ------------------------------------ THE GREAT TEST --------------------------------------------------
                if (((currBlob.Rectangle.X - errorMarginPos) < inBlob.Rectangle.X - currXPPF && (currBlob.Rectangle.X + errorMarginPos) > inBlob.Rectangle.X - currXPPF) &&
                     ((currBlob.Rectangle.Y - errorMarginPos) < inBlob.Rectangle.Y - currYPPF && (currBlob.Rectangle.Y + errorMarginPos) > inBlob.Rectangle.Y - currYPPF) &&
                     ((currBlob.Rectangle.Height - errorMarginSize) < inBlob.Rectangle.Height && (currBlob.Rectangle.Height + errorMarginSize) > inBlob.Rectangle.Height) &&
                     ((currBlob.Rectangle.Width - errorMarginSize) < inBlob.Rectangle.Width && (currBlob.Rectangle.Width + errorMarginSize) > inBlob.Rectangle.Width))
                {
                    // IT IS IN THE PREVIOUS FRAME
                    hasBeenFound = true;

                    if (updateLastBlobs) // If editing lastBlobs is enabled
                    {
                        if (inBlob.Rectangle.Y > CONSIDERED_AT_TOP_RANGE) // It's not at the top, update it
                        {
                            lastBlobs[inc] = new Accord.Imaging.Blob(inBlob);
                        }
                        else // It is at the top, delete it from lastBlobs
                        {
                            for (int i = inc; i < totExistingBlobs; i++) // Starting at the one to be deleted, every blob = the one above it
                            {
                                lastBlobs[i] = new Accord.Imaging.Blob(lastBlobs[i + 1]);

                            }
                            Accord.Imaging.Blob[] newLastBlobs = new Accord.Imaging.Blob[BlobBoyz.Program.MAX_BLOBS_PER_FRAME];
                            for (int i = 0; i < totExistingBlobs - 1; i++)
                            {
                                newLastBlobs[i] = new Accord.Imaging.Blob(lastBlobs[i]);
                            }
                            lastBlobs = newLastBlobs;
                        }
                    }

                }

                inc++;
            }

            if (!hasBeenFound)
            {
                // IT IS NOT IN THE PREVIOUS FRAME
                if (updateLastBlobs) // If editing lastBlobs is enabled
                {
                    lastBlobs[totExistingBlobs] = new Accord.Imaging.Blob(inBlob);
                }
            }

            return hasBeenFound;
        }

        public static bool isBlobPresentInNext(/*Blob to be tested*/Accord.Imaging.Blob inBlob, /*2D list of next blobs used to determine if it continues to exist [frame][blobs]*/ Accord.Imaging.Blob[] nextBlobs,
            /*Number of horizontal pixels moved per frame*/int currXPPF, /*Number of vertical pixels moved per frame*/int currYPPF, int errorMarginSize,
            /*Margin of error for position in pixels*/int errorMarginPos)
        {
            bool result = false;
            if (IsBlobFrameDup(inBlob, nextBlobs, -currXPPF, -currYPPF, errorMarginSize, errorMarginPos, false))
            {
                // If it exists in the next one
                result = true;
            }

            return result;
        }

        public static Blob[] getBlobs(Bitmap inImage, int threshhold)
        {

            FastCornersDetector fast = new FastCornersDetector()
            {
                Suppress = true, // suppress non-maximum points
                Threshold = threshhold   // less leads to more corners
            };

            List<IntPoint> points = fast.ProcessImage(inImage);

            Blob[] totBlobs = new Blob[BlobBoyz.Program.MAX_BLOBS_PER_FRAME];

            int pointsLeft = 0;
            int myInc = 0;

            foreach (var p in points)
            {
                pointsLeft++;
                myInc++;
            }
            IntPoint[] allPoints = new IntPoint[pointsLeft]; // This mess gets all of the points into an array
            foreach (var p in points)
            {
                allPoints[myInc - 1] = p;
                myInc--;
            }

            bool[] isUsed = new bool[pointsLeft];

            for (int i = 0; i < pointsLeft; i++)
            {
                isUsed[i] = false;
            }

            var windowsPoints = new List<System.Drawing.Point>();
            foreach(IntPoint p in points)
            {
                System.Drawing.Point currPoint = new System.Drawing.Point(p.X, p.Y);
                windowsPoints.Add(currPoint);
            }

            while (pointsLeft > 0)
            {
                var result = new HashSet<System.Drawing.Point>();
                var found = new Queue<System.Drawing.Point>();

                windowsPoints.Reverse();
                found.Enqueue(windowsPoints[pointsLeft - 1]);
                windowsPoints.Reverse();
                windowsPoints.RemoveAt(0);

                while (found.Count > 0)
                {
                    var current = found.Dequeue();
                    var candidates = windowsPoints
                        .Where(p => !result.Contains(p) &&
                               Math.Sqrt(Math.Pow((current.X - p.X), 2) + Math.Pow((current.Y - p.Y), 2)) <= threshhold);

                    foreach (var p in candidates)
                    {
                        result.Add(p);
                        found.Enqueue(p);
                    }
                }

                foreach ()
            }


            return totBlobs;
        }

        private static bool rGetBlobs(Blob[] listBlobs, int threshhold)
        {

            

            return true;
        }


    }
}
