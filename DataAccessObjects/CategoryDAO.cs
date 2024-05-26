using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class CategoryDAO
    {
        private static CategoryDAO? instance;

        private static readonly object lockObj = new object();

        private CategoryDAO() { }
        public static CategoryDAO getInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                }
            }
            return instance;
        }

        public IEnumerable<Category> GetCategotyByName(string value)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var cateList = context.Categories.Where(x => x.CategoryName.ToLower().Contains(value)).ToList();
                return cateList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Add(Category category)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var result = context.Categories.Add(category);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(Category category)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var result = context.Categories.Update(category);
                context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Something Wrongs when Update Category !");
            }
        }

        public Category? Delete(short categoryId)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var category = context.Categories.Include(x => x.NewsArticles).SingleOrDefault(c => c.CategoryId == categoryId);
                if (category != null)
                {
                    foreach (var item in category.NewsArticles)
                    {
                        item.CategoryId = null;
                    }
                    context.Categories.Remove(category);
                    context.SaveChanges();
                    return category;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Category> GetAll()
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var categories = context.Categories.Where(x => true).Select(x => new Category
                {
                    CategoryId = x.CategoryId,
                    CategoryDesciption = x.CategoryDesciption,
                    CategoryName = x.CategoryName
                }).ToList();
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
