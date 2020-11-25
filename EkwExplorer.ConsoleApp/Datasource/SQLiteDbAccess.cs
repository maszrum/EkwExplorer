using System.Data.Common;
using System.Data.SQLite;
using System.Threading.Tasks;
using EkwExplorer.Core;

namespace EkwExplorer.Datasource
{
	// ReSharper disable once InconsistentNaming
	internal class SQLiteDbAccess : IDbAccess
	{
		public DbConnection Db { get; }

		public SQLiteDbAccess(string databaseFile)
		{
			Db = new SQLiteConnection("Data Source=" + databaseFile);
		}

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
