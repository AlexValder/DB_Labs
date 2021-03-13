using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace DBlab2 {
    public class LibraryDbAdapter {
        private LibraryContext m_libraryContext;

        public LibraryDbAdapter(LibraryContext libraryContext) {
            m_libraryContext = libraryContext;
        }

        public bool TrySelect(in SqlCommand sqlCommand, out DataSet dataSet) {
            throw new NotImplementedException("TrySelect is not implemented yet.");
        }

        public bool TryUse(in SqlCommand sqlCommand, out DataSet dataSet) {
            throw new NotImplementedException("TryUse is not implemented yet.");
        }

        public bool TryShowTables(out IEnumerable<string> tableNames) {
            throw new NotImplementedException("TryShowTables is not implemented yet.");
        }

        public bool TryDescribe (in SqlCommand sqlCommand, out string description) {
            throw new NotImplementedException("TryDescribe is not implemented yet.");
        }

        public bool TryInsert(in SqlCommand sqlCommand) {
            throw new NotImplementedException("TryInsert is not implemented yet.");
        }

        public bool TryUpdate(in SqlCommand sqlCommand) {
            throw new NotImplementedException("TryUpdate is not implemented yet.");
        }

        public bool TryDelete(in SqlCommand sqlCommand) {
            throw new NotImplementedException("TryDelete is not implemented yet.");
        }
    }
}
