using System.Data.Common;
using System.Data.SQLite;
using System.Threading.Tasks;
using EkwExplorer.Core;

namespace EkwExplorer.Persistence.SQLite
{
	// ReSharper disable once InconsistentNaming
	internal class SQLiteDbAccess : IDbAccess
	{
		public SQLiteDbAccess(string databaseFile, PersistenceConfiguration persistenceConfiguration)
		{
			Db = new SQLiteConnection("Data Source=" + databaseFile);

			Queries = new SqlQueries(persistenceConfiguration);
		}

		public DbConnection Db { get; }
		public ISqlQueries Queries { get; }

		public Task ConnectAsync()
		{
			return Db.OpenAsync();
		}

		public void Dispose()
		{
			Db.Dispose();
		}

		public ValueTask DisposeAsync()
		{
			return Db.DisposeAsync();
		}
	}
}
