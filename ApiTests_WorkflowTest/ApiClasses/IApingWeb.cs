using System.Collections.Specialized;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Refit;

namespace ApiClasses
{
    public interface IApingWeb
    {
        [Post("/register")]
        Task<ApiResponse<string>> RegisterAsync([Body] Dictionary<string, object> clientInfo);

        [Post("/login")]
        Task<Dictionary<string, object>> LoginAsync([Body] Dictionary<string, object> clientInfo);

        [Post("/login")]
        Task<ApiResponse<string>> LoginResponseAsync([Body] Dictionary<string, object> userInfo);

        [Headers("Authorization: Bearer")]
        [Get("/article/1")]
        Task<List<object>> GetArticleAsync();

        [Get("/articles")]
        Task<ArticlesResult> GetArticlesAsync([Authorize("Bearer")] string token);

        [Get("/article/1")]
        Task<ArticlesResult> GetArticleAsync([Authorize("Bearer")] string token);

        [Post("/article/create")]
        Task<ApiResponse<string>> CreateArticleApiResponseAsync([Authorize("Bearer")] string token, [Body] Article article);

        [Post("/article/create")]
        Task<ArticlesResult> CreateArticleAsync([Authorize("Bearer")] string token, [Body] Article article);
    }
}