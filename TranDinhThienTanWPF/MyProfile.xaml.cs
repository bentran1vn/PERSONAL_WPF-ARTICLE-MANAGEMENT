using BusinessObjects;
using Microsoft.VisualBasic.Logging;
using Repositories.SystemAccountRepo;
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
using System.Windows.Shapes;

namespace TranDinhThienTanWPF
{
    /// <summary>
    /// Interaction logic for MyProfile.xaml
    /// </summary>
    public partial class MyProfile : UserControl
    {
        private SystemAccount User;

        private System.Windows.Controls.Grid MainControl;

        ISystemAccountRepository accountRepository = new SystemAccountRepository();
        public MyProfile(SystemAccount user, System.Windows.Controls.Grid mainControl)
        {
            InitializeComponent();
            this.User = user;
            this.MainControl = mainControl;
            InitializeInput();
        }

        public void InitializeInput()
        {
            if (User != null)
            {
                AccountNameTextBox.Text = User.AccountName ?? string.Empty;
                AccountEmailTextBox.Text = User.AccountEmail ?? string.Empty;
                AccountPasswordBox.Password = User.AccountPassword ?? string.Empty;
                var role = User.AccountRole == 1 ? "Staff" : User.AccountRole == 2 ? "Lecturer" : "Hacker";
                Role.Content = $"Role: {role}";
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e) 
        {
            AccountEmailTextBox.IsReadOnly = false;
            AccountNameTextBox.IsReadOnly= false;
            AccountPasswordBox.IsEnabled = false;
            btnEdit.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xA2, 0xAE, 0xEB));
            btnEdit.Foreground = new SolidColorBrush(Colors.White);
        }

        private void SaveButton_Click(Object sender, RoutedEventArgs e)
        {
            SystemAccount acc = new SystemAccount 
            {
                AccountEmail = AccountEmailTextBox.Text,
                AccountPassword = AccountPasswordBox.Password,
                AccountId = short.Parse(User.AccountRole.ToString()),
                AccountRole = User.AccountRole,
                AccountName = AccountNameTextBox.Text,
            };
            accountRepository.UpdateAccount(User);
            AccountEmailTextBox.IsReadOnly = true;
            AccountNameTextBox.IsReadOnly = true;
            AccountPasswordBox.IsEnabled = true;
            btnEdit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            btnEdit.Foreground = new SolidColorBrush(Colors.Black);
            User = acc;
            InitializeInput();
        }
    }
}
