using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlab2
{
    static class GlobalContainer
    {
        private static Dictionary<string, string[]> DictOfLists = new Dictionary<string, string[]>
        {
            ["Author"] = new string[] { "FirstName", "LastName" },
            ["Book"] = new string[] { "Title", "Year", "PublisherId", "Publisher" },
            ["BookAuthor"] = new string[] { "BookId", "AuthorId", "Book", "Author" },
            ["Cathedra"] = new string[] { "FacultyId", "Name", "Faculty" },
            ["Faculty"] = new string[] { "Name" },
            ["Group"] = new string[] { "Number", "SpecialityId", "CathedraId", "Cathedra", "Speciality" },
            ["LibraryContext"] = new string[] { "Specialities", "Faculties", "Groups", "Cathedras",
                "Students", "Authors", "Books", "BookAuthors", "Publishers", "StudentCards", "Teachers", "Positions",
                "TeacherCards", "LibraryEmployees"},
            ["LibraryEmployee"] = new string[] { "FirstName", "LastName", "PositionId", "Position" },
            ["Position"] = new string[] { "PosName" },
            ["Program"] = new string[] { "" },
            ["Publisher"] = new string[] { "Name" },
            ["Speciality"] = new string[] { "Name", "Number", "FacultyId", "Faculty" },
            ["Student"] = new string[] { "FirstName", "LastName", "Year", "GroupId", "Group" },
            ["StudentCard"] = new string[] { "StudentId", "TakenDate", "DueDate", "ReturnedDate",
                "BookId", "LibraryEmployeeId", "Student", "Book", "LibraryEmployee" },
            ["Teacher"] = new string[] { "FirstName", "SecondName", "CathedraId", "Cathedra" },
            ["TeacherCard"] = new string[] { "TeacherId", "TakenDate", "ReturnedDate", "LibraryEmployeeId",
                "BookId", "Teacher", "LibraryEmployee", "Book"}
        };
    }
}
