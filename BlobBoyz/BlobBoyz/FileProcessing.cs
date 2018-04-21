using System.Collections.Generic;
using Accord.Video.FFMPEG;
using System.Drawing;
using System;

namespace ConsoleApp4
{
    class Program
    {
        public static void Main(string[] args)
        {
            string path = "C:\\Users\\Saint\\Downloads\\tet\\hi.mpg";
            List<Color[,]> list = GetAllFrames(path);
            Console.WriteLine("Hello");
            Console.WriteLine(list.Count);
            for(int x = 0; x < list.Count; x++)
            {
                Console.WriteLine("test");
                Console.WriteLine(list[x].ToString());
            }
            Console.ReadLine();
        }
        public static List<Color[,]> GetAllFrames(string filePath)
        {
            VideoFileReader vfr = new VideoFileReader();
            Console.Write(vfr.ToString());
            vfr.Open(filePath);
            List<Color[,]> colorList = new List<Color[,]>();
            for(int i = 0; i < vfr.FrameCount; i++)
            {
                Bitmap bm = vfr.ReadVideoFrame();
                Console.WriteLine(bm.ToString());
                Color[,] rgbs = new Color[bm.Height, bm.Width];
                for(int x = 0; x < bm.Height; x++)
                {
                    for(int y = 0; y < bm.Width; y++)
                    {
                        rgbs[x, y] = bm.GetPixel(x, y);
                    }
                }
                colorList.Add(rgbs);
                
            }
            return colorList;
        }
    }
