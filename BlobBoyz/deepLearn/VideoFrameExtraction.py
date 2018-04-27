import cv2
import os

def video2frames(inputPath=None, outPath=None):
    '''
    Converts video in mp4 format to JPEG frames
    :param inputPath A path to your input. Absolute path recommended.
    :param outPath A path to a directory where you'd like the JPEG files to be saved
    :return: count, number of frames; images generated within specified outPath
    :reference: fireant-Oct15
    '''

    ## Checkpoints:
    assert(inputPath is not None and outPath is not None);
    assert(os.path.exists(inputPath)); 
    assert(os.path.isdir(outPath)); 


    ## Initialization steps:
    count = 0;
    success = True; 
    vidcap = cv2.VideoCapture(inputPath);
    success, image = vidcap.read();
    
    ## Iterative conversion step
    while success:
        cv2.imwrite(outPath+"frame%d.jpg" %count, image);
        success, image = vidcap.read();
        if success:
            print('Reading frame number '+str(count)); 
        count += 1;
    
    ## Message to console:
    print("Total number of frames: " + str(count)); 
    print("Mission complete!");

    return count;