using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;    
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace FaceDetection
{
    class CaptureWebCam
    {
        System.Windows.Controls.Image image1;
        Capture capture;
        DispatcherTimer timer;
        public Image<Bgr, Byte> ImageFrame;

        public CaptureWebCam(System.Windows.Controls.Image im) 
        {
            image1 = im;
            capture = new Capture();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0,0,0,0,33);
            timer.Tick += new EventHandler(ProcessFrame);
            timer.Start();
        }


        private void ProcessFrame(object sender, EventArgs arg)
        {
            ImageFrame=capture.QueryFrame();  //line 1
            image1.Source =BitmapToImageSource( ImageFrame.ToBitmap());
        }

        public static Image<Bgr, Byte> OpenImageFromFIle()
        {

            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

              openFileDialog1.InitialDirectory = @"E:\Bases de datos\FDDB\2002\07\19\big" ;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Image<Bgr, Byte> data = new Image<Bgr, Byte>(openFileDialog1.FileName);

                    return data;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            return null;

        }


        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
    
    

}
