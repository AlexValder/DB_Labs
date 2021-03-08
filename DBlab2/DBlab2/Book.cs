using System.Collections.Generic;

namespace DBlab2 {
    public class Book {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public int PublisherId { get; set; }

        public virtual Publisher Publisher { get; set; }
    }
}
