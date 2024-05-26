using BusinessObjects;
using Repositories.TagRepo;
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

namespace TranDinhThienTanWPF
{
    /// <summary>
    /// Interaction logic for SelectedTag.xaml
    /// </summary>
    public partial class SelectedTag : Window
    {
        public ITagRepository tagRepository = new TagRepository();

        public Dictionary<string?, Tag> TagMapUser = new Dictionary<string?, Tag>();

        public Dictionary<string?, Tag> TagMapAll = new Dictionary<string?, Tag>();

        public Dictionary<string, bool> TagMapSelected = new Dictionary<string, bool>();

        public List<Tag> TagListSelected {  get; set; } = new List<Tag>();
        private string Status {  get; set; }
        private string ArticleId { get; set; }
        public SelectedTag(string status, string articleId, List<Tag> newTags)
        {
            InitializeComponent();
            Status = status;
            ArticleId = articleId;
            RenderTagsAllDefault();
            if(newTags != null && newTags.Count != 0) 
            {
                RenderTagsUserExist(newTags);
            } else if (newTags == null || newTags.Count == 0)
            {
                if (Status.Equals("Update"))
                {
                    RenderTagsUserDefault();
                }
                else
                {
                    TagMapUser.Clear();
                }
            }
            RederTagsSelected();
            RenderToCheckBox();
        }
        private void RenderTagsAllDefault()
        {
            TagMapAll.Clear();
            var tags = tagRepository.GetTags();
            foreach (var tag in tags)
            {
                TagMapAll.Add(tag?.TagName, tag);
            }
        }

        private void RenderTagsUserDefault()
        {
            TagMapUser.Clear();
            var tags = tagRepository.GetTagsByArticleId(ArticleId);
            foreach (var tag in tags)
            {
                TagMapUser.Add(tag?.Tag?.TagName, tag?.Tag);
            }
        }

        private void RenderTagsUserExist(List<Tag> newTags)
        {
            TagMapUser.Clear();
            foreach (var tag in newTags)
            {
                TagMapUser.Add(tag?.TagName, tag);
            }
        }

        private void RederTagsSelected()
        {
            TagMapSelected.Clear();
            foreach (var tag in TagMapAll.Keys.ToList())
            {
                if (TagMapUser.ContainsKey(tag))
                {
                    TagMapSelected.Add(tag, true);
                } else
                {
                    TagMapSelected.Add(tag, false);
                }
            }
        }

        private void RenderToCheckBox()
        {
            List<TagItem> tagItems = new List<TagItem>();

            foreach (var tag in TagMapSelected.Keys.ToList())
            {
                if (TagMapSelected[tag] == true)
                {
                    tagItems.Add(new TagItem
                    {
                        Content = tag,
                        IsSelected = true
                    });
                }
                else
                {
                    tagItems.Add(new TagItem
                    {
                        Content = tag,
                        IsSelected = false
                    });
                }
            }
            lstAllTags.ItemsSource = tagItems;
        }

        private void GetSelectedTags()
        {
            List<Tag> selectedTags = new List<Tag>();

            foreach (TagItem item in lstAllTags.ItemsSource)
            {
                if (item.IsSelected)
                {
                    selectedTags.Add(TagMapAll[item.Content.ToString()]);
                }
            }
            TagListSelected = [];
            TagListSelected = selectedTags;
        }

        private void BtnClose_Click(object sender, EventArgs e) 
        {
            GetSelectedTags();
            Close();
        }
    }

    public class TagItem
    {
        public string Content { get; set; }
        public bool IsSelected { get; set; }
    }
}
