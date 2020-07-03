using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WMPLib;

namespace Helltaker1
{
    //To Edit
    //controlWindow에서 캐릭터 선택하기(보류)
    //우클릭하면 -ㅁX 나오게 하기
    //다운로드 매니저 구현

    public partial class MainWindow : Window
    {
        #region
        /*For Hiding From Alt + Tab*/
        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_DLGMODALFRAME = 0x0001;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_FRAMECHANGED = 0x0020;
        private const int WM_SETICON = 0x0080;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        /*Alt + Tab해서 안뜨는거 겁나 신기하당*/
        #endregion

        System.Windows.Forms.MenuItem Azazel, Cerberus, Glorious_left, Glorious_right, Judgement, Justice, Lucifer, Lucifer_Apron, Malina, Modeus, Pandemonica, Zdrada;
        System.Windows.Forms.MenuItem speed1, speed2, speed3, speed4, speed5;
        public System.Windows.Forms.NotifyIcon noti;

        public int frame_sheet = 24; //count of bitmap in sprite sheet
        int height = 100; //height of each frame image
        int width = 100; //width of each frame image

        Bitmap original;  //bitmap to show
        public Bitmap[] frames; //frame for animation

        //ContorlWindow control = new ContorlWindow();

        public ImageSource[] imgFrame; //frame for split sheet

        string bitmapPath = "Resources/Lucifer.png"; //file directory

        //public int frame; //frame for animated bitmap
        public float fps = 3 / 200f; //fps variable
        //public float speed; //frame speed

        private bool isShowGrip = false;


        /*timer for frame update*/
        DispatcherTimer timer;

        //string[] filePaths = Directory.GetFiles("Resources", "*.png"); //get file name


        /*for release bitmap*/
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);



        public MainWindow()
        {
            InitializeComponent();
            frames = new Bitmap[frame_sheet];
            imgFrame = new ImageSource[frame_sheet];



            //bitmapPath = "Resources/" + CharacterName + ".png";

            #region character

            Azazel = new System.Windows.Forms.MenuItem
            {
                Index = 10,
                Text = "Azazel",
            };
            Cerberus = new System.Windows.Forms.MenuItem
            {
                Index = 9,
                Text = "Cerberus",
            };
            Judgement = new System.Windows.Forms.MenuItem
            {
                Index = 8,
                Text = "Judgement",
            };
            Justice = new System.Windows.Forms.MenuItem
            {
                Index = 7,
                Text = "Justice",
            };
            Lucifer = new System.Windows.Forms.MenuItem
            {
                Index = 6,
                Text = "Lucifer",
            };
            Lucifer_Apron = new System.Windows.Forms.MenuItem
            {
                Text = "Lucifer_Apron",
            };
            Malina = new System.Windows.Forms.MenuItem
            {
                Index = 5,
                Text = "Malina",
            };
            Modeus = new System.Windows.Forms.MenuItem
            {
                Index = 4,
                Text = "Modeus",
            };
            Pandemonica = new System.Windows.Forms.MenuItem
            {
                Index = 3,
                Text = "Pandemonica",
            };
            Zdrada = new System.Windows.Forms.MenuItem
            {
                Index = 2,
                Text = "Zdrada",
            };
            Glorious_right = new System.Windows.Forms.MenuItem
            {
                Index = 1,
                Text = "Glorious_right",
            };
            Glorious_left = new System.Windows.Forms.MenuItem
            {
                Index = 0,
                Text = "Glorious_left",
            };
            #endregion

            #region Character Click Block         
            Azazel.Click += (object o, EventArgs e) => Select_Azazel();
            Cerberus.Click += (object o, EventArgs e) => Select_Cerberus();
            Judgement.Click += (object o, EventArgs e) => Select_Judgement();
            Justice.Click += (object o, EventArgs e) => Select_Justice();
            Lucifer.Click += (object o, EventArgs e) => Select_Lucifer();
            Lucifer_Apron.Click += (object o, EventArgs e) => Select_Lucifer_Apron();
            Malina.Click += (object o, EventArgs e) => Select_Malina();
            Modeus.Click += (object o, EventArgs e) => Select_Modeus();
            Pandemonica.Click += (object o, EventArgs e) => Select_Pandemonica();
            Zdrada.Click += (object o, EventArgs e) => Select_Zdrada();
            Glorious_left.Click += (object o, EventArgs e) => Select_Glorious_left();
            Glorious_right.Click += (object o, EventArgs e) => Select_Glorious_right();
            #endregion

            #region Speed Value
            speed1 = new System.Windows.Forms.MenuItem
            {
                Index = 0,
                Text = "1",
            };
            speed2 = new System.Windows.Forms.MenuItem
            {
                Index = 1,
                Text = "2",
            };
            speed3 = new System.Windows.Forms.MenuItem
            {
                Index = 2,
                Text = "3",
            };
            speed4 = new System.Windows.Forms.MenuItem
            {
                Index = 3,
                Text = "4",
            };
            speed5 = new System.Windows.Forms.MenuItem
            {
                Index = 4,
                Text = "5",
            };
            #endregion

            #region Speed Click Block
            //speed1.Click += (object o, EventArgs e) => Select_Speed1();
            //speed2.Click += (object o, EventArgs e) => Select_Speed2();
            //speed3.Click += (object o, EventArgs e) => Select_Speed3();
            //speed4.Click += (object o, EventArgs e) => Select_Speed4();
            //speed5.Click += (object o, EventArgs e) => Select_Speed5();
            #endregion


            Animation(bitmapPath); //play bitmap animation from directory


            /*frame timer*/
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(fps);
            timer.Tick += NextFrame;
            timer.Start();


            /*mouse event*/
            MouseDown += MainWindow_MouseDown; //drag move
            MouseDoubleClick += MainWindow_MouseDoubleClcik;
            MouseRightButtonDown += MainWindow_MouseRightClick; //right click event


            /*for notify icon*/
            var menu = new System.Windows.Forms.ContextMenu();
            noti = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon()),
                Visible = true,
                Text = "HellTaker_Sticker",
                ContextMenu = menu,
            };
            var close = new System.Windows.Forms.MenuItem
            {
                Index = 0,
                Text = "close",

            };
            var shutdown = new System.Windows.Forms.MenuItem
            {
                Text = "shutdown",
            };
            var overlay = new System.Windows.Forms.MenuItem
            {
                Index = 1,
                Text = "overlay",
            };
            var CharSelect = new System.Windows.Forms.MenuItem
            {
                Index = 3,
                Text = "characters",
            };
            //var SpeedControl = new System.Windows.Forms.MenuItem
            //{
            //    Index = 4,
            //    Text = "프레임 속도 선택",
            //};

            /*reset*/
            this.Topmost = true;
            overlay.Checked = true;
            Lucifer.Checked = true;
            speed3.Checked = true;


            /*close button*/
            close.Click += (object o, EventArgs e) =>
            {
                //Stop();
                WindowClose();
            };
            /*shutdonw button*/
            shutdown.Click += (object o, EventArgs e) =>
            {
                Shutdown();
            };
            /*overlay button*/
            overlay.Click += (object o, EventArgs e) =>
            {
                this.Topmost = !this.Topmost;
                overlay.Checked = !overlay.Checked;
            };



            /*add to menu*/
            menu.MenuItems.Add(CharSelect);
            //menu.MenuItems.Add(SpeedControl);
            menu.MenuItems.Add(overlay);
            menu.MenuItems.Add(close);
            //menu.MenuItems.Add(shutdown);

            #region Add Character in List
            CharSelect.MenuItems.Add(Azazel);
            CharSelect.MenuItems.Add(Cerberus);
            CharSelect.MenuItems.Add(Judgement);
            CharSelect.MenuItems.Add(Justice);
            CharSelect.MenuItems.Add(Lucifer);
            CharSelect.MenuItems.Add(Lucifer_Apron);
            CharSelect.MenuItems.Add(Malina);
            CharSelect.MenuItems.Add(Modeus);
            CharSelect.MenuItems.Add(Pandemonica);
            CharSelect.MenuItems.Add(Zdrada);
            CharSelect.MenuItems.Add(Glorious_left);
            CharSelect.MenuItems.Add(Glorious_right);
            #endregion

            #region Add Speed Controller
            //SpeedControl.MenuItems.Add(speed1);
            //SpeedControl.MenuItems.Add(speed2);
            //SpeedControl.MenuItems.Add(speed3);
            //SpeedControl.MenuItems.Add(speed4);
            //SpeedControl.MenuItems.Add(speed5);
            #endregion

            noti.ContextMenu = menu; //add menu to Contextmenu
        }

        private void MainWindow_MouseDoubleClcik(object sender, MouseButtonEventArgs e)
        {
            WindowClose();
        }

        /*Frame process*/
        private void NextFrame(object sender, EventArgs e)
        {
            ShowGrip();
        }

        /*Drag Function*/
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }
        /*Resize Function*/
        private void MainWindow_MouseRightClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right) isShowGrip = !isShowGrip;
        }

        private void WindowClose()
        {
            this.Close();
            noti.Dispose();
        }

        private void ShowGrip()
        {
            if (isShowGrip)
                this.ResizeMode = ResizeMode.CanResizeWithGrip;
            else
                this.ResizeMode = ResizeMode.NoResize;
        }


        public void Shutdown()
        {
            System.Windows.Application.Current.Shutdown();
        }

        /*split sprites from sheet*/
        private void Animation(string _path)
        {
            original = System.Drawing.Image.FromFile(_path) as Bitmap;
            for (int i = 0; i < frame_sheet; i++)
            {
                frames[i] = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(frames[i]))
                {
                    g.DrawImage(original, new System.Drawing.Rectangle(0, 0, width, height),
                        new System.Drawing.Rectangle(i * width, 0, width, height),
                        GraphicsUnit.Pixel);
                }
                var handle = frames[i].GetHbitmap();
                try
                {
                    imgFrame[i] = Imaging.CreateBitmapSourceFromHBitmap(handle,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(handle);
                }
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr hwnd = new WindowInteropHelper(this).Handle;

            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TOOLWINDOW);

            SendMessage(hwnd, WM_SETICON, new IntPtr(1), IntPtr.Zero);
            SendMessage(hwnd, WM_SETICON, IntPtr.Zero, IntPtr.Zero);

            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE |
                SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }

        #region character select function
        public void Select_Azazel()
        {
            Azazel.Checked = true;
            Cerberus.Checked = false;
            Judgement.Checked = false;
            Justice.Checked = false;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = false;
            Malina.Checked = false;
            Modeus.Checked = false;
            Pandemonica.Checked = false;
            Zdrada.Checked = false;
            Glorious_left.Checked = false;
            Glorious_right.Checked = false;

            bitmapPath = "Resources/Azazel.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());
        }
        public void Select_Cerberus()
        {
            Azazel.Checked = false;
            Cerberus.Checked = true;
            Judgement.Checked = false;
            Justice.Checked = false;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = false;
            Malina.Checked = false;
            Modeus.Checked = false;
            Pandemonica.Checked = false;
            Zdrada.Checked = false;
            Glorious_left.Checked = false;
            Glorious_right.Checked = false;

            bitmapPath = "Resources/Cerberus.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());

        }
        public void Select_Judgement()
        {
            Azazel.Checked = false;
            Cerberus.Checked = false;
            Judgement.Checked = true;
            Justice.Checked = false;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = false;
            Malina.Checked = false;
            Modeus.Checked = false;
            Pandemonica.Checked = false;
            Zdrada.Checked = false;
            Glorious_left.Checked = false;
            Glorious_right.Checked = false;

            bitmapPath = "Resources/Judgement.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());
        }
        public void Select_Justice()
        {
            Azazel.Checked = false;
            Cerberus.Checked = false;
            Judgement.Checked = false;
            Justice.Checked = true;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = false;
            Malina.Checked = false;
            Modeus.Checked = false;
            Pandemonica.Checked = false;
            Zdrada.Checked = false;
            Glorious_left.Checked = false;
            Glorious_right.Checked = false;

            bitmapPath = "Resources/Justice.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());
        }
        public void Select_Lucifer()
        {
            Azazel.Checked = false;
            Cerberus.Checked = false;
            Judgement.Checked = false;
            Justice.Checked = false;
            Lucifer.Checked = true;
            Lucifer_Apron.Checked = false;
            Malina.Checked = false;
            Modeus.Checked = false;
            Pandemonica.Checked = false;
            Zdrada.Checked = false;
            Glorious_left.Checked = false;
            Glorious_right.Checked = false;

            bitmapPath = "Resources/Lucifer.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());
        }
        public void Select_Lucifer_Apron()
        {
            Azazel.Checked = false;
            Cerberus.Checked = false;
            Judgement.Checked = false;
            Justice.Checked = false;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = true;
            Malina.Checked = false;
            Modeus.Checked = false;
            Pandemonica.Checked = false;
            Zdrada.Checked = false;
            Glorious_left.Checked = false;
            Glorious_right.Checked = false;

            bitmapPath = "Resources/Apron.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());
        }
        public void Select_Malina()
        {
            Azazel.Checked = false;
            Cerberus.Checked = false;
            Judgement.Checked = false;
            Justice.Checked = false;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = false;
            Malina.Checked = true;
            Modeus.Checked = false;
            Pandemonica.Checked = false;
            Zdrada.Checked = false;
            Glorious_left.Checked = false;
            Glorious_right.Checked = false;

            bitmapPath = "Resources/Malina.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());
        }
        public void Select_Modeus()
        {
            Azazel.Checked = false;
            Cerberus.Checked = false;
            Judgement.Checked = false;
            Justice.Checked = false;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = false;
            Malina.Checked = false;
            Modeus.Checked = true;
            Pandemonica.Checked = false;
            Zdrada.Checked = false;
            Glorious_left.Checked = false;
            Glorious_right.Checked = false;

            bitmapPath = "Resources/Modeus.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());
        }
        public void Select_Pandemonica()
        {
            Azazel.Checked = false;
            Cerberus.Checked = false;
            Judgement.Checked = false;
            Justice.Checked = false;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = false;
            Malina.Checked = false;
            Modeus.Checked = false;
            Pandemonica.Checked = true;
            Zdrada.Checked = false;
            Glorious_left.Checked = false;
            Glorious_right.Checked = false;

            bitmapPath = "Resources/Pandemonica.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());
        }
        public void Select_Zdrada()
        {
            Azazel.Checked = false;
            Cerberus.Checked = false;
            Judgement.Checked = false;
            Justice.Checked = false;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = false;
            Malina.Checked = false;
            Modeus.Checked = false;
            Pandemonica.Checked = false;
            Zdrada.Checked = true;
            Glorious_left.Checked = false;
            Glorious_right.Checked = false;

            bitmapPath = "Resources/Zdrada.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());
        }
        public void Select_Glorious_left()
        {
            Azazel.Checked = false;
            Cerberus.Checked = false;
            Judgement.Checked = false;
            Justice.Checked = false;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = false;
            Malina.Checked = false;
            Modeus.Checked = false;
            Pandemonica.Checked = false;
            Zdrada.Checked = false;
            Glorious_left.Checked = true;
            Glorious_right.Checked = false;

            bitmapPath = "Resources/Glorious_success_left.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());
        }
        public void Select_Glorious_right()
        {
            Azazel.Checked = false;
            Cerberus.Checked = false;
            Judgement.Checked = false;
            Justice.Checked = false;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = false;
            Malina.Checked = false;
            Modeus.Checked = false;
            Pandemonica.Checked = false;
            Zdrada.Checked = false;
            Glorious_left.Checked = false;
            Glorious_right.Checked = true;

            bitmapPath = "Resources/Glorious_success_right.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());
        }
        public void Select_Bellzebub()
        {
            Azazel.Checked = false;
            Cerberus.Checked = false;
            Judgement.Checked = false;
            Justice.Checked = false;
            Lucifer.Checked = false;
            Lucifer_Apron.Checked = false;
            Malina.Checked = false;
            Modeus.Checked = false;
            Pandemonica.Checked = false;
            Zdrada.Checked = false;
            Glorious_left.Checked = false;
            Glorious_right.Checked = false;


            bitmapPath = "Resources/Bellzebub.png";
            Animation(bitmapPath);
            noti.Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon());

        }
        #endregion

        //public void Select_Speed(int _speed)
        //{
        //    switch (_speed)
        //    {
        //        case 1: _speed = 1;
        //            Select_Speed1();
        //            break;
        //        case 2: _speed = 2;
        //            Select_Speed2();
        //            break;
        //        case 3: _speed = 3;
        //            Select_Speed3();
        //            break;
        //        case 4: _speed = 4;
        //            Select_Speed4();
        //            break;
        //        case 5: _speed = 5;
        //            Select_Speed5();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //#region speed select function
        //private void Select_Speed1()
        //{
        //    speed1.Checked = true;
        //    speed2.Checked = false;
        //    speed3.Checked = false;
        //    speed4.Checked = false;
        //    speed5.Checked = false;
        //    speed = 5f;
        //}
        //private void Select_Speed2()
        //{
        //    speed1.Checked = false;
        //    speed2.Checked = true;
        //    speed3.Checked = false;
        //    speed4.Checked = false;
        //    speed5.Checked = false;
        //    speed = 4f;
        //}
        //private void Select_Speed3()
        //{
        //    speed1.Checked = false;
        //    speed2.Checked = false;
        //    speed3.Checked = true;
        //    speed4.Checked = false;
        //    speed5.Checked = false;
        //    speed = 3f;
        //}
        //private void Select_Speed4()
        //{
        //    speed1.Checked = false;
        //    speed2.Checked = false;
        //    speed3.Checked = false;
        //    speed4.Checked = true;
        //    speed5.Checked = false;
        //    speed = 2f;
        //}
        //private void Select_Speed5()
        //{
        //    speed1.Checked = false;
        //    speed2.Checked = false;
        //    speed3.Checked = false;
        //    speed4.Checked = false;
        //    speed5.Checked = true;
        //    speed = 1f;
        //}
        //#endregion
    }
}
