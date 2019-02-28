using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoadApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Bitmap m_Screen;

        private Func<int, int> HInteruptCallback;

        private Func<int, Tuple<int, int>> HintVAndHCallback;

        private Action UpdateCallback;
        private int m_WaitTime = 250;
        private System.Drawing.Image m_Layer;

        public MainWindow()
        {
            InitializeComponent();
            m_Screen = new Bitmap(320, 224);

            //SimpleDraw draw = new SimpleDraw(ref HInteruptCallback, ref UpdateCallback);

            new Road5(ref HintVAndHCallback, ref UpdateCallback);


            m_Layer = System.Drawing.Bitmap.FromFile("full_road.bmp");

            Task.Run(() => Thread());

            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;
        }




        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        public void Thread()
        {
            while (true)
            {
                UpdateCallback?.Invoke();

                
                using (Graphics graphics = Graphics.FromImage(m_Screen))
                {

                    var iA = new ImageAttributes();
                    iA.SetWrapMode(System.Drawing.Drawing2D.WrapMode.Tile);

                    graphics.DrawImage(m_Layer, new RectangleF(0, 0, 320, 112), new RectangleF(0, 256, 320, 112), GraphicsUnit.Pixel);

                    //graphics.FillRectangle(System.Drawing.Brushes.Magenta, 0, 0, 320, 224);

                    int hline = 0;
                    int color = -2;
                    for (int i = 0; i < 224; i++)
                    {
                        int result = -1;
                        if (HInteruptCallback != null)
                        {
                            result = HInteruptCallback(i);
                        }

                        if (HintVAndHCallback != null)
                        {
                            var r = HintVAndHCallback(i);
                            result = r.Item1;
                            hline = r.Item2;
                        }


                        if (result >= 0)
                        {
                            color = result;
                        }

                        if (color >= 0)
                        {
                            
                            if (color == 0)
                            {
                                graphics.DrawImage(m_Layer, new System.Drawing.Rectangle(0, i, 320, 1), hline + 128, i + 16, 320, 1, GraphicsUnit.Pixel, iA);
                                //graphics.DrawImage(m_Layer, new RectangleF(0, i, 320, 1), new RectangleF(hline, i + 16, 320, 1), GraphicsUnit.Pixel);

                                //graphics.FillRectangle(System.Drawing.Brushes.Red, 0, i, 320, 1);
                                    //(System.Drawing.Brushes.Red, 0, red, 200, 10);
                            }
                            else
                            {
                                graphics.DrawImage(m_Layer, new System.Drawing.Rectangle(0, i, 320, 1), hline + 128, i - 112, 320, 1, GraphicsUnit.Pixel, iA);
                                //graphics.DrawImage(m_Layer, new RectangleF(0, i, 320, 1), new RectangleF(hline, i - 112, 320, 1), GraphicsUnit.Pixel);
                                //graphics.FillRectangle(System.Drawing.Brushes.Black, 0, i, 320, 1);
                            }                            
                        }
                    }


                    //graphics.FillPolygon(System.Drawing.Brushes.Green, new PointF[] {new PointF(0,112), new PointF(130,112), new PointF(0,224)});
                    //graphics.FillPolygon(System.Drawing.Brushes.Green, new PointF[] { new PointF(320, 112), new PointF(190, 112), new PointF(320, 224) });
                }
                
                MD.Dispatcher.Invoke(() =>
                {
                    MD.Source = BitmapToImageSource(m_Screen);
                });
                // 10 FPS
                System.Threading.Thread.Sleep(m_WaitTime);
            }
        }

        BitmapSource MakeTransparent(BitmapSource img, System.Windows.Media.Color maskColor)
        {
            var format = PixelFormats.Pbgra32;
            var stride = ((img.PixelWidth * format.BitsPerPixel + 31) & ~31) >> 3;
            var pixels = new byte[img.PixelHeight * stride];

            img.CopyPixels(pixels, stride, 0);

            for (var i = 0; i < stride * img.PixelHeight; i += 4)
                if (System.Windows.Media.Color.FromRgb(pixels[i + 2], pixels[i + 1], pixels[i]) == maskColor)
                    for (var j = i; j < i + 4; j++)
                        pixels[j] = 0;

            return BitmapSource.Create(
                img.PixelWidth, img.PixelHeight,
                img.DpiX, img.DpiY,
                format,
                null,
                pixels,
                stride);
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RefreshRate.SelectedValue != null)
            {
                float value = float.Parse(((ComboBoxItem)RefreshRate.SelectedValue).Content.ToString());

                if (value == 0)
                {
                    m_WaitTime = int.MaxValue;
                }
                else
                {
                    m_WaitTime = (int)((1 / value) * 1000.0f);
                }
            }
        }
    }
}
