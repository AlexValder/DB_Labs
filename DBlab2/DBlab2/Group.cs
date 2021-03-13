namespace DBlab2 {
    public class Group {
        public int Id { get; set; }
        public int? Number { get; set; }
        public int SpecialityId { get; set; }
        public int CathedraId { get; set; }

        public virtual Cathedra Cathedra { get; set; }
        public virtual Speciality Speciality { get; set; }
    }
}
