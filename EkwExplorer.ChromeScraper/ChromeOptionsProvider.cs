using OpenQA.Selenium.Chrome;

namespace EkwExplorer.ChromeScraper;

internal class ChromeOptionsProvider
{
    private const int MinWindowWidth = 400;
    private const int MaxWindowWidth = 1800;
    private const int MinWindowHeight = 400;
    private const int MaxWindowHeight = 1000;
		
    private static readonly string[] UserAgents =
    {
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:53.0) Gecko/20100101 Firefox/53.0",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393",
        "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.0; Trident/5.0;  Trident/5.0)",
        "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0; MDDCJS)",
        "Mozilla/5.0 (compatible, MSIE 11, Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko"
    };
		
    private int _userAgentIndex;
    private readonly Random _random = new Random(DateTime.Now.Millisecond);
		
    public ChromeOptions Get()
    {
        var options = new ChromeOptions();
        options.AddArgument($"--user-agent={GetUserAgent()}");
        options.AddArgument($"--window-size={GetWindowSize()}");
        options.AddExcludedArgument("enable-automation");
			
        return options;
    }

    private string GetUserAgent()
    {
        var result = UserAgents[_userAgentIndex];
			
        _userAgentIndex++;
        if (_userAgentIndex >= UserAgents.Length)
        {
            _userAgentIndex = 0;
        }
			
        return result;
    }
		
    private string GetWindowSize()
    {
        var width = _random.Next(MinWindowWidth, MaxWindowWidth);
        var height = _random.Next(MinWindowHeight, MaxWindowHeight);
        return $"{width},{height}";
    }
}