using AuctionSearchService.Api.Controllers;
using AuctionSearchService.Domain.Interfaces.Services;
using AuctionSearchService.Domain.Models;
using AuctionSearchService.Domain.RequestHelper;
using Moq;
using Xunit;

namespace AuctionSearchService.Tests.Controller
{
    public class SearchControllerTest
    {
        private readonly SearchController _sut;
        private readonly Mock<ISearchServices> _servicesMock = new();

        public SearchControllerTest()
        {
            _sut = new SearchController(_servicesMock.Object);
        }

        [Fact]
        public async Task SearchItems_Should_Return_ItemsResult()
        {
            _servicesMock.Setup(x => x.GetAllItems(It.IsAny<SearchParams>())).ReturnsAsync(GetItemsResult());

            var result = await _sut.SearchItems(new());

            Assert.IsType<ItemsResult>(result.Value);
            Assert.Equal(4, result.Value.Results.Count);
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
    }
}
