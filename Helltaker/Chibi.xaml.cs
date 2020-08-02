using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Forms;

namespace Helltaker
{
    public partial class Chibi : Window
    {
        Bitmap original;
        Bitmap[] frames = new Bitmap[12];
        ImageSource[] imgFrame = new ImageSource[12];

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
        public Chibi(string CharName)
        {
            InitializeComponent();
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            original = System.Drawing.Image.FromFile(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Resources/Chibi/" + CharName + ".png") as Bitmap;
            
            for(int i=0; i<12; i++)
            {
                frames[i] = new Bitmap(100,100);
                using(Graphics g = Graphics.FromImage(frames[i]))
                {
                    g.DrawImage(original, new System.Drawing.Rectangle(0, 0, 100, 100),
                        new System.Drawing.Rectangle(i * 100, 0, 100, 100),
                        GraphicsUnit.Pixel);
                }
                var handle = frames[i].GetHbitmap();
                try
                {
                    imgFrame[i] = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(handle);
                }
            }
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += NextFrame;
            timer.Start();
        }

        private void NextFrame(object sender, EventArgs e)
        {
            Sticker.Source = imgFrame[MainWindow.syncro];
        }

        private void DragForm(object sender, MouseButtonEventArgs e) { this.DragMove(); e.Handled = true; }
        private void ExitForm(object sender, MouseButtonEventArgs e) { Chibi.GetWindow(this).Close(); }
    }
}
