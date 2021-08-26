using AutoMapper;
using Core.Models;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiWithRedis.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Book Mapper
            CreateMap<Book, BookDto>();
            CreateMap<BookDto, Book>();
            #endregion

            #region Book Mapper
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            #endregion

            #region Book Mapper
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorDto, Author>();
            #endregion
        }
    }
}
