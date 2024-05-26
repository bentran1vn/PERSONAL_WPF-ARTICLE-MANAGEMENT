using BusinessObjects;
using Microsoft.Extensions.Configuration;
using Repositories.SystemAccountRepo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly ISystemAccountRepository systemAccountRepository = new SystemAccountRepository();
        private static IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", true, true)
                 .Build();
        private readonly string  AdminEmailDefault = config["adminAccount:email"];
        private readonly string AdminPasswordDefault = config["adminAccount:password"];
        public Login()
        {
            InitializeComponent();
        }
        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Example logic for login button click
            string email = txtLogin.Text;
            string password = txtPassword.Password;
            try
            {
                SystemAccount? account = systemAccountRepository.CheckLogin(email, password);
                if ((email == AdminEmailDefault && password == AdminPasswordDefault) || account != null)
                {
                    
                    BtnLogin.Content = "Successfully";
                    BtnLogin.Background = new SolidColorBrush(Colors.Green);
                    BtnLogin.Foreground = new SolidColorBrush(Colors.White);
                    usernameError.Visibility = Visibility.Collapsed;
                    passwordError.Visibility = Visibility.Collapsed;
                    if (email == AdminEmailDefault)
                    {
                        Admin screen = new Admin(new SystemAccount
                        {
                            AccountEmail = AdminEmailDefault,
                        }, null);
                        await Task.Delay(1000);
                        screen.Show();
                        Close();
                    } else if(account.AccountRole == 1)
                    {
                        Staff screen = new Staff(account, null);
                        await Task.Delay(1000);
                        screen.Show();
                        Close();
                    }
                } else
                {
                    throw new Exception("Can not find User !");
                }
            }
            catch (Exception)
            {
                usernameError.Visibility = Visibility.Visible;
                passwordError.Visibility = Visibility.Visible;
            }
        }
    }
}
