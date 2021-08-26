using AutoMapper;
using Core.Caching;
using Core.Models;
using DataAccess.DbContexts;
using DataAccess.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IMapper _mapper;
        private readonly BookStoreContext _bookContext;
        private readonly IRedisCacheService _redisCacheService;

        public BookService(
            IRepository<Book> bookRepository,
            IRepository<Category> categoryRepository,
            IRepository<Author> authorRepository,
            IMapper mapper,
            BookStoreContext bookContext,
            IRedisCacheService redisCacheService
            )
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _bookContext = bookContext;
            _redisCacheService = redisCacheService;

        }

        public const string BookKey = "Book:{0}";
        public const string GetAllBooksKey = "Book:*";
        public BookDto GetBookById(int Id)
        {
            //Redis kontrol ediliyor.Eğer rediste bir sonuc bulunamazsa veriler db üzerinden sorgulanıp redise yazılıyor.
            var cacheKey = string.Format(BookKey, Id);
            var result = _redisCacheService.Get<BookDto>(cacheKey);

            if (result != null)
                return result;

            var data = _bookRepository.GetById(Id);
            var mappedData = _mapper.Map<BookDto>(data);
            _redisCacheService.Set(cacheKey, mappedData);

            return mappedData;
        }

        public List<BookDto> GetBookList()
        {
            var result = _redisCacheService.GetAll<BookDto>(GetAllBooksKey);

            if (result.Count > 0)
                return result.ToList();



            var bookList = (from book in _bookContext.Books
                            join author in _bookContext.Authors on book.AuthorId equals author.Id
                            join category in _bookContext.Categories on book.CategoryId equals category.Id
                            select new BookDto
                            {
                                Id = book.Id,
                                CategoryId = book.CategoryId,
                                CategoryName = category.Name,
                                AuthorId = book.AuthorId,
                                AuthorName = author.Name,
                                Name = book.Name,
                                Price = book.Price,
                                CreatedDate = book.CreatedDate
                            }).ToList();

            if (bookList != null)
            {
                foreach (var book in bookList)
                {
                    var cacheKey = string.Format(BookKey, book.Id);
                    _redisCacheService.Set(cacheKey, book);
                }
            }

            return bookList ?? new List<BookDto>();
        }

        public List<BookDto> InsertBook(BookDto book)
        {
            var author = _authorRepository.GetById(book.AuthorId);
            var category = _categoryRepository.GetById(book.CategoryId);
        
            var data = _mapper.Map<Book>(book);
            _bookRepository.Insert(data);

            //Yeni kayıt redise ekleniyor.
            var cacheKey = string.Format(BookKey, data.Id);
            var mappedData = _mapper.Map<BookDto>(data);
            mappedData.AuthorName = author.Name;
            mappedData.CategoryName = category.Name;
            _redisCacheService.Set(cacheKey, mappedData);

            var responseData = GetBookList();
            return responseData;
        }

        public List<BookDto> UpdateBook(BookDto book)
        {
            List<BookDto> response = new();
            var updatedModel = _mapper.Map<Book>(book);

            var currentModel = _bookRepository.GetById(updatedModel.Id);
            if (currentModel != null)
            {
                _bookRepository.UpdateMatchEntity(currentModel, updatedModel);

                //Redis Kayıt
                var cacheKey = string.Format(BookKey, updatedModel.Id);
                var mappedData = _mapper.Map<BookDto>(updatedModel);
                _redisCacheService.Set(cacheKey, mappedData);

                response = GetBookList();
            }
            else
            {
                throw new NotImplementedException();
            }

            return response;
        }

        public List<BookDto> DeleteBook(BookDto book)
        {
            var mappedData = _mapper.Map<Book>(book);
            _bookRepository.Delete(mappedData);

            //Kayıt redisten siliniyor
            var cacheKey = string.Format(BookKey, mappedData.Id);
            _redisCacheService.Remove(cacheKey);

            var response = GetBookList();

            return response;
        }
    }
}
