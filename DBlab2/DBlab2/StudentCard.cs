namespace DBlab2 {
    public class StudentCard {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TakenDate { get; set; }
        public int DueDate { get; set; }
        public int ReturnedDate { get; set; }
        public int BookId { get; set; }
        public int LibraryEmployeeId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Book Book { get; set; }
        public virtual LibraryEmployee LibraryEmployee { get; set; }
    }
}
