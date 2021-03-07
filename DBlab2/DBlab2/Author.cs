using System.Collections.Generic;

namespace DBlab2 {
    public class Author {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
