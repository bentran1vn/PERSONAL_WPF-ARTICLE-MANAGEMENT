using BusinessObjects;
using DataAccessObjects;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CategoryRepo
{
    public class CategoryRepository : ICategoryRepository
    {
        public void AddCategory(Category category)
        {
            try
            {
                CategoryDAO.getInstance().Add(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Category? DeleteCategory(short categopryId)
        {
            try
            {
                var category = CategoryDAO.getInstance().Delete(categopryId);
                if (category == null) throw new Exception("Can not find !");
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Category> GetAll()
        {
            try
            {
                var categories = CategoryDAO.getInstance().GetAll();
                if (categories == null) throw new Exception("Categories is Empty");
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Category> GetCategoriesByName(string searchValue)
        {
            try
            {
                var result = CategoryDAO.getInstance().GetCategotyByName(searchValue);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                CategoryDAO.getInstance().Update(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
