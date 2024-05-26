using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.NewsArticleRepo
{
    public interface INewsArticleRepository
    {
        IEnumerable<NewsArticle> GetAll();
        NewsArticle? DeleteArticle(string articleId);
        NewsArticle? GetArticleById(string articleId);

        void AddArticle(NewsArticle arc);

        void UpdateArticle(NewsArticle arc);

        IEnumerable<NewsArticle> GetNewsArticlesByName(string searchValue);
    }
}
