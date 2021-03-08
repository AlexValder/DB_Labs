using System.ComponentModel.DataAnnotations;

namespace DBlab2 {
    public class TeacherCard {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        [Required] public int TakenDate { get; set; }
        public int? ReturnedDate { get; set; }
        public int LibraryEmployeeId { get; set; }
        public int BookId { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual LibraryEmployee LibraryEmployee { get; set; }
        public virtual Book Book { get; set; }
    }
}
