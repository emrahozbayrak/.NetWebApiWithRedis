using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IAuthorService
    {
        public AuthorDto GetAuthorById(int Id);
        public List<AuthorDto> GetAuthorList();
        public List<AuthorDto> InsertAuthor(AuthorDto author);
        public List<AuthorDto> UpdateAuthor(AuthorDto author);
    }
}
