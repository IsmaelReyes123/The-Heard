using System;
using System.Data.SQLite;
using System.IO;

namespace SQLiteUtil
{
    public static class SQLiteConnectionExtensions
    {
        private static string _connectionString;

        static SQLiteConnectionExtensions()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dungeon.db");
            _connectionString = $"Data Source={dbPath};Version=3;";
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString);
        }
    }
}
