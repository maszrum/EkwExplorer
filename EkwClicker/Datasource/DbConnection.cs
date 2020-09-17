using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace EkwClicker.Datasource
{
	internal class DbConnection : IDisposable, IAsyncDisposable
	{
		public SQLiteConnection Db { get; }

		public DbConnection(string databaseFile)
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
