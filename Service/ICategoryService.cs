using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICategoryService
    {
        public CategoryDto GetCategoryById(int Id);
        public List<CategoryDto> GetCategoryList();
        public List<CategoryDto> InsertCategory(CategoryDto category);
        public List<CategoryDto> UpdateCategory(CategoryDto category);
    }
}
