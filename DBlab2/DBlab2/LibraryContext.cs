using Microsoft.EntityFrameworkCore;

namespace DBlab2 {
    public class LibraryContext : DbContext {
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Cathedra> Cathedras { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<StudentCard> StudentCards { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<TeacherCard> TeacherCards { get; set; }
        public DbSet<LibraryEmployee> LibraryEmployees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder) {
            dbContextOptionsBuilder.UseSqlite("Data Source=Library.db");
        }
    }
}
