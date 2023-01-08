using Refit;

namespace ApiClasses
{
    public interface IJsonPlaceHolderApi
    {
        [Get("/posts")]
        Task<List<Post>> GetPostsAsync();

        [Get("/posts/{id}")]
        Task<Post> GetPostAsync(int id);

        [Get("/posts/{id}")]
        Task<ApiResponse<Post>> GetPostResponseAsync(int id);

        [Post("/posts")]
        Task<Post> CreatePostAsync(Post post);

        [Post("/posts")]
        Task<ApiResponse<Post>> CreatePostResponseAsync(Post post);
    }
}