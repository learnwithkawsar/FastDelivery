using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace FastDelivery.Framework.Core.Pagination;


//[JsonConverter(typeof(PagedListConverter<T>))]
public class PagedList<T>
{
    public IList<T> Data { get; }
    public PagedList(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        if (totalItems > 0)
        {
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        }
        Data = items as IList<T> ?? new List<T>(items);
    }

    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalItems { get; }
    public bool IsFirstPage => PageNumber == 1;
    public bool IsLastPage => PageNumber == TotalPages && TotalPages > 0;

    public static PagedList<T> FromJson(string json, JsonSerializerSettings settings = null)
    {
        var jsonObject = JObject.Parse(json);
        var data = jsonObject["data"]?.ToObject<IList<T>>(JsonSerializer.CreateDefault(settings));
        int totalItems = jsonObject["totalItems"]!.Value<int>();
        int pageNumber = jsonObject["pageNumber"]!.Value<int>();
        int pageSize = jsonObject["pageSize"]!.Value<int>();

        return new PagedList<T>(data!, totalItems, pageNumber, pageSize);
    }
}


