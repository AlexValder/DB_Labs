namespace DBlab2 {
    public class LibraryEmployee {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PositionId { get; set; }

        public virtual Position Position { get; set; }
    }
}
