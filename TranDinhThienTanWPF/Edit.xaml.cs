using BusinessObjects;
using Microsoft.Identity.Client.NativeInterop;
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
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        private readonly ISystemAccountRepository systemAccountRepository = new SystemAccountRepository();

        private string Action;

        private SystemAccount User;

        private Account? Account;

        private string Logs { get; set; }
        public Edit(string action, Account? account, SystemAccount user, string? Logs)
        {
            InitializeComponent();
            this.Action = action;
            this.Account = account;
            this.User = user;
            this.Logs = Logs;
            InitializeInput();
        }

        public void InitializeInput()
        {
            if (Account != null)
            {
                txtAccountId.Text = Account.AccountId.ToString() ?? string.Empty;
                txtAccountName.Text = Account.AccountName ?? string.Empty;
                txtAccountEmail.Text = Account.AccountEmail ?? string.Empty;

                foreach (ComboBoxItem item in cbAccountRole.Items)
                {
                    if (item.Content.Equals(Account.Role) )
                    {
                        item.IsSelected = true;
                        break;
                    }
                }

                pbAccountPassword.Password = string.Empty;
            }
        }

        public SystemAccount GetSystemAccountInput()
        {
            return new SystemAccount
            {
                AccountId = short.Parse(txtAccountId.Text),
                AccountEmail = txtAccountEmail.Text ?? string.Empty,
                AccountName = txtAccountName.Text ?? string.Empty,
                AccountRole = int.Parse((string)((cbAccountRole.SelectedItem as ComboBoxItem)?.Tag)),
                AccountPassword = pbAccountPassword.Password
            };
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var account = GetSystemAccountInput();
            if(account != null)
            {
                if(Action.Equals("Add"))
                {
                    systemAccountRepository.AddAccount(account);
                    Logs += $"{account.AccountName} vừa được thêm !\n";
                    submit.Content = "Add thành công !";
                } else
                {
                    systemAccountRepository.UpdateAccount(account);
                    Logs += $"{account.AccountName} vừa được sửa !\n";
                    submit.Content = "Update thành công !";
                }
            }
        }

        private void BackToAdmin_Click(object sender, RoutedEventArgs e)
        {
            Admin screen = new Admin(User, Logs);
            screen.Show();
            Close();
        }
    }
}
