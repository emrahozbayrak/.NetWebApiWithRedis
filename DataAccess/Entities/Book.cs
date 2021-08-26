using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public partial class Book: BaseEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
    }
}
