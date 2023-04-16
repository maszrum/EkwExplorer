using Dapper;
using EkwExplorer.Core;

namespace EkwExplorer.Persistence.SQLite;

public class DbManager
{
    public DbManager(PersistenceConfiguration persistenceConfiguration)
    {
        _persistenceConfiguration = persistenceConfiguration;
    }

    public string DatabaseFileExtension { get; set; } = ".db";

    public string DatabasesDirectory { get; set; } = "dbo/Databases";

    public string TablesSqlDirectory { get; set; } = "dbo/Tables";

    private readonly PersistenceConfiguration _persistenceConfiguration;

    public bool Exists(string database)
    {
        var dbFilePath = GetDatabasePath(database);

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

    public void Remove(string database)
    {
        var dbFilePath = GetDatabasePath(database);
        File.Delete(dbFilePath);
    }

    public IReadOnlyList<string> GetAvailableDatabases()
    {
        var filePattern = string.Concat("*", DatabaseFileExtension);
        var files = Directory.EnumerateFiles(DatabasesDirectory, filePattern, SearchOption.TopDirectoryOnly);
        return files
            .Select(Path.GetFileName)
            .ToArray();
    }

    private async Task<string> PrepareSeedSqlFromFile(string tableFile)
    {
        var fileContent = await File.ReadAllTextAsync(tableFile);

        var properties = typeof(PersistenceConfiguration)
            .GetProperties()
            .Where(pi => pi.PropertyType == typeof(string) && pi.GetGetMethod() != null)
            .ToArray();

        foreach (var pi in properties)
        {
            var key = $"[{pi.Name}]";
            if (fileContent.Contains(key, StringComparison.Ordinal))
            {
                var value = (string)pi.GetValue(_persistenceConfiguration);
                fileContent = fileContent.Replace(key, value);
            }
        }

        return fileContent;
    }

    private string GetDatabasePath(string database)
    {
        var databaseFilename = IsFilenameWithExtension(database)
            ? database
            : string.Concat(database, DatabaseFileExtension);

        return Path.Combine(DatabasesDirectory, databaseFilename);
    }

    private bool IsFilenameWithExtension(string fileName) =>
        fileName.EndsWith(DatabaseFileExtension);
}
