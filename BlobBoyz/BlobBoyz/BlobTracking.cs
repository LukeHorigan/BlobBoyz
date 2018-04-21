using System;
using Accord;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class BlobTracking
    {

        public static bool IsBlobFrameDup(/* Blob to be tested*/Accord.Imaging.Blob inBlob, /*Array of existing blobs*/Accord.Imaging.Blob[] lastBlobs, /*Number of pixels moved per frame*/int currSpeed)
        {
            // Initialize blob we can work with
            Accord.Imaging.Blob myBlob = new Accord.Imaging.Blob(inBlob);

            int totExistingBlobs = lastBlobs.Length;




            return false;
        }

    }
}
