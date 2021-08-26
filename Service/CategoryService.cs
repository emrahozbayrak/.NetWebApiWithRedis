using AutoMapper;
using Core.Models;
using DataAccess.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IRepository<Category> categoryRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public CategoryDto GetCategoryById(int Id)
        {
            var data = _categoryRepository.GetById(Id);
            var mappedData = _mapper.Map<CategoryDto>(data);

            return mappedData;
        }

        public List<CategoryDto> GetCategoryList()
        {
            var data = _categoryRepository.TableNoTracking.ToList();
            var mappedData = _mapper.Map<List<CategoryDto>>(data);

            return mappedData;
        }

        public List<CategoryDto> InsertCategory(CategoryDto category)
        {
            var data = _mapper.Map<Category>(category);
            _categoryRepository.Insert(data);

            var responseData = GetCategoryList();

            return responseData;
        }

        public List<CategoryDto> UpdateCategory(CategoryDto category)
        {
            List<CategoryDto> response = new();
            var updatedModel = _mapper.Map<Category>(category);

            var currentModel = _categoryRepository.GetById(updatedModel.Id);
            if (currentModel != null)
            {
                _categoryRepository.UpdateMatchEntity(currentModel, updatedModel);


                response = GetCategoryList();
            }
            else
            {
                throw new NotImplementedException();
            }

            return response;
        }
    }
}
