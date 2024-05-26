using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CategoryRepo
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();

        Category? DeleteCategory(short categopryId);

        void AddCategory(Category category);

        void UpdateCategory(Category category);

        IEnumerable<Category> GetCategoriesByName(string searchValue);
    }

}

