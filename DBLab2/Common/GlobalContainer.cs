using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Common {
    public static class GlobalContainer {
        private static readonly StringComparer Comparer = StringComparer.InvariantCultureIgnoreCase;

        private static ImmutableDictionary<string, string[]>? _dictOfLists;

        public static void CacheMetadata(IDictionary<string, string[]> data) {
            _dictOfLists = data.ToImmutableDictionary(Comparer);
        }

        public static string BdSelected { get; set; } = "None";
        public static int TableCount => _dictOfLists?.Count ?? 0;

        public static int FieldCount(string tableName) => _dictOfLists?[tableName].Length ?? 0;

        public static IEnumerable<string> Tables =>
            (_dictOfLists?.Keys ?? new List<string>()).ToImmutableList();

        public static IEnumerable<string> Fields(string tableName)
            => TableExists(tableName)
                ? _dictOfLists![tableName].ToImmutableList()
                : Array.Empty<string>().ToImmutableList();

        public static bool TableExists(string tableName) => _dictOfLists?.ContainsKey(tableName) ?? false;

        public static bool FieldExists(string tableName, in string field)
            => _dictOfLists?[tableName].Contains(field, Comparer) ?? false;

        public static bool FieldsExist(string tableName, in IEnumerable<string> fields)
            => fields.All(field => FieldExists(tableName, field));
    }
}
