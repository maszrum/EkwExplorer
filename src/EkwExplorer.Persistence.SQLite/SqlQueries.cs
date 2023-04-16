using EkwExplorer.Core;
using EkwExplorer.Persistence.Entities;

namespace EkwExplorer.Persistence.SQLite;

internal class SqlQueries : ISqlQueries
{
    public SqlQueries(PersistenceConfiguration configuration)
    {
        AddBook = CreateInsertQueryForEntity<BookEntity>(configuration.BookTable);

        IsAnyNotFilled = $"SELECT EXISTS(SELECT 1 FROM `{configuration.BookTable}` WHERE `Filled` = 0 LIMIT 1);";

        GetRandomNotFilledBook = $"SELECT * FROM `{configuration.BookTable}` WHERE `Filled` = 0 ORDER BY RANDOM() LIMIT 1;";

        UpdateBook = CreateUpdateQueryForEntity<BookEntity>(configuration.BookTable);

        AddProperty = CreateInsertQueryForEntity<PropertyNumberEntity>(configuration.PropertyTable);
    }

    public string AddBook { get; }

    public string IsAnyNotFilled { get; }

    public string GetRandomNotFilledBook { get; }

    public string UpdateBook { get; }

    public string AddProperty { get; }

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
