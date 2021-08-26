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
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IRepository<Author> authorRepository,IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        public AuthorDto GetAuthorById(int Id)
        {
            var data = _authorRepository.GetById(Id);
            var mappedData = _mapper.Map<AuthorDto>(data);

            return mappedData;
        }

        public List<AuthorDto> GetAuthorList()
        {
            var data = _authorRepository.TableNoTracking.ToList();
            var mappedData = _mapper.Map<List<AuthorDto>>(data);

            return mappedData;
        }

        public List<AuthorDto> InsertAuthor(AuthorDto author)
        {
            var data = _mapper.Map<Author>(author);
            _authorRepository.Insert(data);

            var responseData = GetAuthorList();

            return responseData;
        }

        public List<AuthorDto> UpdateAuthor(AuthorDto author)
        {
            List<AuthorDto> response = new();
            var updatedModel = _mapper.Map<Author>(author);

            var currentModel = _authorRepository.GetById(updatedModel.Id);
            if (currentModel != null)
            {
                _authorRepository.UpdateMatchEntity(currentModel, updatedModel);


                response = GetAuthorList();
            }
            else
            {
                throw new NotImplementedException();
            }

            return response;
        }
    }
}
