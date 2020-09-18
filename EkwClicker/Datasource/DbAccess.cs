using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace EkwClicker.Datasource
{
	internal static class DbAccess
	{
		private const string DatabasesDirectory = "dbo/Databases";
		private const string TablesSqlDirectory = "dbo/Tables";

		public static bool Exists(string database)
		{
			var dbFilePath = GetDatabasePath(database);

			if (!Directory.Exists(DatabasesDirectory))
			{
				Directory.CreateDirectory(DatabasesDirectory);
			}

			return File.Exists(dbFilePath);
		}

		public static async Task<DbConnection> Connect(string database)
		{
			var dbFilePath = GetDatabasePath(database);

			var connection = new DbConnection(dbFilePath);
			await connection.ConnectAsync();

			return connection;
		}

		public static async Task<DbConnection> Create(string database)
		{
			var connection = await Connect(database);

			foreach (var tableFile in Directory.EnumerateFiles(TablesSqlDirectory, "*.sqlite"))
			{
				var tableSql = await File.ReadAllTextAsync(tableFile);
				await connection.Db.ExecuteAsync(tableSql);
			}

			return connection;
		}

		public static void Remove(string database)
		{
			var dbFilePath = GetDatabasePath(database);
			File.Delete(dbFilePath);
		}
		
		public static IReadOnlyList<string>GetAvailableDatabases()
		{
			var files = Directory.EnumerateFiles(DatabasesDirectory, "*", SearchOption.TopDirectoryOnly);
			return files
				.Select(fn => Path.GetFileName(fn))
				.ToArray();
		}

		private static string GetDatabasePath(string database)
			=> Path.Combine(DatabasesDirectory, database);
	}
}
