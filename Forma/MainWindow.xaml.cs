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
            private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        var passwordBox = (PasswordBox)FindName("PasswordBox");
        var confirmPasswordBox = (PasswordBox)FindName("ConfirmPasswordBox");

        if (passwordBox.Password.Length <= 6)
        {
            MessageBox.Show("Password must be longer than 6 characters.");
            return;
        }

        if (passwordBox.Password != confirmPasswordBox.Password)
        {
            MessageBox.Show("Passwords do not match.");
            return;
        }

        MessageBox.Show("Registration successful!");
    }
     private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        // Обработчик нажатия кнопки "Register
    }
}