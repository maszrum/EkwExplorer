using System.Linq;
using EkwClicker.Datasource.Entities;

namespace EkwClicker.Datasource
{
    internal static class SqlQueries
    {
        private const string BookTableName = "Book";
        
        public static string AddBook { get; } 
            = CreateInsertQueryForEntity<BookEntity>(BookTableName);
        
        public static string GetRandomNotFilledBook { get; } 
            = $"SELECT * FROM `{BookTableName}` WHERE `Filled` = 0 ORDER BY RANDOM() LIMIT 1;";
        
        private static string CreateInsertQueryForEntity<T>(string tableName)
        {
            var properties = typeof(BookEntity)
                .GetProperties()
                .Where(p => p.CanRead)
                .Select(p => p.Name)
                .ToArray();
            
            var fieldNames = string.Join(", ", properties.Select(p => $"`{p}`"));
            var valueNames = string.Join(", ", properties.Select(p => $"@{p}"));
            
            return $"INSERT INTO `{tableName}` ({fieldNames}) VALUES ({valueNames});";
        }
    }
}