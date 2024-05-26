using BusinessObjects;
using Microsoft.Identity.Client;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace TranDinhThienTanWPF
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private readonly ISystemAccountRepository systemAccountRepository = new SystemAccountRepository();
        private SystemAccount user;
        public Admin(SystemAccount user, string? Logs)
        {
            InitializeComponent();
            Loaded += RenderDefaultDataGrid;
            this.user = user;
            userEmail.Content += user.AccountEmail;
            logs.Content += Logs;
        }

        private void BtLogout_Click(object sender, RoutedEventArgs e)
        {
            user = null;
            Login screen = new Login();
            screen.Show();
            Close();
        }

        private void RenderDefaultDataGrid(object sender, RoutedEventArgs e)
        {
            dgAccountList.Columns.Clear();
            var list = systemAccountRepository.GetAccounts().Select(x =>
            new Account {
                AccountId = x.AccountId!,
                AccountName = x.AccountName ?? "",
                AccountEmail = x.AccountEmail ?? "",
                Role = x.AccountRole == 1 ? "Staff" : x.AccountRole == 2 ? "Lecturer" : "Unknown"
            }).ToList();
            dgAccountList.ItemsSource = list;
        }

        //private void dgAccountList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    if (dgAccountList.SelectedCells.Count > 0)
        //    {
        //        DataGridCellInfo cellInfo = dgAccountList.SelectedCells[0];
        //        object cellValue = cellInfo.Item;
        //        SystemAccount? account = cellValue as SystemAccount;
        //    }
        //}

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Edit screen;
            if (BtnEdit.Content == "Update")
            {
                DataGridCellInfo cellInfo = dgAccountList.SelectedCells[0];
                object cellValue = cellInfo.Item;
                Account? account = cellValue as Account;
                screen = new Edit(BtnEdit.Content.ToString(), account, user, logs.Content.ToString());
            } else
            {
                screen = new Edit(BtnEdit.Content.ToString(), null, user, logs.Content.ToString());
            }
            screen.Show();
            Close();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var text = txtSearch.Text.ToLower();
                dgAccountList.Columns.Clear();
                var list = systemAccountRepository.GetAccountsByEmail(text).Select(x =>
                new Account
                {
                    AccountId = x.AccountId!,
                    AccountName = x.AccountName ?? "",
                    AccountEmail = x.AccountEmail ?? "",
                    Role = x.AccountRole == 1 ? "Staff" : x.AccountRole == 2 ? "Lecturer" : "Unknown"
                }).ToList();
                dgAccountList.ItemsSource = list;
            } catch (Exception)
            {

            }
        }

        private void accounts_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            RenderDefaultDataGrid(null, null);
        }

        private void dgAccountList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAccountList.SelectedItem != null)
            {
                BtnEdit.Content = "Update";
            } else 
            {
                BtnEdit.Content = "Add";
            };
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgAccountList.SelectedCells.Count > 0)
            {
                DataGridCellInfo cellInfo = dgAccountList.SelectedCells[0];
                object cellValue = cellInfo.Item;
                Account? account = cellValue as Account;
                if (account != null)
                {
                    string message = $"Are you sure you want to delete account {account?.AccountName}?";
                    string[] actionOptions = { "Delete", "Cancel" };

                    var result = MessageBox.Show(
                        message,
                        "Delete Confirmation",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    );
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            // Perform the delete action
                            var accountDeleted = systemAccountRepository.DeleteAccount(account.AccountId);
                            if (accountDeleted != null)
                            {
                                // Refresh the DataGrid
                                RenderDefaultDataGrid(null, null);
                                logs.Content += $"{accountDeleted.AccountName} vừa bị xóa !\n";
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete the account.");
                            }
                            break;
                    }
                }
            }
        }

        private void accounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RenderDefaultDataGrid(null, null);
        }
    }

    public class Account
    {
        public short AccountId { get; set; }

        public string? AccountName { get; set; }

        public string? AccountEmail { get; set; }

        public string? Role { get; set; }
    }
}
