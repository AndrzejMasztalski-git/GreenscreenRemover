using Microsoft.Win32;
using RemoverCS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace JAProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport(@"C:\Users\Admin\Desktop\JAProjekt\x64\Debug\RemoverASM.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe static extern void MyProc1(byte[] pixel);
        BitmapImage bitmap = new BitmapImage();
        BitmapImage image1 = new BitmapImage();
        public MainWindow()
        {
            InitializeComponent();
            int processorCount = Environment.ProcessorCount;
            threadsBar.Value = processorCount;
        }

        private void LoadImage(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmap1 = new BitmapImage();
            OpenFileDialog openFileDialog = new OpenFileDialog(); //Klasa które odpowiada za okndo dialogowe, które wybiera plik
            openFileDialog.Filter = "Picture files (*.png)|*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true) //jesli plik zostanie wybrany to:
            {
                string selectedFileName = openFileDialog.FileName;

                bitmap1.BeginInit();
                bitmap1.UriSource = new Uri(selectedFileName);
                bitmap1.EndInit();
                addedPicture.Source = bitmap1;
                bitmap = bitmap1;
            }
        }

        

        private void Run(object sender, RoutedEventArgs e)
        {

            RunFunction();
            

        }

        private void RunFunction()
        {
            
            Stopwatch watch = new Stopwatch();
            int numberOfThreads = (int)(threadsBar.Value);
            Bitmap bit1 = BitmapImage2Bitmap(bitmap);
            if (csButton.IsChecked == true)
            {
                watch.Start();
                Class1 remover = new Class1();
                Bitmap bit2 = remover.RemoveGreenScreen(bit1, numberOfThreads);
                modifiedPicture.Source = ConvertBitmap(bit2);

                watch.Stop();
            }
            else
            {
                watch.Start();
                

                // Create a list to store the tasks
                List<Task> tasks = new List<Task>();
                int tasksCompleted = 0;
                int width = bit1.Width;
                int height = bit1.Height;
                object _lock = new object();
                 
                // Loop through each pixel in the image
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        // Create a new task to process the pixel
                        int _x = x;
                        int _y = y;
                        Task task = Task.Run(() =>
                        {
                            Color pixelColor;
                            lock (_lock)
                            {
                            pixelColor = bit1.GetPixel(_x, _y);
                            byte[] newTable = {pixelColor.R, pixelColor.G, pixelColor.B};
                            MyProc1(newTable);
                            bit1.SetPixel(_x, _y, Color.FromArgb(newTable[0], newTable[1], newTable[2]));                       
                            Interlocked.Increment(ref tasksCompleted);
                                }
                        });

                        // Add the task to the list
                        tasks.Add(task);
                    }
                }

                // Wait for all tasks to complete
                Task.WaitAll(tasks.ToArray());
                modifiedPicture.Source = ConvertBitmap(bit1);
                watch.Stop();
            }
            
            executionTime.Text = $"{watch.ElapsedMilliseconds} ms";
           
        }

        public BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            image1 = image;
            return image;
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        private void SaveImage(object sender, RoutedEventArgs e)
        {
            
            if (image1 == null) return;
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save picture as ";
            save.Filter = "Image File(*.png)|*.png";
            if (save.ShowDialog() == true)
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image1));
                using (var fileStream = new FileStream(save.FileName, FileMode.Create))
                    encoder.Save(fileStream);
            }
        }
    }
}
