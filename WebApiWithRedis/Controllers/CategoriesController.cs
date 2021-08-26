using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiWithRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{Id}")]
        public CategoryDto GetById(int Id)
        {
            return _categoryService.GetCategoryById(Id);
        }

        [HttpGet("GetCategoryList")]
        public IEnumerable<CategoryDto> GetBookList()
        {
            return _categoryService.GetCategoryList();
        }

        [HttpPost("InsertCategory")]
        public IEnumerable<CategoryDto> InsertProduct([FromBody] CategoryDto model)
        {
            if (model != null)
            {
                if (model.Id > 0)
                    return _categoryService.UpdateCategory(model);
                else
                    return _categoryService.InsertCategory(model);
            }
            return new List<CategoryDto>();
        }
    }
}
