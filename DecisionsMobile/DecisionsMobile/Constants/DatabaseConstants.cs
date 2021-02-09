namespace DecisionsMobile.Constants
{
    class DatabaseConstants
    {
        public const string DatabaseFilename = "OfflineData.db";
        public const SQLite.SQLiteOpenFlags Flags =
                            // open the database in read/write mode
                            SQLite.SQLiteOpenFlags.ReadWrite |
                            // create the database if it doesn't exist
                            SQLite.SQLiteOpenFlags.Create |
                            // enable multi-threaded database access
                            SQLite.SQLiteOpenFlags.SharedCache;
    }
}
