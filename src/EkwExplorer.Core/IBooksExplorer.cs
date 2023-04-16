namespace EkwExplorer.Core;

public interface IBooksExplorer
{
    Task Explore(CancellationToken cancellationToken);
}
