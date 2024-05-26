using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class NewArticleDAO
    {
        private static NewArticleDAO? instance;

        private static readonly object lockObj = new object();

        private NewArticleDAO() { }
        public static NewArticleDAO getInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new NewArticleDAO();
                    }
                }
            }
            return instance;
        }

        public NewsArticle? Delete(string articleId)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var article = context.NewsArticles.Include(x => x.NewsTags).SingleOrDefault(a => a.NewsArticleId == articleId);
                if (article != null)
                {
                    context.NewsTags.RemoveRange(article.NewsTags);
                    context.NewsArticles.Remove(article);
                    context.SaveChanges();
                    return article;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<NewsArticle> GetAll()
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var articles = context.NewsArticles.Where(x => true)
                    .Include(x => x.NewsTags)
                        .ThenInclude(n => n.Tag)                                                                                                                                                                                                                                                                                                                                                                                    
                    .Include(x => x.Category)
                    .Include(x => x.CreatedBy).ToList();
                return articles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public NewsArticle GetNewsArticleById(string value)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var articles = context.NewsArticles
                    .Include(x => x.Category)
                    .Include(x => x.CreatedBy)
                    .SingleOrDefault(x => x.NewsArticleId.Equals(value));
                return articles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Add(NewsArticle articles)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var result = context.NewsArticles.Add(articles);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(NewsArticle articles)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var result = context.NewsArticles.Update(articles);
                context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Something Wrongs when Update Category !");
            }
        }

        public IEnumerable<NewsArticle> GetNewsArticleByName(string value)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var arList = context.NewsArticles.Where(x => x.NewsTitle != null ? x.NewsTitle.ToLower().Contains(value) : false 
                                        || x.NewsContent != null ? x.NewsContent.ToLower().Contains(value) : false).ToList();
                return arList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
