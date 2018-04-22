# BlobBoys

An automated visual algorithm for inspecting holes in pipes

**Authors**

Kevin Barkevich, Lucas Horigan, David Chen, Kevin Dickey, Aaron Gdanski, and Ryan 

# Overview

Pipes have to go through visual testing with a human. The main objective of this program was to automate this process with CNN (Convolutional Neural Network) Machine Learning. Automating this process could reduce labor costs as well as increase the accuracy with mistakes being caught at inspection. 

# How we built it
Under the assumption that a costumer has a camera set up, we created a system that is able to store each frame of video into bitmaps (A data type that makes up pixels on a screen). From these bitmaps we used python to convert the bitmap information to JPEG. We then broke these JPEGs up into three matrices of RGB values. These three matrices were then put into a 'flattened matrix' where all RGB values were listed as one matrix (In this array each R G B value is ONE pixel). Tensorflow was then applied to this matrix to give predictions for holes in the pipe. A list of any frame of video with a blob (pipe hole) was then sent back to C# as bitmaps. The .NET's FastCornersAccord was then utilized to determine if these blobs were bad enough to be deemed as a rejected pipe. 

# Challenge we faced

Using an API to talk between C# and Python had to be overcome by using a text file to do a handshake. Additionally Iron Python (An API program) had to be used for running Python from within the C# language. 

# Where we go next
Using other machine learning methods to increase accuracy. Currently the best accuracy we can achieve with one method is 90%. By adding in other machine learning methods such as YOLO (You Only Look Once) we could increase our accuracy beyond this threshold. 

Secondly a user interface could be added to tweak the amount of frames that are recorded, super impose markers on a live image that show where our program is detecting corners, and being able to save JPEGs to a file directory of any errors within the pipe. 
