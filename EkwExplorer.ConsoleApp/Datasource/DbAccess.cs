using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace EkwExplorer.Datasource
{
	internal class DbAccess : IDisposable, IAsyncDisposable
	{
		public DbConnection Db { get; }

		public DbAccess(string databaseFile)
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
