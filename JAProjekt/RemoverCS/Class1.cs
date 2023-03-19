using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoverCS
{
    public class Class1
    {

        public Bitmap RemoveGreenScreen(Bitmap sourceImage, int tasksCompleted)
        {
            // Create a new bitmap to store the processed image
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            // Create a list to store the tasks
            List<Task> tasks = new List<Task>();
            
            int width = sourceImage.Width;
            int height = sourceImage.Height;
            object _lock = new object();

        
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    
                    int _x = x;
                    int _y = y;
                    Task task = Task.Run(() =>
                    {
                        Color pixelColor;
                        lock (_lock)
                        {
                            pixelColor = sourceImage.GetPixel(_x, _y);
                        }

                        if (pixelColor.R < 150 && pixelColor.G > 100 && pixelColor.B < 100)
                        {
                            lock (_lock)
                            {
                                
                                resultImage.SetPixel(_x, _y, Color.Transparent);
                            }
                        }
                        else
                        {
                            lock (_lock)
                            {
                                resultImage.SetPixel(_x, _y, pixelColor);
                            }
                        }
                        Interlocked.Increment(ref tasksCompleted);
                    });

                   
                    tasks.Add(task);
                }
            }

            
            Task.WaitAll(tasks.ToArray());
            return resultImage;
        }


    }
}






//public Bitmap RemoveGreenScreen(Bitmap sourceImage)
//{

//    // Create a new bitmap to store the processed image
//    Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

//    // Loop through each pixel in the image
//    for (int x = 0; x < sourceImage.Width; x++)
//    {
//        for (int y = 0; y < sourceImage.Height; y++)
//        {
//            // Get the pixel's color
//            Color pixelColor = sourceImage.GetPixel(x, y);

//            if (pixelColor.R < 150 && pixelColor.G > 100 && pixelColor.B < 100)
//            {

//                resultImage.SetPixel(x, y, Color.Transparent);
//            }
//            else
//            {
//                resultImage.SetPixel(x, y, pixelColor);
//            }
//        }
//    }
//    return resultImage;
//}