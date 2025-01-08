using System.Text.RegularExpressions;
using HtmlAgilityPack;

public class TuoiTreScrapService : IWebScrapService
{
    private readonly HttpClient _httpClient;

    public TuoiTreScrapService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Int32> GetTotalLike(String url)
    {
        var html = await _httpClient.GetStringAsync(url);
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
        string pattern = @"<span class=""total"">(\d+)</span";

        var totalLike = 0;
        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(html);
        foreach (Match match in matches)
        {
            int number = int.Parse(match.Groups[1].Value);
            totalLike += number;
        }
        return totalLike;
    }

    public virtual async Task<List<Content>> GetContent()
    {
        string url = "https://tuoitre.vn/tin-moi-nhat.htm";

        var html = await _httpClient.GetStringAsync(url);
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
        if (htmlDoc.DocumentNode.SelectSingleNode("//div[@id='load-list-news']") == null)
        {
            return new List<Content>();
        }
        var html1 = htmlDoc
                    .DocumentNode
                    .SelectSingleNode("//div[@id='load-list-news']").OuterHtml;
        htmlDoc.LoadHtml(html1);
        var items = htmlDoc.DocumentNode.SelectNodes("//div[@class='box-category-item']").ToList();
        var data = new List<Content>();
        foreach (var item in items)
        {
            var shareUrl =
                "https://tuoitre.vn" + item.SelectSingleNode("a").Attributes["href"].Value;
            var title = item.SelectSingleNode("a").Attributes["title"].Value;
            var totalLike = await GetTotalLike(shareUrl);
            var content = new Content(
                            totalLike,
                            title,
                            shareUrl
                        );
            data.Add(content);
        }
        data = data.OrderByDescending(content => content.like).ToList();
        return data;
    }
}
