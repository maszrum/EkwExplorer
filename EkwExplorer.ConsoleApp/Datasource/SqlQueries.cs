using System;
using System.Linq;
using EkwExplorer.Datasource.Entities;

namespace EkwExplorer.Datasource
{
    internal static class SqlQueries
    {
        private const string BookTableName = "Book";
        private const string PropertyTableName = "PropertyNumber";
        
        public static string AddBook { get; } 
            = CreateInsertQueryForEntity<BookEntity>(BookTableName);

        public static string IsAnyNotFilled { get; }
            = $"SELECT EXISTS(SELECT 1 FROM `{BookTableName}` WHERE `Filled` = 0 LIMIT 1);";

        public static string GetRandomNotFilledBook { get; } 
            = $"SELECT * FROM `{BookTableName}` WHERE `Filled` = 0 ORDER BY RANDOM() LIMIT 1;";

        public static string UpdateBook { get; }
            = CreateUpdateQueryForEntity<BookEntity>(BookTableName);

        public static string AddProperty { get; }
            = CreateInsertQueryForEntity<PropertyNumberEntity>(PropertyTableName);

        private static string CreateInsertQueryForEntity<T>(string tableName)
        {
            var properties = typeof(T)
                .GetProperties()
                .Where(p => p.CanRead)
                .Select(p => p.Name)
                .ToArray();

            var fieldNames = string.Join(", ", properties.Select(p => $"`{p}`"));
            var valueNames = string.Join(", ", properties.Select(p => $"@{p}"));
            
            return $"INSERT INTO `{tableName}` ({fieldNames}) VALUES ({valueNames});";
        }

        private static string CreateUpdateQueryForEntity<T>(string tableName)
        {
            var idProperty = typeof(T)
                .GetProperty("Id");

            if (idProperty == null)
            {
                throw new ArgumentException(
                    "has no property Id with return type string", nameof(T));
            }
            
            var idGetter = idProperty.GetGetMethod();

            if (idGetter == null || idGetter.ReturnType != typeof(string))
            {
                throw new ArgumentException(
                    "has no getter Id with return type string", nameof(T));
            }
            
            var properties = typeof(T)
                .GetProperties()
                .Where(p => p.CanRead)
                .Where(p => p.Name != "Id")
                .Select(p => $"{p.Name} = @{p.Name}");

            var fields = string.Join(", ", properties);

            return $"UPDATE `{tableName}` SET {fields} WHERE `Id` = @Id";
        }
    }
}