using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Wpf_XMLEditor.ViewModel;

namespace Wpf_XMLEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Tab();
        }
            private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            if (sender is TreeViewItem)
            {
                if (!((TreeViewItem)sender).IsSelected)
                {
                    return;
                }
            }
            DoubleAnimation formAnimation = new DoubleAnimation();
            formAnimation.From = MyWindow.ActualWidth;
            formAnimation.To = 800;
            formAnimation.Duration = TimeSpan.FromSeconds(1);
            this.BeginAnimation(Window.WidthProperty, formAnimation);
            //   MessageBox.Show("kuku");

        }
    
    }
}
