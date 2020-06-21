using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HelltakerWidjet
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap original;
        Bitmap[] frames = new Bitmap[12];
        ImageSource[] imgFrame = new ImageSource[12];
        string bitmapPath = "Resources/cerberus.png";
        int frame = -1;

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public MainWindow()
        {
            InitializeComponent();

            original = System.Drawing.Image.FromFile(bitmapPath) as Bitmap;
            for (int i = 0; i < 12; i++)
            {
                frames[i] = new Bitmap(100, 100);
                using (Graphics g = Graphics.FromImage(frames[i]))
                {
                    g.DrawImage(original,
                        new System.Drawing.Rectangle(0, 0, 100, 100),
                        new System.Drawing.Rectangle(i * 100, 0, 100, 100),
                        GraphicsUnit.Pixel);
                }
                var handle = frames[i].GetHbitmap();
                try
                {
                    imgFrame[i] = Imaging.CreateBitmapSourceFromHBitmap(handle,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                } finally
                {
                    DeleteObject(handle);
                }
            }
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.0167 * 2);
            timer.Tick += NextFrame;
            timer.Start();

            MouseDown += MainWindow_MouseDown;

            var menu = new System.Windows.Forms.ContextMenu();
            var noti = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon()),
                Visible = true,
                Text = "헬-테이커",
                ContextMenu = menu
            };
            var item = new System.Windows.Forms.MenuItem
            {
                Index = 0,
                Text = "바알제붑 부르기"
            };
            item.Click += (object o, EventArgs E) =>
            {
                Application.Current.Shutdown();
            };

            menu.MenuItems.Add(item);
            noti.ContextMenu = menu;
        }

        private void NextFrame(object sender, EventArgs e)
        {
            frame = (frame + 1) % 12;
            WidjetChar.Source = imgFrame[frame];
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }
    }
}
