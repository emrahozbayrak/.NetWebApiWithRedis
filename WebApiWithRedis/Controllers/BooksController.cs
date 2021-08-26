using Core.Models;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiWithRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{Id}")]
        public BookDto GetById(int Id)
        {
            return _bookService.GetBookById(Id);
        }

        [HttpGet("GetBookList")]
        public IEnumerable<BookDto> GetBookList()
        {
            return _bookService.GetBookList();
        }

        [HttpPost("InsertBook")]
        public IEnumerable<BookDto> InsertBook([FromBody] BookDto model)
        {
            if (model != null)
            {
                if (model.Id > 0)
                    return _bookService.UpdateBook(model);
                else
                    return _bookService.InsertBook(model);
            }
            return new List<BookDto>();
        }

        [HttpDelete("DeleteBook")]
        public IEnumerable<BookDto> DeleteBook([FromBody] BookDto model)
        {
            try
            {
                return _bookService.DeleteBook(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookModelExists(model.Id))
                { return new List<BookDto>(); }
                else
                { throw; }
            }
        }

        private bool BookModelExists(int id)
        {
            return _bookService.GetBookList().Any(e => e.Id == id);
        }
    }
}
