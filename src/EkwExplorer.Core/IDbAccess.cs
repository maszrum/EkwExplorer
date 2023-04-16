using System.Data.Common;

namespace EkwExplorer.Core;

public interface IDbAccess : IDisposable, IAsyncDisposable
{
    DbConnection Db { get; }

    ISqlQueries Queries { get; }

    Task ConnectAsync();
}
