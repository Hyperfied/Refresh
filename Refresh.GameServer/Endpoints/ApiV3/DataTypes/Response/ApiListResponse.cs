using Refresh.GameServer.Database;

namespace Refresh.GameServer.Endpoints.ApiV3.DataTypes.Response;

/// <summary>
/// A response from the API containing information about a list and part of it's contents.
/// </summary>
[JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class ApiListResponse<TData> : ApiResponse<List<TData>> where TData : class
{
    public ApiListResponse(IEnumerable<TData> data, ApiListInformation info) : base(data.ToList())
    {
        this.ListInfo = info;
    }
    
    public ApiListResponse(IEnumerable<TData> data) : base(data.ToList())
    {
        ApiListInformation info = new()
        {
            TotalItems = this.Data!.Count,
            NextPageIndex = -1, // Probably doesn't support pagination
        };

        this.ListInfo = info;
    }

    public ApiListResponse(ApiError error) : base(error)
    {
        this.ListInfo = null;
    }

    public static implicit operator ApiListResponse<TData>(DatabaseList<TData> list) =>
        new(list.Items, new ApiListInformation
        {
            TotalItems = list.TotalItems,
            NextPageIndex = list.NextPageIndex,
        });
    
    public static implicit operator ApiListResponse<TData>(ApiError error) => new(error);

    [JsonProperty("listInfo")] public ApiListInformation? ListInfo { get; set; }
}