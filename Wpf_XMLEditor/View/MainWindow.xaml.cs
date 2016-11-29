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
            menu.IsEnabled = false;
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
            menu.IsEnabled = true;
            tabControl.IsEnabled = true;
            add_button.IsEnabled = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = e.Source as TabItem;
            if (tabItem != null)
            {
                TabControl tabControl = tabItem.Parent as TabControl;
                if (tabControl.SelectedItem != tabItem)
                    return;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
            Ok_button.IsEnabled = !binding.HasError;
        }

    }
}
