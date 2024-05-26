using BusinessObjects;
using Repositories.CategoryRepo;
using Repositories.NewsArticleRepo;
using Repositories.TagRepo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TranDinhThienTanWPF
{
    /// <summary>
    /// Interaction logic for EditNewsArticle.xaml
    /// </summary>
    public partial class EditNewsArticle : Window
    {

        public ITagRepository tagRepository = new TagRepository();

        public ICategoryRepository categoryRepository = new CategoryRepository();

        public INewsArticleRepository newArticleRepository = new NewsArticleRepository();

        public List<Tag> TagListSelected { get; set; } = new List<Tag>();

        public Dictionary<string, Category> CateAllMap = new Dictionary<string, Category>();

        private string Action;

        private SystemAccount User;

        private NewsArticle? Article;
        private string Logs { get; set; }
        public System.Windows.Controls.Grid MainControl { get; set; }

        private List<TagItem> SelectedTagItems;
        public EditNewsArticle(string action, string? newArticleId, SystemAccount user, string? Logs, System.Windows.Controls.Grid mainControl)
        {
            InitializeComponent();
            this.Action = action;
            this.User = user;
            this.Logs = Logs;
            this.MainControl = mainControl;
            if (Action.Equals("Update"))
            {
                this.Article = newArticleRepository.GetArticleById(newArticleId);
            }
            RenderCateMap();
            RenderBoxList();
            InitializeInput();
        }

        public void InitializeInput()
        {
            if (Article != null && Action.Equals("Update"))
            {
                txtNewsArticleId.Text = Article.NewsArticleId ?? string.Empty;
                txtNewsContent.Text = Article.NewsContent ?? string.Empty;
                txtNewsTitle.Text = Article.NewsTitle ?? string.Empty;
                cateName.Content = Article.Category?.CategoryName ?? "Unknow";
                chkNewsStatus.IsChecked = Article.NewsStatus;
                cmbSelectedTags.SelectedIndex = 0;
            } else if(Article == null || Action.Equals("Add"))
            {
                var newestAricleId = newArticleRepository.GetAll().Select(x => x.NewsArticleId).DefaultIfEmpty("0").Max();
                txtNewsArticleId.Text = (int.Parse(newestAricleId) + 1).ToString();
                var firstCate = CateAllMap[CateAllMap.Keys.First()].CategoryName;
                cateName.Content = firstCate;
                cmbSelectedTags.SelectedItem = SelectedTagItems.FirstOrDefault(tagItem => tagItem.Content == firstCate);
            }
            submit.Content = $"Xin chào {User.AccountName}";
        }

        private void AddNewTag_Click(object sender, RoutedEventArgs e)
        {
            var newTagWindow = new NewTagWindow();
            if (newTagWindow.ShowDialog() == true)
            {
                string newTagName = newTagWindow.TagName;
                string newTagDescription = newTagWindow.TagDescription;
                var tags = tagRepository.GetTags().ToList();
                if (!string.IsNullOrEmpty(newTagName) && !tags.Any(x =>
                {
                    if(x.TagName != null)
                    {
                        if (x.TagName.Equals(newTagName)) return true;
                        return false;
                    }
                    return false;

                }))
                {
                    tagRepository.AddTag(new Tag
                    {
                        TagId = tags.Count + 1,
                        TagName = newTagName,
                        Note = newTagDescription
                    });
                }
            }
        }
        
        private void RenderCateMap()
        {
            var cates = categoryRepository.GetAll();
            foreach (var cate in cates)
            {
                CateAllMap.Add(cate.CategoryName, cate);
            }
        }

        private void RenderBoxList()
        {
            SelectedTagItems = new List<TagItem>();
            foreach (var tagItem in CateAllMap.Keys) 
            {
                SelectedTagItems.Add(new TagItem { Content = tagItem });
            };
            cmbSelectedTags.ItemsSource = SelectedTagItems;
        }

        private void cmbSelectedTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSelectedTags.SelectedItem != null)
            {
                // Get the selected item
                TagItem selectedTag = cmbSelectedTags.SelectedItem as TagItem;

                // Do something with the selected item
                cateName.Content = selectedTag.Content;
            }
        }

        private Category? getCurrentCate()
        {
            if (cmbSelectedTags.SelectedItem != null)
            {
                // Get the selected item
                TagItem selectedTag = cmbSelectedTags.SelectedItem as TagItem;

                // Do something with the selected item
                return CateAllMap[selectedTag.Content];
            }
            return null;
        }

        private void ChooseTag_Click(object sender, RoutedEventArgs e)
        {
            SelectedTag screen;
            if (Action.Equals("Update")) {
                screen = new SelectedTag("Update", Article.NewsArticleId, TagListSelected);
            } else {
                screen = new SelectedTag("Add", null, TagListSelected);
            }
            if (screen.ShowDialog() == true)
            {

                //if (!string.IsNullOrEmpty(newTagName) && !TagMap.Keys.Contains(newTagName))
                //{
                //    tagRepository.AddTag(new Tag
                //    {
                //        TagId = TagMap.Values.Count + 1,
                //        TagName = newTagName,
                //        Note = newTagDescription
                //    });
                //}
            }
            else
            {
                TagListSelected = screen.TagListSelected;
                Console.WriteLine("haha");
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            DateTime now = DateTime.Now;
            var newsArticle = new NewsArticle
            {
                NewsArticleId = txtNewsArticleId.Text,
                NewsTitle = txtNewsTitle.Text,
                NewsContent = txtNewsContent.Text,
                CreatedById = User.AccountId,
                CategoryId = getCurrentCate()?.CategoryId,
                NewsStatus = chkNewsStatus.IsChecked,
                CreatedDate = now.Date,
                ModifiedDate = now.Date,
            };

            List<NewsTag> newTagsList = new List<NewsTag>();

            foreach (var item in TagListSelected)
            {
                var newestTagId = tagRepository.GetNewTags().Select(x => x.NewsTagId).Max();
                var newsTag = new NewsTag
                {
                    NewsTagId = (newestTagId + newTagsList.Count + 1).ToString(),
                    NewsArticleId = txtNewsArticleId.Text,
                    TagId = item.TagId,
                };
                newTagsList.Add(newsTag);
            }

            if (Action.Equals("Add"))
            {
                newArticleRepository.AddArticle(newsArticle);            }
            else
            {
                newArticleRepository.UpdateArticle(newsArticle);

                var newTagExisted = tagRepository.GetTagsByArticleId(Article.NewsArticleId);

                List<NewsTag> newTagRemove = new List<NewsTag>();

                newTagRemove = newTagExisted
                    .Where(e =>
                        !newTagsList.Any(n => n.TagId == e.TagId && n.NewsArticleId == e.NewsArticleId)).ToList();

                tagRepository.RemoveTags(newTagRemove);
            }
            tagRepository.AddNewTags(newTagsList);

            if (Action.Equals("Add"))
            {
                Logs += $"ArticleId {newsArticle.NewsArticleId} vừa được thêm !\n";
                submit.Content = "Add thành công !";
            } else
            {
                Logs += $"ArticleId {newsArticle.NewsArticleId} vừa được sửa !\n";
                submit.Content = "Update thành công !";
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NewArticleUserControl screen = new NewArticleUserControl(User, Logs, MainControl, null);
            MainControl.Children.Clear();
            MainControl.Children.Add(screen);
            Close();
        }
    }
}
