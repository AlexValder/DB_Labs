using System;

namespace DBlab2 {
    class Program {
        static void Main(string[] args) {
            using (var context = new LibraryContext()) {
                /*
                 * Here will be all the magic with THE DATABASE
                 * 'context' has all the tables (students, teachers and all other shit) and they can be referenced using it.
                 * Example: context.Students.Add(new Student() { FirstName = "Miku", LastName = "Hatsune", Year=3, GroupId=1}); //The rest of the fields is filled by the framework.
                 */
            }
            Console.Read();
        }
    }
}
