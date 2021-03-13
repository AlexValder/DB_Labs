namespace DBlab2 {
    public class Speciality {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Number { get; set; }
        public int FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }
    }
}
