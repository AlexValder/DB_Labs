using System.Collections.Generic;

namespace DBlab2 {
    public class Faculty {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Speciality> Specialities { get; set; }
        public virtual ICollection<Cathedra> Cathedras { get; set; }
    }
}
