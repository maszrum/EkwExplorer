using System;
using System.Collections.Generic;
using System.Linq;
using EkwClicker.Datasource.Entities;

namespace EkwClicker.Datasource
{
    internal static class SqlQueries
    {
        private const string BookTableName = "Book";
        
        public static string AddBook { get; } 
            = CreateInsertQueryForEntity<BookEntity>(BookTableName);

        public static string IsAnyNotFilled { get; }
            = $"SELECT EXISTS(SELECT 1 FROM `{BookTableName}` WHERE `Filled` = 0 LIMIT 1);";

        public static string GetRandomNotFilledBook { get; } 
            = $"SELECT * FROM `{BookTableName}` WHERE `Filled` = 0 ORDER BY RANDOM() LIMIT 1;";

        public static string UpdateBook { get; }
            = CreateUpdateQueryForEntity<BookEntity>(BookTableName);

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
                .GetProperty("Id")
                .GetGetMethod();

            if (idProperty == null || idProperty.ReturnType != typeof(string))
            {
                throw new ArgumentException(
                    "has no property Id with return type string", nameof(T));
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