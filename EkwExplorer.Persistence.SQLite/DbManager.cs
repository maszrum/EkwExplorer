using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EkwExplorer.Core;

namespace EkwExplorer.Persistence.SQLite
{
    public class DbManager
    {
        private const string DatabasesDirectory = "dbo/Databases";
        private const string TablesSqlDirectory = "dbo/Tables";

        public DbManager(PersistenceConfiguration persistenceConfiguration)
        {
            _persistenceConfiguration = persistenceConfiguration;
        }

        private readonly PersistenceConfiguration _persistenceConfiguration;

        public bool Exists(string database)
        {
            var dbFilePath = GetDatabasePath(database);

            if (!Directory.Exists(DatabasesDirectory))
            {
                Directory.CreateDirectory(DatabasesDirectory);
            }

            return File.Exists(dbFilePath);
        }

        public async Task<IDbAccess> Connect(string database)
        {
            var dbFilePath = GetDatabasePath(database);

            var connection = new SQLiteDbAccess(dbFilePath, _persistenceConfiguration);
            await connection.ConnectAsync();

            return connection;
        }

        public async Task<IDbAccess> Create(string database)
        {
            var connection = await Connect(database);

            foreach (var tableFile in Directory.EnumerateFiles(TablesSqlDirectory, "*.sqlite"))
            {
                var tableSql = await PrepareSeedSqlFromFile(tableFile);

                await connection.Db.ExecuteAsync(tableSql);
            }

            return connection;
        }

        private async Task<string> PrepareSeedSqlFromFile(string tableFile)
        {
            var fileContent = await File.ReadAllTextAsync(tableFile);

            var properties = typeof(PersistenceConfiguration)
                .GetProperties()
                .Select(p => (PropertyInfo: p, GetMethod: p.GetGetMethod()))
                .Where(p => p.PropertyInfo.PropertyType == typeof(string) && p.GetMethod != null)
                .ToArray();

            foreach (var p in properties)
            {
                var key = $"[{p.PropertyInfo.Name}]";
                if (fileContent.Contains(key, StringComparison.Ordinal))
                {
                    var value = (string)p.PropertyInfo.GetValue(_persistenceConfiguration);
                    fileContent = fileContent.Replace(key, value);
                }
            }

            return fileContent;
        }

        public void Remove(string database)
        {
            var dbFilePath = GetDatabasePath(database);
            File.Delete(dbFilePath);
        }
        
        public IReadOnlyList<string>GetAvailableDatabases()
        {
            var files = Directory.EnumerateFiles(DatabasesDirectory, "*", SearchOption.TopDirectoryOnly);
            return files
                .Select(Path.GetFileName)
                .ToArray();
        }

        private string GetDatabasePath(string database)
            => Path.Combine(DatabasesDirectory, database);
    }
}
