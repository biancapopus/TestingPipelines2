using Xunit;
using Refit;
using System.Threading.Tasks;
using ApiClasses;
using System.Collections.Generic;
using System.Net;

namespace ApiTests
{
    public class SimpleBooksApiTests
    {
        ISimpleBooksApi simpleBooksApiWithToken = RestService.For<ISimpleBooksApi>("https://simple-books-api.glitch.me", new RefitSettings
        {
            AuthorizationHeaderValueGetter = () =>
            {
                return Task.FromResult("1501fc7a572005e6acff84a9bdbec4a7c3d53e66504cb6a4499885f49a42b5a0");
            }
        });

        ISimpleBooksApi simpleBooksApi = RestService.For<ISimpleBooksApi>("https://simple-books-api.glitch.me");
        object orderId;

        [Fact (Skip = "Already registered")]
        public async Task RegisterClient()
        {
            Dictionary<string, string> clientInfo = new Dictionary<string, string>()
            {
                { "clientName", "Gabriela" },
                { "clientEmail", "gabriela@example.com" }
            };

            var token = await simpleBooksApiWithToken.RegisterAsync(clientInfo);
        }

        [Fact]
        public async Task PostBookOrder_ApiWithToken()
        {
            var orderRequest = new Dictionary<string, object>()
            {
                { "bookId", 1 },
                { "customerName", "Ana" }
            };

            var order = await simpleBooksApiWithToken.PostBookOrderAsync(orderRequest);
        }

        [Fact]
        public async Task PostBookOrder_ApiWithoutToken()
        {
            var orderContent = new Dictionary<string, object>()
            {
                { "bookId", 1 },
                { "customerName", "Ana" }
            };

            var order = await simpleBooksApi.PostBookOrderAsync(orderContent, "1501fc7a572005e6acff84a9bdbec4a7c3d53e66504cb6a4499885f49a42b5a0");
        }

        [Fact]
        public async Task Post_Update_Delete_BookOrder()
        {
            var orderContent = new Dictionary<string, object>()
            {
                { "bookId", 1 },
                { "customerName", "Maria" }
            };

            var order = await simpleBooksApiWithToken.PostBookOrderAsync(orderContent);
            orderId = order["orderId"];

            var newCustomerName = "Ana";
            var updateOrderResponse = await simpleBooksApiWithToken.UpdateOrderAsync(orderId, newCustomerName);
            Assert.True(updateOrderResponse.IsSuccessStatusCode);

            var deleteOrderResponse = await simpleBooksApiWithToken.DeleteOrderAsync(orderId);
            Assert.True(deleteOrderResponse.IsSuccessStatusCode);
        }
    }
}
