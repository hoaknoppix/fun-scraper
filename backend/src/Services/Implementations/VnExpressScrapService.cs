using System.Text.Json;

public class VnExpressScrapService : IWebScrapService
{
    private readonly HttpClient _httpClient;

    public VnExpressScrapService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Int32> GetTotalLike(Int32 articleId, Int32 siteId)
    {
        string url =
            $"https://usi-saas.vnexpress.net/index/get?offset=0&frommobile=0&sort_by=like&is_onload=0&objectid={articleId}&objecttype=1&siteid={siteId}";
        var jsonString = await _httpClient.GetStringAsync(url);

        using (JsonDocument doc = JsonDocument.Parse(jsonString))
        {
            if (doc.RootElement.TryGetProperty("data", out JsonElement fieldValue))
            {
                var totalLike = 0;
                if (
                    fieldValue.TryGetProperty("items", out JsonElement fieldArray)
                    && fieldArray.ValueKind == JsonValueKind.Array
                )
                {
                    foreach (JsonElement element in fieldArray.EnumerateArray())
                    {
                        if (element.TryGetProperty("userlike", out JsonElement userLike))
                        {
                            totalLike += userLike.GetInt32();
                        }
                    }
                }
                return totalLike;
            }
            else
            {
                return -1;
            }
        }
    }

    public virtual async Task<List<Content>> GetContent()
    {
        string url =
            "https://alpha3-api3.vnexpress.net/api/mobile?type=homev5&app_id=9e304d&offset=0";

        var jsonString = await _httpClient.GetStringAsync(url);

        using (JsonDocument doc = JsonDocument.Parse(jsonString))
        {
            if (
                doc.RootElement.TryGetProperty("data", out JsonElement fieldArray)
                && fieldArray.ValueKind == JsonValueKind.Array
            )
            {
                var data = new List<Content>();

                foreach (JsonElement element in fieldArray.EnumerateArray())
                {
                    if (
                        element.TryGetProperty("article_id", out JsonElement id)
                        && element.TryGetProperty("site_id", out JsonElement siteId)
                        && element.TryGetProperty("title", out JsonElement title)
                        && element.TryGetProperty("share_url", out JsonElement shareUrl)
                    )
                    {
                        var totalLike = await GetTotalLike(id.GetInt32(), siteId.GetInt32());
                        var content = new Content(
                            totalLike,
                            title.GetString() ?? "",
                            shareUrl.GetString() ?? ""
                        );
                        data.Add(content);
                    }
                }
                data = data.OrderByDescending(content => content.like).ToList();
                return data;
            }
            throw new JsonException("The format is changed and does not contains 'data' element!");
        }
    }
}
