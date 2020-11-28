using System.Threading;
using System.Threading.Tasks;

namespace EkwExplorer.Core
{
    public interface IBooksExplorer
    {
        Task Explore(CancellationToken cancellationToken);
    }
}