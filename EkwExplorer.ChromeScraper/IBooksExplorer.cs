using System.Threading.Tasks;

namespace EkwExplorer.ChromeScraper
{
	public interface IBooksExplorer
	{
		Task Explore();
		Task Open();
	}
}