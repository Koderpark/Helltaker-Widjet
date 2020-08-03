using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Helltaker {
    public partial class MainWindow : Window {

        //잡다한것들 선언부
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon(); // 오른쪽 하단 작은 아이콘 생성
        public string[] NameIndex = new string[] { // 이름 배열
            "판데모니카",
            "모데우스",
            "케르베로스",
            "말리나",
            "즈드라다",
            "아자젤",
            "저스티스",
            "루시퍼",
            "저지먼트"
        };
        public string[] TitleString = new string[] { // 캐릭터 설명 배열
            "피곤한 악마, 판데모니카",
            "음란한 악마, 모데우스",
            "세쌍둥이 악마, 케르베로스",
            "시큰둥한 악마, 말리나",
            "상스러운 악마, 즈드라다",
            "호기심 많은 천사, 아자젤",
            "끝내주는 악마, 저스티스",
            "지옥의 CEO, 루시퍼",
            "고위 기소관, 저지먼트"
        };
        public int index = 0; // 이름&설명의 인덱스
        public static int syncro = 7; // 스티커들 싱크로맞추기

        public MainWindow() {
            InitializeComponent();

            // 스티커들 움직임 관련 (프레임카운팅)
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.04);
            timer.Tick += AddFrame;
            timer.Start();
        }

        private void AddFrame(object sender, EventArgs e) { syncro = (syncro + 1) % 12; } // 프레임카운팅 함수

        // 캐릭터 선택버튼 함수.
        private void Next_Clicked(object sender, MouseButtonEventArgs e) { index++; if (index == 9) index = 0; ReloadScreen(); }
        private void Previous_Clicked(object sender, MouseButtonEventArgs e) { index--; if (index == -1) index = 8; ReloadScreen(); }

        // 선택된 캐릭터를 출력하는 함수.
        public void ReloadScreen() {
            object TitleNode = this.FindName("Title");
            object ImageNode = this.FindName("Image");
            if (TitleNode is Label) {
                Label title = TitleNode as Label;
                title.Content = TitleString[index];
            }
            if (ImageNode is Image) {
                Image image = ImageNode as Image;
                image.Source = new BitmapImage(new Uri(@"/Resources/Sprite/" + NameIndex[index] + ".png", UriKind.Relative));
            }
        }

        
        private void Addchibi(object sender, MouseButtonEventArgs e) { Chibi window = new Chibi(NameIndex[index]); window.Show(); } // 스티커 생성 함수.

        private void DragForm(object sender, MouseButtonEventArgs e) { this.DragMove(); e.Handled = true; } // 창 드래그 함수.

        private void MinimizeTray(object sender, MouseButtonEventArgs e) { // 종료시 프로그램이 꺼지지 않고 최소화되게함.
            this.Hide();
            this.ShowInTaskbar = false;
            
            // 우클릭시 나오는 선택창 선언.
            System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
            ni.Icon = Properties.Resources.icon;
            ni.Visible = true;
            ni.ContextMenu = menu;
            ni.Text = "헬-테이커";

            // 선택창 0번줄
            System.Windows.Forms.MenuItem i0 = new System.Windows.Forms.MenuItem();
            menu.MenuItems.Add(i0);
            i0.Index = 0;
            i0.Text = "만든사람 / 사용법";
            i0.Click += delegate (object s, EventArgs e1) { System.Diagnostics.Process.Start("https://github.com/Koder0205/Helltaker-Widjet"); };

            // 선택창 1번줄
            System.Windows.Forms.MenuItem i1 = new System.Windows.Forms.MenuItem();
            menu.MenuItems.Add(i1);
            i1.Index = 1;
            i1.Text = "캐릭터생성창 열기";
            i1.Click += delegate (object s, EventArgs e1) { this.Show(); this.WindowState = WindowState.Normal; ni.Visible = false; this.ShowInTaskbar = true; };

            // 선택창 2번줄
            System.Windows.Forms.MenuItem i2 = new System.Windows.Forms.MenuItem();
            menu.MenuItems.Add(i2);
            i2.Index = 2;
            i2.Text = "프로그램 종료하기";
            i2.Click += delegate (object s, EventArgs e1) { System.Windows.Application.Current.Shutdown(); ni.Dispose(); };
        }
    }
}
