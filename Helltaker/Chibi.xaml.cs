using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Helltaker {
    public partial class Chibi : Window {

        //이미지처리 관련
        Bitmap original;
        Bitmap[] frames = new Bitmap[12];
        ImageSource[] imgFrame = new ImageSource[12];
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public Chibi(string CharName) {
            InitializeComponent();

            // 이미지 가져오기
            original = System.Drawing.Image.FromFile(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Resources/Chibi/" + CharName + ".png") as Bitmap;

            // 이미지 12등분으로 자르기.
            for (int i = 0; i < 12; i++) {
                frames[i] = new Bitmap(100, 100);
                using (Graphics g = Graphics.FromImage(frames[i])) {
                    g.DrawImage(original, new System.Drawing.Rectangle(0, 0, 100, 100),
                        new System.Drawing.Rectangle(i * 100, 0, 100, 100),
                        GraphicsUnit.Pixel);
                }
                var handle = frames[i].GetHbitmap();
                try {
                    imgFrame[i] = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally {
                    DeleteObject(handle);
                }
            }

            // 스티커 움직임 갱신용 타이머 (주기가 짧을수록 싱크가 잘 맞음)
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += NextFrame;
            timer.Start();
        }

        private void NextFrame(object sender, EventArgs e) { Sticker.Source = imgFrame[MainWindow.syncro]; } // 프레임갱신
        private void DragForm(object sender, MouseButtonEventArgs e) { this.DragMove(); e.Handled = true; } // 폼 이동
        private void ExitForm(object sender, MouseButtonEventArgs e) { Chibi.GetWindow(this).Close(); } // 더블클릭시 폼 삭제
    }
}
