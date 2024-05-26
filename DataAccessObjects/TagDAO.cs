using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class TagDAO
    {
        private static TagDAO? instance;

        private static readonly object lockObj = new object();

        private TagDAO() { }
        public static TagDAO getInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new TagDAO();
                    }
                }
            }
            return instance;
        }

        public IEnumerable<Tag> GetAll()
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var tags = context.Tags.Where(x => true).ToList();
                return tags;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<NewsTag> GetAllNewTag()
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var tags = context.NewsTags.Where(x => true).AsNoTracking().ToList();
                return tags;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Add(Tag tag)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var result = context.Tags.Add(tag);
                context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Something Wrongs when Adding Tag !");
            }
        }

        public void AddRange(List<NewsTag> newTagList)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var newsArticleId = newTagList.Select(nt => nt.NewsArticleId).FirstOrDefault();
                var existingTags = context.NewsTags
                    .Where(nt => nt.NewsArticleId == newsArticleId)
                    .ToList();

                // Filter out tags that already exist
                var tagsToAdd = newTagList
                    .Where(nt => !existingTags
                        .Any(et => et.TagId == nt.TagId && et.NewsArticleId == nt.NewsArticleId))
                    .ToList();

                if (tagsToAdd.Any())
                {
                    context.NewsTags.AddRange(tagsToAdd);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveRange(List<NewsTag> newTagList)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                context.NewsTags.RemoveRange(newTagList);
                context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Something Wrongs when Adding Tag !");
            }
        }
        public IEnumerable<NewsTag> GetTagByArticle(string newArticleId)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var tags = context.NewsTags.Include(t => t.Tag).Where(x => x.NewsArticleId == newArticleId).ToList();
                return tags;
            }
            catch (Exception)
            {
                throw new Exception("User not exist !");
            }
        }

        public NewsTag DeleteTagOfArticle(int tagId,string newArticleId)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var newsTag = context.NewsTags.SingleOrDefault(t => t.NewsArticleId == newArticleId && t.TagId == tagId);
                if (newsTag != null)
                {
                    context.NewsTags.Remove(newsTag);
                    context.SaveChanges();
                    return newsTag;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
