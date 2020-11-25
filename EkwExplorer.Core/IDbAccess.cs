using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace EkwExplorer.Core
{
    public interface IDbAccess : IDisposable, IAsyncDisposable
    {
        DbConnection Db { get; }
        Task ConnectAsync();
    }
}