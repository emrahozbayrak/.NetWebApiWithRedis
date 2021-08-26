using Core.Models;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IBookService
    {
        public BookDto GetBookById(int Id);
        public List<BookDto> GetBookList();
        public List<BookDto> InsertBook(BookDto book);
        public List<BookDto> UpdateBook(BookDto book);
        public List<BookDto> DeleteBook(BookDto book);
    }
}
