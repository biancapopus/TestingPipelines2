using Xunit;
using Refit;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using ApiClasses;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace ApiTests
{
    public class ApingWebTests
    {
        readonly IApingWeb apingWeb = RestService.For<IApingWeb>("https://apingweb.com/api");

        IConfiguration UserInfo => new ConfigurationBuilder().AddUserSecrets<ApingWebTests>().Build();

        Dictionary<string, object> LoginUserInfo => new ()
        {
            { "email", Environment.GetEnvironmentVariable("email") },
            { "password", Environment.GetEnvironmentVariable("password") }
        };

        Article article = new Article()
        {
            Title = "What is Lorem Ipsum?",
            Body = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s",
            Picture = "https://example.com/lorem.png"
        };

        string Token => GetToken();

        //[Fact (Skip = "already registered")]
        //public async Task CanRegister()
        //{
        //    var registerResponse = await apingWeb.RegisterAsync(UserInfo);
        //    Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);
        //}

        [Fact]
        public async Task CanLogin()
        {
            var loginResponse = await apingWeb.LoginResponseAsync(LoginUserInfo);
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        }

        [Fact]
        public async Task CanGetArticles()
        {
            var articlesResult = await apingWeb.GetArticlesAsync(Token);
            Assert.True(articlesResult.Success);
            Assert.Equal("Success", articlesResult.Message);
            var firstArticle = articlesResult.Articles[0];
            Assert.Equal(2, firstArticle.Id);
            Assert.Equal("What is Lorem Ipsum?", firstArticle.Title);
            Assert.Equal("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s", firstArticle.Body);
            Assert.Equal("https://example.com/lorem.png", firstArticle.Picture);
            Assert.Equal("1", firstArticle.UserId);
            Assert.Equal("2022-03-22T01:21:20.000000Z", firstArticle.CreatedAt);
            Assert.Equal("2022-03-22T01:21:20.000000Z", firstArticle.UpdatedAt);
            Assert.Empty(articlesResult.Errors);
            Assert.Equal(200, articlesResult.Status);
        }

        [Fact]
        public async Task CanGetArticle()
        {
            var articlesResult = await apingWeb.GetArticleAsync(Token);
            Assert.True(articlesResult.Success);
            Assert.Equal("No result", articlesResult.Message);
            Assert.Empty(articlesResult.Articles);
            Assert.Empty(articlesResult.Errors);
            Assert.Equal(200, articlesResult.Status);
        }

        [Fact]
        public async Task CanCreateArticle_GetApiResponse()
        {
            var createArticleApiResponse = await apingWeb.CreateArticleApiResponseAsync(Token, article);
            Assert.Equal(HttpStatusCode.OK, createArticleApiResponse.StatusCode);
        }

        [Fact]
        public async Task CanCreateArticle()
        {
            var articlesResult = await apingWeb.CreateArticleAsync(Token, article);
            Assert.True(articlesResult.Success);
            Assert.Equal("Article has been created", articlesResult.Message);
            Assert.Empty(articlesResult.Articles);
            Assert.Empty(articlesResult.Errors);
            Assert.Equal(200, articlesResult.Status);
        }

        async Task<Dictionary<string, object>> Login()
        {
            return await apingWeb.LoginAsync(LoginUserInfo);
        }

        string GetToken()
        {
            return Login().Result["token"].ToString();
        }
    }
}