using BusinessObjects;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.Logging;
using Repositories.CategoryRepo;
using Repositories.NewsArticleRepo;
using Repositories.TagRepo;
using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TranDinhThienTanWPF
{
    /// <summary>
    /// Interaction logic for NewArticleUserControl.xaml
    /// </summary>
    public partial class NewArticleUserControl : UserControl
    {
        private readonly INewsArticleRepository articleRepository = new NewsArticleRepository();
        private readonly ITagRepository tagRepository = new TagRepository();

        private SystemAccount user;

        private System.Windows.Controls.Grid MainControl;
        public NewArticleUserControl(SystemAccount user, string? Logs, System.Windows.Controls.Grid mainControl, string? txtSearch)
        {
            InitializeComponent();
            if (!txtSearch.IsNullOrEmpty())
            {
                dgArticleList.Columns.Clear();
                dgTagOfCategoryList.Columns.Clear();
                var arcList = articleRepository.GetNewsArticlesByName(txtSearch).Select(x => new ArticleModel
                {
                    NewsArticleId = x.NewsArticleId ?? string.Empty,
                    NewsTitle = x.NewsTitle ?? string.Empty,
                    NewsContent = x.NewsContent ?? string.Empty,
                    CategoryName = x?.Category?.CategoryName ?? string.Empty,
                    AccountName = x?.CreatedBy?.AccountName ?? string.Empty,
                    NewsStatus = x.NewsStatus ?? false,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate
                });
                dgArticleList.ItemsSource = arcList;
                if(arcList.Any())
                {
                    dgTagOfCategoryList.ItemsSource = GetTagOfCategoryById(arcList.First().NewsArticleId);
                }
            }
            else
            {
                Loaded += RenderDefaultDataGrid;
                Loaded += (sender, e) =>
                {
                    if (dgArticleList.ItemsSource is IEnumerable<ArticleModel> articleList && articleList.Any())
                    {
                        var firstArticle = articleList.First();
                        dgTagOfCategoryList.Columns.Clear();
                        dgTagOfCategoryList.ItemsSource = GetTagOfCategoryById(firstArticle.NewsArticleId);
                    }
                };
            }
            this.user = user;
            logs.Content += Logs;
            this.MainControl = mainControl;
        }

        private void RenderDefaultDataGrid(object sender, RoutedEventArgs e)
        {
            dgArticleList.Columns.Clear();
            var list = articleRepository.GetAll().Select(x => new ArticleModel
            {
                NewsArticleId = x.NewsArticleId ?? string.Empty,
                NewsTitle = x.NewsTitle ?? string.Empty,
                NewsContent = x.NewsContent ?? string.Empty,
                CategoryName = x?.Category?.CategoryName ?? string.Empty,
                AccountName = x?.CreatedBy?.AccountName ?? string.Empty,
                NewsStatus = x.NewsStatus ?? false,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate
            });
            dgArticleList.ItemsSource = list;
        }

        private void RenderTagOfCategory(object sender, RoutedEventArgs e)
        {
            dgTagOfCategoryList.Columns.Clear();
            if (dgArticleList.SelectedCells.Count > 0)
            {
                DataGridCellInfo cellInfo = dgArticleList.SelectedCells[0];
                object cellValue = cellInfo.Item;
                ArticleModel? article = cellValue as ArticleModel;
                if (article != null)
                {
                    dgTagOfCategoryList.Columns.Clear();
                    dgTagOfCategoryList.ItemsSource = GetTagOfCategoryById(article.NewsArticleId);
                }
            }
        }

        private IEnumerable<TagModel> GetTagOfCategoryById(string categoryId)
        {
            var list = tagRepository.GetTagsByArticleId(categoryId).Select(x => new TagModel
            {
                TagId = (int)x.TagId,
                TagName = x?.Tag?.TagName ?? string.Empty,
                Note = x?.Tag?.Note ?? string.Empty,
            });
            return list;
        }

        private void dgNewArticleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgArticleList.SelectedItem != null)
            {
                BtnEdit.Content = "Update";
                RenderTagOfCategory(null, null);
            }
            else
            {
                BtnEdit.Content = "Add";
            };
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgArticleList.SelectedCells.Count > 0)
            {
                DataGridCellInfo cellInfo = dgArticleList.SelectedCells[0];
                object cellValue = cellInfo.Item;
                ArticleModel? article = cellValue as ArticleModel;
                if (article != null)
                {
                    string message = $"Are you sure you want to delete article with Id {article?.NewsArticleId}?";
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
                            var categoryDeleted = articleRepository.DeleteArticle(article.NewsArticleId);
                            if (categoryDeleted != null)
                            {
                                RenderDefaultDataGrid(null, null);
                                logs.Content += $"ArticleId {article.NewsArticleId} vừa bị xóa !\n";
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

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditNewsArticle screen;
            if (BtnEdit.Content == "Update")
            {
                DataGridCellInfo cellInfo = dgArticleList.SelectedCells[0];
                object cellValue = cellInfo.Item;
                ArticleModel? article = cellValue as ArticleModel;
                screen = new EditNewsArticle(BtnEdit.Content.ToString(), article.NewsArticleId, user, logs.Content.ToString(), MainControl);
            }
            else
            {
                screen = screen = new EditNewsArticle(BtnEdit.Content.ToString(), string.Empty, user, logs.Content.ToString(), MainControl); ;
            }
            screen.Show();
        }

        private void BtnTagDelete_Click(Object sender, RoutedEventArgs e) 
        {
            if (dgArticleList.SelectedCells.Count > 0)
            {
                DataGridCellInfo cellInfo = dgArticleList.SelectedCells[0];
                object cellValue = cellInfo.Item;
                ArticleModel? article = cellValue as ArticleModel;
                if (article != null)
                {
                    if (dgTagOfCategoryList.SelectedCells.Count > 0)
                    {
                        DataGridCellInfo cellTagInfo = dgTagOfCategoryList.SelectedCells[0];
                        object cellTagValue = cellTagInfo.Item;
                        TagModel? tag = cellTagValue as TagModel;
                        string message = $"Are you sure you want to delete article with Id {tag.TagName}?";
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
                                var tagDeleted = tagRepository.DeleteTagOfCategory(tag.TagId, article.NewsArticleId);
                                if (tagDeleted != null)
                                {
                                    dgTagOfCategoryList.Columns.Clear();
                                    dgTagOfCategoryList.ItemsSource = GetTagOfCategoryById(article.NewsArticleId);
                                    logs.Content += $"Tag {tag.TagName} vừa bị xóa ở ArticleId {article.NewsArticleId} !\n";
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
        }
    }
    public class ArticleModel
    {
        public string NewsArticleId { get; set; } = null!;

        public string? NewsTitle { get; set; }

        public string? NewsContent { get; set; }

        public string? AccountName { get; set; }

        public string? CategoryName { get; set; }

        public bool? NewsStatus { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    public class TagModel 
    {
        public int TagId {  get; set; } 
        public string? TagName { get; set; }
        public string? Note { get; set; }
    } 
}
