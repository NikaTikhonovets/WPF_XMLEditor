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
        private Tab tab;
        public MainWindow()
        {
            InitializeComponent();
            tab = new Tab();
            DataContext = tab;
        }

        public void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            if (sender is TreeViewItem)
            {
                if (!((TreeViewItem)sender).IsSelected)
                {
                    return;
                }
            }
            tab.GetMethod(sender);
            DoubleAnimation formAnimation = new DoubleAnimation();
            formAnimation.From = MyWindow.ActualWidth;
            formAnimation.To = 800;
            formAnimation.Duration = TimeSpan.FromSeconds(1);
            this.BeginAnimation(Window.WidthProperty, formAnimation);
            tabControl.IsEnabled = false;
            add_button.IsEnabled = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation formAnimation = new DoubleAnimation();
            formAnimation.From = MyWindow.ActualWidth;
            formAnimation.To = 545;
            formAnimation.Duration = TimeSpan.FromSeconds(1);
            this.BeginAnimation(Window.WidthProperty, formAnimation);
            tabControl.IsEnabled = true;
            add_button.IsEnabled = true;
        }
    }
}
