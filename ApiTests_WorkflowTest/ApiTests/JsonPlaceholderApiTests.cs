using Xunit;
using Refit;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using ApiClasses;

namespace ApiTests
{
    public class JsonPlaceHolderApiTests
    {
        IJsonPlaceHolderApi jsonPlaceHolderApi = RestService.For<IJsonPlaceHolderApi>("https://jsonplaceholder.typicode.com");

        [Fact]
        public async Task GetPosts()
        {
            var posts = await jsonPlaceHolderApi.GetPostsAsync();
            var postsOrderedDescending = posts.OrderByDescending(x => x.Id).ToList();
            Assert.Equal(100, postsOrderedDescending[0].Id);
        }

        [Fact]
        public async Task GetPostWithId()
        {
            var post = await jsonPlaceHolderApi.GetPostAsync(5);
            Assert.Equal("nesciunt quas odio", post.Title);
        }

        [Fact]
        public async Task AddPost()
        {
            var post = await jsonPlaceHolderApi.CreatePostAsync(new Post { Title = "test", Body = "this is a test", UserId = 1 });
            Assert.Equal(101, post.Id);
        }

        [Fact]
        public async Task Response_CreatedStatusCodeWhenPosingNewPost()
        {

            var response = await jsonPlaceHolderApi.CreatePostResponseAsync(new Post { Title = "test", Body = "this is a test", UserId = 1 });
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Response_NotFoundStatusCodeWhenTryingToGetPostWithWrongId()
        {
            var response = await jsonPlaceHolderApi.GetPostResponseAsync(102);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}