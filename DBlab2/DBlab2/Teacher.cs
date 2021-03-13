namespace DBlab2 {
    public class Teacher {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int CathedraId { get; set; }

        public virtual Cathedra Cathedra { get; set; }
    }
}
