using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DBLab2.Common {
    internal static class GlobalContainer {
        private static readonly StringComparer Comparer = StringComparer.InvariantCultureIgnoreCase;

        private static ImmutableDictionary<string, string[]>? DictOfLists
            = new Dictionary<string, string[]>(Comparer) {
                ["Authors"] = new[] {
                    "Id",
                    "FirstName",
                    "LastName",
                },
                ["Books"] = new[] {
                    "Id",
                    "Title",
                    "Year",
                    "PublisherId",
                },
                ["BookAuthors"] = new[] {
                    "Id",
                    "BookId",
                    "AuthorId",
                },
                ["Cathedras"] = new[] {
                    "Id",
                    "FacultyId",
                    "Name",
                },
                ["Faculties"] = new[] {
                    "Id",
                    "Name",
                },
                ["Groups"] = new[] {
                    "Id",
                    "Number",
                    "SpecialityId",
                    "CathedraId",
                },
                ["LibraryEmployees"] = new[] {
                    "Id",
                    "FirstName",
                    "LastName",
                    "PositionId",
                },
                ["Positions"] = new[] {
                    "Id",
                    "PosName",
                },
                ["Publishers"] = new[] {
                    "Id",
                    "Name"
                },
                ["Specialities"] = new[] {
                    "Id",
                    "Name",
                    "Number",
                    "FacultyId",
                },
                ["Students"] = new[] {
                    "Id",
                    "FirstName",
                    "LastName",
                    "Year",
                    "GroupId",
                },
                ["StudentCards"] = new[] {
                    "Id",
                    "StudentId",
                    "TakenDate",
                    "DueDate",
                    "ReturnedDate",
                    "BookId",
                    "LibraryEmployeeId",
                },
                ["Teachers"] = new[] {
                    "Id",
                    "FirstName",
                    "SecondName",
                    "CathedraId",
                },
                ["TeacherCards"] = new[] {
                    "Id",
                    "TeacherId",
                    "TakenDate",
                    "ReturnedDate",
                    "LibraryEmployeeId",
                    "BookId",
                },
            }.ToImmutableDictionary(Comparer);

        public static bool TryLoadDb(string path) {
            var backup1 = DictOfLists;
            var backup2 = BdSelected;
            try {
                // 1. Try to find file in `path`
                // 2. Verify it's an actual DB
                // 3. Create a Dictionary with
                // tables and their fields in this
                // database
                BdSelected = Path.GetFileName(path);
            }
            catch (Exception ex) {
                DictOfLists = backup1;
                BdSelected = backup2;
                Printer.Error(ex, "Failed to load DB");
                return false;
            }

            return true;
        }

        public static string BdSelected { get; private set; } = "None";
        public static int TableCount => DictOfLists?.Count ?? 0;

        public static int FieldCount(string tableName) => DictOfLists?[tableName].Length ?? 0;

        public static IEnumerable<string> Tables =>
            (DictOfLists?.Keys ?? new List<string>()).ToImmutableList();

        public static IEnumerable<string> Fields(string tableName)
            => TableExists(tableName)
                ? DictOfLists![tableName].ToImmutableList()
                : Array.Empty<string>().ToImmutableList();

        public static bool TableExists(string tableName) => DictOfLists?.ContainsKey(tableName) ?? false;

        public static bool FieldExists(string tableName, in string field)
            => DictOfLists?[tableName].Contains(field, Comparer) ?? false;

        public static bool FieldsExist(string tableName, in IEnumerable<string> fields)
            => fields.All(field => FieldExists(tableName, field));
    }
}
