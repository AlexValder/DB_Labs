using System.ComponentModel.DataAnnotations;

namespace DBlab2 {
    public class StudentCard {
        public int Id { get; set; }
        public int StudentId { get; set; }
        [Required] public int TakenDate { get; set; }
        [Required] public int DueDate { get; set; }
        public int? ReturnedDate { get; set; }
        public int BookId { get; set; }
        public int LibraryEmployeeId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Book Book { get; set; }
        public virtual LibraryEmployee LibraryEmployee { get; set; }
    }
}
