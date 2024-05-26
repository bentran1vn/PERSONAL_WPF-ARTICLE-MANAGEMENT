using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.TagRepo
{
    public interface ITagRepository
    {
        IEnumerable<NewsTag> GetTagsByArticleId(string articleId);

        NewsTag? DeleteTagOfCategory(int tagId, string newsArticleId);

        void AddTag(Tag tag);

        IEnumerable<Tag> GetTags();

        void AddNewTags(List<NewsTag> tag);

        void RemoveTags(List<NewsTag> tag);

        IEnumerable<NewsTag> GetNewTags();
    }
}
