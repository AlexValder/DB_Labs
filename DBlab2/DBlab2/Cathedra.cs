using System.Collections.Generic;

namespace DBlab2 {
    public class Cathedra {
        public int Id { get; set; }
        public int FacultyId { get; set; }
        public string Name { get; set; }

        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
