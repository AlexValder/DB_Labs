using System.Collections.Generic;

namespace DBlab2 {
    public class Book {
        public int Id { get; set; }
        public int Title { get; set; }
        public int Year { get; set; }
        public int PublisherId { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public virtual ICollection<StudentCard> StudentCards { get; set; }
        public virtual ICollection<TeacherCard> TeacherCards { get; set; }
    }
}
