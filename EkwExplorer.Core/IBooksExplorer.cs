using System.Threading.Tasks;

namespace EkwExplorer.Core
{
    public interface IBooksExplorer
    {
        Task Explore();
        Task Open();
    }
}