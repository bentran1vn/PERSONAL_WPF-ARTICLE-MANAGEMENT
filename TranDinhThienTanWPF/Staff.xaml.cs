using BusinessObjects;
using Microsoft.VisualBasic.Logging;
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
    /// Interaction logic for Staff.xaml
    /// </summary>
    public partial class Staff : Window
    {
        private SystemAccount user;

        public Staff(SystemAccount user, string? Logs)
        {
            InitializeComponent();
            MainContent.Children.Add(new CategoryUserControl(user, null, MainContent, null));
            this.user = user;
            labelUser.Content = user.AccountName;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                if (MainContent.Children[0] is CategoryUserControl)
                {
                    MainContent.Children.Add(new CategoryUserControl(user, null, MainContent, searchText));
                } else if(MainContent.Children[0] is NewArticleUserControl)
                {
                    MainContent.Children.Add(new NewArticleUserControl(user, null, MainContent, searchText));
                }
            }
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new CategoryUserControl(user, null, MainContent, null));
            txtSearch.Visibility = Visibility.Visible;
            btnSearch.Visibility = Visibility.Visible;
        }

        private void NewArticle_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new NewArticleUserControl(user, null, MainContent, null));
            txtSearch.Visibility = Visibility.Visible;
            btnSearch.Visibility = Visibility.Visible;
        }

        private void MyProfile_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new MyProfile(user, MainContent));
            txtSearch.Visibility = Visibility.Hidden;
            btnSearch.Visibility = Visibility.Hidden;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login screen = new Login();
            screen.Show();
            Close();
        }

    }
}
