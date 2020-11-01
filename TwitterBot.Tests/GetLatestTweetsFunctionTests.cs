using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TwitterBot.AzureFunctions.Configurations;
using TwitterBot.AzureFunctions.Http;
using TwitterBot.Framework.Contracts.Data;
using TwitterBot.Framework.Types;
using TwitterBot.Tests.Helpers;
using Xunit;

namespace TwitterBot.Tests
{
    public class GetLatestTweetsFunctionTests
    {
        private readonly Mock<IDocumentDbRepository<User>> mockUserRepository;
        private readonly Mock<IDocumentDbRepository<Tweet>> mockTweetRepository;
        private readonly Mock<IOptions<AppSettingsConfiguration>> mockConfig;
        private readonly Mock<ILogger> mockLogger;
        private readonly string UserId;

        public GetLatestTweetsFunctionTests()
        {
            mockUserRepository = new Mock<IDocumentDbRepository<User>>();
            mockTweetRepository = new Mock<IDocumentDbRepository<Tweet>>();
            mockConfig = new Mock<IOptions<AppSettingsConfiguration>>();
            mockLogger = new Mock<ILogger>();
            UserId = "bcb5419c-262b-449c-9942-b077dcb1fc97";
            mockConfig.Setup(p => p.Value).Returns(new AppSettingsConfiguration
            {
                AppSettings = new AppSettings { TweetsFilterIntervalInDays = 1 }
            });
        }

        [Fact]
        public async Task GetLatestTweets_Null_User_Test()
        {
            // Arange
            var users = new List<User>();
            mockUserRepository.Setup(p => p.TopAsync(
                It.IsAny<Expression<Func<User, bool>>>(), 
                It.IsAny<int>())).ReturnsAsync(users.AsQueryable());

            var function = new GetLatestTweets(
                mockUserRepository.Object,
                mockTweetRepository.Object,
                mockConfig.Object);
            
            // Act
            var result = await function.Run(HttpTestHelper.CreateHttpRequest(
                string.Format("https://localhost/api/getlatesttweets?uid={0}", UserId), "GET"),
                mockLogger.Object);
            var jsonObject = (JsonResult)result;

            // Assert
            Assert.Null(jsonObject.Value);
        }
        [Fact]
        public async Task GetLatestTweets_Null_Hashtags_Test()
        {
            // Arrange
            var users = new List<User> { new User { UserId = UserId } };
            mockUserRepository.Setup(p => p.TopAsync(
                It.IsAny<Expression<Func<User, bool>>>(), 
                It.IsAny<int>()))
                .ReturnsAsync(users.AsQueryable());

            var function = new GetLatestTweets(
                mockUserRepository.Object,
                mockTweetRepository.Object,
                mockConfig.Object);

            // Act
            var result = await function.Run(
                HttpTestHelper.CreateHttpRequest(
                    string.Format("https://localhost/api/getlatesttweets?uid={0}", UserId), "GET"),
                    mockLogger.Object);
            var jsonObject = (JsonResult)result;

            // Assert
            Assert.Null(jsonObject.Value);
        }
    }
}
