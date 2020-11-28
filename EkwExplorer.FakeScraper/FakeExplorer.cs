using System.Threading;
using System.Threading.Tasks;
using EkwExplorer.Core;
using Serilog;

namespace EkwExplorer.FakeScraper
{
    public class FakeExplorer : IBooksExplorer
    {
        private readonly ILogger _logger;
        private readonly IBooksRepository _booksRepository;

        public FakeExplorer(ILogger logger, IBooksRepository booksRepository)
        {
            _logger = logger;
            _booksRepository = booksRepository;
        }

        public Task Explore(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
