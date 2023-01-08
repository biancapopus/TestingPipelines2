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
                return Task.FromResult("42e94e0f99efe02a8181930a30fbe83970a6601d3ad9dea8b660fb0c3370c28a");
            }
        });

        ISimpleBooksApi simpleBooksApi = RestService.For<ISimpleBooksApi>("https://simple-books-api.glitch.me");
        object orderId;

        [Fact (Skip = "already registered")]
        public async Task RegisterClient()
        {
            Dictionary<string, string> clientInfo = new Dictionary<string, string>()
            {
                { "clientName", "Maria" },
                { "clientEmail", "maria@example.com" }
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

            var order = await simpleBooksApi.PostBookOrderAsync(orderContent, "42e94e0f99efe02a8181930a30fbe83970a6601d3ad9dea8b660fb0c3370c28a");
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
