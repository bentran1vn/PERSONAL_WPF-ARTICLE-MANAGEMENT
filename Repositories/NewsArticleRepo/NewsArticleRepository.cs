using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.NewsArticleRepo
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        public void AddArticle(NewsArticle arc)
        {
            try
            {
                NewArticleDAO.getInstance().Add(arc);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public NewsArticle? DeleteArticle(string articleId)
        {
            try
            {
                var category = NewArticleDAO.getInstance().Delete(articleId);
                if (category == null) throw new Exception("Can not find !");
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<NewsArticle> GetAll()
        {
            try
            {
                var categories = NewArticleDAO.getInstance().GetAll();
                if (categories == null) throw new Exception("Categories is Empty");
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public NewsArticle? GetArticleById(string articleId)
        {
            try
            {
                var news = NewArticleDAO.getInstance().GetNewsArticleById(articleId);
                if (news == null) throw new Exception("Categories is Empty");
                return news;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<NewsArticle> GetNewsArticlesByName(string searchValue)
        {
            try
            {
                var result = NewArticleDAO.getInstance().GetNewsArticleByName(searchValue);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateArticle(NewsArticle arc)
        {
            try
            {
                NewArticleDAO.getInstance().Update(arc);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}
