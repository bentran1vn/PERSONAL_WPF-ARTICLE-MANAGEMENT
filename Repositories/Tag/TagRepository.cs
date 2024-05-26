using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.TagRepo
{
    public class TagRepository : ITagRepository
    {
        public void AddNewTags(List<NewsTag> tag)
        {
            try
            {
                TagDAO.getInstance().AddRange(tag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddTag(Tag tag)
        {
            try
            {
                TagDAO.getInstance().Add(tag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public NewsTag? DeleteTagOfCategory(int tagId, string newsArticleId)
        {
            try
            {
                var categories = TagDAO.getInstance().DeleteTagOfArticle(tagId, newsArticleId);
                if (categories == null) throw new Exception("Categories is Empty");
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<NewsTag> GetNewTags()
        {
            try
            {
                var account = TagDAO.getInstance().GetAllNewTag();
                if (account == null) throw new Exception("Acounts is Empty");
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Tag> GetTags()
        {
            try
            {
                var account = TagDAO.getInstance().GetAll();
                if (account == null) throw new Exception("Acounts is Empty");
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<NewsTag> GetTagsByArticleId(string articleId)
        {
            try
            {
                var categories = TagDAO.getInstance().GetTagByArticle(articleId);
                if (categories == null) throw new Exception("Categories is Empty");
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveTags(List<NewsTag> tag)
        {
            try
            {
                TagDAO.getInstance().RemoveRange(tag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
