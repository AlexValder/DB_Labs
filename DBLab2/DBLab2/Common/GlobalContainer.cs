using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace DBLab2.Common {
    static class GlobalContainer {
        private static readonly StringComparer Comparer = StringComparer.InvariantCultureIgnoreCase;
        
        private static readonly ImmutableDictionary<string, string[]> DictOfLists
            = new Dictionary<string, string[]>(Comparer) {
                ["Author"] = new[] {
                    "FirstName",
                    "LastName",
                },
                ["Book"] = new[] {
                    "Title",
                    "Year",
                    "PublisherId",
                },
                ["BookAuthor"] = new[] {
                    "BookId",
                    "AuthorId",
                },
                ["Cathedra"] = new[] {
                    "FacultyId",
                    "Name",
                },
                ["Faculty"] = new[] {
                    "Name",
                },
                ["Group"] = new[] {
                    "Number",
                    "SpecialityId",
                    "CathedraId",
                },
                ["LibraryEmployee"] = new[] {
                    "FirstName",
                    "LastName",
                    "PositionId",
                },
                ["Position"] = new[] {
                    "PosName",
                },
                ["Publisher"] = new[] {
                    "Name"
                },
                ["Speciality"] = new[] {
                    "Name",
                    "Number",
                    "FacultyId",
                },
                ["Student"] = new[] {
                    "FirstName",
                    "LastName",
                    "Year",
                    "GroupId",
                },
                ["StudentCard"] = new[] {
                    "StudentId",
                    "TakenDate",
                    "DueDate",
                    "ReturnedDate",
                    "BookId",
                    "LibraryEmployeeId",
                },
                ["Teacher"] = new[] {
                    "FirstName",
                    "SecondName",
                    "CathedraId",
                },
                ["TeacherCard"] = new[] {
                    "TeacherId",
                    "TakenDate",
                    "ReturnedDate",
                    "LibraryEmployeeId",
                    "BookId",
                },
            }.ToImmutableDictionary(Comparer);

        public static bool TableExists(string tableName) => DictOfLists.Keys.Contains(tableName, Comparer);

        public static bool FieldExists(string tableName, string field)
            => TableExists(tableName) && DictOfLists[tableName].Contains(field, Comparer);

        public static bool FieldsExist(string tableName, IEnumerable<string> fields)
            => fields.All((string field) => FieldExists(tableName, field));
    }
}
