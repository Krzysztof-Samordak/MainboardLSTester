/// <copyright>3Shape A/S</copyright>
using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;


namespace _3ShapeMainboardLSTester.Pages.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }
        void WindowClosing(object sender, EventArgs e)
        {
            MessageBox.Show("Turn off the tester!");
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //this.DragMove();
            }
        }
    }
}