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

namespace Helltaker
{

    public partial class Chibi : Window
    {

        public Chibi()
        {
            InitializeComponent();
        }

        private void DeleteWindow(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void DeleteForm(object sender, MouseButtonEventArgs e)
        {

        }

        private void DragForm(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            e.Handled = true;
        }
    }
}
