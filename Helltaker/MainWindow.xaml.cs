using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Threading;
using System.Media;

namespace Helltaker
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        //잡다한것들 선언부
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
        SoundPlayer click = new SoundPlayer(Properties.Resources.clicksound);
        public string[] NameIndex = new string[] { 
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
        public string[] TitleString = new string[] {
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
        public int index = 0;

        public MainWindow()
        {
            InitializeComponent();

            ni.Icon = Properties.Resources.icon;
            ni.Visible = true;
            ni.DoubleClick += delegate (object sender, EventArgs args)
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            };
        }
        private void Next_Clicked(object sender, MouseButtonEventArgs e) { index++; if (index == 9) index = 0; ReloadScreen(); }
        private void Previous_Clicked(object sender, MouseButtonEventArgs e) { index--; if (index == -1) index = 8; ReloadScreen(); }
        public void ReloadScreen()
        {
            object TitleNode = this.FindName("Title");
            object ImageNode = this.FindName("Image");

            if (TitleNode is Label)
            {
                Label title = TitleNode as Label;
                title.Content = TitleString[index];
            }
            if(ImageNode is Image)
            {
                Image image = ImageNode as Image;
                image.Source = new BitmapImage(new Uri(@"/Resources/Sprite/" + NameIndex[index] + ".png", UriKind.Relative));
            }
        }

        private void Addchibi(object sender, MouseButtonEventArgs e)
        {
            Chibi window = new Chibi();
            window.Show();

            click.Play();
        }

        private void DragForm(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            e.Handled = true;
        }

        private void MinimizeTray(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
