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
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("{Id}")]
        public AuthorDto GetById(int Id)
        {
            return _authorService.GetAuthorById(Id);
        }

        [HttpGet("GetAuthorList")]
        public IEnumerable<AuthorDto> GetAuthorList()
        {
            return _authorService.GetAuthorList();
        }

        [HttpPost("InsertAuthor")]
        public IEnumerable<AuthorDto> InsertAuthor([FromBody] AuthorDto model)
        {
            if (model != null)
            {
                if (model.Id > 0)
                    return _authorService.UpdateAuthor(model);
                else
                    return _authorService.InsertAuthor(model);
            }
            return new List<AuthorDto>();
        }
    }
}
