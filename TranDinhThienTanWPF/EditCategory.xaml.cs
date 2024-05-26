using BusinessObjects;
using Microsoft.Identity.Client.NativeInterop;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
using Repositories.CategoryRepo;
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
    /// Interaction logic for EditCategory.xaml
    /// </summary>
    public partial class EditCategory : Window
    {
        private readonly ICategoryRepository categoryRepository = new CategoryRepository();

        private string Action;

        private SystemAccount User;

        private CategoryModel? Category;
        private string Logs { get; set; }

        public System.Windows.Controls.Grid MainControl { get; set; }

        public EditCategory(string action, CategoryModel? category, SystemAccount user, string? Logs, System.Windows.Controls.Grid mainControl)
        {
            InitializeComponent();
            this.Action = action;
            this.Category = category;
            this.User = user;
            this.Logs = Logs;
            this.MainControl = mainControl;
            if (Action.Equals("Add"))
            {
                txtCategoryId.IsReadOnly = true;
            }
            InitializeInput();
        }

        public void InitializeInput()
        {
            if (Category != null)
            {
                txtCategoryId.Text = Category.CategoryId.ToString() ?? string.Empty;
                txtCategoryName.Text = Category.CategoryName ?? string.Empty;
                txtCategoryDesciption.Text = Category.CategoryDesciption ?? string.Empty;
            }
        }

        public Category GetSystemAccountInput(bool isAdd)
        {
            if (isAdd)
            {
                return new Category
                {
                    CategoryName = txtCategoryName.Text ?? string.Empty,
                    CategoryDesciption = txtCategoryDesciption.Text ?? string.Empty,
                };
            }
            return new Category
            {
                CategoryId = short.Parse(txtCategoryId.Text),
                CategoryName = txtCategoryName.Text ?? string.Empty,
                CategoryDesciption = txtCategoryDesciption.Text ?? string.Empty,
            };
        }

        private void Submit_Click(object sender, RoutedEventArgs e) 
        {

            if (Action.Equals("Add"))
            {
                var category = GetSystemAccountInput(true);
                categoryRepository.AddCategory(category);
                Logs += $"{category.CategoryName} vừa được thêm !\n";
                submit.Content = "Add thành công !";
            }
            else
            {
                var category = GetSystemAccountInput(false);
                categoryRepository.UpdateCategory(category);
                Logs += $"{category.CategoryName} vừa được sửa !\n";
                submit.Content = "Update thành công !";
            }
        }

        private void BackToAdmin_Click(object sender, RoutedEventArgs e)
        {
            CategoryUserControl screen = new CategoryUserControl(User, Logs, MainControl, null);
            MainControl.Children.Clear();
            MainControl.Children.Add(screen);
            Close();
        }
    }
}
