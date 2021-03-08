using System.Collections.Generic;

namespace DBlab2 {
    public class Student {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Year { get; set; }
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}
