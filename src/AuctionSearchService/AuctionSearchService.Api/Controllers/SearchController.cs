using AuctionSearchService.Domain.Interfaces.Services;
using AuctionSearchService.Domain.Models;
using AuctionSearchService.Domain.RequestHelper;
using Microsoft.AspNetCore.Mvc;

namespace AuctionSearchService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchServices _services;

        public SearchController(ISearchServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<ActionResult<ItemsResult>> SearchItems([FromQuery] SearchParams searchParams)
        {
            var items = await _services.GetAllItems(searchParams);

            return items;
        }
    }
}
