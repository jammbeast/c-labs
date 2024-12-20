using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace RegistrationForm
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик нажатия на ссылки в TextBlock
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        // Обработчик нажатия кнопки "Register"
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Registration successful!");
        }
    }
}