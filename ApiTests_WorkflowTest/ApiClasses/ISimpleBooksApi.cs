using Refit;

namespace ApiClasses
{
    public interface ISimpleBooksApi
    {
        [Post("/api-clients")]
        Task<string> RegisterAsync([Body] Dictionary<string, string> clientInfo);

        [Post("/api-clients")]
        Task<ApiResponse<string>> GetRegisterResponseAsync([Body] Dictionary<string, string> clientInfo);

        [Headers("Authorization: Bearer")]
        [Post("/orders")]
        Task<Dictionary<string, object>> PostBookOrderAsync([Body] Dictionary<string, object> order);

        [Post("/orders")]
        Task<Dictionary<string, object>> PostBookOrderAsync([Body] Dictionary<string, object> order, [Authorize("Bearer")] string token);

        [Headers("Authorization: Bearer")]
        [Patch("/orders/{orderId}")]
        Task<ApiResponse<string>> UpdateOrderAsync([AliasAs("orderId")] object orderId, [Body] string customerName);

        [Headers("Authorization: Bearer")]
        [Delete("/orders/{orderId}")]
        Task<ApiResponse<string>> DeleteOrderAsync([AliasAs("orderId")] object orderId);
    }
}
