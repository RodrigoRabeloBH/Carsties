using AuctionSearchService.Application;
using AuctionSearchService.Domain.Interfaces.Repositories;
using AuctionSearchService.Domain.Models;
using AuctionSearchService.Domain.RequestHelper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using Xunit;

namespace AuctionSearchService.Tests.Application
{
    public class SearchServicesTest
    {
        private readonly Mock<ILogger<SearchServices>> _loggerMock = new();
        private readonly Mock<IItemRepository> _repMock = new();
        private readonly SearchServices _sut;

        public SearchServicesTest()
        {
            _sut = new SearchServices(_loggerMock.Object, _repMock.Object);
        }

        [Fact]
        public async Task GetAllItems_Should_Return_4_Items()
        {
            var itemsResult = GetItemsResult();

            var searchParams = new SearchParams();

            _repMock.Setup(x => x.GetAll(It.IsAny<SearchParams>())).ReturnsAsync(itemsResult);

            var items = await _sut.GetAllItems(searchParams);

            Assert.IsType<ItemsResult>(items);
            Assert.Equal(4, items.TotalCount);
            VerifyLogTest($"[Search Services].[Get All Items] - Getting all items with search term like: {JsonSerializer.Serialize(searchParams)}");
        }

        [Fact]
        public async Task GetAllItems_Should_Return_0_Items()
        {
            var itemsResult = new ItemsResult { PageCount = 0, TotalCount = 0, Results = new List<Item>() };

            var searchParams = new SearchParams();

            _repMock.Setup(x => x.GetAll(It.IsAny<SearchParams>())).ReturnsAsync(itemsResult);

            var items = await _sut.GetAllItems(searchParams);

            Assert.IsType<ItemsResult>(items);
            Assert.Equal(0, items.TotalCount);
            Assert.Equal(0, items.Results.Count);
            VerifyLogTest($"[Search Services].[Get All Items] - Getting all items with search term like: {JsonSerializer.Serialize(searchParams)}");
        }

        private static ItemsResult GetItemsResult()
        {
            var items = new ItemsResult
            {
                PageCount = 1,
                TotalCount = 4,
                Results = new List<Item>
                {
                    new Item
                    {
                        Color = "Black",
                        Make = "Audi",
                        Model = "TT"
                    },
                    new Item
                    {
                        Color = "Red",
                        Make = "Audi",
                        Model = "R8"
                    },
                    new Item
                    {
                        Color = "Yellow",
                        Make = "Ford",
                        Model = "Escort"
                    },
                    new Item
                    {
                        Color = "Green",
                        Make = "BMW",
                        Model = "X1"
                    }
                }
            };

            return items;
        }

        private void VerifyLogTest(string message, bool timesOnce = true)
        {

            _loggerMock.Verify(
               x => x.Log(
                   LogLevel.Information,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((o, t) => string.Equals(message, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                   It.IsAny<Exception>(),
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()), timesOnce ? Times.Once : Times.Never);
        }
    }
}
